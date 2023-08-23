// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;

namespace InternTrack.Portal.Web.Brokers.DateTimes
{
    public class DateTimeBroker : IDateTimeBroker
    {
        public DateTimeOffset GetCurrentDateTimeOffset() => DateTimeOffset.UtcNow;

        public DateTimeOffset GetCurrentDateTimeOffSet()
        {
            throw new NotImplementedException();
        }
    }
}