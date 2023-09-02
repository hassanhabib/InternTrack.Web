// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;
using System.Threading.Tasks;
using InternTrack.Portal.Web.Brokers.DateTimes;
using InternTrack.Portal.Web.Brokers.Loggings;
using InternTrack.Portal.Web.Models.Interns;
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

        public ValueTask<InternView> AddInternViewAsync(InternView internView) =>
            TryCatch(async () =>
            {
                Intern mappedIntern = MapToIntern(internView);

                await this.internService.AddInternAsync(mappedIntern);

                return internView;
            });

        public void NavigateTo(string route)
        {
            throw new NotImplementedException();
        }

        private Intern MapToIntern(InternView internView)
        {
            Guid currentLoggedInUserId = this.userService.GetCurrentlyLoggedInUser();
            DateTimeOffset currentTime = this.dateTimeBroker.GetCurrentDateTimeOffset();

            return new Intern
            {
                Id = Guid.NewGuid(),
                FirstName = internView.FirstName,
                MiddleName = internView.MiddleName,
                LastName = internView.LastName,
                Email = internView.Email,
                PhoneNumber = internView.PhoneNumber,
                Status = internView.Status,
                CreatedDate = currentTime,
                UpdatedDate = currentTime,
                JoinDate = currentTime,
                CreatedBy = currentLoggedInUserId,
                UpdatedBy = currentLoggedInUserId
            };
        }
    }
}
