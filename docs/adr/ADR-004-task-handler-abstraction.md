# ADR-004: Task Handler Abstraction — Invoke + Completion-Callback Interface

> Status: ACCEPTED
> Date: 2026-08-12
> Architecture: docs/architecture.md (v1)

## Context
The PRD's core validation goal is that workflow modeling/execution tracking must be decoupled from actual task implementation. The user asked for a "real abstraction" rather than an inline mock — specifically an interface with an invoke operation, plus a separate mechanism for the handler to notify the engine of completion (rather than assuming every task completes synchronously within the call that started it).

## Decision
Define two small interfaces that the Workflow Runtime Engine depends on and that all task handlers (mocked or, in the future, real) implement/consume:

- `ITaskHandler` — exposes `InvokeAsync(TaskInvocationContext context, ITaskCompletionCallback callback, CancellationToken cancellationToken)`. The engine calls this when a Task node executes; the handler owns however long its work takes.
- `ITaskCompletionCallback` — exposes `NotifyCompletedAsync(...)` and `NotifyFailedAsync(...)`. The handler calls this (immediately, for POC mocks; potentially much later, for a real async/human-approval handler) to tell the engine the outcome.

For the POC, mock handlers (e.g. SendEmail, CreateInvoice, ValidateRecord, HumanApproval-style stubs) call the callback synchronously/immediately after simulating work — but the engine never assumes that timing, so swapping in a real long-running handler later requires no engine changes.

## Alternatives Considered

| Option | Why not chosen |
|--------|-----------------|
| Single synchronous `Execute()` method returning a result directly | Cannot model long-running or human-approval-style tasks without the engine blocking or polling; doesn't satisfy "real abstraction" requirement |
| Event bus / message queue between engine and handlers | Correct direction for a future real integration, but adds infrastructure (broker, subscriptions) disproportionate to POC scope |
| Handler returns a `Task<TResult>` the engine awaits directly | Couples the engine's execution model to however long the handler takes to await, which breaks down for handlers that complete out-of-process (e.g. a human clicking "approve" hours later) |

## Consequences
- Gains: the engine and task handlers are genuinely decoupled — a new handler type is just a new `ITaskHandler` implementation, and the engine's node-advancement logic is identical whether completion is instant (mock) or delayed (future real handler).
- Tradeoff: slightly more scaffolding for a POC than a single mock function would need, but this is the explicit requirement — a "real abstraction," not just simulated behavior.

## Related
- Architecture section: §3 Components — Task Handler Layer; §4 Data Flow
- Supersedes: none
