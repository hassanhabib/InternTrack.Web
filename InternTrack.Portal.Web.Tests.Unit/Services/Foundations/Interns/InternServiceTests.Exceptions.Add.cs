// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Collections;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using InternTrack.Portal.Web.Models.Interns;
using InternTrack.Portal.Web.Models.Interns.Exceptions;
using Microsoft.AspNetCore.Http;
using Moq;
using RESTFulSense.Exceptions;
using Xunit;

namespace InternTrack.Portal.Web.Tests.Unit.Services.Foundations.Interns
{
    public partial class InternServiceTests
    {
        [Theory]
        [MemberData(nameof(CriticalDependencyException))]
        private async Task ShouldThrowCriticalDependencyExceptionOnAddIfCriticalErrorOccursAndLogItAsync(
            Exception criticalDependencyException)
        {
            //given
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
                broker.PostInternAsync(It.IsAny<Intern>()))
                    .ThrowsAsync(criticalDependencyException);

            //when
            ValueTask<Intern> addInternTask =
                this.internService.AddInternAsync(someIntern);

            InternDependencyException actualInternDependencyException =
                await Assert.ThrowsAsync<InternDependencyException>(
                    addInternTask.AsTask);

            //then
            actualInternDependencyException.Should()
                .BeEquivalentTo(expectedInternDependencyException);

            this.apiBrokerMock.Verify(broker =>
                broker.PostInternAsync(It.IsAny<Intern>()),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameAsExceptionAs(
                    expectedInternDependencyException))),
                        Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyValidationExceptionOnAddIfBadRequestExceptionOccursAndLogItAsync()
        {
            //given
            Intern someIntern = CreateRandomIntern();
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
                broker.PostInternAsync(It.IsAny<Intern>()))
                    .ThrowsAsync(httpResponseBadRequestException);

            //when
            ValueTask<Intern> addInternTask =
                this.internService.AddInternAsync(someIntern);

            InternDependencyValidationException actualInternDependencyValidationException =
                await Assert.ThrowsAsync<InternDependencyValidationException>(
                    addInternTask.AsTask);

            //then
            actualInternDependencyValidationException.Should()
                .BeEquivalentTo(expectedInternDependencyValidationException);

            this.apiBrokerMock.Verify(broker =>
                broker.PostInternAsync(It.IsAny<Intern>()),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameAsExceptionAs(
                    expectedInternDependencyValidationException))),
                        Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyValidationExceptionOnAddIfConflictExceptionOccursAndLogItAsync()
        {
            //given
            Intern someIntern = CreateRandomIntern();
            IDictionary randomDictionary = CreateRandomDictionary();
            IDictionary exceptionData = randomDictionary;
            string someMessage = GetRandomMessage();
            var someResponseMessage = new HttpResponseMessage();

            var httpResponseConflictException =
                new HttpResponseConflictException(
                    someResponseMessage,
                        someMessage);

            httpResponseConflictException.AddData(exceptionData);

            var invalidInternException =
                new InvalidInternException(
                    message: "Invalid Intern error occurred. Please correct the errors and try again.",
                        innerException: httpResponseConflictException,
                            data: exceptionData);

            var expectedInternDependencyValidationException =
                new InternDependencyValidationException(
                    message: "Intern dependency validation error occurred, please try again.",
                        innerException: invalidInternException);

            this.apiBrokerMock.Setup(broker =>
                broker.PostInternAsync(It.IsAny<Intern>()))
                    .ThrowsAsync(httpResponseConflictException);

            //when
            ValueTask<Intern> addInternTask =
                this.internService.AddInternAsync(someIntern);

            InternDependencyValidationException actualInternDependencyValidationException =
                await Assert.ThrowsAsync<InternDependencyValidationException>(
                    addInternTask.AsTask);

            //then
            actualInternDependencyValidationException.Should()
                .BeEquivalentTo(expectedInternDependencyValidationException);

            this.apiBrokerMock.Verify(broker =>
                broker.PostInternAsync(It.IsAny<Intern>()),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameAsExceptionAs(
                    expectedInternDependencyValidationException))),
                        Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowInternDependencyExceptionOnAddIfResponseExceptionOccursAndLogItAsync()
        {
            //given
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
                broker.PostInternAsync(It.IsAny<Intern>()))
                    .ThrowsAsync(httpResponseException);

            //when
            ValueTask<Intern> addInternTask =
                this.internService.AddInternAsync(someIntern);

            InternDependencyException actualInternDependencyException =
                await Assert.ThrowsAsync<InternDependencyException>(() =>
                    addInternTask.AsTask());

            //then
            actualInternDependencyException.Should()
                .BeEquivalentTo(expectedInternDependencyException);

            this.apiBrokerMock.Verify(broker =>
                broker.PostInternAsync(It.IsAny<Intern>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameAsExceptionAs(
                    expectedInternDependencyException))),
                        Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
