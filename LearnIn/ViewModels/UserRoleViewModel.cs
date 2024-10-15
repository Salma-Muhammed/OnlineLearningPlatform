using LearnIn.Models;

namespace LearnIn.ViewModels
{
    public class UserRoleViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public IList<string> Roles { get; set; } // List of roles
    }
}

