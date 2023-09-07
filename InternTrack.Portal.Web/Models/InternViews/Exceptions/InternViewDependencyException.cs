// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;
using Xeptions;

namespace InternTrack.Portal.Web.Models.InternViews.Exceptions
{
    public class InternViewDependencyException : Xeption
    {
        public InternViewDependencyException(Exception innerException)
            : base(message: "Intern View dependency error occurred, contact support.", innerException)
        { }

        public InternViewDependencyException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
