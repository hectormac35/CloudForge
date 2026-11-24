output "resource_group_name" {
  value = module.resource_group.resource_group_name
}

output "storage_account_name" {
  value = module.storage.storage_account_name
}

output "storage_blob_endpoint" {
  value = module.storage.primary_blob_endpoint
}

output "log_analytics_workspace_name" {
  value = module.monitoring.log_analytics_workspace_name
}

output "application_insights_name" {
  value = module.monitoring.application_insights_name
}

output "application_insights_connection_string" {
  value     = module.monitoring.application_insights_connection_string
  sensitive = true
}

output "web_app_url" {
  description = "URL pública de la API (null si enable_app_service = false)."
  value       = var.enable_app_service ? "https://${module.app_service[0].web_app_default_hostname}" : null
}
