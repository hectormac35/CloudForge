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
