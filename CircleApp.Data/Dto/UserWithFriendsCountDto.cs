using CircleApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CircleApp.Data.Dto
{
    public class UserWithFriendsCountDto
    {
        public User User { get; set; }
        public int FriendsCount { get; set; }
    }
}
