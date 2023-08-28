using System;
using InternTrack.Portal.Web.Models.Interns;
using InternTrack.Portal.Web.Models.Interns.Exceptions;

namespace InternTrack.Portal.Web.Services.Foundations.Interns
{
    public partial class InternService
    {
        private void ValidateInternOnAdd(Intern intern)
        {
            ValidateInternIsNotNull(intern);
        }

        private static void ValidateInternIsNotNull(Intern intern)
        {
            if (intern is null)
            {
                throw new NullInternException();
            }
        }
    }
}
