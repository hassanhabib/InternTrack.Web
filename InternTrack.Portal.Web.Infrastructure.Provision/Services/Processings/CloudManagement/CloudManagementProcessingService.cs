// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.ResourceManager.ApplicationInsights;
using Azure.ResourceManager.AppService;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Sql;
using InternTrack.Portal.Web.Infrastructure.Provision.Brokers.Configurations;
using InternTrack.Portal.Web.Infrastructure.Provision.Models.Configurations;
using InternTrack.Portal.Web.Infrastructure.Provision.Models.Storages;
using InternTrack.Portal.Web.Infrastructure.Provision.Services.Foundations.CloudManagements;

namespace InternTrack.Portal.Web.Infrastructure.Provision.Services.Processings.CloudManagement
{
    public class CloudManagementProcessingService : ICloudManagementProcessingService
    {
        private readonly ICloudManagementService cloudManagementService;
        private readonly IConfigurationBroker configurationBroker;

        public CloudManagementProcessingService()
        {
            this.cloudManagementService = new CloudManagementService();
            this.configurationBroker = new ConfigurationBroker();
        }

        public async ValueTask ProcessAsync()
        {
            CloudManagementConfiguration cloudManagementConfiguration =
                configurationBroker.GetConfigurations();

            await ProvisionAsync(
                projectName: cloudManagementConfiguration.ProjectName,
                cloudAction: cloudManagementConfiguration.Up);

            await DeprovisionAsync(
                projectName: cloudManagementConfiguration.ProjectName,
                cloudAction: cloudManagementConfiguration.Down);
        }

        private async ValueTask ProvisionAsync(
            string projectName,
            CloudActions cloudAction)
        {
            List<string> environments = RetrieveEnvironments(cloudAction);

            foreach (string environmentName in environments)
            {
                ResourceGroupResource resourceGroup = await cloudManagementService
                    .ProvisionResourceGroupAsync(
                        projectName,
                        environmentName);

                AppServicePlanResource appServicePlan = await cloudManagementService
                    .ProvisionPlanAsync(
                        projectName,
                        environmentName,
                        resourceGroup);

                SqlServerResource sqlServer = await cloudManagementService
                    .ProvisionSqlServerAsync(
                        projectName,
                        environmentName,
                        resourceGroup);

                SqlDatabase sqlDatabase = await cloudManagementService
                    .ProvisionSqlDatabaseAsync(
                        projectName,
                        environmentName,
                        sqlServer);

                WebSiteResource webApp = await cloudManagementService
                    .ProvisionWebAppAsync(
                        projectName,
                        environmentName,
                        sqlDatabase.ConnectionString,
                        resourceGroup,
                        appServicePlan);

                ApplicationInsightsComponentResource applicationInsight =
                    await cloudManagementService
                        .ProvisionApplicationInsightComponentAsync(
                            projectName,
                            environmentName,
                            resourceGroup);
            }
        }

        private async ValueTask DeprovisionAsync(
            string projectName,
            CloudActions cloudAction)
        {
            List<string> environments = RetrieveEnvironments(cloudAction);

            foreach (string environmentName in environments)
            {
                await cloudManagementService.DeprovisionResourceGroupAsync(
                    projectName,
                    environmentName);
            }
        }

        private static List<string> RetrieveEnvironments(CloudActions cloudAction) =>
            cloudAction?.Environments ?? new List<string>();
    }
}
