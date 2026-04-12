using CircleAPP.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CircleApp.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string? ProfilePictureUrl { get; set; }
        //navigation properties
        public ICollection<Post>Posts { get; set; }= new List<Post>();
    }
}
