using CircleApp.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace CircleAPP.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly IPostService _postsService;
        public FavoritesController(IPostService postsService)
        {
            _postsService = postsService;
        }


        public async Task<IActionResult> Index()
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var myFavoritePosts = await _postsService.GetAllFavoritedPostsAsync(int.Parse(loggedInUserId));

            return View(myFavoritePosts);
        }
    }
}
