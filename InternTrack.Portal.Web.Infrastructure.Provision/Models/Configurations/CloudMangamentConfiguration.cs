// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

namespace InternTrack.Portal.Web.Infrastructure.Provision.Models.Configurations
{
    public class CloudManagementConfiguration
    {
        public string ProjectName { get; set; }
        public CloudActions Up { get; set; }
        public CloudActions Down { get; set; }
    }
}
