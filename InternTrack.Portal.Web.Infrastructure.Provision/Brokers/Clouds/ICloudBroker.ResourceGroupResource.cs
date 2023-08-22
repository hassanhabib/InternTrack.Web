// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Azure.ResourceManager.Resources;
using System.Threading.Tasks;

namespace InternTrack.Portal.Web.Infrastructure.Provision.Brokers.Clouds
{
    public partial interface ICloudBroker
    {
        ValueTask<ResourceGroupResource> CreateResourceGroupAsync(string resourceGroupName);
        ValueTask DeleteResourceGroupAsync(string resourceGroupName);
        ValueTask<bool> CheckResourceGroupExistAsync(string resourceGroupName);
    }
}
