using CircleApp.Data.Services;
using CircleAPP.ViewModels.Settings;
using Microsoft.AspNetCore.Mvc;

namespace CircleAPP.Controllers
{
    public class SettingsController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly IFilesService _filesService;
        public SettingsController(IUsersService usersService, IFilesService filesService)
        {
            _usersService = usersService;
            _filesService = filesService;
        }
        public async Task<IActionResult> Index()
        {
            var loggedInUserId = 1;
            var user = await _usersService.GetUser(loggedInUserId);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfilePicture(UpdateProfilePictureVM profilePicturevm)
        {
            var loggedInUserId = 1;
            var imageUploadPath = await _filesService.UploadImageAsync(profilePicturevm.ProfilePictureImage, CircleApp.Data.Helpers.Enums.ImageFileType.ProfilePicture);
            await _usersService.UpdateUserProfilePicture(loggedInUserId, imageUploadPath);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateProfileVM profileVm)
        {
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordVM updatePasswordVMD)
        {
            return RedirectToAction("Index");
        }
    }
}