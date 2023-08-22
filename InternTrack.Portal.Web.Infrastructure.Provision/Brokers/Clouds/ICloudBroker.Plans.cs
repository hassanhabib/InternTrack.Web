// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System.Threading.Tasks;
using Azure.ResourceManager.AppService;
using Azure.ResourceManager.Resources;

namespace InternTrack.Portal.Web.Infrastructure.Provision.Brokers.Clouds
{
    public partial interface ICloudBroker
    {
        ValueTask<AppServicePlanResource> CreatePlanAsync(
            string planName,
            ResourceGroupResource resourceGroup);
    }
}
