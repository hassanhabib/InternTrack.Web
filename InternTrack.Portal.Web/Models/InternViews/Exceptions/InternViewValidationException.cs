// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace InternTrack.Portal.Web.Models.InternViews.Exceptions
{
    public class InternViewValidationException : Xeption
    {
        public InternViewValidationException(Exception innerException) 
            : base(message: "Intern View validation error occurred, try again.", 
                    innerException) 
        { }

        public InternViewValidationException(string message,  Exception innerException)
            : base(message, innerException) 
        { }
    }
}
