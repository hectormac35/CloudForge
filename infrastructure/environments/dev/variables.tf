variable "location" {
  description = "Región de Azure donde se crearán los recursos."
  type        = string
}

variable "resource_group_name" {
  description = "Nombre del Resource Group del entorno."
  type        = string
}

variable "storage_account_name" {
  description = "Nombre único de la cuenta de almacenamiento."
  type        = string
}

variable "log_analytics_workspace_name" {
  description = "Nombre del Log Analytics Workspace."
  type        = string
}

variable "application_insights_name" {
  description = "Nombre de Application Insights."
  type        = string
}

variable "log_retention_days" {
  description = "Días de retención de logs."
  type        = number
  default     = 30
}

variable "environment" {
  description = "Nombre corto del entorno (se usa en nombres y etiquetas)."
  type        = string
  default     = "dev"
}

variable "enable_app_service" {
  description = "Crea el App Service con la API en contenedor. Desactivado por defecto para no generar costes."
  type        = bool
  default     = false
}

variable "web_app_name" {
  description = "Nombre global único del App Service (solo si enable_app_service = true)."
  type        = string
  default     = "app-cloudforge-dev"
}

variable "container_image" {
  description = "Imagen de la API en GHCR, sin el registro."
  type        = string
  default     = "hectormac35/cloudforge-api:latest"
}
