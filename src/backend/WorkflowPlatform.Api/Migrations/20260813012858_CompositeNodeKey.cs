using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkflowPlatform.Api.Migrations
{
    /// <inheritdoc />
    public partial class CompositeNodeKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Connections_Nodes_SourceNodeId",
                table: "Connections");

            migrationBuilder.DropForeignKey(
                name: "FK_Connections_Nodes_TargetNodeId",
                table: "Connections");

            migrationBuilder.DropForeignKey(
                name: "FK_NodeExecutions_Nodes_NodeId",
                table: "NodeExecutions");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskHandlerReferences_Nodes_NodeId",
                table: "TaskHandlerReferences");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowInstanceCurrentNodes_Nodes_CurrentNodesNodeId",
                table: "WorkflowInstanceCurrentNodes");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowInstanceCurrentNodes_WorkflowInstances_WorkflowInstanceInstanceId",
                table: "WorkflowInstanceCurrentNodes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkflowInstanceCurrentNodes",
                table: "WorkflowInstanceCurrentNodes");

            migrationBuilder.DropIndex(
                name: "IX_WorkflowInstanceCurrentNodes_WorkflowInstanceInstanceId",
                table: "WorkflowInstanceCurrentNodes");

            migrationBuilder.DropIndex(
                name: "IX_TaskHandlerReferences_NodeId",
                table: "TaskHandlerReferences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Nodes",
                table: "Nodes");

            migrationBuilder.DropIndex(
                name: "IX_NodeExecutions_NodeId",
                table: "NodeExecutions");

            migrationBuilder.DropIndex(
                name: "IX_Connections_SourceNodeId",
                table: "Connections");

            migrationBuilder.DropIndex(
                name: "IX_Connections_TargetNodeId",
                table: "Connections");

            migrationBuilder.RenameColumn(
                name: "WorkflowInstanceInstanceId",
                table: "WorkflowInstanceCurrentNodes",
                newName: "WorkflowDefinitionId");

            migrationBuilder.RenameColumn(
                name: "CurrentNodesNodeId",
                table: "WorkflowInstanceCurrentNodes",
                newName: "NodeId");

            migrationBuilder.AddColumn<Guid>(
                name: "WorkflowInstanceId",
                table: "WorkflowInstanceCurrentNodes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "WorkflowDefinitionId",
                table: "TaskHandlerReferences",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "WorkflowDefinitionId",
                table: "NodeExecutions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkflowInstanceCurrentNodes",
                table: "WorkflowInstanceCurrentNodes",
                columns: new[] { "WorkflowInstanceId", "NodeId", "WorkflowDefinitionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Nodes",
                table: "Nodes",
                columns: new[] { "NodeId", "WorkflowDefinitionId" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowInstanceCurrentNodes_NodeId_WorkflowDefinitionId",
                table: "WorkflowInstanceCurrentNodes",
                columns: new[] { "NodeId", "WorkflowDefinitionId" });

            migrationBuilder.CreateIndex(
                name: "IX_TaskHandlerReferences_NodeId_WorkflowDefinitionId",
                table: "TaskHandlerReferences",
                columns: new[] { "NodeId", "WorkflowDefinitionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NodeExecutions_NodeId_WorkflowDefinitionId",
                table: "NodeExecutions",
                columns: new[] { "NodeId", "WorkflowDefinitionId" });

            migrationBuilder.CreateIndex(
                name: "IX_Connections_SourceNodeId_WorkflowDefinitionId",
                table: "Connections",
                columns: new[] { "SourceNodeId", "WorkflowDefinitionId" });

            migrationBuilder.CreateIndex(
                name: "IX_Connections_TargetNodeId_WorkflowDefinitionId",
                table: "Connections",
                columns: new[] { "TargetNodeId", "WorkflowDefinitionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_Nodes_SourceNodeId_WorkflowDefinitionId",
                table: "Connections",
                columns: new[] { "SourceNodeId", "WorkflowDefinitionId" },
                principalTable: "Nodes",
                principalColumns: new[] { "NodeId", "WorkflowDefinitionId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_Nodes_TargetNodeId_WorkflowDefinitionId",
                table: "Connections",
                columns: new[] { "TargetNodeId", "WorkflowDefinitionId" },
                principalTable: "Nodes",
                principalColumns: new[] { "NodeId", "WorkflowDefinitionId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NodeExecutions_Nodes_NodeId_WorkflowDefinitionId",
                table: "NodeExecutions",
                columns: new[] { "NodeId", "WorkflowDefinitionId" },
                principalTable: "Nodes",
                principalColumns: new[] { "NodeId", "WorkflowDefinitionId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskHandlerReferences_Nodes_NodeId_WorkflowDefinitionId",
                table: "TaskHandlerReferences",
                columns: new[] { "NodeId", "WorkflowDefinitionId" },
                principalTable: "Nodes",
                principalColumns: new[] { "NodeId", "WorkflowDefinitionId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowInstanceCurrentNodes_Nodes_NodeId_WorkflowDefinitionId",
                table: "WorkflowInstanceCurrentNodes",
                columns: new[] { "NodeId", "WorkflowDefinitionId" },
                principalTable: "Nodes",
                principalColumns: new[] { "NodeId", "WorkflowDefinitionId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowInstanceCurrentNodes_WorkflowInstances_WorkflowInstanceId",
                table: "WorkflowInstanceCurrentNodes",
                column: "WorkflowInstanceId",
                principalTable: "WorkflowInstances",
                principalColumn: "InstanceId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Connections_Nodes_SourceNodeId_WorkflowDefinitionId",
                table: "Connections");

            migrationBuilder.DropForeignKey(
                name: "FK_Connections_Nodes_TargetNodeId_WorkflowDefinitionId",
                table: "Connections");

            migrationBuilder.DropForeignKey(
                name: "FK_NodeExecutions_Nodes_NodeId_WorkflowDefinitionId",
                table: "NodeExecutions");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskHandlerReferences_Nodes_NodeId_WorkflowDefinitionId",
                table: "TaskHandlerReferences");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowInstanceCurrentNodes_Nodes_NodeId_WorkflowDefinitionId",
                table: "WorkflowInstanceCurrentNodes");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowInstanceCurrentNodes_WorkflowInstances_WorkflowInstanceId",
                table: "WorkflowInstanceCurrentNodes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkflowInstanceCurrentNodes",
                table: "WorkflowInstanceCurrentNodes");

            migrationBuilder.DropIndex(
                name: "IX_WorkflowInstanceCurrentNodes_NodeId_WorkflowDefinitionId",
                table: "WorkflowInstanceCurrentNodes");

            migrationBuilder.DropIndex(
                name: "IX_TaskHandlerReferences_NodeId_WorkflowDefinitionId",
                table: "TaskHandlerReferences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Nodes",
                table: "Nodes");

            migrationBuilder.DropIndex(
                name: "IX_NodeExecutions_NodeId_WorkflowDefinitionId",
                table: "NodeExecutions");

            migrationBuilder.DropIndex(
                name: "IX_Connections_SourceNodeId_WorkflowDefinitionId",
                table: "Connections");

            migrationBuilder.DropIndex(
                name: "IX_Connections_TargetNodeId_WorkflowDefinitionId",
                table: "Connections");

            migrationBuilder.DropColumn(
                name: "WorkflowInstanceId",
                table: "WorkflowInstanceCurrentNodes");

            migrationBuilder.DropColumn(
                name: "WorkflowDefinitionId",
                table: "TaskHandlerReferences");

            migrationBuilder.DropColumn(
                name: "WorkflowDefinitionId",
                table: "NodeExecutions");

            migrationBuilder.RenameColumn(
                name: "WorkflowDefinitionId",
                table: "WorkflowInstanceCurrentNodes",
                newName: "WorkflowInstanceInstanceId");

            migrationBuilder.RenameColumn(
                name: "NodeId",
                table: "WorkflowInstanceCurrentNodes",
                newName: "CurrentNodesNodeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkflowInstanceCurrentNodes",
                table: "WorkflowInstanceCurrentNodes",
                columns: new[] { "CurrentNodesNodeId", "WorkflowInstanceInstanceId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Nodes",
                table: "Nodes",
                column: "NodeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowInstanceCurrentNodes_WorkflowInstanceInstanceId",
                table: "WorkflowInstanceCurrentNodes",
                column: "WorkflowInstanceInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskHandlerReferences_NodeId",
                table: "TaskHandlerReferences",
                column: "NodeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NodeExecutions_NodeId",
                table: "NodeExecutions",
                column: "NodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_SourceNodeId",
                table: "Connections",
                column: "SourceNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_TargetNodeId",
                table: "Connections",
                column: "TargetNodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_Nodes_SourceNodeId",
                table: "Connections",
                column: "SourceNodeId",
                principalTable: "Nodes",
                principalColumn: "NodeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_Nodes_TargetNodeId",
                table: "Connections",
                column: "TargetNodeId",
                principalTable: "Nodes",
                principalColumn: "NodeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NodeExecutions_Nodes_NodeId",
                table: "NodeExecutions",
                column: "NodeId",
                principalTable: "Nodes",
                principalColumn: "NodeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskHandlerReferences_Nodes_NodeId",
                table: "TaskHandlerReferences",
                column: "NodeId",
                principalTable: "Nodes",
                principalColumn: "NodeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowInstanceCurrentNodes_Nodes_CurrentNodesNodeId",
                table: "WorkflowInstanceCurrentNodes",
                column: "CurrentNodesNodeId",
                principalTable: "Nodes",
                principalColumn: "NodeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowInstanceCurrentNodes_WorkflowInstances_WorkflowInstanceInstanceId",
                table: "WorkflowInstanceCurrentNodes",
                column: "WorkflowInstanceInstanceId",
                principalTable: "WorkflowInstances",
                principalColumn: "InstanceId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
