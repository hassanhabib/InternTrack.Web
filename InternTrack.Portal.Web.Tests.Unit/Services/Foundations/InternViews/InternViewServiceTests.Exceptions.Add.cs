// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using InternTrack.Portal.Web.Models.Interns;
using InternTrack.Portal.Web.Models.InternViews;
using InternTrack.Portal.Web.Models.InternViews.Exceptions;
using Moq;
using Xunit;

namespace InternTrack.Portal.Web.Tests.Unit.Services.Foundations.InternViews
{
    public partial class InternViewServiceTests
    {
        [Theory]
        [MemberData(nameof(InternServiceValidationExceptions))]
        private async Task
            ShouldThrowDependencyValidationExceptionOnAddIfInternValidationErrorOccurredAndLogItAsync(
                Exception internServiceValidationException)
        {
            // given
            InternView someInternView = CreateRandomInternView();

            var expectedDependencyValidationException =
                new InternViewDependencyValidationException(
                    message: "Intern View dependency validation error occurred, try again.",
                        innerException: internServiceValidationException);

            this.internServiceMock.Setup(service =>
                service.AddInternAsync(It.IsAny<Intern>()))
                    .ThrowsAsync(internServiceValidationException);

            // when
            ValueTask<InternView> addInternViewTask =
                this.internViewService.AddInternViewAsync(someInternView);

            InternViewDependencyValidationException actualDependencyValidationException =
                await Assert.ThrowsAsync<InternViewDependencyValidationException>(() =>
                    addInternViewTask.AsTask());

            // then
            actualDependencyValidationException.Should()
                .BeEquivalentTo(expectedDependencyValidationException);

            this.userServiceMock.Verify(service =>
                service.GetCurrentlyLoggedInUser(),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.internServiceMock.Verify(service =>
                service.AddInternAsync(It.IsAny<Intern>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDependencyValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.internServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InternServiceDependencyExceptions))]
        private async Task 
            ShouldThrowDependencyExceptionOnAddIfInternDependencyErrorOccurredAndLogItAsync(
            Exception internServiceDependencyException)
        {
            // given
            InternView someInternView = CreateRandomInternView();

            var expectedDependencyException =
                new InternViewDependencyException(
                    message: "Intern View dependency error occurred, contact support.",
                        innerException: internServiceDependencyException);

            this.internServiceMock.Setup(service =>
                service.AddInternAsync(It.IsAny<Intern>()))
                    .ThrowsAsync(internServiceDependencyException);

            // when
            ValueTask<InternView> addInternViewTask =
                this.internViewService.AddInternViewAsync(someInternView);

            InternViewDependencyException actualDependencyValidationException =
                await Assert.ThrowsAsync<InternViewDependencyException>(() =>
                    addInternViewTask.AsTask());

            // then
            actualDependencyValidationException.Should()
                .BeEquivalentTo(expectedDependencyException);

            this.userServiceMock.Verify(service =>
                service.GetCurrentlyLoggedInUser(),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.internServiceMock.Verify(service =>
                service.AddInternAsync(It.IsAny<Intern>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDependencyException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.internServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task 
            ShouldThrowServiceExceptionOnAddIfServiceErrorOccurredAndLogItAsync()
        {
            // given
            InternView someInternView = CreateRandomInternView();
            var serviceException = new Exception();

            var expectedInternServiceException =
                new InternViewServiceException(
                    message: "Intern View service error occurred, contact support.", 
                        innerException: serviceException);

            this.internServiceMock.Setup(service =>
                service.AddInternAsync(It.IsAny<Intern>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<InternView> addInternViewTask =
                this.internViewService.AddInternViewAsync(someInternView);

            InternViewServiceException actualInternServiceException =
                await Assert.ThrowsAsync<InternViewServiceException>(() =>
                    addInternViewTask.AsTask());

            // then
            actualInternServiceException.Should()
                .BeEquivalentTo(expectedInternServiceException);

            this.userServiceMock.Verify(service =>
                service.GetCurrentlyLoggedInUser(),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);
                        
            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedInternServiceException))),
                        Times.Once);

            this.internServiceMock.Verify(service =>
                service.AddInternAsync(It.IsAny<Intern>()),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.internServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async void ShouldThrowServiceExceptionOnNavigateIfServiceErrorOccursAndLogIt()
        {
            // given
            string someRoute = GetRandomRoute();
            Exception serviceException = new Exception();

            var expectedInternServiceException =
               new InternViewServiceException(
                   message: "Intern View service error occurred, contact support.",
                       innerException: serviceException);

            this.navigationBrokerMock.Setup(broker =>
                broker.NavigateTo(It.IsAny<string>()))
                    .Throws(serviceException);

            // when
            Action navigationToAction = () =>
                this.internViewService.NavigateTo(someRoute);

            InternViewServiceException actualInternServiceException =
                Assert.Throws<InternViewServiceException>(navigationToAction);

            //then
            actualInternServiceException.Should()
                .BeEquivalentTo(expectedInternServiceException);

            this.navigationBrokerMock.Verify(broker =>
                broker.NavigateTo(It.IsAny<string>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedInternServiceException))),
                        Times.Once);

            this.navigationBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.internServiceMock.VerifyNoOtherCalls();
        }
    }
}
