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
        private async void ShouldThrowValidationExceptionOnAddWhenInternIsNullAndLogItAsync()
        {
            // given
            Intern nullIntern = null;
            var someException = new Exception();

            var nullInternException = new NullInternException(
                message: "The Intern is null.",
                    innerException: someException);

            var expectedInternValidationException =
                new InternValidationException(
                    message: "Intern validation error occurred. Please, try again.",
                        innerException: nullInternException);

            // when
            ValueTask<Intern> createInternTask =
                this.internService.AddInternAsync(nullIntern);

            var actualInternValidationException =
                await Assert.ThrowsAsync<InternValidationException>(() =>
                    createInternTask.AsTask());

            // then
            actualInternValidationException.Should()
                .BeEquivalentTo(expectedInternValidationException);

            this.apiBrokerMock.Verify(broker =>
                broker.PostInternAsync(It.IsAny<Intern>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedInternValidationException))),
                        Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        private async Task ShouldThrowValidationExceptionOnAddIfInternIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var innerException = new Exception();

            var invalidIntern = new Intern
            {
                FirstName = invalidText,
                MiddleName = invalidText,
                LastName = invalidText,
                Email = invalidText,
                PhoneNumber = invalidText,
                Status = invalidText,

            };

            var invalidInternException = new InvalidInternException(
                message: "Invalid Intern error occurred. Please correct the errors and try again.",
                    innerException: innerException);

            invalidInternException.AddData(
                key: nameof(Intern.Id),
                values: "Id is required");

            invalidInternException.AddData(
                key: nameof(Intern.FirstName),
                values: "Text is required");

            invalidInternException.AddData(
                key: nameof(Intern.MiddleName),
                values: "Text is required");

            invalidInternException.AddData(
                key: nameof(Intern.LastName),
                values: "Text is required");

            invalidInternException.AddData(
                key: nameof(Intern.Email),
                values: "Text is required");

            invalidInternException.AddData(
                key: nameof(Intern.PhoneNumber),
                values: "Text is required");

            invalidInternException.AddData(
                key: nameof(Intern.Status),
                values: "Text is required");

            invalidInternException.AddData(
                key: nameof(Intern.CreatedDate),
                values: "Date is required");

            invalidInternException.AddData(
                key: nameof(Intern.CreatedBy),
                values: "Id is required");

            invalidInternException.AddData(
                key: nameof(Intern.UpdatedDate),
                values: "Date is required");

            invalidInternException.AddData(
                key: nameof(Intern.UpdatedBy),
                values: "Id is required");

            invalidInternException.AddData(
                key: nameof(Intern.JoinDate),
                values: "Date is required");

            var expectedInternValidationException =
                new InternValidationException(
                    message: "Intern validation error occurred. Please, try again.",
                        innerException: invalidInternException);

            // when
            ValueTask<Intern> addInternTask =
                this.internService.AddInternAsync(invalidIntern);

            InternValidationException actualInternValidationException =
                await Assert.ThrowsAsync<InternValidationException>(
                    addInternTask.AsTask);

            // then
            actualInternValidationException.Should().BeEquivalentTo(
                expectedInternValidationException);

            this.apiBrokerMock.Verify(broker =>
                broker.PostInternAsync(It.IsAny<Intern>()),
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
