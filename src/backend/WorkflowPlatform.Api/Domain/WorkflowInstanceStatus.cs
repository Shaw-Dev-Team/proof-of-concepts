namespace WorkflowPlatform.Api.Domain;

/// <summary>
/// Matches the workflow-instance-lifecycle diagram (docs/diagrams/mmd/workflow-instance-lifecycle.mmd).
/// </summary>
public enum WorkflowInstanceStatus
{
    Draft,
    Running,
    Completed,
    Failed,
    Paused,
    Cancelled
}
