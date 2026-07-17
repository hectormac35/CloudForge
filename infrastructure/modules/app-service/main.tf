resource "azurerm_service_plan" "this" {
  name                = var.service_plan_name
  resource_group_name = var.resource_group_name
  location            = var.location
  os_type             = "Linux"
  sku_name            = var.sku_name

  tags = {
    project     = "CloudForge"
    environment = var.environment
    managed_by  = "Terraform"
  }
}

resource "azurerm_linux_web_app" "this" {
  name                = var.web_app_name
  resource_group_name = var.resource_group_name
  location            = var.location
  service_plan_id     = azurerm_service_plan.this.id
  https_only          = true

  site_config {
    always_on = false

    application_stack {
      dotnet_version = var.dotnet_version
    }
  }

  app_settings = {
    ASPNETCORE_ENVIRONMENT                      = "Development"
    APPLICATIONINSIGHTS_CONNECTION_STRING       = var.application_insights_connection_string
    ApplicationInsightsAgent_EXTENSION_VERSION  = "~3"
  }

  tags = {
    project     = "CloudForge"
    environment = var.environment
    managed_by  = "Terraform"
  }
}
