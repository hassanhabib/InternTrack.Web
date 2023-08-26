// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace InternTrack.Portal.Web.Models.Interns.Exceptions
{
    public class FailedInternDependencyException : Xeption
    {
        public FailedInternDependencyException(Exception innerException)
            : base(message: "Failed Intern dependency error occurred, contact support.",
                  innerException)
        { }

        public FailedInternDependencyException(string message, Exception innerException)
            : base(message: message, innerException: innerException)
        { }
    }
}