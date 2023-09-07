// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InternTrack.Portal.Web.Models.Interns;

namespace InternTrack.Portal.Web.Services.Foundations.Interns
{
    public interface IInternService
    {
        ValueTask<Intern> AddInternAsync(Intern intern);
<<<<<<< HEAD
        ValueTask<Intern> RetrieveInternByIdAsync(Guid internId);
        ValueTask<Intern> RemoveInternByIdAsync(Guid internId);
=======
        ValueTask<Intern> RetrieveInternByIdAsync(Guid internId);        
>>>>>>> beeebe95776cceac6f8bcf66f2729a71b85ae3cb
    }
}
