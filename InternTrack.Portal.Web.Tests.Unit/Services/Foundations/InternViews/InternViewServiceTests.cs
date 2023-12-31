﻿// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;
using System.Linq.Expressions;
using InternTrack.Portal.Web.Brokers.DateTimes;
using InternTrack.Portal.Web.Brokers.Loggings;
using InternTrack.Portal.Web.Brokers.Navigations;
using InternTrack.Portal.Web.Models.Interns;
using InternTrack.Portal.Web.Models.Interns.Exceptions;
using InternTrack.Portal.Web.Models.InternViews;
using InternTrack.Portal.Web.Services.Foundations.Interns;
using InternTrack.Portal.Web.Services.Foundations.Users;
using InternTrack.Portal.Web.Services.Views.InternViews;
using KellermanSoftware.CompareNetObjects;
using Moq;
using Tynamix.ObjectFiller;
using Xunit;

namespace InternTrack.Portal.Web.Tests.Unit.Services.Foundations.InternViews
{
    public partial class InternViewServiceTests
    {
        private readonly Mock<IInternService> internServiceMock;
        private readonly Mock<IUserService> userServiceMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<INavigationBroker> navigationBrokerMock;
        private readonly ICompareLogic compareLogic;
        private readonly IInternViewService internViewService;

        public InternViewServiceTests()
        {
            this.internServiceMock = new Mock<IInternService>();
            this.userServiceMock = new Mock<IUserService>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.navigationBrokerMock = new Mock<INavigationBroker>();
            var compareConfig = new ComparisonConfig();
            compareConfig.IgnoreProperty<Intern>(intern => intern.Id);
            this.compareLogic = new CompareLogic(compareConfig);

            this.internViewService = new InternViewService(
                internService: this.internServiceMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                userService: this.userServiceMock.Object,
                navigationBroker: this.navigationBrokerMock.Object);
        }

        public static TheoryData InternServiceValidationExceptions()
        {
            var innerException = new Exception();

            return new TheoryData<Exception>
            {
                new InternValidationException(
                    message: "Intern validation error occurred. Please, try again.",
                        innerException),

                new InternDependencyValidationException(
                    message: "Intern dependency validation error occurred, please try again.",
                        innerException)
            };
        }

        public static TheoryData InternServiceDependencyExceptions()
        {
            var innerException = new Exception();

            return new TheoryData<Exception>
            {
                new InternDependencyException(
                    message: "Intern dependency error occurred, contact support.",
                        innerException),

                new InternServiceException(
                    message: "Intern service error occurred, contact support.",
                        innerException)
            };
        }

        private static dynamic CreateRandomInternViewProperties(
            DateTimeOffset auditDates,
            Guid auditIds)
        {

            return new
            {
                Id = Guid.NewGuid(),
                FirstName = GetRandomName(),
                MiddleName = GetRandomName(),
                LastName = GetRandomName(),
                Email = GetRandomName(),
                PhoneNumber = GetRandomString(),
                Status = GetRandomString(),
                CreatedDate = auditDates,
                UpdatedDate = auditDates,
                JoinDate = auditDates,
                CreatedBy = auditIds,
                Updatedby = auditIds
            };
        }

        private Expression<Func<Intern, bool>> SameInternAs(
            Intern expectedIntern)
        {
            return actualIntern => this.compareLogic.Compare(
                expectedIntern, actualIntern).AreEqual;
        }

        private static string GetRandomName() =>
            new RealNames(NameStyle.FirstName).GetValue();

        private static DateTimeOffset GetRandomDate() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static string GetRandomRoute() =>
            new RandomUrl().GetValue();


        private static InternView CreateRandomInternView() =>
            CreateInternViewFiller().Create();

        private Expression<Func<Exception, bool>> SameExceptionAs(
            Exception expectedException)
        {
            return actualException => actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message;
        }

        private static Filler<InternView> CreateInternViewFiller()
        {
            var filler = new Filler<InternView>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(DateTimeOffset.UtcNow);

            return filler;
        }
    }
}
