// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace InternTrack.Portal.Web.Brokers.Navigations
{
    public class NavigationBroker : INavigationBroker
    {

        private readonly NavigationManager navigationManager;

        public NavigationBroker(NavigationManager navigationManager)
        {
            this.navigationManager = navigationManager;
        }
        
        public void NavigateTo(string route) =>
            this.navigationManager.NavigateTo(route);
    }
}
