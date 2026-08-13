using Microsoft.EntityFrameworkCore;
using WorkflowPlatform.Api.Domain;
using Xunit;

namespace WorkflowPlatform.Api.Tests;

public class NodeConnectionTests
{
    private static WorkflowDefinition NewDefinition(string name = "Graph Test", int version = 1) => new()
    {
        DefinitionId = Guid.NewGuid(),
        Name = name,
        Version = version,
        CreatedBy = "test-user",
        CreatedAt = DateTime.UtcNow
    };

    [Fact]
    public void WorkflowDefinition_LoadsItsNodesAndConnections()
    {
        using var context = TestDbContextFactory.Create();

        var definition = NewDefinition();
        var start = new Node { NodeId = Guid.NewGuid(), WorkflowDefinitionId = definition.DefinitionId, Type = NodeType.Start, Name = "Start" };
        var end = new Node { NodeId = Guid.NewGuid(), WorkflowDefinitionId = definition.DefinitionId, Type = NodeType.End, Name = "End" };
        var connection = new Connection
        {
            ConnectionId = Guid.NewGuid(),
            WorkflowDefinitionId = definition.DefinitionId,
            SourceNodeId = start.NodeId,
            TargetNodeId = end.NodeId
        };

        context.WorkflowDefinitions.Add(definition);
        context.Nodes.AddRange(start, end);
        context.Connections.Add(connection);
        context.SaveChanges();

        var retrieved = context.WorkflowDefinitions
            .Include(d => d.Nodes)
            .Include(d => d.Connections)
            .Single(d => d.DefinitionId == definition.DefinitionId);

        Assert.Equal(2, retrieved.Nodes.Count);
        Assert.Single(retrieved.Connections);
    }

    [Fact]
    public void Connection_ConditionExpression_NullForNonBranchingConnection()
    {
        using var context = TestDbContextFactory.Create();

        var definition = NewDefinition();
        var start = new Node { NodeId = Guid.NewGuid(), WorkflowDefinitionId = definition.DefinitionId, Type = NodeType.Start, Name = "Start" };
        var end = new Node { NodeId = Guid.NewGuid(), WorkflowDefinitionId = definition.DefinitionId, Type = NodeType.End, Name = "End" };
        var connection = new Connection
        {
            ConnectionId = Guid.NewGuid(),
            WorkflowDefinitionId = definition.DefinitionId,
            SourceNodeId = start.NodeId,
            TargetNodeId = end.NodeId,
            ConditionExpression = null
        };

        context.WorkflowDefinitions.Add(definition);
        context.Nodes.AddRange(start, end);
        context.Connections.Add(connection);
        context.SaveChanges();

        var retrieved = context.Connections.Single(c => c.ConnectionId == connection.ConnectionId);
        Assert.Null(retrieved.ConditionExpression);
    }

    [Fact]
    public void Connection_ConditionExpression_PopulatedForBranchingConnection()
    {
        using var context = TestDbContextFactory.Create();

        var definition = NewDefinition();
        var conditionNode = new Node { NodeId = Guid.NewGuid(), WorkflowDefinitionId = definition.DefinitionId, Type = NodeType.Condition, Name = "If" };
        var end = new Node { NodeId = Guid.NewGuid(), WorkflowDefinitionId = definition.DefinitionId, Type = NodeType.End, Name = "End" };
        var connection = new Connection
        {
            ConnectionId = Guid.NewGuid(),
            WorkflowDefinitionId = definition.DefinitionId,
            SourceNodeId = conditionNode.NodeId,
            TargetNodeId = end.NodeId,
            ConditionExpression = "amount > 100"
        };

        context.WorkflowDefinitions.Add(definition);
        context.Nodes.AddRange(conditionNode, end);
        context.Connections.Add(connection);
        context.SaveChanges();

        var retrieved = context.Connections.Single(c => c.ConnectionId == connection.ConnectionId);
        Assert.Equal("amount > 100", retrieved.ConditionExpression);
    }

    [Fact]
    public void TaskHandlerReference_AssociatesWithTaskNode()
    {
        using var context = TestDbContextFactory.Create();

        var definition = NewDefinition();
        var taskNode = new Node { NodeId = Guid.NewGuid(), WorkflowDefinitionId = definition.DefinitionId, Type = NodeType.Task, Name = "Send Email" };
        var handlerReference = new TaskHandlerReference
        {
            TaskHandlerReferenceId = Guid.NewGuid(),
            NodeId = taskNode.NodeId,
            HandlerType = "SendEmail"
        };

        context.WorkflowDefinitions.Add(definition);
        context.Nodes.Add(taskNode);
        context.TaskHandlerReferences.Add(handlerReference);
        context.SaveChanges();

        var retrieved = context.TaskHandlerReferences.Single(t => t.NodeId == taskNode.NodeId);
        Assert.Equal("SendEmail", retrieved.HandlerType);
    }

    [Fact]
    public void TaskHandlerReference_MissingRequiredHandlerType_ThrowsOnSave()
    {
        using var context = TestDbContextFactory.Create();

        var definition = NewDefinition();
        var taskNode = new Node { NodeId = Guid.NewGuid(), WorkflowDefinitionId = definition.DefinitionId, Type = NodeType.Task, Name = "Send Email" };
        var handlerReference = new TaskHandlerReference
        {
            TaskHandlerReferenceId = Guid.NewGuid(),
            NodeId = taskNode.NodeId,
            HandlerType = "placeholder"
        };
        handlerReference.HandlerType = null!;

        context.WorkflowDefinitions.Add(definition);
        context.Nodes.Add(taskNode);
        context.TaskHandlerReferences.Add(handlerReference);

        Assert.Throws<DbUpdateException>(() => context.SaveChanges());
    }
}
