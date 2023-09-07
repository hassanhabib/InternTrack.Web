// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;
using Xeptions;

namespace InternTrack.Portal.Web.Models.InternViews.Exceptions
{
    public class NullInternViewException : Xeption
    {
        public NullInternViewException()
            : base("Null Intern error occurred.")
        { }

        public NullInternViewException(Exception innerException)
            : base("Null Intern error occurred.", innerException)
        { }

        public NullInternViewException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
