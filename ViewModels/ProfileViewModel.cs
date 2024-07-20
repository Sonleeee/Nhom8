using Nhom8.Data;

namespace Nhom8.ViewModels
{
    public class ProfileViewModel
    {
        public User User { get; set; }
        public ProfileViewModel()
        {
            User = new User();
        }
    }
}
