namespace WorkflowPlatform.Api.Services;

/// <summary>
/// Thrown when a <see cref="Contracts.CreateWorkflowDefinitionRequest"/> fails input validation
/// (required fields, connection endpoints not present among the submitted nodes). Callers map this
/// to a 400 response rather than letting it surface as an unhandled exception.
/// </summary>
public sealed class WorkflowDefinitionValidationException : Exception
{
    public IDictionary<string, string[]> Errors { get; }

    public WorkflowDefinitionValidationException(IDictionary<string, string[]> errors)
        : base("Workflow definition request failed validation.")
    {
        Errors = errors;
    }
}

/// <summary>
/// Thrown when persisting a definition/version would violate the unique (Name, Version) index —
/// e.g. a concurrent version-creation race. Callers map this to a 409 response.
/// </summary>
public sealed class WorkflowDefinitionConflictException : Exception
{
    public WorkflowDefinitionConflictException(string message)
        : base(message)
    {
    }
}
