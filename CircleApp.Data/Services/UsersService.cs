using CircleApp.Data.Models;
using CircleAPP.Data;
using CircleApp.Data.Dto;
using CircleAPP.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CircleApp.Data.Services
{
    public class UsersService : IUsersService
    {
        private readonly AppDbContext _appDbContext;
        public UsersService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<User> GetUser(int loggedInUserId)
        {
            return await _appDbContext.Users.FirstOrDefaultAsync(n => n.Id == loggedInUserId) ?? new User();
        }

        public async Task UpdateUserProfilePicture(int loggedInUserId, string profilePictureUrl)
        {
            var userDb = await _appDbContext.Users.FirstOrDefaultAsync(n => n.Id == loggedInUserId);

            if (userDb != null)
            {
                userDb.ProfilePictureUrl = profilePictureUrl;
                _appDbContext.Users.Update(userDb);
                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Post>> GetUserPosts(int userId)
        {
            var allPosts = await _appDbContext.Posts
                .Where(n => n.UserId == userId && n.Reports.Count < 5 && !n.IsDeleted)
                .Include(n => n.User)
                .Include(n => n.Likes)
                .Include(n => n.Favorites)
                .Include(n => n.Comments).ThenInclude(n => n.User)
                .Include(n => n.Reports)
                .OrderByDescending(n => n.DateCreated)
                .ToListAsync();

            return allPosts;
        }
    public async Task<GetUserProfileDTO> GetUserProfileAsync(int profileUserId)
        {
            var user = await _appDbContext.Users
                .FirstOrDefaultAsync(n => n.Id == profileUserId);

            var posts = await _appDbContext.Posts
                .Where(n => n.UserId == profileUserId && n.Reports.Count < 5 && !n.IsDeleted)
                .Include(n => n.User)
                .Include(n => n.Likes)
                .Include(n => n.Favorites)
                .Include(n => n.Comments).ThenInclude(n => n.User)
                .Include(n => n.Reports)
                .OrderByDescending(n => n.DateCreated)
                .ToListAsync();

            // Get both sides of the friendship (sender or receiver)
            var friendships = await _appDbContext.Friendships
                .Where(f => f.SenderId == profileUserId || f.ReceiverId == profileUserId)
                .Include(f => f.Sender)
                .Include(f => f.Receiver)
                .ToListAsync();

            // Extract the friend User object (the other person)
            var friends = friendships
                .Select(f => f.SenderId == profileUserId ? f.Receiver : f.Sender)
                .ToList();

            return new GetUserProfileDTO
            {
                User = user,
                Posts = posts,
                Friends = friends
            };
        }
    }
}