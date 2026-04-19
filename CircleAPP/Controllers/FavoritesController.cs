using Microsoft.AspNetCore.Mvc;
using CircleApp.Data.Services;
namespace CircleAPP.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly IPostService _postsService;

        public FavoritesController(IPostService postsService)
        {
            _postsService = postsService;
        }
        public async Task<IActionResult> Index()
        {
            int loggedInUserId = 1;
            var myFavoritePosts = await _postsService.GetAllFavoritedPostsAsync(loggedInUserId);

            return View(myFavoritePosts);
        }
    }
}
