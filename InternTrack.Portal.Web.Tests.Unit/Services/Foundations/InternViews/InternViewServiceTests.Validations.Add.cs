// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InternTrack.Portal.Web.Models.Interns.Exceptions;
using InternTrack.Portal.Web.Models.InternViews.Exceptions;
using InternTrack.Portal.Web.Models.InternViews;
using Xunit;
using FluentAssertions;
using Moq;
using InternTrack.Portal.Web.Models.Interns;
using Microsoft.AspNetCore.Components;

namespace InternTrack.Portal.Web.Tests.Unit.Services.Foundations.InternViews
{
    public partial class InternViewServiceTests
    {
        [Fact]
        private async Task
            ShouldThrowValidationExceptionOnAddIfInternViewIsNullAndLogItAsync()
        {
            InternView nullInternView = null;
            var exception = new Exception();

            var nullInternViewException = new NullInternViewException(
                message: "Null Intern error occurred.",
                    innerException: exception);

            var expectedInternViewValidationException =
                new InternViewValidationException(
                    message: "Intern View validation error occurred, try again.",
                        innerException: nullInternViewException);

            // when
            ValueTask<InternView> addInternViewTask =
                this.internViewService.AddInternViewAsync(nullInternView);

            InternViewValidationException actualInternViewValidationException =
                await Assert.ThrowsAsync<InternViewValidationException>(() =>
                    addInternViewTask.AsTask());

            // then
            actualInternViewValidationException.Should()
                .BeEquivalentTo(expectedInternViewValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedInternViewValidationException))),
                        Times.Once);

            this.userServiceMock.Verify(service =>
                service.GetCurrentlyLoggedInUser(),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.internServiceMock.Verify(service =>
                service.AddInternAsync(It.IsAny<Intern>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.internServiceMock.VerifyNoOtherCalls();
            this.navigationBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        private void
            ShouldThrowValidationExceptionOnNavigateIfRouteIsInvalidAndLogItAsync(
           string invalidRoute)
        {
            // given
            var innerException = new Exception();

            var invalidInternViewException =
                new InvalidInternViewException(
                    message: $"Invalid Intern View error occurred. " +
                        $"parameter name: Route, " +
                        $"parameter value: {invalidRoute}",
                            innerException);

            var expectedInternValidationException =
                new InternValidationException(
                    message: "Intern View validation error occurred, try again.",
                        innerException: invalidInternViewException);

            // when
            Action navigateToTask = () =>
                this.internViewService.NavigateTo(invalidRoute);

            InternValidationException actualInternValidationException =
                Assert.Throws<InternValidationException>(navigateToTask);

            // then
            actualInternValidationException.Should()
                .BeEquivalentTo(expectedInternValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedInternValidationException))),
                        Times.Once);

            this.navigationBrokerMock.Verify(broker =>
                broker.NavigateTo(It.IsAny<string>()),
                    Times.Never);

            this.navigationBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.internServiceMock.VerifyNoOtherCalls();
        }
    }
}
