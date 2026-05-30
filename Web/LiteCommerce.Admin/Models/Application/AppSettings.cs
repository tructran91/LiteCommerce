namespace LiteCommerce.Admin.Models.Application
{
    public class AppSettings
    {
        public bool IsDarkMode { get; set; } = false;
        public string PrimaryColor { get; set; } = "#594AE2";
        public string SecondaryColor { get; set; } = "#FF4081";
        public LayoutDirection LayoutDirection { get; set; } = LayoutDirection.Vertical;
        public bool DrawerMiniMode { get; set; } = false;
        public string FontFamily { get; set; } = "Nunito";
        public double BorderRadius { get; set; } = 8;
        public bool EnableAnimations { get; set; } = true;
        public string Language { get; set; } = "vi-VN";
    }

    public enum LayoutDirection
    {
        Vertical,
        Horizontal
    }
}
