// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using InternTrack.Portal.Web.Models.Interns;
using InternTrack.Portal.Web.Models.Interns.Exceptions;
using Moq;
using Xunit;

namespace InternTrack.Portal.Web.Tests.Unit.Services.Foundations.Interns
{
    public partial class InternServiceTests
    {
        [Fact]
        private async void ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidInternId = Guid.Empty;
            var innerException = new Exception();

            var invalidInternException = new InvalidInternException(
                message: "Invalid Intern error occurred. Please correct the errors and try again.",
                    innerException: innerException);

            invalidInternException.AddData(
                key: nameof(Intern.Id),
                values: "Id is required");

            var expectedInternValidationException =
                new InternValidationException(
                    message: "Intern validation error occurred. Please, try again.",
                        innerException: invalidInternException);

            // when
            ValueTask<Intern> addInternTask =
                this.internService.RemoveInternByIdAsync(invalidInternId);

            InternValidationException actualInternValidationException =
                await Assert.ThrowsAsync<InternValidationException>(
                    addInternTask.AsTask);

            // then
            actualInternValidationException.Should().BeEquivalentTo(
                expectedInternValidationException);

            this.apiBrokerMock.Verify(broker =>
                broker.DeleteInternByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedInternValidationException))),
                        Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
