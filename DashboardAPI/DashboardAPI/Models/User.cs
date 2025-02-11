namespace DashboardAPI.Models
{
    public class User
    {
        public int UserRoleId { get; set; }
        public string UserId { get; set; }

        public User(string userId, int userRoleId)
        {
            UserId = userId;
            UserRoleId = userRoleId;
        }
    }
}
