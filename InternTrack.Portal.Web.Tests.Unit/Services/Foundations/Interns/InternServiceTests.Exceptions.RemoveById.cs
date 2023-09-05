// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------


using System.Threading.Tasks;
using System;
using Xunit;
using InternTrack.Portal.Web.Models.Interns.Exceptions;
using InternTrack.Portal.Web.Models.Interns;
using Moq;
using FluentAssertions;
using RESTFulSense.Exceptions;
using System.Net.Http;

namespace InternTrack.Portal.Web.Tests.Unit.Services.Foundations.Interns
{
    public partial class InternServiceTests
    {
        [Theory]
        [MemberData(nameof(CriticalDependencyException))]
        private async Task 
            ShouldThrowCriticalDependencyExceptionOnRemoveIfCriticalErrorOccursAndLogItAsync(
            Exception criticalDependencyException)
        {
            // given
            Guid someInternId = Guid.NewGuid();

            var failedInternDependencyException =
                new FailedInternDependencyException(
                    message: "Failed Intern dependency error occurred, contact support.",
                        innerException: criticalDependencyException);

            var expectedInternDependencyException =
                new InternDependencyException(
                    message: "Intern dependency error occurred, contact support.",
                        innerException: failedInternDependencyException);

            this.apiBrokerMock.Setup(broker =>
                broker.DeleteInternByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(criticalDependencyException);

            // when
            ValueTask<Intern> removeInternByIdTask =
                this.internService.RemoveInternByIdAsync(someInternId);

            InternDependencyException actualInternDependencyException =
                await Assert.ThrowsAsync<InternDependencyException>(
                    removeInternByIdTask.AsTask);

            //then
            actualInternDependencyException.Should()
                .BeEquivalentTo(expectedInternDependencyException);

            this.apiBrokerMock.Verify(broker =>
                broker.DeleteInternByIdAsync(It.IsAny<Guid>()),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedInternDependencyException))),
                        Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task
            ShouldThrowDependencyValidationExceptionOnRemoveIfInternIsNotFoundAndLogItAsync()
        {
            // given
            Guid someInternId = Guid.NewGuid();
            string responseMessage = GetRandomMessage();
            var httpResponseMessage = new HttpResponseMessage();

            var httpResponseNotFoundException =
                new HttpResponseNotFoundException(
                    httpResponseMessage,
                        responseMessage);

            var notFoundInternException =
                new NotFoundInternException(
                    message: "Not found Intern error occurred, please try again.",
                        innerException: httpResponseNotFoundException);

            var expectedInternDependencyValidationException =
                new InternDependencyValidationException(
                    message: "Intern dependency validation error occurred, please try again.",
                        innerException: notFoundInternException);

            this.apiBrokerMock.Setup(broker =>
                broker.DeleteInternByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(httpResponseNotFoundException);

            // when
            ValueTask<Intern> removeInternByIdTask =
                this.internService.RemoveInternByIdAsync(someInternId);

            InternDependencyValidationException actualInternDependencyValidationException =
                await Assert.ThrowsAsync<InternDependencyValidationException>(
                    removeInternByIdTask.AsTask);

            //then
            actualInternDependencyValidationException.Should()
                .BeEquivalentTo(expectedInternDependencyValidationException);

            this.apiBrokerMock.Verify(broker =>
                broker.DeleteInternByIdAsync(It.IsAny<Guid>()),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedInternDependencyValidationException))),
                        Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
