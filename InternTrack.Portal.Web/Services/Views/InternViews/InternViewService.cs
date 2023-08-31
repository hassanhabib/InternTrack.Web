// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System.Threading.Tasks;
using InternTrack.Portal.Web.Brokers.DateTimes;
using InternTrack.Portal.Web.Brokers.Loggings;
using InternTrack.Portal.Web.Models.InternViews;
using InternTrack.Portal.Web.Services.Foundations.Interns;
using InternTrack.Portal.Web.Services.Foundations.Users;

namespace InternTrack.Portal.Web.Services.Views.InternViews
{
    public partial class InternViewService : IInternViewService
    {
        private readonly IInternService internService;
        private readonly IUserService userService;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public InternViewService(
            IInternService internService,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker,
            IUserService userService)
        {
            this.internService = internService;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
            this.userService = userService;
        }

        public ValueTask<InternView> AddInternViewAsync(InternView internView)
        {
            throw new System.NotImplementedException();
        }
    }
}
