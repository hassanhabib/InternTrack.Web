// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InternTrack.Portal.Web.Models.Interns.Exceptions;
using InternTrack.Portal.Web.Models.InternViews;
using InternTrack.Portal.Web.Models.InternViews.Exceptions;

namespace InternTrack.Portal.Web.Services.Views.InternViews
{
    public partial class InternViewService
    {
        private delegate ValueTask<InternView> ReturningInternViewFunction();

        private async ValueTask<InternView> TryCatch(
            ReturningInternViewFunction returningInternViewFunction)
        {
            try
            {
                return await returningInternViewFunction();
            }
            catch (InternValidationException internValidationException)
            {
                throw CreateAndLogDependencyValidationException(internValidationException);
            }
            catch (InternDependencyValidationException internValidationException)
            {
                throw CreateAndLogDependencyValidationException(internValidationException);
            }
        }

        private InternViewDependencyValidationException CreateAndLogDependencyValidationException(
            Exception exception)
        {
            var internViewDependencyValidationException =
                new InternViewDependencyValidationException(
                    message: "Intern View dependency validation error occurred, try again.",
                        innerException: exception);

            this.loggingBroker.LogError(internViewDependencyValidationException);

            return internViewDependencyValidationException;
        }
    }
}
