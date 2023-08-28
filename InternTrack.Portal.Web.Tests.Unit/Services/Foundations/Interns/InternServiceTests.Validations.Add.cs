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
        public async void ShouldThrowValidationExceptionOnAddWhenInternIsNullAndLogItAsync()
        {
            //given
            Intern nullIntern = null;
            var someException = new Exception();

            var nullInternException = new NullInternException(
                message: "The Intern is null.",
                    innerException: someException);

            var expectedInternValidationException =
                new InternValidationException(
                    message: "Intern validation error occurred. Please, try again.",
                        innerException: nullInternException);

            //when
            ValueTask<Intern> createInternTask =
                this.internService.AddInternAsync(nullIntern);

            var actualInternValidationException =
                await Assert.ThrowsAsync<InternValidationException>(() =>
                    createInternTask.AsTask());

            //then
            actualInternValidationException.Should()
                .BeEquivalentTo(expectedInternValidationException);

            this.apiBrokerMock.Verify(broker =>
                broker.PostInternAsync(It.IsAny<Intern>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameAsExceptionAs(
                    expectedInternValidationException))),
                        Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
