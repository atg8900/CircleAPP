using CircleApp.Data.Helpers;
using CircleApp.Data.Models;
using CircleApp.Data.Services;
using CircleAPP.Data;
using CircleAPP.Models;
using CircleAPP.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CircleAPP.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IPostService _postService;
        private readonly IHashtagService _hashtagService;
        private readonly IFilesService _filesService;
        public HomeController(IHashtagService hashtagService, IPostService postService, IFilesService filesService)
        {
            _hashtagService = hashtagService;
            _postService = postService;
            _filesService = filesService;
        }
        public async Task<IActionResult> Index()
        {
            int LoggedInUserId = 1;
            var allposts = await _postService.GetAllPostsAsync(LoggedInUserId);

            return View(allposts);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(PostVM post)
        {
            // Get the logged in user (Hardcoded for now)
            int loggedInUser = 1;
            
            // Upload the image
            var imageUploadPath = await _filesService.UploadImageAsync(post.Image, CircleApp.Data.Helpers.Enums.ImageFileType.PostImage);

            // Create a new post object
            var newPost = new Post
            {
                Content = post.Content,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                ImageUrl = imageUploadPath,
                NrOfReports = 0,
                UserId = loggedInUser
            };
            //Check and save the image

            await _postService.CreatePostAsync(newPost);

            await _hashtagService.ProcessHashtagsForNewPostAsync(newPost.Content);

          

            return RedirectToAction("Index"); 
        }

        [HttpPost]
        public async Task<IActionResult> TogglePostLike(PostLikeVM postLikeVM)
        {
            
            int loggedInUserId = 1;
            await _postService.TogglePostLikeAsync(postLikeVM.PostId, loggedInUserId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddPostComment(PostCommentVM postCommentVM)
        {
            int loggedInUserId = 1;
            

            //Create a post object
            var newComment = new Comment()
            {
                UserId = loggedInUserId,
                PostId = postCommentVM.PostId,
                Content = postCommentVM.Content,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            };

            await _postService.AddPostCommentAsync(newComment);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemovePostComment(RemoveCommentVM removeCommentVM)
        {
            await _postService.RemovePostCommentAsync(removeCommentVM.CommentId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> TogglePostFavorite(PostFavoriteVM postFavoriteVM)
        {

            int loggedInUserId = 1;
            await _postService.TogglePostFavoriteAsync(postFavoriteVM.PostId, loggedInUserId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> TogglePostVisibility(PostVisibilityVM postVisibilityVM)
        {
            
            int loggedInUserId = 1;

         await _postService.TogglePostVisibilityAsync(postVisibilityVM.PostId, loggedInUserId);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddPostReport(PostReportVM postReportVM)
        {
            
            int loggedInUserId = 1;
            
           await _postService.ReportPostAsync(postReportVM.PostId, loggedInUserId);

            return RedirectToAction("Index");
        }


        //PostDelete
        [HttpPost]
        public async Task<IActionResult> PostRemove(PostRemoveVM postRemoveVM)
        {
            var removedPost = await _postService.RemovePostAsync(postRemoveVM.PostId);
            await _hashtagService.ProcessHashtagsForRemovedPostAsync(removedPost.Content);
            return RedirectToAction("Index");
        }

    }
}
