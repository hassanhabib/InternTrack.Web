using Xeptions;

namespace InternTrack.Portal.Web.Models.Interns.Exceptions
{
    public class InternDependencyValidationException : Xeption
    {
        public InternDependencyValidationException(Xeption innerException)
            : base(message: "Intern dependency validation error occurred, please try again.",
                  innerException)
        { }

        public InternDependencyValidationException(string message, Xeption innerException) 
            :base(message, innerException)
        { }
    }
}
