// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System.Threading.Tasks;
using System;
using Xunit;
using InternTrack.Portal.Web.Models.Interns.Exceptions;
using InternTrack.Portal.Web.Models.Interns;
using Moq;
using FluentAssertions;

namespace InternTrack.Portal.Web.Tests.Unit.Services.Foundations.Interns
{
    public partial class InternServiceTests
    {
        [Theory]
        [MemberData(nameof(CriticalDependencyException))]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfDependencyApiErrorOccursAndLogItAsync(
            Exception criticalDependencyException)
        {
            //given
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
                broker.GetInternByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(criticalDependencyException);

            //when
            var retrieveInternByIdTask =
                this.internService.RetrieveInternByIdAsync(someInternId);

            InternDependencyException actualInternDependencyException =
                await Assert.ThrowsAsync<InternDependencyException>(() =>
                    retrieveInternByIdTask.AsTask());

            //then
            actualInternDependencyException.Should()
                .BeEquivalentTo(expectedInternDependencyException);

            this.apiBrokerMock.Verify(broker =>
                broker.GetInternByIdAsync(It.IsAny<Guid>()),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedInternDependencyException))),
                        Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
