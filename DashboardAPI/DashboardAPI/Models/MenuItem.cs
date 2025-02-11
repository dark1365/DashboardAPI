namespace DashboardAPI.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string IconName { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
        public int MenuLevel { get; set; }
        public string Route { get; set; }
        public List<MenuItem> subMenu { get; set; }
        public int? parentId { get; set; }

        public MenuItem(int id, string iconName, string title, int order, int menuLevel, int? parentId)
        {
            Id = id;
            IconName = iconName;
            Title = title;
            Order = order;
            MenuLevel = menuLevel;
            this.parentId = parentId;
        }

        public MenuItem()
        { 
        
        }
    }

   
}
