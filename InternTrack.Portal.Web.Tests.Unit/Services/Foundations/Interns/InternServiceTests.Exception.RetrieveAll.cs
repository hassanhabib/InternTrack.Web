// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternTrack.Portal.Web.Models.Interns;
using InternTrack.Portal.Web.Models.Interns.Exceptions;
using Moq;
using Xunit;

namespace InternTrack.Portal.Web.Tests.Unit.Services.Foundations.Interns
{
    public partial class InternServiceTests
    {
        [Theory]
        [MemberData(nameof(CriticalDependencyException))]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveAllIfDependencyApiErrorOccursAndLogItAsync(
            Exception criticalDependencyException)
        {
            // given
            var failedInternDependencyException =
                new FailedInternDependencyException(criticalDependencyException);

            var expectedInternDependencyException =
                new InternDependencyException(
                    failedInternDependencyException);

            this.apiBrokerMock.Setup(broker =>
                broker.GetAllInternsAsync())
                    .ThrowsAsync(criticalDependencyException);

            // when
            ValueTask<List<Intern>> retrieveAllInternsTask =
                this.internService.RetrieveAllInternsAsync();

            // then
            await Assert.ThrowsAsync<InternDependencyException>(() =>
                retrieveAllInternsTask.AsTask());

            this.apiBrokerMock.Verify(broker =>
                broker.GetAllInternsAsync(), 
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedInternDependencyException))),
                        Times.Once());

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
