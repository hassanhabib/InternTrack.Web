// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using InternTrack.Portal.Web.Models.Interns;
using Moq;
using Xunit;

namespace InternTrack.Portal.Web.Tests.Unit.Services.Foundations.Interns
{
    public partial class InternServiceTests
    {
        [Fact]
        private async Task ShouldRetrieveInternByIdAsync()
        {
            // given
            Intern randomIntern = CreateRandomIntern();
            Intern inputIntern = randomIntern;
            Intern retrieveIntern = inputIntern;
            Intern expectedIntern = retrieveIntern;

            this.apiBrokerMock.Setup(broker =>
                broker.GetInternByIdAsync(inputIntern.Id))
                    .ReturnsAsync(retrieveIntern);

            // when
            Intern actualIntern =
                await this.internService
                    .RetrieveInternByIdAsync(inputIntern.Id);

            // then
            actualIntern.Should().BeEquivalentTo(expectedIntern);

            this.apiBrokerMock.Verify(broker =>
                broker.GetInternByIdAsync(inputIntern.Id),
                    Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
