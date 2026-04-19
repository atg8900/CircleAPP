using CircleAPP.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CircleApp.Data.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public int PostId { get; set; }

        public int UserId { get; set; }

        // Navigation properties
        public Post Post { get; set; }

        public User User { get; set; }
    }
}
