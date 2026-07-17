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
