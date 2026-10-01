using MudBlazor;

namespace LiteCommerce.Admin.Constants
{
    // Filter options and display helpers shared by the Activity Logs and Audit Logs pages.
    public static class LogConstants
    {
        public const string DateTimeFormat = "dd/MM/yyyy HH:mm:ss";

        // EntityName values written by ActivityLogBehavior (one per command group).
        public static readonly string[] ActivityEntityNames =
        [
            "Brand", "Category", "Product", "ProductPrice", "ProductOption",
            "ProductAttributeGroup", "ProductAttribute", "ProductTemplate"
        ];

        // EntityName values written by AuditLogInterceptor (entity class names, including child rows).
        public static readonly string[] AuditEntityNames =
        [
            "Brand", "Category", "Media", "Product", "ProductAttribute", "ProductAttributeGroup",
            "ProductAttributeValue", "ProductCategory", "ProductLink", "ProductMedia", "ProductOption",
            "ProductOptionValue", "ProductPriceHistory", "ProductTemplate", "ProductTemplateProductAttribute"
        ];

        public static readonly string[] ActivityActions = ["Create", "Update", "Delete", "Upload", "BulkUpdate"];

        public static readonly string[] AuditActions = ["Created", "Modified", "SoftDeleted", "Deleted"];

        public static Color GetActionColor(string action) => action switch
        {
            "Create" or "Created" => Color.Success,
            "Update" or "Modified" or "BulkUpdate" => Color.Info,
            "Delete" or "Deleted" or "SoftDeleted" => Color.Error,
            "Upload" => Color.Secondary,
            _ => Color.Default
        };

        public static string ToLocalDisplay(DateTime utc)
            => DateTime.SpecifyKind(utc, DateTimeKind.Utc).ToLocalTime().ToString(DateTimeFormat);

        // The API compares UTC timestamps with inclusive bounds: start of the first local day to end of the last.
        public static DateTime? ToUtcStartOfDay(DateTime? localDate)
            => localDate is null ? null : DateTime.SpecifyKind(localDate.Value.Date, DateTimeKind.Local).ToUniversalTime();

        public static DateTime? ToUtcEndOfDay(DateTime? localDate)
            => localDate is null ? null : DateTime.SpecifyKind(localDate.Value.Date.AddDays(1).AddTicks(-1), DateTimeKind.Local).ToUniversalTime();
    }
}
