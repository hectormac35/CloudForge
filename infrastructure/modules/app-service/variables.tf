variable "resource_group_name" {
  description = "Nombre del Resource Group."
  type        = string
}

variable "location" {
  description = "Región de Azure."
  type        = string
}

variable "service_plan_name" {
  description = "Nombre del App Service Plan."
  type        = string
}

variable "web_app_name" {
  description = "Nombre único de la aplicación web."
  type        = string
}

variable "sku_name" {
  description = "SKU del App Service Plan."
  type        = string
  default     = "F1"
}

variable "dotnet_version" {
  description = "Versión de .NET usada por la aplicación."
  type        = string
  default     = "8.0"
}

variable "application_insights_connection_string" {
  description = "Cadena de conexión de Application Insights."
  type        = string
  sensitive   = true
}

variable "environment" {
  description = "Entorno de despliegue."
  type        = string
  default     = "dev"
}
