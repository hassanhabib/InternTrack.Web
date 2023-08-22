// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using InternTrack.Portal.Web.Infrastructure.Provision.Models.Configurations;

namespace InternTrack.Portal.Web.Infrastructure.Provision.Brokers.Configurations
{
    public interface IConfigurationBroker
    {
        CloudManagementConfiguration GetConfigurations();
    }
}
