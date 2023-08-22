// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;

namespace InternTrack.Portal.Web.Infrastructure.Provision.Brokers.Logging
{
    public class LoggingBroker
    {
        public void LogActivity(string message) =>
            Console.WriteLine(message);
    }
}
