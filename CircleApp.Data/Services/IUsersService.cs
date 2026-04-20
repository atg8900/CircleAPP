using CircleApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CircleApp.Data.Services
{
    public interface IUsersService
    {
        Task<User> GetUser(int loggedInUserId);

        Task UpdateUserProfilePicture(int userId, string profilePictureUrl);
    }
}
