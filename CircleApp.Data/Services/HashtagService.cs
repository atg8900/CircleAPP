using CircleApp.Data.Helpers;
using CircleApp.Data.Models;
using CircleAPP.Data;
using CircleAPP.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CircleApp.Data.Services
{
    public class HashtagService : IHashtagService
    {
        private readonly AppDbContext _context;

        public HashtagService(AppDbContext context)
        {
            _context = context;
        }
        public async Task ProcessHashtagsForNewPostAsync(string content)
        {
            // Find and store hashtags
            var postHashtags = HashtagHelper.GetHashtags(content);
            foreach (var hashTag in postHashtags)
            {
                var hashtagDb = await _context.Hashtags.FirstOrDefaultAsync(n => n.Name == hashTag);
                if (hashtagDb != null)
                {
                    hashtagDb.Count += 1;
                    hashtagDb.DateUpdated = DateTime.UtcNow;

                    _context.Hashtags.Update(hashtagDb);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var newHashtag = new Hashtag()
                    {
                        Name = hashTag,
                        Count = 1,
                        DateCreated = DateTime.UtcNow,
                        DateUpdated = DateTime.UtcNow
                    };

                    await _context.Hashtags.AddAsync(newHashtag);
                    await _context.SaveChangesAsync();
                }
            }

        }

        public async Task ProcessHashtagsForRemovedPostAsync(string content)
        {
            //Update hashtags
            var postHashtags = HashtagHelper.GetHashtags(content);
            foreach (var hashtag in postHashtags)
            {
                var hashtagDb = await _context.Hashtags.FirstOrDefaultAsync(n => n.Name == hashtag);
                if (hashtagDb != null)
                {
                    hashtagDb.Count -= 1;
                    hashtagDb.DateUpdated = DateTime.UtcNow;

                    _context.Hashtags.Update(hashtagDb);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
