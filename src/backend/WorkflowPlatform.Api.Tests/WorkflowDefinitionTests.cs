using Microsoft.EntityFrameworkCore;
using WorkflowPlatform.Api.Domain;
using Xunit;

namespace WorkflowPlatform.Api.Tests;

public class WorkflowDefinitionTests
{
    [Fact]
    public void WorkflowDefinition_CanBePersistedAndRetrieved_WithZeroNodes()
    {
        using var context = TestDbContextFactory.Create();

        var definition = new WorkflowDefinition
        {
            DefinitionId = Guid.NewGuid(),
            Name = "Empty Draft",
            Version = 1,
            CreatedBy = "test-user",
            CreatedAt = DateTime.UtcNow
        };

        context.WorkflowDefinitions.Add(definition);
        context.SaveChanges();

        var retrieved = context.WorkflowDefinitions
            .Include(d => d.Nodes)
            .Include(d => d.Connections)
            .Single(d => d.DefinitionId == definition.DefinitionId);

        Assert.Empty(retrieved.Nodes);
        Assert.Empty(retrieved.Connections);
    }

    [Fact]
    public void WorkflowDefinition_MissingRequiredName_ThrowsOnSave()
    {
        using var context = TestDbContextFactory.Create();

        var definition = new WorkflowDefinition
        {
            DefinitionId = Guid.NewGuid(),
            Name = "placeholder",
            Version = 1,
            CreatedBy = "test-user",
            CreatedAt = DateTime.UtcNow
        };
        definition.Name = null!;

        context.WorkflowDefinitions.Add(definition);

        Assert.Throws<DbUpdateException>(() => context.SaveChanges());
    }

    [Fact]
    public void TwoDefinitions_SameName_DifferentVersions_CoexistIndependently()
    {
        using var context = TestDbContextFactory.Create();

        var v1 = new WorkflowDefinition
        {
            DefinitionId = Guid.NewGuid(),
            Name = "Onboarding",
            Version = 1,
            CreatedBy = "test-user",
            CreatedAt = DateTime.UtcNow
        };
        var v2 = new WorkflowDefinition
        {
            DefinitionId = Guid.NewGuid(),
            Name = "Onboarding",
            Version = 2,
            CreatedBy = "test-user",
            CreatedAt = DateTime.UtcNow
        };

        context.WorkflowDefinitions.AddRange(v1, v2);
        context.SaveChanges();

        var stored = context.WorkflowDefinitions.Where(d => d.Name == "Onboarding").ToList();
        Assert.Equal(2, stored.Count);
        Assert.Contains(stored, d => d.Version == 1);
        Assert.Contains(stored, d => d.Version == 2);
    }
}
