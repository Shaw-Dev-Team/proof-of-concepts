using Microsoft.EntityFrameworkCore;
using WorkflowPlatform.Api.Domain;
using Xunit;

namespace WorkflowPlatform.Api.Tests;

public class WorkflowInstanceExecutionTests
{
    private static WorkflowDefinition NewDefinition() => new()
    {
        DefinitionId = Guid.NewGuid(),
        Name = "Instance Test",
        Version = 1,
        CreatedBy = "test-user",
        CreatedAt = DateTime.UtcNow
    };

    [Fact]
    public void WorkflowInstance_LoadsItsExecutionHistory()
    {
        using var context = TestDbContextFactory.Create();

        var definition = NewDefinition();
        var node = new Node { NodeId = Guid.NewGuid(), WorkflowDefinitionId = definition.DefinitionId, Type = NodeType.Task, Name = "Do Work" };
        var instance = new WorkflowInstance
        {
            InstanceId = Guid.NewGuid(),
            WorkflowDefinitionId = definition.DefinitionId,
            DefinitionVersion = definition.Version,
            Status = WorkflowInstanceStatus.Running,
            StartTime = DateTime.UtcNow
        };
        var execution = new NodeExecution
        {
            EventId = Guid.NewGuid(),
            WorkflowInstanceId = instance.InstanceId,
            NodeId = node.NodeId,
            WorkflowDefinitionId = definition.DefinitionId,
            State = NodeExecutionState.Completed,
            StartedAt = DateTime.UtcNow,
            CompletedAt = DateTime.UtcNow
        };

        context.WorkflowDefinitions.Add(definition);
        context.Nodes.Add(node);
        context.WorkflowInstances.Add(instance);
        context.NodeExecutions.Add(execution);
        context.SaveChanges();

        var retrieved = context.WorkflowInstances
            .Include(i => i.ExecutionHistory)
            .Single(i => i.InstanceId == instance.InstanceId);

        Assert.Single(retrieved.ExecutionHistory);
        Assert.Equal(NodeExecutionState.Completed, retrieved.ExecutionHistory.Single().State);
    }

    [Fact]
    public void WorkflowInstance_DefinitionVersion_IsSnapshotNotLiveReference()
    {
        using var context = TestDbContextFactory.Create();

        var definitionV1 = NewDefinition();
        context.WorkflowDefinitions.Add(definitionV1);

        var instance = new WorkflowInstance
        {
            InstanceId = Guid.NewGuid(),
            WorkflowDefinitionId = definitionV1.DefinitionId,
            DefinitionVersion = definitionV1.Version,
            Status = WorkflowInstanceStatus.Draft
        };
        context.WorkflowInstances.Add(instance);
        context.SaveChanges();

        // A new definition version is published later; the running instance's snapshot is untouched.
        var definitionV2 = new WorkflowDefinition
        {
            DefinitionId = Guid.NewGuid(),
            Name = definitionV1.Name,
            Version = 2,
            CreatedBy = "test-user",
            CreatedAt = DateTime.UtcNow
        };
        context.WorkflowDefinitions.Add(definitionV2);
        context.SaveChanges();

        var retrieved = context.WorkflowInstances.Single(i => i.InstanceId == instance.InstanceId);
        Assert.Equal(1, retrieved.DefinitionVersion);
    }

    [Fact]
    public void NodeExecution_EvaluationOutcome_NullUntilEvaluated()
    {
        using var context = TestDbContextFactory.Create();

        var definition = NewDefinition();
        var conditionNode = new Node { NodeId = Guid.NewGuid(), WorkflowDefinitionId = definition.DefinitionId, Type = NodeType.Condition, Name = "If" };
        var instance = new WorkflowInstance
        {
            InstanceId = Guid.NewGuid(),
            WorkflowDefinitionId = definition.DefinitionId,
            DefinitionVersion = definition.Version,
            Status = WorkflowInstanceStatus.Running
        };
        var execution = new NodeExecution
        {
            EventId = Guid.NewGuid(),
            WorkflowInstanceId = instance.InstanceId,
            NodeId = conditionNode.NodeId,
            WorkflowDefinitionId = definition.DefinitionId,
            State = NodeExecutionState.Running,
            EvaluationOutcome = null
        };

        context.WorkflowDefinitions.Add(definition);
        context.Nodes.Add(conditionNode);
        context.WorkflowInstances.Add(instance);
        context.NodeExecutions.Add(execution);
        context.SaveChanges();

        var retrieved = context.NodeExecutions.Single(e => e.EventId == execution.EventId);
        Assert.Null(retrieved.EvaluationOutcome);
    }
}
