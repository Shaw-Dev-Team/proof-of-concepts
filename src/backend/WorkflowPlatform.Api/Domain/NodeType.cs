namespace WorkflowPlatform.Api.Domain;

/// <summary>
/// Node type catalog per Architecture §4 "Node Types". ManualApproval is intentionally
/// excluded — the architecture models it as a Task node targeting a HumanApproval-style
/// handler (ADR-004), not a distinct engine primitive.
/// </summary>
public enum NodeType
{
    Start,
    End,
    Task,
    Condition,
    Switch,
    Loop,
    ParallelSplit,
    ParallelMerge,
    WaitPause
}
