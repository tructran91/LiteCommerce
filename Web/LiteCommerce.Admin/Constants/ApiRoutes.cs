namespace LiteCommerce.Admin.Constants
{
    public static class ApiRoutes
    {
        public static class Brand
        {
            private const string Base = "/api/admin/brand";
            public const string GetAll = Base;
            public const string GetById = $"{Base}/{{id}}";
            public const string Create = Base;
            public const string Update = Base;
            public const string Delete = $"{Base}/{{id}}";
        }

        public static class Category
        {
            private const string Base = "/api/admin/category";
            public const string GetAll = Base;
            public const string GetAllBasic = $"{Base}/basic";
            public const string GetById = $"{Base}/{{id}}";
            public const string Create = Base;
            public const string Update = Base;
            public const string Delete = $"{Base}/{{id}}";
        }

        public static class ProductOption
        {
            private const string Base = "/api/admin/product-option";
            public const string GetAll = Base;
            public const string GetById = $"{Base}/{{id}}";
            public const string Create = Base;
            public const string Update = Base;
            public const string Delete = $"{Base}/{{id}}";
        }

        public static class ProductAttributeGroup
        {
            private const string Base = "/api/admin/product-attribute-group";
            public const string GetAll = Base;
            public const string GetById = $"{Base}/{{id}}";
            public const string Create = Base;
            public const string Update = Base;
            public const string Delete = $"{Base}/{{id}}";
        }

        public static class ProductAttribute
        {
            private const string Base = "/api/admin/product-attribute";
            public const string GetAll = Base;
            public const string GetById = $"{Base}/{{id}}";
            public const string Create = Base;
            public const string Update = Base;
            public const string Delete = $"{Base}/{{id}}";
        }

        public static class ProductTemplate
        {
            private const string Base = "/api/admin/product-template";
            public const string GetAll = Base;
            public const string GetById = $"{Base}/{{id}}";
            public const string Create = Base;
            public const string Update = Base;
            public const string Delete = $"{Base}/{{id}}";
        }

        public static class Product
        {
            private const string Base = "/api/admin/product";
            public const string GetAll = Base;
            public const string GetById = $"{Base}/{{id}}";
            public const string Create = Base;
            public const string Update = Base;
            public const string Delete = $"{Base}/{{id}}";
            public const string UploadContentImage = $"{Base}/upload-content-image";
        }

        public static class ProductPrice
        {
            private const string Base = "/api/admin/product-price";
            public const string GetProductPricing = Base;
            public const string UpdateProductPricing = Base;
        }
    }
}
