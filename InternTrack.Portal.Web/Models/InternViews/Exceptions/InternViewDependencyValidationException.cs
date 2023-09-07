// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;
using Xeptions;

namespace InternTrack.Portal.Web.Models.InternViews.Exceptions
{
    public class InternViewDependencyValidationException : Xeption
    {
        public InternViewDependencyValidationException(Exception innerException)
            : base("Intern View dependency validation error occurred, try again.", 
                  innerException)
        { }

        public InternViewDependencyValidationException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
