// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using InternTrack.Portal.Web.Models.Interns.Exceptions;
using InternTrack.Portal.Web.Models.InternViews;

namespace InternTrack.Portal.Web.Services.Views.InternViews
{
    public partial class InternViewService
    {
        private static void ValidateInternView(InternView internView)
        {
            var innerException = new Exception();

            if (internView == null)
            {
                throw new NullInternViewException(
                    message: "Null Intern error occurred.",
                        innerException: innerException);
            }
        }
    }
}
