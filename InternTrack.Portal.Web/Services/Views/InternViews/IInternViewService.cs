// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System.Threading.Tasks;
using InternTrack.Portal.Web.Models.InternViews;

namespace InternTrack.Portal.Web.Services.Views.InternViews
{
    public interface IInternViewService
    {
        ValueTask<InternView> AddInternViewAsync(InternView internView);
        void NavigateTo(string route);
    }
}
