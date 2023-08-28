// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace InternTrack.Portal.Web.Models.Interns.Exceptions
{
    public class FailedInternServiceException : Xeption
    {
        public FailedInternServiceException(Exception innerException)
            : base(message: "Failed Intern service error occurred, contact support",
                  innerException)
        { }

        public FailedInternServiceException(string message, Exception innerException) 
            : base(message, innerException)
        { } 
    }
}
