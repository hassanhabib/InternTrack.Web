// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace InternTrack.Portal.Web.Models.Interns.Exceptions
{
    public class InternValidationException : Xeption
    {
        public InternValidationException(Xeption innerException)
            : base("Intern validation error occurred. Please, try again.",
                  innerException)
        { }

        public InternValidationException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public InternValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
