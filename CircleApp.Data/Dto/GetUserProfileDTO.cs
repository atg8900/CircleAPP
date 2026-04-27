using CircleApp.Data.Models;
using CircleAPP.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CircleApp.Data.Dto
{
    public class GetUserProfileDTO
    {
        public User User { get; set; }
        public List<Post> Posts { get; set; }
        public List<User> Friends { get; set; }
    }
}
