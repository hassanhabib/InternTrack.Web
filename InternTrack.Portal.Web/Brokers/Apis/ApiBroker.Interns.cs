// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InternTrack.Portal.Web.Models.Interns;

namespace InternTrack.Portal.Web.Brokers.Apis
{
    public partial class ApiBroker
    {
        private const string InternsRelativeUrl = "api/interns";

        public async ValueTask<Intern> PostInternAsync(Intern intern) =>
            await this.PostAsync(InternsRelativeUrl, intern);

        public async ValueTask<List<Intern>> GetAllInternsAsync() =>
            await this.GetAsync<List<Intern>>(InternsRelativeUrl);

        public async ValueTask<Intern> GetInternByIdAsync(Guid internId) =>
            await this.GetAsync<Intern>($"{InternsRelativeUrl}/{internId}");

        public async ValueTask<Intern> PutInternAsync(Intern intern) =>
            await this.PutAsync(InternsRelativeUrl, intern);

        public async ValueTask<Intern> DeleteInternByIdAsync(Guid internId) =>
            await this.DeleteAsync<Intern>($"{InternsRelativeUrl}/{internId}");
    }
}