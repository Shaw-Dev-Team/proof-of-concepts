using WorkflowPlatform.Api.Contracts;

namespace WorkflowPlatform.Api.Services;

/// <summary>
/// Creates, retrieves, and versions <see cref="Domain.WorkflowDefinition"/> graphs. Per NFR-005,
/// editing a definition never mutates a previously persisted version's row — <see cref="CreateVersionAsync"/>
/// always inserts a new row with an incremented <see cref="Domain.WorkflowDefinition.Version"/>.
/// </summary>
public interface IWorkflowDefinitionService
{
    Task<WorkflowDefinitionResponse> CreateAsync(CreateWorkflowDefinitionRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new version of the definition identified by <paramref name="name"/>. If no prior
    /// version exists for that name, this behaves as an initial (version 1) creation.
    /// </summary>
    Task<WorkflowDefinitionResponse> CreateVersionAsync(string name, CreateWorkflowDefinitionRequest request, CancellationToken cancellationToken = default);

    Task<WorkflowDefinitionResponse?> GetByIdAsync(Guid definitionId, CancellationToken cancellationToken = default);

    /// <summary>Returns the highest-<see cref="Domain.WorkflowDefinition.Version"/> row for <paramref name="name"/>.</summary>
    Task<WorkflowDefinitionResponse?> GetLatestByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>Returns every version of the definition named <paramref name="name"/>, ordered by version.</summary>
    Task<IReadOnlyList<WorkflowDefinitionSummaryResponse>> ListVersionsByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>Returns one row per distinct definition name — the latest version of each.</summary>
    Task<IReadOnlyList<WorkflowDefinitionSummaryResponse>> ListDistinctDefinitionsAsync(CancellationToken cancellationToken = default);
}
