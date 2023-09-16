// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using InternTrack.Portal.Web.Models.Interns;
using InternTrack.Portal.Web.Models.Interns.Exceptions;
using RESTFulSense.Exceptions;
using Xeptions;

namespace InternTrack.Portal.Web.Services.Foundations.Interns
{
    public partial class InternService
    {
        private delegate ValueTask<Intern> ReturningInternFunction();
        private delegate ValueTask<List<Intern>> ReturningInternsFunction();

        private async ValueTask<Intern> TryCatch(ReturningInternFunction returningFunction)
        {
            try
            {
                return await returningFunction();
            }
            catch (NullInternException nullInternException)
            {
                var isNullInternException =
                    new NullInternException(nullInternException);

                throw CreateAndLogValidationException(nullInternException);
            }
            catch (InvalidInternException invalidInternException)
            {
                var isInvalidInternException =
                    new InvalidInternException(invalidInternException);

                throw CreateAndLogValidationException(invalidInternException);
            }
            catch (HttpRequestException httpRequestException)
            {
                var failedInternDependencyException =
                    new FailedInternDependencyException(httpRequestException);

                throw CreateAndLogCriticalDependencyException(failedInternDependencyException);
            }
            catch (HttpResponseUrlNotFoundException httpResponseUrlNotFoundException)
            {
                var failedInternDependencyException =
                    new FailedInternDependencyException(httpResponseUrlNotFoundException);

                throw CreateAndLogCriticalDependencyException(failedInternDependencyException);
            }           
            catch (HttpResponseUnauthorizedException httpResponseUnauthorizedException)
            {
                var failedInternDependencyException =
                    new FailedInternDependencyException(httpResponseUnauthorizedException);

                throw CreateAndLogCriticalDependencyException(failedInternDependencyException);
            }
            catch (HttpResponseNotFoundException httpResponseNotFoundException)
            {
                var notFoundInternException =
                    new NotFoundInternException(httpResponseNotFoundException);

                throw CreateAndLogDependencyValidationException(notFoundInternException);
            }
            catch (HttpResponseBadRequestException httpResponseBadRequestException)
            {
                var invalidInternException =
                    new InvalidInternException(
                        innerException: httpResponseBadRequestException,
                                data: httpResponseBadRequestException.Data);

                throw CreateAndLogDependencyValidationException(invalidInternException);
            }
            catch (HttpResponseConflictException httpResponseConflictException)
            {
                var invalidInternException =
                    new InvalidInternException(
                        innerException: httpResponseConflictException,
                                data: httpResponseConflictException.Data);

                throw CreateAndLogDependencyValidationException(invalidInternException);
            }
            catch (HttpResponseLockedException httpLockedException)
            {
                var lockedInternException =
                    new LockedInternException(httpLockedException);

                throw CreateAndLogDependencyValidationException(lockedInternException);
            }
            catch (HttpResponseException httpResponseException)
            {
                var failedInternDependencyException =
                    new FailedInternDependencyException(httpResponseException);

                throw CreateAndLogDependencyException(failedInternDependencyException);
            }          
            catch (Exception exception)
            {
                var failedInternServiceException =
                    new FailedInternServiceException(exception);

                throw CreateAndLogInternServiceException(failedInternServiceException);
            }
        }

        private async ValueTask<List<Intern>> TryCatch(ReturningInternsFunction returningFunction)
        {
            try
            {
                return await returningFunction();
            }
            catch (HttpRequestException httpRequestException) 
            {
                var failedInternDependencyException =
                    new FailedInternDependencyException(httpRequestException);

                throw CreateAndLogCriticalDependencyException(failedInternDependencyException);
            }    
            catch (HttpResponseUrlNotFoundException httpResponseUrlNotFoundException)
            {
                var failedInternDependencyException =
                    new FailedInternDependencyException(httpResponseUrlNotFoundException);

                throw CreateAndLogCriticalDependencyException(failedInternDependencyException);
            }
            catch (HttpResponseUnauthorizedException httpResponseUnauthorizedException)
            {
                var failedInternDependencyException =
                    new FailedInternDependencyException(httpResponseUnauthorizedException);

                throw CreateAndLogCriticalDependencyException(failedInternDependencyException);
            }
            catch (HttpResponseException httpResponseException)
            {
                var failedInternDependencyException =
                    new FailedInternDependencyException(httpResponseException);

                throw CreateAndLogDependencyException(failedInternDependencyException);
            }
            catch (Exception exception)
            {
                var failedInternServiceException =
                    new FailedInternServiceException(exception);

                throw CreateAndLogInternServiceException(failedInternServiceException);
            }
        }

        private InternValidationException CreateAndLogValidationException(Xeption exception)
        {
            var internValidationException =
                new InternValidationException(exception);

            this.loggingBroker.LogError(internValidationException);

            return internValidationException;
        }

        private InternDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var internDependencyException =
                new InternDependencyException(exception);

            this.loggingBroker.LogCritical(internDependencyException);

            return internDependencyException;
        }

        private InternDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var internDependencyValidationException =
                new InternDependencyValidationException(exception);

            this.loggingBroker.LogError(internDependencyValidationException);

            return internDependencyValidationException;
        }

        private InternDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var internDependencyException =
                new InternDependencyException(exception);

            this.loggingBroker.LogError(internDependencyException);

            return internDependencyException;
        }

        private InternServiceException CreateAndLogInternServiceException(Xeption exception)
        {
            var internServiceException =
                new InternServiceException(exception);

            this.loggingBroker.LogError(internServiceException);

            return internServiceException;
        }
    }
}
