using CircleApp.Data.Models;
using CircleAPP.Data;
using CircleAPP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace CircleApp.Data.Services
{
    public class PostService : IPostService
    {
        private readonly AppDbContext _context;

        public PostService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Post>> GetAllPostsAsync(int LoggedInUserId)
        {
            var allposts = await _context.Posts
                .Where(p =>
                    (!p.IsPrivate || p.UserId == LoggedInUserId) && !p.IsDeleted && p.Reports.Count < 5)
                .Include(p => p.User)
                .Include(p => p.Likes)
                .Include(p => p.Favorites)
                .Include(p => p.Comments).ThenInclude(c => c.User)
                .Include(p => p.Reports)
                .OrderByDescending(p => p.DateCreated)
                .ToListAsync();
            return allposts;
        }

        public async Task<List<Post>> GetAllFavoritedPostsAsync(int loggedInUserId)
        {
            var allFavoritedPosts = await _context.Posts
                
                .Where(p => p.Favorites.Any(f => f.UserId == loggedInUserId)
                       && !p.IsDeleted
                       && p.Reports.Count < 5)
               
                .Include(p => p.User)
                .Include(p => p.Likes)
                .Include(p => p.Reports)
                .Include(p => p.Favorites)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .OrderByDescending(p => p.DateCreated)
                .ToListAsync();

            return allFavoritedPosts;
        }

        public async Task AddPostCommentAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<Post> CreatePostAsync(Post post)
        {
            //Check and save the image
            
            await _context.Posts.AddAsync(post);    
            await _context.SaveChangesAsync();
            return post;

        }



        public async Task<Post> RemovePostAsync(int postId)
        {
            var postDb = await _context.Posts.FirstOrDefaultAsync(n => n.Id == postId);

            if (postDb != null)
            {
                //_context.Posts.Remove(postDb);
                postDb.IsDeleted = true;
                _context.Posts.Update(postDb);
                await _context.SaveChangesAsync();
            }
            return postDb;
        }

        public async Task RemovePostCommentAsync(int commentId)
        {
            var commentDb = _context.Comments.FirstOrDefault(n => n.Id == commentId);

            if (commentDb != null)
            {
                _context.Comments.Remove(commentDb);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ReportPostAsync(int postId, int userId)
        {
           var newReport = new Report()
            {
                PostId = postId,
                UserId = userId,
                DateCreated = DateTime.UtcNow
            };

            await _context.Reports.AddAsync(newReport);
            await _context.SaveChangesAsync();
        }

        public async Task TogglePostFavoriteAsync(int postId, int userId)
        {
            var favorite = await _context.Favorites
                .Where(f => f.PostId == postId && f.UserId == userId)
                .FirstOrDefaultAsync();

            if (favorite != null)
            {

                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }
            else
            {
                var newFavorite = new Favorite()
                {
                    PostId = postId,
                    UserId = userId,
                    DateCreated = DateTime.UtcNow
                };

                await _context.Favorites.AddAsync(newFavorite);
                await _context.SaveChangesAsync();
            }

           
        }

        public async Task TogglePostVisibilityAsync(int postId, int userId)
        {

            var post = await _context.Posts
                .FirstOrDefaultAsync(p => p.Id == postId && p.UserId == userId);

            if (post != null)
            {

                post.IsPrivate = !post.IsPrivate;

                _context.Posts.Update(post);
                await _context.SaveChangesAsync();
            }

        }

        public async Task TogglePostLikeAsync(int postId, int userId)
        {
            // check if user has already liked the post
            var like = await _context.Likes
                .Where(l => l.PostId == postId && l.UserId == userId)
                .FirstOrDefaultAsync();

            if (like != null)
            {
                // If like exists, remove it (Unlike)
                _context.Likes.Remove(like);
                await _context.SaveChangesAsync();

            }
            else
            {
                // If like doesn't exist, add it (Like)
                var newLike = new Like()
                {
                    PostId = postId,
                    UserId = userId
                };

                await _context.Likes.AddAsync(newLike);
                await _context.SaveChangesAsync();

            }
        }

       



    }
}
