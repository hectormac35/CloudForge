# CloudForge

> Build. Deploy. Automate.

CloudForge is a DevOps portfolio project focused on provisioning Microsoft Azure infrastructure with Terraform and deploying a containerized ASP.NET Core API through GitHub Actions.

## Goals

- Provision Azure resources using Infrastructure as Code.
- Containerize an ASP.NET Core API with Docker.
- Automate build, test and deployment workflows.
- Add monitoring and centralized logging.
- Apply secure configuration and cost-control practices.

## Technology stack

- .NET 10 and ASP.NET Core
- Docker
- Terraform
- Microsoft Azure
- GitHub Actions
- Application Insights
- Log Analytics

## Architecture

CloudForge uses a simplified Clean Architecture:

- `CloudForge.Domain`
- `CloudForge.Application`
- `CloudForge.Infrastructure`
- `CloudForge.Api`
- `CloudForge.SharedKernel`

## Current status

Initial solution structure, project references and dependency security checks completed.
