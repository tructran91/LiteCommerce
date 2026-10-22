namespace LiteCommerce.Admin.Constants
{
    public static class ApiRoutes
    {
        // Identity service, not Catalog.
        public static class Auth
        {
            private const string Base = "/api/auth";
            public const string Login = $"{Base}/login";
            public const string Refresh = $"{Base}/refresh";
            public const string Revoke = $"{Base}/revoke";
            public const string Me = $"{Base}/me";
            public const string ChangePassword = $"{Base}/change-password";
            public const string Sessions = $"{Base}/sessions";
            public const string SessionById = $"{Base}/sessions/{{id}}";
            public const string ForgotPassword = $"{Base}/forgot-password";
            public const string ResetPassword = $"{Base}/reset-password";
        }

        public static class AdminUser
        {
            private const string Base = "/api/admin/users";
            public const string GetAll = Base;
            public const string GetById = $"{Base}/{{id}}";
            public const string Create = Base;
            public const string Update = $"{Base}/{{id}}";
            public const string Delete = $"{Base}/{{id}}";
            public const string UpdateStatus = $"{Base}/{{id}}/status";
            public const string UpdateRole = $"{Base}/{{id}}/role";
            public const string Unlock = $"{Base}/{{id}}/unlock";
            public const string RevokeSessions = $"{Base}/{{id}}/revoke-sessions";
        }

        public static class AdminRole
        {
            public const string GetAll = "/api/admin/roles";
        }

        public static class Session
        {
            public const string Get = "/api/admin/session";
        }

        public static class Brand
        {
            private const string Base = "/api/admin/brand";
            public const string GetAll = Base;
            public const string GetById = $"{Base}/{{id}}";
            public const string Create = Base;
            public const string Update = $"{Base}/{{id}}";
            public const string Delete = $"{Base}/{{id}}";
        }

        public static class Category
        {
            private const string Base = "/api/admin/category";
            public const string GetAll = Base;
            public const string GetAllBasic = $"{Base}/basic";
            public const string GetById = $"{Base}/{{id}}";
            public const string Create = Base;
            public const string Update = $"{Base}/{{id}}";
            public const string Delete = $"{Base}/{{id}}";
        }

        public static class ProductOption
        {
            private const string Base = "/api/admin/product-option";
            public const string GetAll = Base;
            public const string GetById = $"{Base}/{{id}}";
            public const string Create = Base;
            public const string Update = $"{Base}/{{id}}";
            public const string Delete = $"{Base}/{{id}}";
        }

        public static class ProductAttributeGroup
        {
            private const string Base = "/api/admin/product-attribute-group";
            public const string GetAll = Base;
            public const string GetById = $"{Base}/{{id}}";
            public const string Create = Base;
            public const string Update = $"{Base}/{{id}}";
            public const string Delete = $"{Base}/{{id}}";
        }

        public static class ProductAttribute
        {
            private const string Base = "/api/admin/product-attribute";
            public const string GetAll = Base;
            public const string GetById = $"{Base}/{{id}}";
            public const string Create = Base;
            public const string Update = $"{Base}/{{id}}";
            public const string Delete = $"{Base}/{{id}}";
        }

        public static class ProductTemplate
        {
            private const string Base = "/api/admin/product-template";
            public const string GetAll = Base;
            public const string GetById = $"{Base}/{{id}}";
            public const string Create = Base;
            public const string Update = $"{Base}/{{id}}";
            public const string Delete = $"{Base}/{{id}}";
        }

        public static class Product
        {
            private const string Base = "/api/admin/product";
            public const string GetAll = Base;
            public const string GetById = $"{Base}/{{id}}";
            public const string Create = Base;
            public const string Update = $"{Base}/{{id}}";
            public const string Delete = $"{Base}/{{id}}";
            public const string Copy = $"{Base}/{{id}}/copy";
            public const string UploadContentImage = $"{Base}/upload-content-image";
            public const string ImportPreview = $"{Base}/import/preview";
            public const string ImportCommit = $"{Base}/import/commit";
            public const string ImportErrorReport = $"{Base}/import/error-report";
        }

        public static class ProductPrice
        {
            private const string Base = "/api/admin/product-price";
            public const string GetProductPricing = Base;
            public const string UpdateProductPricing = Base;
        }

        public static class AuditLog
        {
            private const string Base = "/api/admin/audit-log";
            public const string GetAll = Base;
            public const string GetById = $"{Base}/{{id}}";
        }

        public static class ActivityLog
        {
            private const string Base = "/api/admin/activity-log";
            public const string GetAll = Base;
            public const string GetById = $"{Base}/{{id}}";
        }
    }
}
