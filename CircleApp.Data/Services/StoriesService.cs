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
        public async Task<List<Story>> GetAllStoriesAsync()
        {
            var allStories = await _context.Stories
                .Where(s => s.DateCreated >= DateTime.UtcNow.AddHours(-24))
                .Include(s => s.User).
                ToListAsync();
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
