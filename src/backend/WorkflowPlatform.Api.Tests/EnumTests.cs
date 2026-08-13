using WorkflowPlatform.Api.Domain;
using Xunit;

namespace WorkflowPlatform.Api.Tests;

public class EnumTests
{
    [Fact]
    public void NodeExecutionState_ContainsExactlyTheLifecycleDiagramStates()
    {
        var expected = new[] { "Pending", "Ready", "Running", "Completed", "Failed", "Skipped" };
        var actual = Enum.GetNames<NodeExecutionState>();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void WorkflowInstanceStatus_ContainsExactlyTheLifecycleDiagramStates()
    {
        var expected = new[] { "Draft", "Running", "Completed", "Failed", "Paused", "Cancelled" };
        var actual = Enum.GetNames<WorkflowInstanceStatus>();

        Assert.Equal(expected, actual);
    }
}
