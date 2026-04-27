using CircleApp.Data.Models;
using CircleAPP.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CircleApp.Data.Services
{
    public class StoriesService : IStoriesService
    {
        private readonly AppDbContext _context;

        public StoriesService(AppDbContext context)
        {
            _context = context;
        }
public async Task<List<Story>> GetAllStoriesAsync(int userId)
{
    // Get IDs of all users who are friends with the logged-in user
    var friendIds = await _context.Friendships
        .Where(f => f.SenderId == userId || f.ReceiverId == userId)
        .Select(f => f.SenderId == userId ? f.ReceiverId : f.SenderId)
        .ToListAsync();

    var allStories = await _context.Stories
        .Where(s => s.DateCreated >= DateTime.UtcNow.AddHours(-24)
                 && (s.UserId == userId || friendIds.Contains(s.UserId)))
        .Include(s => s.User)
        .ToListAsync();

    return allStories;
}
        public async Task<Story> CreateStoryAsync(Story story)
        {
           
            await _context.Stories.AddAsync(story);
            await _context.SaveChangesAsync();
            return story;
        }


    }
}
