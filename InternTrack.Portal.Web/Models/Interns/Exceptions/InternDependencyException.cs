// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace InternTrack.Portal.Web.Models.Interns.Exceptions
{
    public class InternDependencyException : Xeption
    {
        public InternDependencyException(Xeption innerException)
            : base(message: "Intern dependency error occurred, contact support.",
                  innerException)
        { }

        public InternDependencyException(string message, Exception innerException) 
            : base(message, innerException)
        { }

        public InternDependencyException(string message, Xeption innerException)
            : base(message: message, innerException: innerException)
        { }
    }
}
