using CircleApp.Data.Helpers.Enums;
using CircleApp.Data.Models;
using CircleApp.Data.Services;
using CircleAPP.Controllers.Base;
using CircleAPP.Data;
using CircleAPP.Models;
using CircleAPP.ViewModels.Home;
using CircleAPP.ViewModels.Stories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CircleAPP.Controllers
{
    [Authorize]
    public class StoriesController : BaseController
    {
        private readonly IStoriesService _storiesService;
        private readonly IFilesService _filesService;
        public StoriesController(IStoriesService storiesService,
            IFilesService filesService)
        {
            _storiesService = storiesService;
            _filesService = filesService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStory(StoryVM storyVM)
        {
            var loggedInUserId = GetUserId();
            if (loggedInUserId == null) return RedirectToLogin();

            var imageUploadPath = await _filesService.UploadImageAsync(storyVM.Image, ImageFileType.StoryImage);

            var newStory = new Story
            {
                DateCreated = DateTime.UtcNow,
                IsDeleted = false,
                ImageUrl = imageUploadPath,
                UserId = loggedInUserId.Value
            };

            await _storiesService.CreateStoryAsync(newStory);

            return RedirectToAction("Index", "Home");
        }

    }
}
