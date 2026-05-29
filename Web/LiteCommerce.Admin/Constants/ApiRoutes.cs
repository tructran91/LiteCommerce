namespace LiteCommerce.Admin.Constants
{
    public static class ApiRoutes
    {
        public static class Brand
        {
            private const string Base = "/api/admin/brand";
            public const string GetAll = $"{Base}/GetAllBrands";
            public const string GetById = $"{Base}/GetBrandById/{{id}}";
            public const string Create = $"{Base}/CreateBrand";
            public const string Update = $"{Base}/UpdateBrand";
            public const string Delete = $"{Base}/DeleteBrand/{{id}}";
        }

        public static class Category
        {
            private const string Base = "/api/admin/category";
            public const string GetAll = $"{Base}/GetAllCategories";
            public const string GetAllBasic = $"{Base}/GetAllBasicCategories";
            public const string GetById = $"{Base}/GetCategoryById/{{id}}";
            public const string Create = $"{Base}/CreateCategory";
            public const string Update = $"{Base}/UpdateCategory";
            public const string Delete = $"{Base}/DeleteCategory/{{id}}";
        }

        public static class ProductOption
        {
            private const string Base = "/api/admin/product-option";
            public const string GetAll = $"{Base}/GetAllProductOptions";
            public const string GetById = $"{Base}/GetProductOptionById/{{id}}";
            public const string Create = $"{Base}/CreateProductOption";
            public const string Update = $"{Base}/UpdateProductOption";
            public const string Delete = $"{Base}/DeleteProductOption/{{id}}";
        }

        public static class ProductAttributeGroup
        {
            private const string Base = "/api/admin/product-attribute-group";
            public const string GetAll = $"{Base}/GetAllProductAttributeGroups";
            public const string GetById = $"{Base}/GetProductAttributeGroupById/{{id}}";
            public const string Create = $"{Base}/CreateProductAttributeGroup";
            public const string Update = $"{Base}/UpdateProductAttributeGroup";
            public const string Delete = $"{Base}/DeleteProductAttributeGroup/{{id}}";
        }

        public static class ProductAttribute
        {
            private const string Base = "/api/admin/product-attribute";
            public const string GetAll = $"{Base}/GetAllProductAttributes";
            public const string GetById = $"{Base}/GetProductAttributeById/{{id}}";
            public const string Create = $"{Base}/CreateProductAttribute";
            public const string Update = $"{Base}/UpdateProductAttribute";
            public const string Delete = $"{Base}/DeleteProductAttribute/{{id}}";
        }

        public static class ProductTemplate
        {
            private const string Base = "/api/admin/product-template";
            public const string GetAll = $"{Base}/GetAllProductTemplates";
            public const string GetById = $"{Base}/GetProductTemplateById/{{id}}";
            public const string Create = $"{Base}/CreateProductTemplate";
            public const string Update = $"{Base}/UpdateProductTemplate";
            public const string Delete = $"{Base}/DeleteProductTemplate/{{id}}";
        }

        public static class Product
        {
            private const string Base = "/api/admin/product";
            public const string GetAll = $"{Base}/GetAllProducts";
            public const string GetById = $"{Base}/GetProductById/{{id}}";
            public const string Create = $"{Base}/CreateProduct";
            public const string Update = $"{Base}/UpdateProduct";
            public const string Delete = $"{Base}/DeleteProduct/{{id}}";
        }

        public static class ProductPrice
        {
            private const string Base = "/api/admin/product-price";
            public const string GetProductPricing = $"{Base}/GetProductPricing";
            public const string UpdateProductPricing = $"{Base}/UpdateProductPricing";
        }
    }
}
