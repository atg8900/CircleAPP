using CircleApp.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CircleApp.Data.Services
{
    public interface IStoriesService
    {
        Task<List<Story>> GetAllStoriesAsync(int userId);

        Task<Story> CreateStoryAsync(Story story);
    }
}
