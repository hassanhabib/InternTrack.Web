// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System.Threading.Tasks;
using InternTrack.Portal.Web.Infrastructure.Provision.Services.Processings.CloudManagement;

namespace InternTrack.Portal.Web.Infrastructure.Provision
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ICloudManagementProcessingService cloudManagementProcessingService =
                new CloudManagementProcessingService();

            await cloudManagementProcessingService.ProcessAsync();
        }
    }
}