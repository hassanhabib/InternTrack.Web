// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using InternTrack.Portal.Web.Models.InternViews;
using InternTrack.Portal.Web.Models.InternViews.Exceptions;

namespace InternTrack.Portal.Web.Services.Views.InternViews
{
    public partial class InternViewService
    {
        private static void ValidateInternView(InternView internView)
        {
            if (internView == null)
            {
                throw new NullInternViewException();
            }
        }

        private static void ValidateRoute(string route)
        {
            if (IsInvalid(route))
            {
                throw new InvalidInternViewException(
                    parameterName: "Route",
                        parameterValue: route);
            }
        }

        private static bool IsInvalid(string text) =>
            String.IsNullOrWhiteSpace(text);
    }
}
