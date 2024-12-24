namespace LiteCommerce.Admin.Models
{
    public class LayoutSettingsModel
    {
        public string Layout { get; set; } = "vertical";
        public string SidebarType { get; set; } = "full";
        public string ContainerOption { get; set; } = "full";
        public string Direction { get; set; } = "ltr";
        public string CardLayout { get; set; } = "shadow";
        public string Theme { get; set; } = "light";
        public string ColorTheme { get; set; } = "default";
    }
}
