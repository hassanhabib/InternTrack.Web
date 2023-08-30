// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using InternTrack.Portal.Web.Models.Interns;
using Moq;
using Xunit;

namespace InternTrack.Portal.Web.Tests.Unit.Services.Foundations.Interns
{
    public partial class InternServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveAllInternsAsync()
        {
            // given
            List<Intern> randomInterns = CreateRandomInterns();
            List<Intern> apiInterns = randomInterns;
            List<Intern> expectedInterns = apiInterns.DeepClone();

            this.apiBrokerMock.Setup(broker =>
                broker.GetAllInternsAsync())
                    .ReturnsAsync(apiInterns);

            // when
            List<Intern> retrievedInterns =
                await internService.RetrieveAllInternsAsync();

            // then
            retrievedInterns.Should().BeEquivalentTo(expectedInterns);

            this.apiBrokerMock.Verify(broker =>
                broker.GetAllInternsAsync(),
                    Times.Once());

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
