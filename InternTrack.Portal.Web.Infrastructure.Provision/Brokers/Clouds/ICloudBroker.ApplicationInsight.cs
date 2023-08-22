// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Azure.ResourceManager.ApplicationInsights;
using Azure.ResourceManager.Resources;
using System.Threading.Tasks;

namespace InternTrack.Portal.Web.Infrastructure.Provision.Brokers.Clouds
{
    public partial interface ICloudBroker
    {
        ValueTask<ApplicationInsightsComponentResource> CreateApplicationInsightComponentAsync(
            string componentName,
            ResourceGroupResource resourceGroup);
    }
}
