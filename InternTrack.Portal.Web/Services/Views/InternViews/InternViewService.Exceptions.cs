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
        private delegate void ReturningNothingFunction();

        private async ValueTask<InternView> TryCatch(
            ReturningInternViewFunction returningInternViewFunction)
        {
            try
            {
                return await returningInternViewFunction();
            }
            catch (NullInternViewException nullInternViewException)
            {
                throw CreateAndLogValidationException(nullInternViewException);
            }
            catch (InternValidationException internValidationException)
            {
                throw CreateAndLogDependencyValidationException(internValidationException);
            }
            catch (InternDependencyValidationException internValidationException)
            {
                throw CreateAndLogDependencyValidationException(internValidationException);
            }
            catch (InternDependencyException internDependencyException)
            {
                throw CreateAndLogDependencyException(internDependencyException);
            }
            catch (InternServiceException internServiceException)
            {
                throw CreateAndLogDependencyException(internServiceException);
            }
            catch (Exception internServiceException)
            {
                throw CreateAndLogServiceException(internServiceException);
            }
        }

        private void TryCatch(ReturningNothingFunction returnNothingFunction)
        {
            try
            {
                returnNothingFunction();
            }
            catch (InvalidInternViewException invalidInternViewException)
            {
                throw CreateAndLogValidationException(invalidInternViewException);
            }
            catch (Exception serviceException)
            {
                throw CreateAndLogServiceException(serviceException);
            }
        }

        private Exception CreateAndLogValidationException(
            Exception exception)
        {
            var internValidationException =
                new InternViewValidationException(exception);

            this.loggingBroker.LogError(internValidationException);

            return internValidationException;
        }

        private InternViewServiceException CreateAndLogServiceException(
            Exception exception)
        {
            var internViewServiceException =
                new InternViewServiceException(exception);

            this.loggingBroker.LogError(internViewServiceException);

            return internViewServiceException;
        }

        private InternViewDependencyValidationException CreateAndLogDependencyValidationException(
            Exception exception)
        {
            var internViewDependencyValidationException =
                new InternViewDependencyValidationException(exception);

            this.loggingBroker.LogError(internViewDependencyValidationException);

            return internViewDependencyValidationException;
        }

        private InternViewDependencyException CreateAndLogDependencyException(
            Exception exception)
        {
            var internViewDependencyException =
                new InternViewDependencyException(exception);

            this.loggingBroker.LogError(internViewDependencyException);

            return internViewDependencyException;
        }
    }
}
