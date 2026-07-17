module "resource_group" {
  source = "../../modules/resource-group"

  resource_group_name = var.resource_group_name
  location            = var.location
}

module "storage" {
  source = "../../modules/storage"

  storage_account_name = var.storage_account_name
  resource_group_name  = module.resource_group.resource_group_name
  location             = var.location
}
module "monitoring" {
  source = "../../modules/monitoring"

  resource_group_name          = module.resource_group.resource_group_name
  location                     = var.location
  log_analytics_workspace_name = var.log_analytics_workspace_name
  application_insights_name    = var.application_insights_name
  log_retention_days           = var.log_retention_days
  environment                  = "dev"
}
