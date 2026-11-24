resource "azurerm_service_plan" "this" {
  name                = var.service_plan_name
  resource_group_name = var.resource_group_name
  location            = var.location
  os_type             = "Linux"
  sku_name            = var.sku_name

  tags = local.tags
}

resource "azurerm_linux_web_app" "this" {
  name                = var.web_app_name
  resource_group_name = var.resource_group_name
  location            = var.location
  service_plan_id     = azurerm_service_plan.this.id
  https_only          = true

  site_config {
    # El plan F1 (gratuito) no admite always_on
    always_on         = var.sku_name != "F1"
    health_check_path = "/health"
    ftps_state        = "Disabled"

    application_stack {
      # Imagen publicada por el workflow publish-container.yml
      docker_image_name   = var.container_image
      docker_registry_url = var.container_registry_url
    }
  }

  app_settings = {
    ASPNETCORE_ENVIRONMENT                = var.aspnetcore_environment
    WEBSITES_PORT                         = "8080"
    APPLICATIONINSIGHTS_CONNECTION_STRING = var.application_insights_connection_string
  }

  tags = local.tags
}

locals {
  tags = {
    project     = "CloudForge"
    environment = var.environment
    managed_by  = "Terraform"
  }
}
