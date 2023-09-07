// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------


using System;
using System.Collections;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using InternTrack.Portal.Web.Models.Interns;
using InternTrack.Portal.Web.Models.Interns.Exceptions;
using Moq;
using RESTFulSense.Exceptions;
using Xunit;

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

        [Fact]
        private async Task
            ShouldThrowDependencyValidationExceptionOnRemoveIfValidationErrorOccursAndLogItAsync()
        {
            // given
            Guid someInternId = Guid.NewGuid();
            IDictionary randomDictionary = CreateRandomDictionary();
            IDictionary exceptionData = randomDictionary;
            string someMessage = GetRandomMessage();
            var someResponseMessage = new HttpResponseMessage();

            var httpResponseBadRequestException =
                new HttpResponseBadRequestException(
                    someResponseMessage,
                        someMessage);

            httpResponseBadRequestException.AddData(exceptionData);

            var invalidInternException =
                new InvalidInternException(
                    message: "Invalid Intern error occurred. Please correct the errors and try again.",
                        innerException: httpResponseBadRequestException,
                            data: exceptionData);

            var expectedInternDependencyValidationException =
                new InternDependencyValidationException(
                    message: "Intern dependency validation error occurred, please try again.",
                        innerException: invalidInternException);

            this.apiBrokerMock.Setup(broker =>
                broker.DeleteInternByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(httpResponseBadRequestException);

            //when
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

        [Fact]
        private async Task
            ShouldThrowDependencyValidationExceptionOnRemoveIfInternIsLockedAndLogItAsync()
        {
            // given
            Guid someInternId = Guid.NewGuid();
            string someMessage = GetRandomMessage();
            var httpResponseMessage = new HttpResponseMessage();

            var httpResponseLockedException =
                new HttpResponseLockedException(
                    httpResponseMessage,
                        someMessage);

            var lockedInternException =
                new LockedInternException(
                    message: "Locked Intern error occurred, please try again later.",
                        innerException: httpResponseLockedException);

            var expectedInternDependencyValidationException =
                new InternDependencyValidationException(
                    message: "Intern dependency validation error occurred, please try again.",
                        innerException: lockedInternException);

            this.apiBrokerMock.Setup(broker =>
                broker.DeleteInternByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(httpResponseLockedException);

            //when
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

        [Fact]
        private async Task
            ShouldThrowInternDependencyExceptionOnRemoveIfDependencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someInternId = Guid.NewGuid();
            string someMessage = GetRandomMessage();
            var httpResponseMessage = new HttpResponseMessage();

            var httpResponseException =
                new HttpResponseException(
                    httpResponseMessage,
                        someMessage);

            var failedInternDependencyException =
                new FailedInternDependencyException(
                    message: "Failed Intern dependency error occurred, contact support.",
                        innerException: httpResponseException);

            var expectedInternDependencyException =
                new InternDependencyException(
                    message: "Intern dependency error occurred, contact support.",
                        innerException: failedInternDependencyException);

            this.apiBrokerMock.Setup(broker =>
                broker.DeleteInternByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(httpResponseException);

            //when
            ValueTask<Intern> removeInternByIdTask =
                this.internService.RemoveInternByIdAsync(someInternId);

            InternDependencyException actualInternDependencyException =
                await Assert.ThrowsAsync<InternDependencyException>(() =>
                    removeInternByIdTask.AsTask());

            //then
            actualInternDependencyException.Should()
                .BeEquivalentTo(expectedInternDependencyException);

            this.apiBrokerMock.Verify(broker =>
                broker.DeleteInternByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedInternDependencyException))),
                        Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task
            ShouldThrowServiceExceptionOnRemoveIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someInternId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedInternServiceException =
                new FailedInternServiceException(
                    message: "Failed Intern service error occurred, contact support.",
                        innerException: serviceException);

            var expectedInternServiceException =
                new InternServiceException(
                    message: "Intern service error occurred, contact support.",
                        innerException: failedInternServiceException);

            this.apiBrokerMock.Setup(broker =>
                broker.DeleteInternByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            //when
            ValueTask<Intern> removeInternByIdTask =
                this.internService.RemoveInternByIdAsync(someInternId);

            InternServiceException actualInternServiceException =
                await Assert.ThrowsAsync<InternServiceException>(() =>
                    removeInternByIdTask.AsTask());

            //then
            actualInternServiceException.Should()
                .BeEquivalentTo(expectedInternServiceException);

            this.apiBrokerMock.Verify(broker =>
                broker.DeleteInternByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedInternServiceException))),
                        Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
