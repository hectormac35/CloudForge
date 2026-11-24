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
  environment          = var.environment
}

module "monitoring" {
  source = "../../modules/monitoring"

  resource_group_name          = module.resource_group.resource_group_name
  location                     = var.location
  log_analytics_workspace_name = var.log_analytics_workspace_name
  application_insights_name    = var.application_insights_name
  log_retention_days           = var.log_retention_days
  environment                  = var.environment
}

# La API solo se despliega si se activa expresamente (enable_app_service = true)
module "app_service" {
  source = "../../modules/app-service"
  count  = var.enable_app_service ? 1 : 0

  resource_group_name                    = module.resource_group.resource_group_name
  location                               = var.location
  service_plan_name                      = "asp-cloudforge-${var.environment}"
  web_app_name                           = var.web_app_name
  container_image                        = var.container_image
  application_insights_connection_string = module.monitoring.application_insights_connection_string
  environment                            = var.environment
}
