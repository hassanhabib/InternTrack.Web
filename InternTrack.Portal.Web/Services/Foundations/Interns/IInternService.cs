// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InternTrack.Portal.Web.Models.Interns;

namespace InternTrack.Portal.Web.Services.Foundations.Interns
{
    public interface IInternService
    {
        ValueTask<Intern> AddInternAsync(Intern intern);
        ValueTask<Intern> RetrieveInternByIdAsync(Guid internId);
        ValueTask<List<Intern>> RetrieveAllInternsAsync();
    }
}
