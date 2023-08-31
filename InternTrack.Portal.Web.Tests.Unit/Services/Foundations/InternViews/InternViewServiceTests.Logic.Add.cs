// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using FluentAssertions;
using InternTrack.Portal.Web.Models.Interns;
using InternTrack.Portal.Web.Models.InternViews;
using Moq;
using Xunit;

namespace InternTrack.Portal.Web.Tests.Unit.Services.Foundations.InternViews
{
    public partial class InternViewServiceTests
    {
        [Fact]
        private async Task ShouldAddInternViewAsync()
        {
            //given
            Guid randomUserId = Guid.NewGuid();
            DateTimeOffset randomDatTime = GetRandomDate();

            dynamic randomInternViewProperties =
                CreateRandomInternViewProperties(
                    auditDates: randomDatTime,
                        auditIds: randomUserId);

            var randomInternView = new InternView
            {
                FirstName = randomInternViewProperties.FirstName,
                MiddleName = randomInternViewProperties.MiddleName,
                LastName = randomInternViewProperties.LastName,
                Email = randomInternViewProperties.Email,
                PhoneNumber = randomInternViewProperties.PhoneNumber,
                Status = randomInternViewProperties.Status,
                JoinDate = randomInternViewProperties.JoinDate
            };

            InternView inputInternView = randomInternView;
            InternView expectedInternView = inputInternView;

            var randomIntern = new Intern
            {
                Id = randomInternViewProperties.Id,
                FirstName = randomInternViewProperties.FirstName,
                MiddleName = randomInternViewProperties.MiddleName,
                LastName = randomInternViewProperties.LastName,
                Email = randomInternViewProperties.Email,
                PhoneNumber = randomInternViewProperties.PhoneNumber,
                Status = randomInternViewProperties.Status,
                CreatedDate = randomDatTime,
                UpdatedDate = randomDatTime,
                JoinDate = randomDatTime,
                CreatedBy = randomUserId,
                UpdatedBy = randomUserId
            };

            Intern expectedInputIntern = randomIntern;
            Intern returnIntern = expectedInputIntern;

            this.userServiceMock.Setup(service =>
                service.GetCurrentlyLoggedInUser())
                    .Returns(randomUserId);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDatTime);

            this.internServiceMock.Setup(service =>
                service.AddInternAsync(It.Is(
                    SameInternAs(expectedInputIntern))))
                        .ReturnsAsync(returnIntern);

            //when
            InternView actualInternView =
                await this.internViewService
                    .AddInternViewAsync(inputInternView);

            //then
            actualInternView.Should().BeEquivalentTo(expectedInternView);

            this.userServiceMock.Verify(service =>
                service.GetCurrentlyLoggedInUser(),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.internServiceMock.Verify(service =>
                service.AddInternAsync(It.Is(
                    SameInternAs(expectedInputIntern))), 
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userServiceMock.VerifyNoOtherCalls();
            this.internServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
