namespace LiteCommerce.Shared.Authorization
{
    // Which permissions exist is code; which role holds them is data (Identity's
    // RolePermissions table). To add one: declare the const, add it to All, and put
    // [HasPermission(...)] on the endpoint. PermissionSyncer picks it up on startup,
    // so no migration is needed.
    //
    // Never rename a Code: grants key on the string, so a rename silently revokes it.
    //
    // Linked into the Blazor client, so this file must stay dependency-free.
    public static class AppPermissions
    {
        public const string Wildcard = "*";

        public const string WildcardSuffix = ".*";

        public const char Separator = '.';

        public static class Services
        {
            public const string Catalog = "Catalog";
            public const string Identity = "Identity";
        }

        // Gates AdminSessionController, which the Blazor client calls right after login
        // to decide whether to keep the tokens. A role without this cannot sign in at all.
        public static class BackOffice
        {
            public const string Access = "backoffice.access";
        }

        public static class Brand
        {
            public const string View = "catalog.brand.view";
            public const string Create = "catalog.brand.create";
            public const string Update = "catalog.brand.update";
            public const string Delete = "catalog.brand.delete";
        }

        public static class Category
        {
            public const string View = "catalog.category.view";
            public const string Create = "catalog.category.create";
            public const string Update = "catalog.category.update";
            public const string Delete = "catalog.category.delete";
        }

        public static class Product
        {
            public const string View = "catalog.product.view";
            public const string Create = "catalog.product.create";
            public const string Update = "catalog.product.update";
            public const string Delete = "catalog.product.delete";
            public const string Copy = "catalog.product.copy";
            public const string UploadMedia = "catalog.product.upload-media";
            public const string Import = "catalog.product.import";
        }

        public static class ProductPrice
        {
            public const string View = "catalog.product-price.view";
            public const string Update = "catalog.product-price.update";
        }

        public static class ProductOption
        {
            public const string View = "catalog.product-option.view";
            public const string Create = "catalog.product-option.create";
            public const string Update = "catalog.product-option.update";
            public const string Delete = "catalog.product-option.delete";
        }

        public static class ProductAttribute
        {
            public const string View = "catalog.product-attribute.view";
            public const string Create = "catalog.product-attribute.create";
            public const string Update = "catalog.product-attribute.update";
            public const string Delete = "catalog.product-attribute.delete";
        }

        public static class ProductAttributeGroup
        {
            public const string View = "catalog.product-attribute-group.view";
            public const string Create = "catalog.product-attribute-group.create";
            public const string Update = "catalog.product-attribute-group.update";
            public const string Delete = "catalog.product-attribute-group.delete";
        }

        public static class ProductTemplate
        {
            public const string View = "catalog.product-template.view";
            public const string Create = "catalog.product-template.create";
            public const string Update = "catalog.product-template.update";
            public const string Delete = "catalog.product-template.delete";
        }

        public static class Database
        {
            public const string ViewProfiles = "catalog.database.view-profiles";
            public const string Seed = "catalog.database.seed";
        }

        public static class CatalogLog
        {
            public const string ViewActivity = "catalog.log.view-activity";
            public const string ViewAudit = "catalog.log.view-audit";
        }

        public static class User
        {
            public const string View = "identity.user.view";
            public const string Create = "identity.user.create";
            public const string Update = "identity.user.update";
            public const string Delete = "identity.user.delete";
            public const string UpdateStatus = "identity.user.update-status";
            public const string UpdateRole = "identity.user.update-role";
            public const string Unlock = "identity.user.unlock";
            public const string RevokeSessions = "identity.user.revoke-sessions";
        }

        public static class Role
        {
            public const string View = "identity.role.view";
            public const string Create = "identity.role.create";
            public const string Update = "identity.role.update";
            public const string Delete = "identity.role.delete";
            public const string ViewPermissions = "identity.role.view-permissions";
            public const string UpdatePermissions = "identity.role.update-permissions";
        }

        public static class IdentityLog
        {
            public const string ViewActivity = "identity.log.view-activity";
            public const string ViewAudit = "identity.log.view-audit";
        }

        // Order here is the order the role editor shows them in.
        public static readonly IReadOnlyList<PermissionDescriptor> All = new List<PermissionDescriptor>
        {
            new(BackOffice.Access, Services.Identity, "Back Office", "Đăng nhập trang quản trị",
                "Bắt buộc phải có để đăng nhập được vào trang quản trị."),

            new(Brand.View,   Services.Catalog, "Catalog / Thương hiệu", "Xem thương hiệu"),
            new(Brand.Create, Services.Catalog, "Catalog / Thương hiệu", "Tạo thương hiệu"),
            new(Brand.Update, Services.Catalog, "Catalog / Thương hiệu", "Sửa thương hiệu"),
            new(Brand.Delete, Services.Catalog, "Catalog / Thương hiệu", "Xoá thương hiệu"),

            new(Category.View,   Services.Catalog, "Catalog / Danh mục", "Xem danh mục"),
            new(Category.Create, Services.Catalog, "Catalog / Danh mục", "Tạo danh mục"),
            new(Category.Update, Services.Catalog, "Catalog / Danh mục", "Sửa danh mục"),
            new(Category.Delete, Services.Catalog, "Catalog / Danh mục", "Xoá danh mục"),

            new(Product.View,        Services.Catalog, "Catalog / Sản phẩm", "Xem sản phẩm"),
            new(Product.Create,      Services.Catalog, "Catalog / Sản phẩm", "Tạo sản phẩm"),
            new(Product.Update,      Services.Catalog, "Catalog / Sản phẩm", "Sửa sản phẩm"),
            new(Product.Delete,      Services.Catalog, "Catalog / Sản phẩm", "Xoá sản phẩm"),
            new(Product.Copy,        Services.Catalog, "Catalog / Sản phẩm", "Nhân bản sản phẩm"),
            new(Product.UploadMedia, Services.Catalog, "Catalog / Sản phẩm", "Tải ảnh nội dung"),
            new(Product.Import,      Services.Catalog, "Catalog / Sản phẩm", "Nhập sản phẩm từ file"),

            new(ProductPrice.View,   Services.Catalog, "Catalog / Giá", "Xem bảng giá"),
            new(ProductPrice.Update, Services.Catalog, "Catalog / Giá", "Cập nhật giá"),

            new(ProductOption.View,   Services.Catalog, "Catalog / Option", "Xem option"),
            new(ProductOption.Create, Services.Catalog, "Catalog / Option", "Tạo option"),
            new(ProductOption.Update, Services.Catalog, "Catalog / Option", "Sửa option"),
            new(ProductOption.Delete, Services.Catalog, "Catalog / Option", "Xoá option"),

            new(ProductAttribute.View,   Services.Catalog, "Catalog / Thuộc tính", "Xem thuộc tính"),
            new(ProductAttribute.Create, Services.Catalog, "Catalog / Thuộc tính", "Tạo thuộc tính"),
            new(ProductAttribute.Update, Services.Catalog, "Catalog / Thuộc tính", "Sửa thuộc tính"),
            new(ProductAttribute.Delete, Services.Catalog, "Catalog / Thuộc tính", "Xoá thuộc tính"),

            new(ProductAttributeGroup.View,   Services.Catalog, "Catalog / Nhóm thuộc tính", "Xem nhóm thuộc tính"),
            new(ProductAttributeGroup.Create, Services.Catalog, "Catalog / Nhóm thuộc tính", "Tạo nhóm thuộc tính"),
            new(ProductAttributeGroup.Update, Services.Catalog, "Catalog / Nhóm thuộc tính", "Sửa nhóm thuộc tính"),
            new(ProductAttributeGroup.Delete, Services.Catalog, "Catalog / Nhóm thuộc tính", "Xoá nhóm thuộc tính"),

            new(ProductTemplate.View,   Services.Catalog, "Catalog / Template", "Xem template"),
            new(ProductTemplate.Create, Services.Catalog, "Catalog / Template", "Tạo template"),
            new(ProductTemplate.Update, Services.Catalog, "Catalog / Template", "Sửa template"),
            new(ProductTemplate.Delete, Services.Catalog, "Catalog / Template", "Xoá template"),

            new(Database.ViewProfiles, Services.Catalog, "Catalog / Database", "Xem seed profile"),
            new(Database.Seed,         Services.Catalog, "Catalog / Database", "Chạy seed dữ liệu",
                "Nguy hiểm: ghi đè dữ liệu catalog hiện có."),

            new(CatalogLog.ViewActivity, Services.Catalog, "Catalog / Nhật ký", "Xem activity log (Catalog)"),
            new(CatalogLog.ViewAudit,    Services.Catalog, "Catalog / Nhật ký", "Xem audit log (Catalog)"),

            new(User.View,           Services.Identity, "Identity / Người dùng", "Xem người dùng"),
            new(User.Create,         Services.Identity, "Identity / Người dùng", "Tạo người dùng"),
            new(User.Update,         Services.Identity, "Identity / Người dùng", "Sửa người dùng"),
            new(User.Delete,         Services.Identity, "Identity / Người dùng", "Xoá người dùng"),
            new(User.UpdateStatus,   Services.Identity, "Identity / Người dùng", "Bật/tắt tài khoản"),
            new(User.UpdateRole,     Services.Identity, "Identity / Người dùng", "Đổi vai trò người dùng",
                "Nguy hiểm: có thể dùng để nâng quyền cho tài khoản khác."),
            new(User.Unlock,         Services.Identity, "Identity / Người dùng", "Mở khoá tài khoản"),
            new(User.RevokeSessions, Services.Identity, "Identity / Người dùng", "Thu hồi phiên đăng nhập"),

            new(Role.View,              Services.Identity, "Identity / Vai trò", "Xem vai trò"),
            new(Role.Create,            Services.Identity, "Identity / Vai trò", "Tạo vai trò"),
            new(Role.Update,            Services.Identity, "Identity / Vai trò", "Sửa vai trò"),
            new(Role.Delete,            Services.Identity, "Identity / Vai trò", "Xoá vai trò"),
            new(Role.ViewPermissions,   Services.Identity, "Identity / Vai trò", "Xem quyền của vai trò"),
            new(Role.UpdatePermissions, Services.Identity, "Identity / Vai trò", "Gán quyền cho vai trò",
                "Nguy hiểm: ai có quyền này có thể tự nâng quyền cho chính mình."),

            new(IdentityLog.ViewActivity, Services.Identity, "Identity / Nhật ký", "Xem activity log (Identity)"),
            new(IdentityLog.ViewAudit,    Services.Identity, "Identity / Nhật ký", "Xem audit log (Identity)"),
        };

        public static readonly IReadOnlySet<string> AllCodes =
            All.Select(x => x.Code).ToHashSet(StringComparer.Ordinal);
    }
}
