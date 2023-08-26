// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using InternTrack.Portal.Web.Models.Interns;

namespace InternTrack.Portal.Web.Services.Foundations.Interns
{
    public interface IInternService
    {
        ValueTask<Intern> PostIntern(Intern intern);
    }
}
