variable "resource_group_name" {
  description = "Nombre del Resource Group donde se crearán los recursos de monitorización."
  type        = string
}

variable "location" {
  description = "Región de Azure."
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

variable "environment" {
  description = "Entorno de despliegue."
  type        = string
  default     = "dev"
}

variable "log_retention_days" {
  description = "Número de días de retención de logs."
  type        = number
  default     = 30

  validation {
    condition     = var.log_retention_days >= 30 && var.log_retention_days <= 730
    error_message = "La retención debe estar entre 30 y 730 días."
  }
}
