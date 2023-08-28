// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using InternTrack.Portal.Web.Models.Interns;
using InternTrack.Portal.Web.Models.Interns.Exceptions;
using RESTFulSense.Exceptions;
using Xeptions;

namespace InternTrack.Portal.Web.Services.Foundations.Interns
{
    public partial class InternService
    {
        private delegate ValueTask<Intern> ReturningInternFunction();

        private async ValueTask<Intern> TryCatch(ReturningInternFunction returningFunction)
        {
            try
            {
                return await returningFunction();
            }
            catch (HttpRequestException httpRequestException)
            {
                var failedInternDependencyException =
                    new FailedInternDependencyException(
                        message: "Failed Intern dependency error occurred, contact support.",
                            innerException: httpRequestException);

                throw CreateAndLogCriticalDependencyException(failedInternDependencyException);
            }
            catch (HttpResponseUnauthorizedException httpResponseUnauthorizedException)
            {
                var failedInternDependencyException =
                    new FailedInternDependencyException(
                        message: "Failed Intern dependency error occurred, contact support.",
                            innerException: httpResponseUnauthorizedException);

                throw CreateAndLogCriticalDependencyException(failedInternDependencyException);
            }
            catch (HttpResponseUrlNotFoundException httpResponseUrlNotFoundException) 
            {
                var failedInternDependencyException =
                    new FailedInternDependencyException(
                        message: "Failed Intern dependency error occurred, contact support.", 
                            innerException: httpResponseUrlNotFoundException);

                throw CreateAndLogCriticalDependencyException(failedInternDependencyException);
            }
            catch (HttpResponseBadRequestException httpResponseBadRequestException)
            {
                var invalidInternException =
                    new InvalidInternException(
                        message: "Invalid Intern error occurred. Please correct the errors and try again.",
                            innerException: httpResponseBadRequestException,
                                data: httpResponseBadRequestException.Data);

                throw CreateAndLogDependencyValidationException(invalidInternException);
            }
            catch (HttpResponseConflictException httpResponseConflictException)
            {
                var invalidInternException =
                    new InvalidInternException(
                        message: "Invalid Intern error occurred. Please correct the errors and try again.",
                            innerException: httpResponseConflictException,
                                data: httpResponseConflictException.Data);

                throw CreateAndLogDependencyValidationException(invalidInternException);
            }
            catch (HttpResponseException httpResponseException)
            {
                var failedInternDependencyException =
                    new FailedInternDependencyException(
                        message: "Failed Intern dependency error occurred, contact support.",
                            innerException: httpResponseException);

                throw CreateAndLogDependencyException(failedInternDependencyException);
            }
        }
        
        private InternDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var internDependencyException =
                new InternDependencyException(
                    message: "Intern dependency error occurred, contact support.",
                        innerException: exception);

            this.loggingBroker.LogCritical(internDependencyException);

            return internDependencyException;
        }

        private InternDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var internDependencyValidationException =
                new InternDependencyValidationException(
                    message: "Intern dependency validation error occurred, please try again.",
                        innerException: exception);

            this.loggingBroker.LogError(internDependencyValidationException);

            return internDependencyValidationException;
        }

        private InternDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var internDependencyException =
                new InternDependencyException(
                    message: "Intern dependency error occurred, contact support.",
                        innerException: exception);

            this.loggingBroker.LogError(internDependencyException);

            return internDependencyException;
        }
    }
}
