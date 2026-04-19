using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CircleApp.Data.Models
{
    public class Story
    {
        
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
       
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsDeleted { get; set; }
        //fk
        public int UserId { get; set; }
        //navigation properties
        public User User { get; set; }
    }
}
