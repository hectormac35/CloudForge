output "service_plan_id" {
  description = "ID del App Service Plan."
  value       = azurerm_service_plan.this.id
}

output "service_plan_name" {
  description = "Nombre del App Service Plan."
  value       = azurerm_service_plan.this.name
}

output "web_app_id" {
  description = "ID del Linux Web App."
  value       = azurerm_linux_web_app.this.id
}

output "web_app_name" {
  description = "Nombre del Linux Web App."
  value       = azurerm_linux_web_app.this.name
}

output "web_app_default_hostname" {
  description = "Hostname público de la aplicación."
  value       = azurerm_linux_web_app.this.default_hostname
}
