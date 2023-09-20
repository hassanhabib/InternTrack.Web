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
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfCriticalErrorOccursAndLogItAsync(
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

            // then
            await Assert.ThrowsAsync<InternDependencyException>(() =>
               modifyInternTask.AsTask());

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
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfBadRequestExceptionOccursAndLogItAsync()
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
                    httpResponseBadRequestException,
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

            // then
            await Assert.ThrowsAsync<InternDependencyValidationException>(() =>
                modifyInternTask.AsTask());

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

    }
}