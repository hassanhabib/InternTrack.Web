// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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
        private async Task ShouldThrowCriticalDependencyExceptionOnModifyIfCriticalErrorOccursAndLogItAsync(
            Exception criticalDependencyException)
        {
            // given
            Intern someIntern = CreateRandomIntern();

            var failedInternDependencyException =
                new FailedInternDependencyException(
                    message: "Failed Intern dependency error occurred, contact support.",
                        innerException: criticalDependencyException);

            var expectedInternDependencyException =
                new InternDependencyException(
                    message: "Intern dependency error occurred, contact support.",
                        innerException: failedInternDependencyException);
            
            this.apiBrokerMock.Setup(broker =>
                broker.PutInternAsync(It.IsAny<Intern>()))
                    .ThrowsAsync(criticalDependencyException);
            
            // when
            ValueTask<Intern> modifyInternTask =
                this.internService.ModifyInternAsync(someIntern);

            InternDependencyException actualInternDependencyException =
                await Assert.ThrowsAsync<InternDependencyException>(
                    modifyInternTask.AsTask);

            // then
            actualInternDependencyException.Should().BeEquivalentTo(
                expectedInternDependencyException);

            this.apiBrokerMock.Verify(broker =>
                broker.PutInternAsync(It.IsAny<Intern>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedInternDependencyException))),
                        Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyValidationExceptionOnModifyIfBadRequestExceptionOccursAndLogItAsync()
        {
            // given
            Intern someIntern = CreateRandomIntern();
            IDictionary randomDictionary = CreateRandomDictionary();
            IDictionary exceptionData = randomDictionary;
            string someMessage = GetRandomMessage();
            var someRepsonseMessage = new HttpResponseMessage();

            var httpResponseBadRequestException =
                new HttpResponseBadRequestException(
                    someRepsonseMessage,
                        someMessage);

            httpResponseBadRequestException.AddData(exceptionData);

            var invalidInternException =
                new InvalidInternException(
                    message:"Invalid Intern error occurred. Please correct the errors and try again.",
                        innerException: httpResponseBadRequestException, 
                            exceptionData);

            var expectedInternDependencyValidationException =
                new InternDependencyValidationException(
                    message: "Intern dependency validation error occurred, please try again.",
                        innerException: invalidInternException);
            
            this.apiBrokerMock.Setup(broker =>
                broker.PutInternAsync(It.IsAny<Intern>()))
                    .ThrowsAsync(httpResponseBadRequestException);

            // when
            ValueTask<Intern> modifyInternTask =
                this.internService.ModifyInternAsync(someIntern);

            InternDependencyValidationException actualInternDependencyValidationException =
                await Assert.ThrowsAsync<InternDependencyValidationException>(
                    modifyInternTask.AsTask);

            // then
            actualInternDependencyValidationException.Should().BeEquivalentTo(
                expectedInternDependencyValidationException);

            this.apiBrokerMock.Verify(broker =>
                broker.PutInternAsync(It.IsAny<Intern>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedInternDependencyValidationException))),
                        Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyValidationExceptionOnModifyIfConflictExceptionOccursAndLogItAsync()
        {
            // given
            Intern someIntern = CreateRandomIntern();
            IDictionary randomDictionary = CreateRandomDictionary();
            IDictionary exceptionData = randomDictionary;
            string someMessage = GetRandomMessage();
            var someRepsonseMessage = new HttpResponseMessage();

            var httpResponseConflictException =
                new HttpResponseConflictException(
                    someRepsonseMessage,
                    someMessage);

            httpResponseConflictException.AddData(exceptionData);

            var invalidInternException =
                new InvalidInternException(message: "Invalid Intern error occurred. Please correct the errors and try again.",
                  innerException: httpResponseConflictException, 
                    exceptionData);

            var expectedInternDependencyValidationException =
                new InternDependencyValidationException(message: "Intern dependency validation error occurred, please try again.",
                  innerException: invalidInternException);
            
            this.apiBrokerMock.Setup(broker =>
                broker.PutInternAsync(It.IsAny<Intern>()))
                    .ThrowsAsync(httpResponseConflictException);

            // when
            ValueTask<Intern> modifyInternTask =
                this.internService.ModifyInternAsync(someIntern);

            InternDependencyValidationException actualInternDependencyValidationException =
                await Assert.ThrowsAsync<InternDependencyValidationException>(
                    modifyInternTask.AsTask);

            // then
            actualInternDependencyValidationException.Should().BeEquivalentTo(
                expectedInternDependencyValidationException);

            this.apiBrokerMock.Verify(broker =>
                broker.PutInternAsync(It.IsAny<Intern>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedInternDependencyValidationException))),
                        Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowInternDependencyExceptionOnModifyIfResponseExceptionOccursAndLogItAsync()
        {
            // given
            Intern someIntern = CreateRandomIntern();
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
                broker.PutInternAsync(It.IsAny<Intern>()))
                    .ThrowsAsync(httpResponseException);

            // when
            ValueTask<Intern> modifyInternTask =
                this.internService.ModifyInternAsync(someIntern);

            InternDependencyException actualInternDependencyException =
                await Assert.ThrowsAsync<InternDependencyException>(
                    modifyInternTask.AsTask);

            // then
            actualInternDependencyException.Should().BeEquivalentTo(
                expectedInternDependencyException);

            this.apiBrokerMock.Verify(broker =>
                broker.PutInternAsync(It.IsAny<Intern>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedInternDependencyException))),
                        Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Intern someIntern = CreateRandomIntern();
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
                broker.PutInternAsync(It.IsAny<Intern>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Intern> modifyInternTask =
                this.internService.ModifyInternAsync(someIntern);

            InternServiceException actualInternServiceException =
                await Assert.ThrowsAsync<InternServiceException>(
                    modifyInternTask.AsTask);

            // then
            actualInternServiceException.Should().BeEquivalentTo(
                expectedInternServiceException);

            this.apiBrokerMock.Verify(broker =>
                broker.PutInternAsync(It.IsAny<Intern>()),
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