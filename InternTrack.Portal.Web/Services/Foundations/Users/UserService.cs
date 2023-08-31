// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;

namespace InternTrack.Portal.Web.Services.Foundations.Users
{
    public class UserService
    {
        public Guid GetCurrentlyLoggedInUser() => Guid.NewGuid();
    }
}
