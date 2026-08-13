namespace WorkflowPlatform.Api.Domain;

/// <summary>
/// Matches the rendered node-lifecycle diagram (docs/diagrams/mmd/workflow-node-lifecycle.mmd).
/// </summary>
public enum NodeExecutionState
{
    Pending,
    Ready,
    Running,
    Completed,
    Failed,
    Skipped
}
