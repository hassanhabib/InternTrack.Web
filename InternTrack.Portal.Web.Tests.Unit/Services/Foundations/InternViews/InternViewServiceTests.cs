// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using InternTrack.Portal.Web.Brokers.DateTimes;
using InternTrack.Portal.Web.Brokers.Loggings;
using InternTrack.Portal.Web.Services.Foundations.Interns;
using InternTrack.Portal.Web.Services.Foundations.Users;
using InternTrack.Portal.Web.Services.Views.InternViews;
using Moq;

namespace InternTrack.Portal.Web.Tests.Unit.Services.Foundations.InternViews
{
    public partial class InternViewServiceTests
    {
        private readonly Mock<IInternService> internServiceMock;
        private readonly Mock<IUserService> userServiceMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IInternViewService internViewService;

        public InternViewServiceTests()
        {
            this.internServiceMock = new Mock<IInternService>();
            this.userServiceMock = new Mock<IUserService>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.internViewService = new InternViewService(
                internService: this.internServiceMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                userService: this.userServiceMock.Object);
        }
    }
}
