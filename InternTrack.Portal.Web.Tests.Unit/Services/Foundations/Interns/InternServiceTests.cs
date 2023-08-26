// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;
using InternTrack.Portal.Web.Brokers.Apis;
using InternTrack.Portal.Web.Brokers.Loggings;
using InternTrack.Portal.Web.Models.Interns;
using InternTrack.Portal.Web.Services.Foundations.Interns;
using Moq;
using Tynamix.ObjectFiller;

namespace InternTrack.Portal.Web.Tests.Unit.Services.Foundations.Interns
{
    public partial class InternServiceTests
    {
        private readonly Mock<IApiBroker> apiBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IInternService internService;

        public InternServiceTests()
        {
            this.apiBrokerMock = new Mock<IApiBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.internService = new InternService(
                apiBroker: this.apiBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static DateTimeOffset GetRandomDatetimeOffSet() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Intern CreateRandomIntern() => 
            CreateInternFiller().Create();

        private static Filler<Intern> CreateInternFiller()
        {
            var filler = new Filler<Intern>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(GetRandomDatetimeOffSet());

            return filler;
        }        
    }
}
