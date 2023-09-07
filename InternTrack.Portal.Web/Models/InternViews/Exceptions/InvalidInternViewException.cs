// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace InternTrack.Portal.Web.Models.InternViews.Exceptions
{
    public class InvalidInternViewException : Xeption
    {
        public InvalidInternViewException(string parameterName, object parameterValue)
            : base($"Invalid Intern View error occurred. " +
                 $"parameter name: {parameterName}, " +
                 $"parameter value: {parameterValue}")
        { }

        public InvalidInternViewException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
