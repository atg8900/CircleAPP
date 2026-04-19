using CircleApp.Data.Helpers.Enums;
using CircleApp.Data.Models;
using CircleApp.Data.Services;
using CircleAPP.Data;
using CircleAPP.Models;
using CircleAPP.ViewModels.Home;
using CircleAPP.ViewModels.Stories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CircleAPP.Controllers
{
    public class StoriesController : Controller
    {
        private readonly IStoriesService _storiesService;
        private readonly IFilesService _filesService;

        public StoriesController(IStoriesService storiesService ,IFilesService filesService)
        {
            _storiesService = storiesService;
            _filesService = filesService;   
        }
      

        [HttpPost]
        public async Task<IActionResult> CreateStory(StoryVM storyVM)
        {
            // Get the logged in user (Hardcoded for now)
            int loggedInUser = 1;
            var imageUploadPath = await _filesService.UploadImageAsync(storyVM.Image,ImageFileType.StoryImage);
            var newStory = new Story
            {
               
                DateCreated = DateTime.UtcNow,
                ImageUrl = imageUploadPath,
                IsDeleted = false,
                UserId = loggedInUser
            };
            

            //Check and save the image
           
           await _storiesService.CreateStoryAsync(newStory); 
            return RedirectToAction("Index","Home");
        }

    }
}
