output "log_analytics_workspace_id" {
  description = "ID del Log Analytics Workspace."
  value       = azurerm_log_analytics_workspace.this.id
}

output "log_analytics_workspace_name" {
  description = "Nombre del Log Analytics Workspace."
  value       = azurerm_log_analytics_workspace.this.name
}

output "application_insights_id" {
  description = "ID de Application Insights."
  value       = azurerm_application_insights.this.id
}

output "application_insights_name" {
  description = "Nombre de Application Insights."
  value       = azurerm_application_insights.this.name
}

output "application_insights_connection_string" {
  description = "Cadena de conexión de Application Insights."
  value       = azurerm_application_insights.this.connection_string
  sensitive   = true
}

output "application_insights_instrumentation_key" {
  description = "Instrumentation Key de Application Insights."
  value       = azurerm_application_insights.this.instrumentation_key
  sensitive   = true
}
