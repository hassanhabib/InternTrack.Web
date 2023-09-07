// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace InternTrack.Portal.Web.Models.Interns.Exceptions
{
    public class InternDependencyValidationException : Xeption
    {
        public InternDependencyValidationException(Xeption innerException)
            : base(message: "Intern dependency validation error occurred, please try again.",
                  innerException)
        { }

        public InternDependencyValidationException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public InternDependencyValidationException(string message, Xeption innerException) 
            :base(message, innerException)
        { }
    }
}
