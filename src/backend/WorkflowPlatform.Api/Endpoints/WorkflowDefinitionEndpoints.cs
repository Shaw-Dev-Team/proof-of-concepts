using WorkflowPlatform.Api.Contracts;
using WorkflowPlatform.Api.Services;

namespace WorkflowPlatform.Api.Endpoints;

/// <summary>
/// Minimal API endpoint group for <see cref="Domain.WorkflowDefinition"/> CRUD + versioning (FR-002).
/// </summary>
public static class WorkflowDefinitionEndpoints
{
    public static RouteGroupBuilder MapWorkflowDefinitionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/workflow-definitions").WithTags("WorkflowDefinitions");

        group.MapPost("/", CreateAsync);
        group.MapGet("/", ListDistinctDefinitionsAsync);
        group.MapGet("/{definitionId:guid}", GetByIdAsync);
        group.MapGet("/by-name/{name}", GetLatestByNameAsync);
        group.MapGet("/by-name/{name}/versions", ListVersionsByNameAsync);
        group.MapPost("/by-name/{name}/versions", CreateVersionAsync);

        return group;
    }

    private static async Task<IResult> CreateAsync(
        CreateWorkflowDefinitionRequest request, IWorkflowDefinitionService service, CancellationToken cancellationToken)
    {
        try
        {
            var response = await service.CreateAsync(request, cancellationToken);
            return Results.Created($"/api/workflow-definitions/{response.DefinitionId}", response);
        }
        catch (WorkflowDefinitionValidationException ex)
        {
            return Results.ValidationProblem(ex.Errors);
        }
        catch (WorkflowDefinitionConflictException ex)
        {
            return Results.Conflict(ex.Message);
        }
    }

    private static async Task<IResult> CreateVersionAsync(
        string name, CreateWorkflowDefinitionRequest request, IWorkflowDefinitionService service, CancellationToken cancellationToken)
    {
        try
        {
            var response = await service.CreateVersionAsync(name, request, cancellationToken);
            return Results.Created($"/api/workflow-definitions/{response.DefinitionId}", response);
        }
        catch (WorkflowDefinitionValidationException ex)
        {
            return Results.ValidationProblem(ex.Errors);
        }
        catch (WorkflowDefinitionConflictException ex)
        {
            return Results.Conflict(ex.Message);
        }
    }

    private static async Task<IResult> GetByIdAsync(Guid definitionId, IWorkflowDefinitionService service, CancellationToken cancellationToken)
    {
        var response = await service.GetByIdAsync(definitionId, cancellationToken);
        return response is null ? Results.NotFound() : Results.Ok(response);
    }

    private static async Task<IResult> GetLatestByNameAsync(string name, IWorkflowDefinitionService service, CancellationToken cancellationToken)
    {
        var response = await service.GetLatestByNameAsync(name, cancellationToken);
        return response is null ? Results.NotFound() : Results.Ok(response);
    }

    private static async Task<IResult> ListVersionsByNameAsync(string name, IWorkflowDefinitionService service, CancellationToken cancellationToken)
    {
        var response = await service.ListVersionsByNameAsync(name, cancellationToken);
        return Results.Ok(response);
    }

    private static async Task<IResult> ListDistinctDefinitionsAsync(IWorkflowDefinitionService service, CancellationToken cancellationToken)
    {
        var response = await service.ListDistinctDefinitionsAsync(cancellationToken);
        return Results.Ok(response);
    }
}
