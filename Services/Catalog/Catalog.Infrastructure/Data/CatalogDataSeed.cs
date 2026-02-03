using Catalog.Core.Entities;
using Catalog.Core.Enums;

namespace Catalog.Infrastructure.Data
{
    public static class CatalogDataSeed
    {
        public static async Task SeedAsync(CatalogContext context)
        {
            if (!context.Brands.Any())
            {
                context.Brands.AddRange(
                    new Brand { Id = Guid.Parse("EE00DDE2-9E30-475D-8A9A-3DFE6173FB30"), Name = "Lenovo", Slug = "lenovo", IsPublished = true, CreatedDate = DateTime.UtcNow, LastModifiedDate = DateTime.UtcNow, IsDeleted = false },
                    new Brand { Id = Guid.Parse("148DF6D1-65C1-4E00-B664-5F0A461160A9"), Name = "Apple", Slug = "apple", IsPublished = true, CreatedDate = DateTime.UtcNow, LastModifiedDate = DateTime.UtcNow, IsDeleted = false },
                    new Brand { Id = Guid.Parse("8AC1A1A5-6CDC-48AD-9292-89E912A6A80A"), Name = "Nokia", Slug = "nokia", IsPublished = true, CreatedDate = DateTime.UtcNow, LastModifiedDate = DateTime.UtcNow, IsDeleted = false },
                    new Brand { Id = Guid.Parse("7CC391E1-8783-4F92-A0CC-9271B34C9EA8"), Name = "Samsung", Slug = "samsung", IsPublished = true, CreatedDate = DateTime.UtcNow, LastModifiedDate = DateTime.UtcNow, IsDeleted = false },
                    new Brand { Id = Guid.Parse("B9FB2C43-BEE6-43DE-8996-9F39E444FAC0"), Name = "Hp", Slug = "hp", IsPublished = true, CreatedDate = DateTime.UtcNow, LastModifiedDate = DateTime.UtcNow, IsDeleted = false },
                    new Brand { Id = Guid.Parse("B2647F68-1529-44B5-8B2E-E2F4E969D2A7"), Name = "Dell", Slug = "dell", IsPublished = true, CreatedDate = DateTime.UtcNow, LastModifiedDate = DateTime.UtcNow, IsDeleted = false }
                );

                await context.SaveChangesAsync();
            }

            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category
                    {
                        Id = Guid.Parse("8AC51586-431D-4642-8435-5926CB6C04F4"),
                        Name = "Accessories",
                        Slug = "accessories",
                        IsPublished = true,
                        IncludeInMenu = true,
                        DisplayOrder = 0,
                        ParentId = null,
                        MetaTitle = "Accessories - Shop all accessories",
                        MetaKeywords = "accessories, gadgets, tech",
                        MetaDescription = "Browse a wide selection of accessories including gadgets, tech gear, and more.",
                        OgTitle = "Accessories - Shop the latest tech accessories",
                        OgDescription = "Discover the best accessories for your devices",
                        OgImage = "/images/accessories.jpg",
                        OgUrl = "/categories/accessories",
                        SchemaJson = "",
                        CreatedDate = DateTime.UtcNow,
                        LastModifiedDate = DateTime.UtcNow,
                        IsDeleted = false,
                    },
                    new Category
                    {
                        Id = Guid.Parse("C3F05C5C-3FA3-4C6D-81C8-E33526E39511"),
                        Name = "Cables",
                        Slug = "cables",
                        IsPublished = true,
                        IncludeInMenu = true,
                        DisplayOrder = 0,
                        ParentId = Guid.Parse("8AC51586-431D-4642-8435-5926CB6C04F4"),
                        MetaTitle = "Cables - Find the best cables",
                        MetaKeywords = "cables, charging cables, tech accessories",
                        MetaDescription = "Shop high-quality cables for your gadgets and devices.",
                        OgTitle = "Cables - The best selection of cables online",
                        OgDescription = "Browse our extensive collection of charging and data cables",
                        OgImage = "/images/cables.jpg",
                        OgUrl = "/categories/cables",
                        SchemaJson = "",
                        CreatedDate = DateTime.UtcNow,
                        LastModifiedDate = DateTime.UtcNow,
                        IsDeleted = false,
                    },
                    new Category
                    {
                        Id = Guid.Parse("D8C4A2D8-3F4D-4D3C-923C-F4F46482AB7A"),
                        Name = "Headphones",
                        Slug = "headphones",
                        IsPublished = true,
                        IncludeInMenu = true,
                        DisplayOrder = 0,
                        ParentId = Guid.Parse("8AC51586-431D-4642-8435-5926CB6C04F4"),
                        MetaTitle = "Headphones - Quality sound for all",
                        MetaKeywords = "headphones, audio, music",
                        MetaDescription = "Shop for high-quality headphones and audio accessories.",
                        OgTitle = "Headphones - Superior sound quality",
                        OgDescription = "Discover top-rated headphones for music lovers",
                        OgImage = "/images/headphones.jpg",
                        OgUrl = "/categories/headphones",
                        SchemaJson = "",
                        CreatedDate = DateTime.UtcNow,
                        LastModifiedDate = DateTime.UtcNow,
                        IsDeleted = false,
                    },
                    new Category
                    {
                        Id = Guid.Parse("633B7E0D-E227-450E-A3A2-574125AA6024"),
                        Name = "USB Drives",
                        Slug = "usb-drives",
                        IsPublished = true,
                        IncludeInMenu = true,
                        DisplayOrder = 0,
                        ParentId = Guid.Parse("8AC51586-431D-4642-8435-5926CB6C04F4"),
                        MetaTitle = "USB Drives - Portable storage solutions",
                        MetaKeywords = "usb drives, storage, portable storage",
                        MetaDescription = "Browse and buy USB drives for secure and portable data storage.",
                        OgTitle = "USB Drives - Shop portable storage devices",
                        OgDescription = "Find the best USB drives for your storage needs",
                        OgImage = "/images/usb-drives.jpg",
                        OgUrl = "/categories/usb-drives",
                        SchemaJson = "",
                        CreatedDate = DateTime.UtcNow,
                        LastModifiedDate = DateTime.UtcNow,
                        IsDeleted = false,
                    },
                    new Category
                    {
                        Id = Guid.Parse("F64602E6-D373-42EF-A3BE-4360449BADA1"),
                        Name = "Computers",
                        Slug = "computers",
                        IsPublished = true,
                        IncludeInMenu = true,
                        DisplayOrder = 0,
                        ParentId = null,
                        MetaTitle = "Computers - High-performance computers",
                        MetaKeywords = "computers, laptops, desktops, high-performance",
                        MetaDescription = "Explore a variety of high-performance computers including laptops, desktops, and gaming rigs.",
                        OgTitle = "Computers - Powerful machines for every need",
                        OgDescription = "Browse through our selection of powerful computers for work, gaming, and more",
                        OgImage = "/images/computers.jpg",
                        OgUrl = "/categories/computers",
                        SchemaJson = "",
                        CreatedDate = DateTime.UtcNow,
                        LastModifiedDate = DateTime.UtcNow,
                        IsDeleted = false,
                    },
                    new Category
                    {
                        Id = Guid.Parse("ACFB3C87-502B-44A0-B39B-6B644C57FFB6"),
                        Name = "Gaming",
                        Slug = "gaming",
                        IsPublished = true,
                        IncludeInMenu = true,
                        DisplayOrder = 0,
                        ParentId = Guid.Parse("F64602E6-D373-42EF-A3BE-4360449BADA1"),
                        MetaTitle = "Gaming - Top gaming equipment",
                        MetaKeywords = "gaming, gaming laptops, gaming desktops",
                        MetaDescription = "Find the best gaming equipment including gaming laptops and desktops.",
                        OgTitle = "Gaming - Shop top gaming gear",
                        OgDescription = "Explore gaming laptops, desktops, and accessories",
                        OgImage = "/images/gaming.jpg",
                        OgUrl = "/categories/gaming",
                        SchemaJson = "",
                        CreatedDate = DateTime.UtcNow,
                        LastModifiedDate = DateTime.UtcNow,
                        IsDeleted = false,
                    },
                    new Category
                    {
                        Id = Guid.Parse("761D55DA-023C-41E0-ACE7-C7316602E0F6"),
                        Name = "MacBook",
                        Slug = "macbook",
                        IsPublished = true,
                        IncludeInMenu = true,
                        DisplayOrder = 0,
                        ParentId = Guid.Parse("F64602E6-D373-42EF-A3BE-4360449BADA1"),
                        MetaTitle = "MacBook - Apple laptops",
                        MetaKeywords = "MacBook, Apple laptops, MacBook Pro",
                        MetaDescription = "Shop MacBook and MacBook Pro models for powerful performance and sleek design.",
                        OgTitle = "MacBook - The best Apple laptops",
                        OgDescription = "Explore MacBook models for premium performance and design",
                        OgImage = "/images/macbook.jpg",
                        OgUrl = "/categories/macbook",
                        SchemaJson = "",
                        CreatedDate = DateTime.UtcNow,
                        LastModifiedDate = DateTime.UtcNow,
                        IsDeleted = false,
                    },
                    new Category
                    {
                        Id = Guid.Parse("7E226ECB-6FC1-4602-BF0B-D14D02A14555"),
                        Name = "Phones",
                        Slug = "phones",
                        IsPublished = true,
                        IncludeInMenu = true,
                        DisplayOrder = 0,
                        ParentId = null,
                        MetaTitle = "Phones - Latest mobile phones",
                        MetaKeywords = "phones, smartphones, mobile phones",
                        MetaDescription = "Shop for the latest smartphones and mobile phones at great prices.",
                        OgTitle = "Phones - Shop the latest smartphones",
                        OgDescription = "Browse through our collection of the latest mobile phones",
                        OgImage = "/images/phones.jpg",
                        OgUrl = "/categories/phones",
                        SchemaJson = "",
                        CreatedDate = DateTime.UtcNow,
                        LastModifiedDate = DateTime.UtcNow,
                        IsDeleted = false,
                    },
                    new Category
                    {
                        Id = Guid.Parse("5A6895A9-CE31-4A37-8300-60BB6A939C2C"),
                        Name = "iPhone",
                        Slug = "iphone",
                        IsPublished = true,
                        IncludeInMenu = true,
                        DisplayOrder = 0,
                        ParentId = Guid.Parse("7E226ECB-6FC1-4602-BF0B-D14D02A14555"),
                        MetaTitle = "iPhone - The latest iPhone models",
                        MetaKeywords = "iPhone, Apple, smartphones",
                        MetaDescription = "Shop for the latest iPhone models and accessories.",
                        OgTitle = "iPhone - Buy the latest iPhone",
                        OgDescription = "Explore the latest iPhone models from Apple",
                        OgImage = "/images/iphone.jpg",
                        OgUrl = "/categories/iphone",
                        SchemaJson = "",
                        CreatedDate = DateTime.UtcNow,
                        LastModifiedDate = DateTime.UtcNow,
                        IsDeleted = false,
                    },
                    new Category
                    {
                        Id = Guid.Parse("A5924453-4608-427F-9751-717F217EA569"),
                        Name = "Basic Phones",
                        Slug = "basic-phones",
                        IsPublished = true,
                        IncludeInMenu = true,
                        DisplayOrder = 0,
                        ParentId = Guid.Parse("7E226ECB-6FC1-4602-BF0B-D14D02A14555"),
                        MetaTitle = "Basic Phones - Affordable mobile solutions",
                        MetaKeywords = "basic phones, mobile, affordable phones",
                        MetaDescription = "Browse affordable and reliable basic phones for simple communication.",
                        OgTitle = "Basic Phones - Shop simple and reliable mobile phones",
                        OgDescription = "Find durable and affordable basic mobile phones",
                        OgImage = "/images/basic-phones.jpg",
                        OgUrl = "/categories/basic-phones",
                        SchemaJson = "",
                        CreatedDate = DateTime.UtcNow,
                        LastModifiedDate = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new Category
                    {
                        Id = Guid.Parse("D5F3B0E4-350D-4188-A0D1-28EBC5CC3AEA"),
                        Name = "Gaming",
                        Slug = "gaming",
                        IsPublished = true,
                        IncludeInMenu = true,
                        DisplayOrder = 0,
                        ParentId = Guid.Parse("7E226ECB-6FC1-4602-BF0B-D14D02A14555"),
                        MetaTitle = "Gaming - Explore the ultimate gaming devices",
                        MetaKeywords = "gaming, gaming devices, gaming phones",
                        MetaDescription = "Discover high-performance gaming phones and devices for gamers.",
                        OgTitle = "Gaming - High-performance gaming phones",
                        OgDescription = "Explore the latest gaming phones and devices for the ultimate gaming experience.",
                        OgImage = "/images/gaming.jpg",
                        OgUrl = "/categories/gaming",
                        SchemaJson = "",
                        CreatedDate = DateTime.UtcNow,
                        LastModifiedDate = DateTime.UtcNow,
                        IsDeleted = false
                    }
                );

                await context.SaveChangesAsync();
            }

            if (!context.ProductOptions.Any())
            {
                context.ProductOptions.AddRange(
                    new() { Id = Guid.Parse("452d918b-b5bf-41b7-90e6-51cad397b292"), Name = "Color" },
                    new() { Id = Guid.Parse("a900d061-715e-4682-b493-4a3f75f95b01"), Name = "Size" },
                    new() { Id = Guid.Parse("646dceee-5eca-46a0-8a5f-db8d280dab4a"), Name = "Warranty" }
                );

                await context.SaveChangesAsync();
            }

            if (!context.ProductAttributeGroups.Any())
            {
                context.ProductAttributeGroups.AddRange(
                    new() { Id = Guid.Parse("206E76F1-1F4D-4C0B-84B2-38431548EA49"), Name = "General", Attributes = new(), CreatedDate = DateTime.UtcNow },
                    new() { Id = Guid.Parse("B8DD305A-ABF7-41F1-83FE-3AB7432926C9"), Name = "Screen", Attributes = new(), CreatedDate = DateTime.UtcNow },
                    new() { Id = Guid.Parse("58B6906E-8589-42CA-8D8E-EA85867D4CB7"), Name = "Connectivity", Attributes = new(), CreatedDate = DateTime.UtcNow },
                    new() { Id = Guid.Parse("4D6E8118-5CF2-4DE6-BA04-3CF53425F910"), Name = "Camera", Attributes = new(), CreatedDate = DateTime.UtcNow }
                );

                await context.SaveChangesAsync();
            }

            if (!context.ProductAttributes.Any())
            {
                context.ProductAttributes.AddRange(
                    new() { Id = Guid.Parse("6DDA4BD4-538C-4A56-BB9A-804E4E477456"), GroupId = Guid.Parse("206E76F1-1F4D-4C0B-84B2-38431548EA49"), Name = "CPU", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("18BBCB21-0C25-4543-AF2A-B855E87E01F4"), GroupId = Guid.Parse("206E76F1-1F4D-4C0B-84B2-38431548EA49"), Name = "OS", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("022C909E-BCF6-40CE-A490-E781557B9B96"), GroupId = Guid.Parse("206E76F1-1F4D-4C0B-84B2-38431548EA49"), Name = "GPU", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("0993DC9D-38B9-4C9A-A315-EC6E63A540EA"), GroupId = Guid.Parse("206E76F1-1F4D-4C0B-84B2-38431548EA49"), Name = "RAM", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("0ae35309-b38e-4002-a418-8c3f2d5ff66b"), GroupId = Guid.Parse("206E76F1-1F4D-4C0B-84B2-38431548EA49"), Name = "Capacity", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("74D4F4B8-64F7-46EE-9C15-3EE3743514BF"), GroupId = Guid.Parse("B8DD305A-ABF7-41F1-83FE-3AB7432926C9"), Name = "Size", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("7D637BDF-7801-40C2-B813-4E3C68A0502B"), GroupId = Guid.Parse("B8DD305A-ABF7-41F1-83FE-3AB7432926C9"), Name = "Type", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("67054B20-DEB6-46DD-9E8F-EDAF0B55A551"), GroupId = Guid.Parse("B8DD305A-ABF7-41F1-83FE-3AB7432926C9"), Name = "Resolution", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("EAAE84DE-3092-4E5B-ADF4-98260ED31B39"), GroupId = Guid.Parse("58B6906E-8589-42CA-8D8E-EA85867D4CB7"), Name = "3G", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("9F898DBD-1C78-4890-BA23-A2703DEF84F6"), GroupId = Guid.Parse("58B6906E-8589-42CA-8D8E-EA85867D4CB7"), Name = "4G", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("D2641E35-ADE1-48AC-A660-1FA70C648757"), GroupId = Guid.Parse("58B6906E-8589-42CA-8D8E-EA85867D4CB7"), Name = "5G", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("2f16e475-7ccf-4bfd-ab37-46fe0dd725bf"), GroupId = Guid.Parse("58B6906E-8589-42CA-8D8E-EA85867D4CB7"), Name = "Wifi", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("1416eaa3-2f64-4fb9-966a-86c6dd76166a"), GroupId = Guid.Parse("58B6906E-8589-42CA-8D8E-EA85867D4CB7"), Name = "Bluetooth", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("775a1c47-acd5-4794-a859-09f9aa6cc901"), GroupId = Guid.Parse("58B6906E-8589-42CA-8D8E-EA85867D4CB7"), Name = "NFC", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("fe158671-2d0c-4775-ace8-0017f3e21f02"), GroupId = Guid.Parse("4D6E8118-5CF2-4DE6-BA04-3CF53425F910"), Name = "Main camera", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("8d5fa038-27c6-4184-9b4c-eb50ec7ceb00"), GroupId = Guid.Parse("4D6E8118-5CF2-4DE6-BA04-3CF53425F910"), Name = "Sub camera", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null }
                );

                await context.SaveChangesAsync();
            }

            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new Product
                    {
                        Id = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Name = "iPhone 17 Pro Max",
                        Slug = "iphone-17-pro-max",
                        Price = 1024,
                        IsCallForPricing = false,
                        IsAllowToOrder = false,
                        StockQuantity = 50,
                        IsPublished = true,
                        DisplayOrder = 0,
                        BrandId = Guid.Parse("148DF6D1-65C1-4E00-B664-5F0A461160A9"),
                        MetaTitle = "iPhone 17 Pro Max - Latest Model",
                        MetaKeywords = "iPhone 17, iPhone, Pro Max, Latest iPhone",
                        MetaDescription = "iPhone 17 Pro Max with stunning features and performance.",
                        CanonicalUrl = "iphone-17-pro-max",
                        OgTitle = "iPhone 17 Pro Max - Latest Model",
                        OgDescription = "iPhone 17 Pro Max with cutting-edge technology.",
                        OgImage = "images/iphone-17-pro-max.jpg",
                        OgUrl = "iphone-17-pro-max",
                        SchemaJson = "",
                        CreatedDate = DateTime.UtcNow
                    },
                    new Product
                    {
                        Id = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Name = "Samsung Galaxy S22 Ultra 5G",
                        Slug = "samsung-galaxy-s22-ultra-5g",
                        Price = 2048,
                        IsCallForPricing = false,
                        IsAllowToOrder = false,
                        StockQuantity = 50,
                        IsPublished = true,
                        DisplayOrder = 0,
                        BrandId = Guid.Parse("7CC391E1-8783-4F92-A0CC-9271B34C9EA8"),
                        MetaTitle = "Samsung Galaxy S22 Ultra 5G",
                        MetaKeywords = "Samsung Galaxy, Galaxy S22 Ultra, 5G, Samsung",
                        MetaDescription = "Samsung Galaxy S22 Ultra 5G with top-tier features and performance.",
                        CanonicalUrl = "samsung-galaxy-s22-ultra-5g",
                        OgTitle = "Samsung Galaxy S22 Ultra 5G",
                        OgDescription = "Samsung Galaxy S22 Ultra 5G with high-end specifications.",
                        OgImage = "images/samsung-galaxy-s22-ultra-5g.jpg",
                        OgUrl = "samsung-galaxy-s22-ultra-5g",
                        SchemaJson = "",
                        CreatedDate = DateTime.UtcNow
                    }
                );

                await context.SaveChangesAsync();
            }

            if (!context.ProductAttributeValues.Any())
            {
                context.ProductAttributeValues.AddRange(
                    // Apple iPhone 17 Pro Max
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("81571321-2f2e-4aa5-a648-f94d613dd322"),
                        AttributeId = Guid.Parse("6DDA4BD4-538C-4A56-BB9A-804E4E477456"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "Hexa-core (2x3.46 GHz Everest + 4x2.02 GHz Sawtooth)"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("7313d5e6-6c00-4fdb-af71-3af5a593598b"),
                        AttributeId = Guid.Parse("18BBCB21-0C25-4543-AF2A-B855E87E01F4"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "iOS 16, upgradable to iOS 16.3"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("dfc25c73-6f00-43e8-938f-cc42310338ae"),
                        AttributeId = Guid.Parse("45F254D5-7658-490C-B7C5-1895C04DAA86"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "2024, September 07"
                    },
                    // Samsung Galaxy S22 Ultra 5G
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("eb2ffad9-db6c-4690-87d0-37735f205e8c"),
                        AttributeId = Guid.Parse("45F254D5-7658-490C-B7C5-1895C04DAA86"),
                        ProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Value = "2022, February 09"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("4abf11a5-1b63-4e5a-9d0f-18f2c06eaa69"),
                        AttributeId = Guid.Parse("6DDA4BD4-538C-4A56-BB9A-804E4E477456"),
                        ProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Value = "Octa-core (1x2.8 GHz Cortex-X2 & 3x2.50 GHz Cortex-A710 & 4x1.8 GHz Cortex-A510) - Europe"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("a04c17d2-7f99-4c99-8e12-be7c65dfd3cc"),
                        AttributeId = Guid.Parse("18BBCB21-0C25-4543-AF2A-B855E87E01F4"),
                        ProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Value = "Android 12, upgradable to Android 13, One UI 5"
                    }
                );

                await context.SaveChangesAsync();
            }

            if (!context.ProductLinks.Any())
            {
                context.ProductLinks.AddRange(
                    new ProductLink
                    {
                        Id = Guid.Parse("207fd35e-3e05-4861-9dee-80be435b3bdf"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        LinkedProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        LinkType = ProductLinkType.Super,
                        CreatedDate = DateTime.UtcNow,
                        IsDeleted = false
                    }
                );

                await context.SaveChangesAsync();
            }
        }

        private static Media CheckAndAddMedia(CatalogContext context, Guid mediaId)
        {
            var existingMedia = context.Medias.FirstOrDefault(m => m.Id == mediaId);

            if (existingMedia == null)
            {
                existingMedia = new Media
                {
                    Id = mediaId,
                };

                context.Medias.Add(existingMedia);
                context.SaveChanges();
            }
            return existingMedia;
        }
    }
}
