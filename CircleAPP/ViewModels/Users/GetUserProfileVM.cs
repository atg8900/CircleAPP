using CircleApp.Data.Models;
using CircleAPP.Models;

namespace CircleAPP.ViewModels.Users
{
    public class GetUserProfileVM
    {
        public User User { get; set; }
        public List<Post> Posts { get; set; }
    }
}
