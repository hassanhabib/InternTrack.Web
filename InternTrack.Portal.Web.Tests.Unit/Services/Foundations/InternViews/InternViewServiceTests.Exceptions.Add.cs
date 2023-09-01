// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using InternTrack.Portal.Web.Models.Interns;
using InternTrack.Portal.Web.Models.Interns.Exceptions;
using InternTrack.Portal.Web.Models.InternViews;
using InternTrack.Portal.Web.Models.InternViews.Exceptions;
using Moq;
using Xunit;

namespace InternTrack.Portal.Web.Tests.Unit.Services.Foundations.InternViews
{
    public partial class InternViewServiceTests
    {
        [Theory]
        [MemberData(nameof(InternServiceValidationExceptions))]
        private async Task
            ShouldThrowDependencyValidationExceptionOnAddIfInternValidationErrorOccurredAndLogItAsync(
                Exception internServiceValidationException)
        {
            // given
            InternView someInternView = CreateRandomInternView();

            var expectedDependencyValidationException =
                new InternViewDependencyValidationException(
                    message: "Intern dependency validation error occurred, please try again.",
                        innerException: internServiceValidationException);

            this.internServiceMock.Setup(service =>
                service.AddInternAsync(It.IsAny<Intern>()))
                    .ThrowsAsync(internServiceValidationException);

            // when
            ValueTask<InternView> addInternViewTask =
                this.internViewService.AddInternViewAsync(someInternView);

            InternDependencyValidationException actualDependencyValidationException =
                await Assert.ThrowsAsync<InternDependencyValidationException>(() =>
                    addInternViewTask.AsTask());

            // then
            actualDependencyValidationException.Should()
                .BeEquivalentTo(expectedDependencyValidationException);

            this.userServiceMock.Verify(service =>
                service.GetCurrentlyLoggedInUser(),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.internServiceMock.Verify(service =>
                service.AddInternAsync(It.IsAny<Intern>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDependencyValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.internServiceMock.VerifyNoOtherCalls();
        }
    }
}
