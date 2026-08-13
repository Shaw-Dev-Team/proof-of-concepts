using Microsoft.EntityFrameworkCore;
using WorkflowPlatform.Api.Contracts;
using WorkflowPlatform.Api.Data;
using WorkflowPlatform.Api.Domain;

namespace WorkflowPlatform.Api.Services;

public class WorkflowDefinitionService : IWorkflowDefinitionService
{
    private readonly WorkflowPlatformDbContext _dbContext;

    public WorkflowDefinitionService(WorkflowPlatformDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<WorkflowDefinitionResponse> CreateAsync(CreateWorkflowDefinitionRequest request, CancellationToken cancellationToken = default)
    {
        Validate(request);

        var definition = BuildDefinition(request.Name, request, version: 1);
        _dbContext.WorkflowDefinitions.Add(definition);
        await SaveAsync(cancellationToken);

        return ToResponse(definition);
    }

    public async Task<WorkflowDefinitionResponse> CreateVersionAsync(string name, CreateWorkflowDefinitionRequest request, CancellationToken cancellationToken = default)
    {
        Validate(request);

        var latestVersion = await _dbContext.WorkflowDefinitions
            .Where(d => d.Name == name)
            .Select(d => (int?)d.Version)
            .MaxAsync(cancellationToken);
        var nextVersion = (latestVersion ?? 0) + 1;

        var definition = BuildDefinition(name, request, nextVersion);
        _dbContext.WorkflowDefinitions.Add(definition);
        await SaveAsync(cancellationToken);

        return ToResponse(definition);
    }

    public async Task<WorkflowDefinitionResponse?> GetByIdAsync(Guid definitionId, CancellationToken cancellationToken = default)
    {
        var definition = await _dbContext.WorkflowDefinitions
            .Include(d => d.Nodes)
            .Include(d => d.Connections)
            .AsNoTracking()
            .SingleOrDefaultAsync(d => d.DefinitionId == definitionId, cancellationToken);

        return definition is null ? null : ToResponse(definition);
    }

    public async Task<WorkflowDefinitionResponse?> GetLatestByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var definition = await _dbContext.WorkflowDefinitions
            .Include(d => d.Nodes)
            .Include(d => d.Connections)
            .AsNoTracking()
            .Where(d => d.Name == name)
            .OrderByDescending(d => d.Version)
            .FirstOrDefaultAsync(cancellationToken);

        return definition is null ? null : ToResponse(definition);
    }

    public async Task<IReadOnlyList<WorkflowDefinitionSummaryResponse>> ListVersionsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var definitions = await _dbContext.WorkflowDefinitions
            .AsNoTracking()
            .Where(d => d.Name == name)
            .OrderBy(d => d.Version)
            .ToListAsync(cancellationToken);

        return definitions.Select(ToSummary).ToList();
    }

    public async Task<IReadOnlyList<WorkflowDefinitionSummaryResponse>> ListDistinctDefinitionsAsync(CancellationToken cancellationToken = default)
    {
        var latestVersionByName = _dbContext.WorkflowDefinitions
            .GroupBy(d => d.Name)
            .Select(g => new { Name = g.Key, MaxVersion = g.Max(d => d.Version) });

        var definitions = await _dbContext.WorkflowDefinitions
            .AsNoTracking()
            .Join(
                latestVersionByName,
                d => new { d.Name, Version = d.Version },
                lv => new { lv.Name, Version = lv.MaxVersion },
                (d, lv) => d)
            .OrderBy(d => d.Name)
            .ToListAsync(cancellationToken);

        return definitions.Select(ToSummary).ToList();
    }

    private async Task SaveAsync(CancellationToken cancellationToken)
    {
        try
        {
            // A single SaveChangesAsync call persists the definition, its nodes, and its connections
            // in one transaction — a definition is never left partially saved (AC4).
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            throw new WorkflowDefinitionConflictException(
                "A workflow definition with this Name and Version already exists.");
        }
    }

    private static void Validate(CreateWorkflowDefinitionRequest request)
    {
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            errors["Name"] = new[] { "Name is required." };
        }

        if (string.IsNullOrWhiteSpace(request.CreatedBy))
        {
            errors["CreatedBy"] = new[] { "CreatedBy is required." };
        }

        for (var i = 0; i < request.Nodes.Count; i++)
        {
            if (string.IsNullOrWhiteSpace(request.Nodes[i].Name))
            {
                errors[$"Nodes[{i}].Name"] = new[] { "Node Name is required." };
            }
        }

        var nodeIds = request.Nodes.Select(n => n.Id).ToHashSet();
        for (var i = 0; i < request.Connections.Count; i++)
        {
            var connection = request.Connections[i];
            if (!nodeIds.Contains(connection.SourceNodeId))
            {
                errors[$"Connections[{i}].SourceNodeId"] = new[] { $"SourceNodeId '{connection.SourceNodeId}' does not reference a node in this payload." };
            }

            if (!nodeIds.Contains(connection.TargetNodeId))
            {
                errors[$"Connections[{i}].TargetNodeId"] = new[] { $"TargetNodeId '{connection.TargetNodeId}' does not reference a node in this payload." };
            }
        }

        if (errors.Count > 0)
        {
            throw new WorkflowDefinitionValidationException(errors);
        }
    }

    private static WorkflowDefinition BuildDefinition(string name, CreateWorkflowDefinitionRequest request, int version)
    {
        var definitionId = Guid.NewGuid();
        var definition = new WorkflowDefinition
        {
            DefinitionId = definitionId,
            Name = name,
            Version = version,
            Description = request.Description,
            Status = request.Status,
            CreatedBy = request.CreatedBy,
            CreatedAt = DateTime.UtcNow,
            Metadata = request.Metadata
        };

        foreach (var nodeRequest in request.Nodes)
        {
            definition.Nodes.Add(new Node
            {
                NodeId = nodeRequest.Id,
                WorkflowDefinitionId = definitionId,
                Type = nodeRequest.Type,
                Name = nodeRequest.Name,
                Description = nodeRequest.Description,
                Configuration = nodeRequest.Configuration
            });
        }

        foreach (var connectionRequest in request.Connections)
        {
            definition.Connections.Add(new Connection
            {
                ConnectionId = Guid.NewGuid(),
                WorkflowDefinitionId = definitionId,
                SourceNodeId = connectionRequest.SourceNodeId,
                TargetNodeId = connectionRequest.TargetNodeId,
                ConditionExpression = connectionRequest.ConditionExpression
            });
        }

        return definition;
    }

    private static WorkflowDefinitionResponse ToResponse(WorkflowDefinition definition) => new(
        definition.DefinitionId,
        definition.Name,
        definition.Version,
        definition.Description,
        definition.Status,
        definition.CreatedBy,
        definition.CreatedAt,
        definition.Metadata,
        definition.Nodes.Select(n => new NodeResponse(n.NodeId, n.Type, n.Name, n.Description, n.Configuration)).ToList(),
        definition.Connections.Select(c => new ConnectionResponse(c.ConnectionId, c.SourceNodeId, c.TargetNodeId, c.ConditionExpression)).ToList());

    private static WorkflowDefinitionSummaryResponse ToSummary(WorkflowDefinition definition) => new(
        definition.DefinitionId,
        definition.Name,
        definition.Version,
        definition.Description,
        definition.Status,
        definition.CreatedBy,
        definition.CreatedAt);
}
