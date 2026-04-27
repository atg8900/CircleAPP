using CircleApp.Data.Services;
using CircleAPP.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CircleAPP.ViewComponents
{
    public class StoriesViewComponent:ViewComponent
    {
        private readonly IStoriesService _storiesService;

        public StoriesViewComponent(IStoriesService storiesService)
        {
            _storiesService = storiesService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = int.Parse(UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier));
            var allStories = await _storiesService.GetAllStoriesAsync(userId);
            return View(allStories);
        }
    }
}
