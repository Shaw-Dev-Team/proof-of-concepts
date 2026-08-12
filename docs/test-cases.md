# Test Cases: Workflow Visualization and Execution Platform POC

> Status: DRAFT
> Version: v1
> Last updated: 2026-08-12
> PRD: docs/prd.md (built against v5)
> PMBook: docs/project-management.md

Scenarios below are drawn from the PRD's Success Metrics and the PMBook's demo-scenario features (F-010/F-011/F-012). These are the E2E/smoke checks the Integration Agent runs once all features in a phase are Done.

## E2E Scenarios

| ID | Scenario | Steps | Expected Result | Validates |
|----|----------|-------|------------------|-----------|
| TC-001 | Author and version a workflow definition | 1. Create a new workflow definition with a simple node graph (Start → Task → End).<br>2. Save it.<br>3. Modify the graph and save again as a new version. | Both versions exist independently; the original version is unchanged; the definition list shows the correct current version. | PRD Success Metric "definitions can be versioned and reused independently from runtime instances"; F-002 |
| TC-002 | Customer Onboarding — Condition + Manual Approval | 1. Build/launch the Customer Onboarding demo workflow (Start → ValidateCustomerData → Condition → [Yes: CreateAccount → NotifyCustomer → End] / [No: HumanApproval → End]).<br>2. Run an instance that evaluates to each branch. | Instance follows the correct branch based on the condition outcome; Manual Approval (HumanApproval-style handler) node reaches Completed after invocation; runtime viewer shows the completed/skipped paths correctly. | PRD Success Metrics (execution simulation, node state/completed-skipped paths); F-004, F-007, F-010 |
| TC-003 | Invoice Processing — Parallel Split/Merge | 1. Build/launch the Invoice Processing demo workflow (Start → CreateInvoice → ParallelSplit → [SendInvoice, ValidateData] → ParallelMerge → Condition → End).<br>2. Run to completion. | Both parallel branches execute and both must complete before ParallelMerge advances; final state is Completed; execution history shows both branches. | PRD Success Metrics (execution simulation, state transitions/history); F-004, F-011 |
| TC-004 | Employee Offboarding — Wait/Pause | 1. Build/launch the Employee Offboarding demo workflow (Start → NotifySecurity → RevokeAccess → Wait(manual confirmation) → End).<br>2. Run until the Wait node, then supply the confirmation. | Instance correctly pauses at the Wait node (Paused/waiting state) and only advances after confirmation is supplied; End reached afterward. | PRD Success Metrics (runtime state transitions); Architecture Workflow Instance Lifecycle (Paused state); F-004, F-012 |
| TC-005 | Runtime visualization reflects live state | 1. Launch any instance.<br>2. Observe the runtime viewer while it progresses. | Current node is highlighted; completed nodes and skipped nodes are visually distinguished; status matches the node-lifecycle states (Pending/Ready/Running/Completed/Failed/Skipped). | PRD Success Metric "workflow visualization with node state and completed/skipped paths"; F-007 |
| TC-006 | Dashboard reflects instance/definition counts | 1. Create multiple definitions and launch multiple instances in varying end states (Completed, Failed).<br>2. Open the dashboard. | Counts by status are accurate; filtering the instance list by definition/status/time works; drilling into an instance shows its execution history. | Architecture FR-007 (dashboard detail); F-008 |
| TC-007 | Persona selection — unbuilt personas disabled | 1. Load the app.<br>2. View the persona-selection screen. | Workflow Designer is selectable; Process Owner and Operations Analyst are visibly present but disabled/greyed out and not selectable. | Architecture FR-001; F-005 |
| TC-008 | Task Handler abstraction is real, not hardcoded | 1. Run an instance containing a Task node targeting each of the mock handlers (SendEmail, CreateInvoice, ValidateRecord, HumanApproval-style stub).<br>2. Confirm each completes via the callback, not a direct return value. | Engine advances only after `ITaskCompletionCallback.NotifyCompletedAsync` is called by the handler; swapping which handler a Task node targets requires no engine code changes. | PRD Success Metric "separates workflow metadata from task execution logic"; ADR-004; F-003 |

## Regression-Sensitive Flows

| ID | Flow | Why it's regression-sensitive |
|----|------|-------------------------------|
| TC-R01 | Definition versioning after a running instance already exists on an older version | Must not corrupt or retroactively change an in-flight instance's behavior when its definition is later edited/versioned |
| TC-R02 | Node execution history ordering | `NodeExecution`/`ExecutionHistoryEntry` records must remain in correct chronological order as the engine advances, especially across parallel branches |

## Version History
| Version | Date | Change | Triggered By |
|---------|------|--------|---------------|
| v1 | 2026-08-12 | Initial draft, derived from PRD Success Metrics + PMBook demo scenarios F-010/F-011/F-012 | Orchestrator Phase 0 init |
