// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System.Threading.Tasks;
using Azure.Core;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager;
using Azure;

namespace InternTrack.Portal.Web.Infrastructure.Provision.Brokers.Clouds
{
    public partial class CloudBroker
    {
        public async ValueTask<ResourceGroupResource> CreateResourceGroupAsync(
            string resourceGroupName)
        {
            var resourceGroupData = new ResourceGroupData(
                AzureLocation.WestUS3);

            ArmOperation<ResourceGroupResource> operation = await client
                .GetDefaultSubscriptionAsync()
                .Result
                .GetResourceGroups()
                .CreateOrUpdateAsync(WaitUntil.Completed,
                    resourceGroupName,
                    resourceGroupData);

            return operation.Value;
        }

        public async ValueTask<bool> CheckResourceGroupExistAsync(
            string resourceGroupName)
        {
            return await client.GetDefaultSubscriptionAsync()
                .GetAwaiter()
                .GetResult()
                .GetResourceGroups()
                .ExistsAsync(resourceGroupName);
        }

        public async ValueTask DeleteResourceGroupAsync(string resourceGroupName)
        {
            await client.GetDefaultSubscriptionAsync()
                .GetAwaiter()
                .GetResult()
                .GetResourceGroup(resourceGroupName)
                .Value
                .DeleteAsync(WaitUntil.Completed);
        }
    }
}
