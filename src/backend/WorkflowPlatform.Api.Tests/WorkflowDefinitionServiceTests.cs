using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using WorkflowPlatform.Api.Contracts;
using WorkflowPlatform.Api.Data;
using WorkflowPlatform.Api.Domain;
using WorkflowPlatform.Api.Services;
using Xunit;

namespace WorkflowPlatform.Api.Tests;

public class WorkflowDefinitionServiceTests
{
    private static CreateWorkflowDefinitionRequest ValidRequest(string name = "Onboarding", string createdBy = "test-user")
    {
        return new CreateWorkflowDefinitionRequest
        {
            Name = name,
            CreatedBy = createdBy
        };
    }

    [Fact]
    public async Task CreateAsync_WithNodesAndConnections_PersistsAllThreeEntityTypes()
    {
        using var context = TestDbContextFactory.Create();
        var service = new WorkflowDefinitionService(context);

        var startId = Guid.NewGuid();
        var endId = Guid.NewGuid();
        var request = ValidRequest();
        request.Nodes.Add(new CreateNodeRequest { Id = startId, Type = NodeType.Start, Name = "Start" });
        request.Nodes.Add(new CreateNodeRequest { Id = endId, Type = NodeType.End, Name = "End" });
        request.Connections.Add(new CreateConnectionRequest { SourceNodeId = startId, TargetNodeId = endId });

        var response = await service.CreateAsync(request);

        var stored = await context.WorkflowDefinitions
            .Include(d => d.Nodes)
            .Include(d => d.Connections)
            .SingleAsync(d => d.DefinitionId == response.DefinitionId);

        Assert.Equal(1, stored.Version);
        Assert.Equal(2, stored.Nodes.Count);
        Assert.Single(stored.Connections);
        Assert.Equal(startId, stored.Connections.Single().SourceNodeId);
        Assert.Equal(endId, stored.Connections.Single().TargetNodeId);
    }

    [Fact]
    public async Task CreateAsync_WithZeroNodesAndConnections_PersistsAsEmptyDraft()
    {
        using var context = TestDbContextFactory.Create();
        var service = new WorkflowDefinitionService(context);

        var response = await service.CreateAsync(ValidRequest());

        var stored = await context.WorkflowDefinitions
            .Include(d => d.Nodes)
            .Include(d => d.Connections)
            .SingleAsync(d => d.DefinitionId == response.DefinitionId);

        Assert.Empty(stored.Nodes);
        Assert.Empty(stored.Connections);
    }

    [Fact]
    public async Task CreateVersionAsync_ForExistingDefinition_IncrementsVersionAndLeavesPriorVersionUnchanged()
    {
        using var context = TestDbContextFactory.Create();
        var service = new WorkflowDefinitionService(context);

        var v1Request = ValidRequest();
        v1Request.Description = "v1 description";
        var v1NodeId = Guid.NewGuid();
        v1Request.Nodes.Add(new CreateNodeRequest { Id = v1NodeId, Type = NodeType.Start, Name = "Start" });
        var v1Response = await service.CreateAsync(v1Request);

        var v2Request = ValidRequest();
        v2Request.Description = "v2 description";
        var v2Response = await service.CreateVersionAsync("Onboarding", v2Request);

        Assert.Equal(1, v1Response.Version);
        Assert.Equal(2, v2Response.Version);
        Assert.NotEqual(v1Response.DefinitionId, v2Response.DefinitionId);

        var priorVersion = await context.WorkflowDefinitions
            .Include(d => d.Nodes)
            .Include(d => d.Connections)
            .AsNoTracking()
            .SingleAsync(d => d.DefinitionId == v1Response.DefinitionId);

        Assert.Equal(1, priorVersion.Version);
        Assert.Equal("v1 description", priorVersion.Description);
        Assert.Single(priorVersion.Nodes);
        Assert.Equal(v1NodeId, priorVersion.Nodes.Single().NodeId);
    }

    [Fact]
    public async Task CreateVersionAsync_ForNameThatDoesNotExistYet_BehavesAsVersionOneCreation()
    {
        using var context = TestDbContextFactory.Create();
        var service = new WorkflowDefinitionService(context);

        var response = await service.CreateVersionAsync("Brand New", ValidRequest(name: "Brand New"));

        Assert.Equal(1, response.Version);
    }

    [Fact]
    public async Task CreateVersionAsync_ReusingAPriorVersionsNodeId_SucceedsAsDistinctRowPerVersion()
    {
        // Node's PK is scoped to (NodeId, WorkflowDefinitionId), not NodeId alone, precisely so a
        // client (the eventual Angular Designer) can preserve a node's logical identity across
        // version saves. Reusing a v1 NodeId in a v2 payload must succeed as a brand-new, fully
        // independent row \u2014 not collide with v1's row nor mutate it.
        using var context = TestDbContextFactory.Create();
        var service = new WorkflowDefinitionService(context);

        var sharedNodeId = Guid.NewGuid();
        var v1Request = ValidRequest();
        v1Request.Nodes.Add(new CreateNodeRequest { Id = sharedNodeId, Type = NodeType.Start, Name = "Start v1" });
        var v1Response = await service.CreateAsync(v1Request);

        var v2Request = ValidRequest();
        v2Request.Nodes.Add(new CreateNodeRequest { Id = sharedNodeId, Type = NodeType.Start, Name = "Start v2" });
        var v2Response = await service.CreateVersionAsync("Onboarding", v2Request);

        Assert.Equal(2, v2Response.Version);
        Assert.Equal(sharedNodeId, v2Response.Nodes.Single().Id);

        var v1Stored = await context.WorkflowDefinitions
            .Include(d => d.Nodes)
            .AsNoTracking()
            .SingleAsync(d => d.DefinitionId == v1Response.DefinitionId);
        var v2Stored = await context.WorkflowDefinitions
            .Include(d => d.Nodes)
            .AsNoTracking()
            .SingleAsync(d => d.DefinitionId == v2Response.DefinitionId);

        Assert.Equal(sharedNodeId, v1Stored.Nodes.Single().NodeId);
        Assert.Equal("Start v1", v1Stored.Nodes.Single().Name);
        Assert.Equal(sharedNodeId, v2Stored.Nodes.Single().NodeId);
        Assert.Equal("Start v2", v2Stored.Nodes.Single().Name);
        Assert.NotEqual(v1Stored.Nodes.Single().WorkflowDefinitionId, v2Stored.Nodes.Single().WorkflowDefinitionId);
    }

    [Fact]
    public async Task GetLatestByNameAsync_ReturnsHighestVersionRow()
    {
        using var context = TestDbContextFactory.Create();
        var service = new WorkflowDefinitionService(context);

        await service.CreateAsync(ValidRequest());
        var v2 = await service.CreateVersionAsync("Onboarding", ValidRequest());
        await service.CreateVersionAsync("Onboarding", ValidRequest());
        var v4 = await service.CreateVersionAsync("Onboarding", ValidRequest());

        var latest = await service.GetLatestByNameAsync("Onboarding");

        Assert.NotNull(latest);
        Assert.Equal(4, latest!.Version);
        Assert.Equal(v4.DefinitionId, latest.DefinitionId);
        Assert.NotEqual(v2.DefinitionId, latest.DefinitionId);
    }

    [Fact]
    public async Task ListVersionsByNameAsync_ReturnsAllVersionsInVersionOrder()
    {
        using var context = TestDbContextFactory.Create();
        var service = new WorkflowDefinitionService(context);

        await service.CreateAsync(ValidRequest());
        await service.CreateVersionAsync("Onboarding", ValidRequest());
        await service.CreateVersionAsync("Onboarding", ValidRequest());

        var versions = await service.ListVersionsByNameAsync("Onboarding");

        Assert.Equal(3, versions.Count);
        Assert.Equal([1, 2, 3], versions.Select(v => v.Version));
    }

    [Fact]
    public async Task ListDistinctDefinitionsAsync_ReturnsOneRowPerNameAtLatestVersion()
    {
        using var context = TestDbContextFactory.Create();
        var service = new WorkflowDefinitionService(context);

        await service.CreateAsync(ValidRequest(name: "Onboarding"));
        await service.CreateVersionAsync("Onboarding", ValidRequest(name: "Onboarding"));
        await service.CreateAsync(ValidRequest(name: "Offboarding"));

        var distinct = await service.ListDistinctDefinitionsAsync();

        Assert.Equal(2, distinct.Count);
        Assert.Equal(2, distinct.Single(d => d.Name == "Onboarding").Version);
        Assert.Equal(1, distinct.Single(d => d.Name == "Offboarding").Version);
    }

    [Fact]
    public async Task CreateAsync_MissingName_ThrowsValidationException()
    {
        using var context = TestDbContextFactory.Create();
        var service = new WorkflowDefinitionService(context);

        var request = ValidRequest();
        request.Name = " ";

        var ex = await Assert.ThrowsAsync<WorkflowDefinitionValidationException>(() => service.CreateAsync(request));
        Assert.Contains("Name", ex.Errors.Keys);
    }

    [Fact]
    public async Task CreateAsync_MissingCreatedBy_ThrowsValidationException()
    {
        using var context = TestDbContextFactory.Create();
        var service = new WorkflowDefinitionService(context);

        var request = ValidRequest();
        request.CreatedBy = "";

        var ex = await Assert.ThrowsAsync<WorkflowDefinitionValidationException>(() => service.CreateAsync(request));
        Assert.Contains("CreatedBy", ex.Errors.Keys);
    }

    [Fact]
    public async Task CreateAsync_ConnectionReferencesUnknownNode_ThrowsValidationException()
    {
        using var context = TestDbContextFactory.Create();
        var service = new WorkflowDefinitionService(context);

        var request = ValidRequest();
        var knownNodeId = Guid.NewGuid();
        request.Nodes.Add(new CreateNodeRequest { Id = knownNodeId, Type = NodeType.Start, Name = "Start" });
        request.Connections.Add(new CreateConnectionRequest { SourceNodeId = knownNodeId, TargetNodeId = Guid.NewGuid() });

        var ex = await Assert.ThrowsAsync<WorkflowDefinitionValidationException>(() => service.CreateAsync(request));
        Assert.Contains(ex.Errors.Keys, key => key.Contains("TargetNodeId"));
    }

    [Fact]
    public void ConcurrentSameNameAndVersion_ViolatesUniqueIndex()
    {
        // Simulates a version-creation race: two rows with the same (Name, Version) must never
        // both persist — the unique index configured in WorkflowPlatformDbContext.OnModelCreating
        // is relied upon here rather than bypassed with application-level locking.
        // Uses SQLite (a real relational engine), not the InMemory provider — InMemory doesn't
        // enforce secondary unique indexes, so this constraint can only be genuinely exercised here.
        using var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        var options = new DbContextOptionsBuilder<WorkflowPlatformDbContext>().UseSqlite(connection).Options;
        using var context = new WorkflowPlatformDbContext(options);
        context.Database.EnsureCreated();

        context.WorkflowDefinitions.Add(new WorkflowDefinition
        {
            DefinitionId = Guid.NewGuid(),
            Name = "Racing",
            Version = 1,
            CreatedBy = "test-user",
            CreatedAt = DateTime.UtcNow
        });
        context.SaveChanges();

        context.WorkflowDefinitions.Add(new WorkflowDefinition
        {
            DefinitionId = Guid.NewGuid(),
            Name = "Racing",
            Version = 1,
            CreatedBy = "test-user",
            CreatedAt = DateTime.UtcNow
        });

        Assert.Throws<DbUpdateException>(() => context.SaveChanges());
    }
}
