using Xeptions;

namespace InternTrack.Portal.Web.Models.Interns.Exceptions
{
    public class InternDependencyException : Xeption
    {
        public InternDependencyException(Xeption innerException)
            : base(message: "Intern dependency error occurred, contact support.",
                  innerException)
        { }

        public InternDependencyException(string message, Xeption innerException)
            : base(message: message, innerException: innerException)
        { }
    }
}
