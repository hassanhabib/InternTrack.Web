// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace InternTrack.Portal.Web.Models.Interns.Exceptions
{
    public class NotFoundInternException : Xeption
    {
        public NotFoundInternException(Exception innerException)
        : base(message: "Not found Intern error occurred, please try again.",
                  innerException)
        { }

        public NotFoundInternException(string message, Exception innerException) 
            : base(message, innerException) 
        { }
    }
}
