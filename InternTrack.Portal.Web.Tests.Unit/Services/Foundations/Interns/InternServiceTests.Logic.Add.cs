// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;
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
        private async Task ShouldAddInternAsync()
        {
            //given
            Intern randomIntern = CreateRandomIntern();
            Intern inputIntern = randomIntern;
            Intern retrievedIntern = inputIntern;
            Intern expectedIntern = retrievedIntern;

            this.apiBrokerMock.Setup(broker =>
                broker.PostInternAsync(inputIntern))
                    .ReturnsAsync(retrievedIntern);

            //when
            Intern actualIntern =
                await this.internService.AddInternAsync(inputIntern);

            //then
            actualIntern.Should().BeEquivalentTo(expectedIntern);

            this.apiBrokerMock.Verify(broker =>
                broker.PostInternAsync(inputIntern),
                    Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }        
    }
}
