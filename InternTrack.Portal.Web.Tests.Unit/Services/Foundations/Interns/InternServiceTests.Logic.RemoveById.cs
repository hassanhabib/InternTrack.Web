// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
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
        private async void ShouldRemoveByIdAsync()
        {
            // given
            Guid randomInternId = Guid.NewGuid();
            Guid inputInternId = randomInternId;
            Intern randomIntern = CreateRandomIntern();
            Intern deletedIntern = randomIntern;
            Intern expectedIntern = deletedIntern.DeepClone();

            this.apiBrokerMock.Setup(broker =>
                broker.DeleteInternByIdAsync(inputInternId))
                    .ReturnsAsync(deletedIntern);

            // when
            Intern actualIntern =
                await this.internService
                    .RemoveInternByIdAsync(inputInternId);

            // then
            actualIntern.Should().BeEquivalentTo(expectedIntern);

            this.apiBrokerMock.Verify(broker =>
                broker.DeleteInternByIdAsync(inputInternId),
                    Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
