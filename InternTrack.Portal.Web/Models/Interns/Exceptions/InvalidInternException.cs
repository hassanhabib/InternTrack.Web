// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Collections;
using Xeptions;

namespace InternTrack.Portal.Web.Models.Interns.Exceptions
{
    public class InvalidInternException : Xeption
    {
        public InvalidInternException()
            : base(message: "Invalid Intern. Correct the errors and try again.")
        { }

        public InvalidInternException(Exception innerException, IDictionary data)
            : base(message: "Invalid Intern error occurred. Please correct the errors and try again.",
                  innerException,
                  data)
        { }

        public InvalidInternException(string message, Xeption innerException)
            : base(message, innerException)
        { }

        public InvalidInternException(string message, Xeption innerException, IDictionary data) 
            : base(message, innerException, data) 
        { }
    }
}
