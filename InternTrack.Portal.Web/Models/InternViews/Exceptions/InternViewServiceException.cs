// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;
using Xeptions;

namespace InternTrack.Portal.Web.Models.InternViews.Exceptions
{
    public class InternViewServiceException : Xeption
    {
        public InternViewServiceException(Exception innerException)
            : base(message: "Intern View service error occurred, contact support.", innerException)
        { }

        public InternViewServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
