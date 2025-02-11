namespace DashboardAPI.Models
{
    public class UserRole
    {
        public int Id { get; set; }
        public string UserId {  get; set; }
        public string Roles {  get; set; }

        public UserRole(int id, string userId, string roles)
        {
            Id = id;
            UserId = userId;
            Roles = roles;
        }
    }
}
