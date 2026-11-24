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

variable "container_image" {
  description = "Imagen y etiqueta del contenedor, sin el registro (p. ej. hectormac35/cloudforge-api:latest)."
  type        = string
}

variable "container_registry_url" {
  description = "URL del registro de contenedores."
  type        = string
  default     = "https://ghcr.io"
}

variable "aspnetcore_environment" {
  description = "Valor de ASPNETCORE_ENVIRONMENT en la aplicación."
  type        = string
  default     = "Production"

  validation {
    condition     = contains(["Development", "Staging", "Production"], var.aspnetcore_environment)
    error_message = "aspnetcore_environment debe ser Development, Staging o Production."
  }
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
