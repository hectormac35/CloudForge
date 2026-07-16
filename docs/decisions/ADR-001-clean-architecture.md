# ADR-001: Use a simplified Clean Architecture

## Status

Accepted

## Context

CloudForge needs a maintainable structure that separates business rules, application use cases, external infrastructure, and API concerns.

## Decision

The solution will use the following projects:

- CloudForge.Domain
- CloudForge.Application
- CloudForge.Infrastructure
- CloudForge.Api
- CloudForge.SharedKernel

The Domain project will remain independent from infrastructure and presentation concerns.

## Consequences

- Clear separation of responsibilities.
- Easier testing and replacement of infrastructure components.
- Slightly more initial structure than a single-project API.
