variable "storage_account_name" {
  description = "Nombre único de la cuenta de almacenamiento."
  type        = string
}

variable "resource_group_name" {
  description = "Nombre del Resource Group donde se creará la cuenta."
  type        = string
}

variable "location" {
  description = "Región de Azure."
  type        = string
}

variable "account_tier" {
  description = "Nivel de rendimiento de la cuenta de almacenamiento."
  type        = string
  default     = "Standard"
}

variable "account_replication_type" {
  description = "Tipo de replicación del almacenamiento."
  type        = string
  default     = "LRS"
}

variable "environment" {
  description = "Entorno de despliegue (se usa en las etiquetas)."
  type        = string
  default     = "dev"
}
