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
                        ParentCategoryId = null,
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
                        ParentCategoryId = Guid.Parse("8AC51586-431D-4642-8435-5926CB6C04F4"),
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
                        ParentCategoryId = Guid.Parse("8AC51586-431D-4642-8435-5926CB6C04F4"),
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
                        ParentCategoryId = Guid.Parse("8AC51586-431D-4642-8435-5926CB6C04F4"),
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
                        ParentCategoryId = null,
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
                        ParentCategoryId = Guid.Parse("F64602E6-D373-42EF-A3BE-4360449BADA1"),
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
                        ParentCategoryId = Guid.Parse("F64602E6-D373-42EF-A3BE-4360449BADA1"),
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
                        ParentCategoryId = null,
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
                        ParentCategoryId = Guid.Parse("7E226ECB-6FC1-4602-BF0B-D14D02A14555"),
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
                        ParentCategoryId = Guid.Parse("7E226ECB-6FC1-4602-BF0B-D14D02A14555"),
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
                        ParentCategoryId = Guid.Parse("7E226ECB-6FC1-4602-BF0B-D14D02A14555"),
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

            if (!context.ProductAttributeGroups.Any())
            {
                context.ProductAttributeGroups.AddRange(
                    new() { Id = Guid.Parse("206E76F1-1F4D-4C0B-84B2-38431548EA49"), Name = "Platform", Attributes = new() },
                    new() { Id = Guid.Parse("B8DD305A-ABF7-41F1-83FE-3AB7432926C9"), Name = "Main Camera", Attributes = new() },
                    new() { Id = Guid.Parse("58B6906E-8589-42CA-8D8E-EA85867D4CB7"), Name = "Selfie Camera", Attributes = new() },
                    new() { Id = Guid.Parse("4D6E8118-5CF2-4DE6-BA04-3CF53425F910"), Name = "Features", Attributes = new() },
                    new() { Id = Guid.Parse("A900D061-715E-4682-B493-4A3F75F95B01"), Name = "Sound", Attributes = new() },
                    new() { Id = Guid.Parse("99BAB448-5D08-4EB1-98CE-5784A8DFFBCC"), Name = "Body", Attributes = new() },
                    new() { Id = Guid.Parse("3673D751-C834-45A2-9416-6053581922B4"), Name = "General", Attributes = new() },
                    new() { Id = Guid.Parse("1AC2765D-43C9-49B6-8088-876AD2E9B712"), Name = "Connectivity", Attributes = new() },
                    new() { Id = Guid.Parse("5CDD890A-F6B8-406E-A054-B608BAA85C70"), Name = "Memory", Attributes = new() },
                    new() { Id = Guid.Parse("646DCEEE-5ECA-46A0-8A5F-DB8D280DAB4A"), Name = "Display", Attributes = new() },
                    new() { Id = Guid.Parse("FD3426A4-D80F-4F85-9C71-FDA22E71C662"), Name = "Battery", Attributes = new() }
                );

                await context.SaveChangesAsync();
            }

            if (!context.ProductAttributes.Any())
            {
                context.ProductAttributes.AddRange(
                    new() { Id = Guid.Parse("45F254D5-7658-490C-B7C5-1895C04DAA86"), GroupId = Guid.Parse("3673D751-C834-45A2-9416-6053581922B4"), Name = "Announced", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("6DDA4BD4-538C-4A56-BB9A-804E4E477456"), GroupId = Guid.Parse("206E76F1-1F4D-4C0B-84B2-38431548EA49"), Name = "CPU", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("18BBCB21-0C25-4543-AF2A-B855E87E01F4"), GroupId = Guid.Parse("206E76F1-1F4D-4C0B-84B2-38431548EA49"), Name = "OS", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("022C909E-BCF6-40CE-A490-E781557B9B96"), GroupId = Guid.Parse("206E76F1-1F4D-4C0B-84B2-38431548EA49"), Name = "GPU", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("0993DC9D-38B9-4C9A-A315-EC6E63A540EA"), GroupId = Guid.Parse("206E76F1-1F4D-4C0B-84B2-38431548EA49"), Name = "Chipset", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("74D4F4B8-64F7-46EE-9C15-3EE3743514BF"), GroupId = Guid.Parse("B8DD305A-ABF7-41F1-83FE-3AB7432926C9"), Name = "Modules", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("7D637BDF-7801-40C2-B813-4E3C68A0502B"), GroupId = Guid.Parse("B8DD305A-ABF7-41F1-83FE-3AB7432926C9"), Name = "Video", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("67054B20-DEB6-46DD-9E8F-EDAF0B55A551"), GroupId = Guid.Parse("B8DD305A-ABF7-41F1-83FE-3AB7432926C9"), Name = "Features", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("D2641E35-ADE1-48AC-A660-1FA70C648757"), GroupId = Guid.Parse("58B6906E-8589-42CA-8D8E-EA85867D4CB7"), Name = "Video", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("EAAE84DE-3092-4E5B-ADF4-98260ED31B39"), GroupId = Guid.Parse("58B6906E-8589-42CA-8D8E-EA85867D4CB7"), Name = "Modules", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("9F898DBD-1C78-4890-BA23-A2703DEF84F6"), GroupId = Guid.Parse("58B6906E-8589-42CA-8D8E-EA85867D4CB7"), Name = "Features", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("61EF7843-0D3A-4F3E-8F75-CFA2F9C366D4"), GroupId = Guid.Parse("4D6E8118-5CF2-4DE6-BA04-3CF53425F910"), Name = "Sensors", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("91995FC8-087C-42B5-989F-02738205C09C"), GroupId = Guid.Parse("A900D061-715E-4682-B493-4A3F75F95B01"), Name = "3.5mm jack", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("B627C3B1-48DA-4A10-8E84-436532E3CF1E"), GroupId = Guid.Parse("A900D061-715E-4682-B493-4A3F75F95B01"), Name = "Loudspeaker", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("3A79F080-A507-4016-89FE-608EF1A41E88"), GroupId = Guid.Parse("99BAB448-5D08-4EB1-98CE-5784A8DFFBCC"), Name = "SIM", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("D9D56DA3-BF2A-4FA0-957C-C6E586A99400"), GroupId = Guid.Parse("99BAB448-5D08-4EB1-98CE-5784A8DFFBCC"), Name = "Weight", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("35E47E74-1D9A-4F8A-AF64-DFAB49030BB5"), GroupId = Guid.Parse("99BAB448-5D08-4EB1-98CE-5784A8DFFBCC"), Name = "Build", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("4DDAC538-909E-4E06-ACC9-E0763D7C59D4"), GroupId = Guid.Parse("99BAB448-5D08-4EB1-98CE-5784A8DFFBCC"), Name = "Dimensions", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("BC9A2A55-FA69-4526-AA89-07A3B5B748BC"), GroupId = Guid.Parse("5CDD890A-F6B8-406E-A054-B608BAA85C70"), Name = "Internal", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("79BBA792-9E11-44FA-842C-D2CA0919AB75"), GroupId = Guid.Parse("5CDD890A-F6B8-406E-A054-B608BAA85C70"), Name = "Card slot", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("72BA299F-9AB6-4429-B083-1C673811672F"), GroupId = Guid.Parse("646DCEEE-5ECA-46A0-8A5F-DB8D280DAB4A"), Name = "Type", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("75CF4E98-7BA8-4190-98C9-2086D1B90A63"), GroupId = Guid.Parse("646DCEEE-5ECA-46A0-8A5F-DB8D280DAB4A"), Name = "Protection", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("841DE7A5-82CE-4F64-AEB2-23E1EA70DD2B"), GroupId = Guid.Parse("646DCEEE-5ECA-46A0-8A5F-DB8D280DAB4A"), Name = "Resolution", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("D8ECB7AE-6539-4FD9-9F72-51A37BA754C4"), GroupId = Guid.Parse("646DCEEE-5ECA-46A0-8A5F-DB8D280DAB4A"), Name = "Size", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("C50D9A30-E567-4553-85DB-7BB40125433F"), GroupId = Guid.Parse("1AC2765D-43C9-49B6-8088-876AD2E9B712"), Name = "Bluetooth", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("3A95802A-B1FB-4CA6-88C1-87448981A019"), GroupId = Guid.Parse("1AC2765D-43C9-49B6-8088-876AD2E9B712"), Name = "USB", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("522FFA4D-D907-43E2-BB23-96FA7FEF24B1"), GroupId = Guid.Parse("1AC2765D-43C9-49B6-8088-876AD2E9B712"), Name = "3G", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("600FA0C8-D3DE-4C78-9E2E-0B79D2A794F1"), GroupId = Guid.Parse("1AC2765D-43C9-49B6-8088-876AD2E9B712"), Name = "WLAN", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("C3F8CD5F-168D-4F4D-ADA3-1344CB7E75E7"), GroupId = Guid.Parse("1AC2765D-43C9-49B6-8088-876AD2E9B712"), Name = "Positioning", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("52832EB3-A80E-4828-B0EC-19B458FE1DF7"), GroupId = Guid.Parse("1AC2765D-43C9-49B6-8088-876AD2E9B712"), Name = "NFC", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("409F8969-4650-4B2F-94F0-A5BDFF20CA0C"), GroupId = Guid.Parse("1AC2765D-43C9-49B6-8088-876AD2E9B712"), Name = "5G", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("11D6F0D6-D9BC-4B33-897A-BC66E6B8BF5D"), GroupId = Guid.Parse("1AC2765D-43C9-49B6-8088-876AD2E9B712"), Name = "Infrared port", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("F60DA4E8-2D75-4A5D-B6D1-EAD215939720"), GroupId = Guid.Parse("1AC2765D-43C9-49B6-8088-876AD2E9B712"), Name = "Radio", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("99E02E27-604B-4487-A6EE-FEF0DFD90051"), GroupId = Guid.Parse("1AC2765D-43C9-49B6-8088-876AD2E9B712"), Name = "4G", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("80BBADDC-4D0A-44C7-BCE0-041BA78139F9"), GroupId = Guid.Parse("FD3426A4-D80F-4F85-9C71-FDA22E71C662"), Name = "Type", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null },
                    new() { Id = Guid.Parse("7B314B22-4695-4140-9861-E44D08624052"), GroupId = Guid.Parse("FD3426A4-D80F-4F85-9C71-FDA22E71C662"), Name = "Charging", CreatedDate = DateTime.UtcNow, LastModifiedDate = null, IsDeleted = false, Group = null }
                );

                await context.SaveChangesAsync();
            }

            if (!context.Medias.Any())
            {
                context.Medias.AddRange(
                    new Media { Id = Guid.Parse("3609A9BA-8552-49E7-BA94-3FE4EF689C8A"), FileSize = 100, FileName = "81b606ea-0bb0-4cea-a9d7-6406175df9bb.jpg", MediaType = MediaType.Image, CreatedDate = DateTime.Now, LastModifiedDate = DateTime.Now, IsDeleted = false },
                    new Media { Id = Guid.Parse("D42D97CF-8470-4FCE-99B2-85FF4D0B3996"), FileSize = 100, FileName = "89374e88-b14c-4d38-b5cd-eacdc5ce3015.jpg", MediaType = MediaType.Image, CreatedDate = DateTime.Now, LastModifiedDate = DateTime.Now, IsDeleted = false },
                    new Media { Id = Guid.Parse("94E57AAC-1E2C-44F0-824F-986CAACD8D7B"), FileSize = 100, FileName = "68c7ff8f-014e-46c8-8daa-f35c646cc10a.jpg", MediaType = MediaType.Image, CreatedDate = DateTime.Now, LastModifiedDate = DateTime.Now, IsDeleted = false },
                    new Media { Id = Guid.Parse("6BBBA2AD-8DD0-46EB-85D6-B22D3A85823B"), FileSize = 100, FileName = "d74fd909-6fe0-4bc3-bf61-86d12dc98a2e.jpg", MediaType = MediaType.Image, CreatedDate = DateTime.Now, LastModifiedDate = DateTime.Now, IsDeleted = false },
                    new Media { Id = Guid.Parse("473CDB36-4998-481A-8119-CA2C42C52783"), FileSize = 100, FileName = "ffc255b3-07c8-4ee5-94e9-d472c6af3f07.jpg", MediaType = MediaType.Image, CreatedDate = DateTime.Now, LastModifiedDate = DateTime.Now, IsDeleted = false }
                );

                await context.SaveChangesAsync();
            }

            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new Product
                    {
                        Id = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Name = "iPhone 14 Pro Max",
                        Slug = "iphone-14-pro-max",
                        Price = 1024,
                        IsCallForPricing = false,
                        IsAllowToOrder = false,
                        StockQuantity = 50,
                        IsPublished = true,
                        DisplayOrder = 0,
                        BrandId = Guid.Parse("148DF6D1-65C1-4E00-B664-5F0A461160A9"),
                        ThumbnailImage = CheckAndAddMedia(context, Guid.Parse("6BBBA2AD-8DD0-46EB-85D6-B22D3A85823B")),
                        MetaTitle = "iPhone 14 Pro Max - Latest Model",
                        MetaKeywords = "iPhone 14, iPhone, Pro Max, Latest iPhone",
                        MetaDescription = "iPhone 14 Pro Max with stunning features and performance.",
                        CanonicalUrl = "iphone-14-pro-max",
                        OgTitle = "iPhone 14 Pro Max - Latest Model",
                        OgDescription = "iPhone 14 Pro Max with cutting-edge technology.",
                        OgImage = "images/iphone-14-pro-max.jpg",
                        OgUrl = "iphone-14-pro-max",
                        SchemaJson = "",
                        CreatedDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                        IsDeleted = false
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
                        ThumbnailImage = CheckAndAddMedia(context, Guid.Parse("473CDB36-4998-481A-8119-CA2C42C52783")),
                        MetaTitle = "Samsung Galaxy S22 Ultra 5G",
                        MetaKeywords = "Samsung Galaxy, Galaxy S22 Ultra, 5G, Samsung",
                        MetaDescription = "Samsung Galaxy S22 Ultra 5G with top-tier features and performance.",
                        CanonicalUrl = "samsung-galaxy-s22-ultra-5g",
                        OgTitle = "Samsung Galaxy S22 Ultra 5G",
                        OgDescription = "Samsung Galaxy S22 Ultra 5G with high-end specifications.",
                        OgImage = "images/samsung-galaxy-s22-ultra-5g.jpg",
                        OgUrl = "samsung-galaxy-s22-ultra-5g",
                        SchemaJson = "",
                        CreatedDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                        IsDeleted = false
                    }
                );

                await context.SaveChangesAsync();
            }

            if (!context.ProductAttributeValues.Any())
            {
                context.ProductAttributeValues.AddRange(
                    // Apple iPhone 14 Pro Max
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("dfc25c73-6f00-43e8-938f-cc42310338ae"),
                        AttributeId = Guid.Parse("45F254D5-7658-490C-B7C5-1895C04DAA86"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "2022, September 07"
                    },
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
                        Id = Guid.Parse("fb5c745d-6821-44f8-8bfc-3f59ad44ea7f"),
                        AttributeId = Guid.Parse("022C909E-BCF6-40CE-A490-E781557B9B96"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "Apple GPU (5-core graphics)"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("c0237a00-f71c-4984-a799-6a1054ede58f"),
                        AttributeId = Guid.Parse("0993DC9D-38B9-4C9A-A315-EC6E63A540EA"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "Apple A16 Bionic (4 nm)"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("3fb59467-1b7a-4456-98b5-84992454d9d9"),
                        AttributeId = Guid.Parse("74D4F4B8-64F7-46EE-9C15-3EE3743514BF"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "48 MP, f/1.8, 24mm (wide), 1/1.28\", 1.22µm, dual pixel PDAF, sensor-shift OIS"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("a4378078-1748-4a61-8ec2-a647550dfcdb"),
                        AttributeId = Guid.Parse("7D637BDF-7801-40C2-B813-4E3C68A0502B"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "4K@24/25/30/60fps, 1080p@25/30/60/120/240fps, 10-bit HDR"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("f3844a9b-c4ba-4c30-b9f3-b0e7ca7054e6"),
                        AttributeId = Guid.Parse("67054B20-DEB6-46DD-9E8F-EDAF0B55A551"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "Dual-LED dual-tone flash, HDR (photo/panorama)"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("2e609cf4-4351-4c6d-8c86-bb288596005b"),
                        AttributeId = Guid.Parse("D2641E35-ADE1-48AC-A660-1FA70C648757"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "4K@24/25/30/60fps, 1080p@25/30/60/120fps"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("b2573af2-e1ff-4d48-9b20-62f2e1ca669c"),
                        AttributeId = Guid.Parse("EAAE84DE-3092-4E5B-ADF4-98260ED31B39"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "12 MP, f/1.9, 23mm (wide), 1/3.6\", PDAF, OIS (unconfirmed)"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("a20f76d3-4c1a-41a2-8b1c-f5d47d1f7481"),
                        AttributeId = Guid.Parse("9F898DBD-1C78-4890-BA23-A2703DEF84F6"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "HDR, Cinematic mode (4K@24/30fps)"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("41250112-7527-4f32-9a33-9a27a81a2559"),
                        AttributeId = Guid.Parse("61EF7843-0D3A-4F3E-8F75-CFA2F9C366D4"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "Face ID, accelerometer, gyro, proximity, compass, barometer"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("da436d7d-893d-429c-9402-6cdf60d49d03"),
                        AttributeId = Guid.Parse("91995FC8-087C-42B5-989F-02738205C09C"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "No"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("688e1b0f-5082-4cab-81e1-ba823b32d1c9"),
                        AttributeId = Guid.Parse("B627C3B1-48DA-4A10-8E84-436532E3CF1E"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "Yes, with stereo speakers"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("af1bd123-c6e8-4b34-a57e-3f33094f07bf"),
                        AttributeId = Guid.Parse("3A79F080-A507-4016-89FE-608EF1A41E88"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "Nano-SIM and eSIM - International"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("d28542a6-a2f4-4b26-b056-c9fa2efa522f"),
                        AttributeId = Guid.Parse("D9D56DA3-BF2A-4FA0-957C-C6E586A99400"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "240 g (8.47 oz)"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("8cf1b681-5a2a-4e1d-8d8b-27c8ab6a7187"),
                        AttributeId = Guid.Parse("35E47E74-1D9A-4F8A-AF64-DFAB49030BB5"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "Glass front (Corning-made glass), glass back (Corning-made glass), stainless steel frame"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("51d48d7f-025d-4d37-9fd2-75a09052c70e"),
                        AttributeId = Guid.Parse("4DDAC538-909E-4E06-ACC9-E0763D7C59D4"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "160.7 x 77.6 x 7.9 mm (6.33 x 3.06 x 0.31 in)"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("0884ad2e-81b1-466d-8b01-3475e990fa84"),
                        AttributeId = Guid.Parse("BC9A2A55-FA69-4526-AA89-07A3B5B748BC"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "128GB 6GB RAM, 256GB 6GB RAM, 512GB 6GB RAM, 1TB 6GB RAM"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("9ef4022f-507c-4223-9e11-232810583423"),
                        AttributeId = Guid.Parse("79BBA792-9E11-44FA-842C-D2CA0919AB75"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "No"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("68b136fb-3419-421c-885e-083364cd0ddf"),
                        AttributeId = Guid.Parse("72BA299F-9AB6-4429-B083-1C673811672F"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "LTPO Super Retina XDR OLED, 120Hz, HDR10, Dolby Vision, 1000 nits (typ), 2000 nits (HBM)"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("32db522f-6ff9-4691-88f3-ebd1a96411b9"),
                        AttributeId = Guid.Parse("75CF4E98-7BA8-4190-98C9-2086D1B90A63"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "Ceramic Shield glass"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("2d726d86-1de8-4204-a17a-7730315509f8"),
                        AttributeId = Guid.Parse("841DE7A5-82CE-4F64-AEB2-23E1EA70DD2B"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "1290 x 2796 pixels, 19.5:9 ratio (~460 ppi density)"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("df33d0fc-86ed-49d3-9cd1-b6d1432edbee"),
                        AttributeId = Guid.Parse("D8ECB7AE-6539-4FD9-9F72-51A37BA754C4"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "6.7 inches, 110.2 cm2 (~88.3% screen-to-body ratio)"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("cb8a6824-c5c8-4dae-8640-91eebbc14b83"),
                        AttributeId = Guid.Parse("C50D9A30-E567-4553-85DB-7BB40125433F"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "5.3, A2DP, LE"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("cc425f97-91c4-4197-8b42-f65efafedb8a"),
                        AttributeId = Guid.Parse("3A95802A-B1FB-4CA6-88C1-87448981A019"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "Lightning, USB 2.0"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("81b538e3-6cce-40cb-8b3d-fc74a0c02d45"),
                        AttributeId = Guid.Parse("522FFA4D-D907-43E2-BB23-96FA7FEF24B1"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "Yes"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("c66cd791-0324-4eab-8b73-41823b97c0b5"),
                        AttributeId = Guid.Parse("600FA0C8-D3DE-4C78-9E2E-0B79D2A794F1"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "Wi-Fi 802.11 a/b/g/n/ac/6, dual-band, hotspot"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("c0aa391c-7fed-4632-89f5-38b14d91e509"),
                        AttributeId = Guid.Parse("C3F8CD5F-168D-4F4D-ADA3-1344CB7E75E7"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "GPS (L1+L5), GLONASS, GALILEO, BDS, QZSS"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("2ef1b5cc-d752-4a4f-a98d-b5afb83a1517"),
                        AttributeId = Guid.Parse("52832EB3-A80E-4828-B0EC-19B458FE1DF7"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "Yes"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("3f2f7549-7ea8-4fe6-bfa5-9c82a68664ef"),
                        AttributeId = Guid.Parse("409F8969-4650-4B2F-94F0-A5BDFF20CA0C"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "Yes"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("104761b5-2e57-42f2-b3bf-d6053ba9f557"),
                        AttributeId = Guid.Parse("11D6F0D6-D9BC-4B33-897A-BC66E6B8BF5D"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "No"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("27e730de-877d-4b84-9293-3c2cde3ad0e0"),
                        AttributeId = Guid.Parse("F60DA4E8-2D75-4A5D-B6D1-EAD215939720"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "No"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("f39e4815-3aef-444d-834c-d544e3a7c71e"),
                        AttributeId = Guid.Parse("99E02E27-604B-4487-A6EE-FEF0DFD90051"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "Yes"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("dd1b7178-af01-45ff-bf68-d1cf4cadc38e"),
                        AttributeId = Guid.Parse("80BBADDC-4D0A-44C7-BCE0-041BA78139F9"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "Li-Ion 4323 mAh, non-removable (16.68 Wh)"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("738b7e96-ceff-4a21-b93f-ba6ea6cbaa2d"),
                        AttributeId = Guid.Parse("7B314B22-4695-4140-9861-E44D08624052"),
                        ProductId = Guid.Parse("1761d0be-6a7d-49c2-bb97-d40cea3eb8d5"),
                        Value = "Wired, PD2.0, 50% in 30 min (advertised)"
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
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("f9325ec1-d580-4846-9cfd-da70e8587746"),
                        AttributeId = Guid.Parse("022C909E-BCF6-40CE-A490-E781557B9B96"),
                        ProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Value = "Xclipse 920 - Europe"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("de45c963-c688-4b71-968e-51e7fd33a9d5"),
                        AttributeId = Guid.Parse("0993DC9D-38B9-4C9A-A315-EC6E63A540EA"),
                        ProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Value = "Exynos 2200 (4 nm) - Europe"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("d4ffe6d2-b5f2-4d89-8a29-5dbe9abe6d9b"),
                        AttributeId = Guid.Parse("74D4F4B8-64F7-46EE-9C15-3EE3743514BF"),
                        ProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Value = "108 MP, f/1.8, 23mm (wide), 1/1.33\", 0.8µm, PDAF, Laser AF, OIS"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("f1189432-7d0a-4db5-b6d4-c4d833907811"),
                        AttributeId = Guid.Parse("7D637BDF-7801-40C2-B813-4E3C68A0502B"),
                        ProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Value = "8K@24fps, 4K@30/60fps, 1080p@30/60/240fps, 720p@960fps"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("57fb1584-eba3-456e-a649-ac6020f0a1fc"),
                        AttributeId = Guid.Parse("67054B20-DEB6-46DD-9E8F-EDAF0B55A551"),
                        ProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Value = "LED flash, auto-HDR, panorama"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("97314ad3-e81d-4c3e-9c7b-31eeaecfbbdd"),
                        AttributeId = Guid.Parse("D2641E35-ADE1-48AC-A660-1FA70C648757"),
                        ProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Value = "4K@30/60fps, 1080p@30fps"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("5c387e5c-225d-4dbd-bab8-ada76998a3c8"),
                        AttributeId = Guid.Parse("EAAE84DE-3092-4E5B-ADF4-98260ED31B39"),
                        ProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Value = "40 MP, f/2.2, 26mm (wide), 1/2.82"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("c1d28b94-237f-4f34-829c-c58d3426c33e"),
                        AttributeId = Guid.Parse("9F898DBD-1C78-4890-BA23-A2703DEF84F6"),
                        ProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Value = "Dual video call, Auto-HDR"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("d7bdcaa5-0a4f-4e0c-a733-dde8d557c6e5"),
                        AttributeId = Guid.Parse("61EF7843-0D3A-4F3E-8F75-CFA2F9C366D4"),
                        ProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Value = "Fingerprint (under display, ultrasonic), accelerometer, gyro, proximity, compass, barometer"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("c7789177-2fae-48c4-b7fc-fd3dce22c793"),
                        AttributeId = Guid.Parse("91995FC8-087C-42B5-989F-02738205C09C"),
                        ProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Value = "No"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("1285ff55-55dd-4f18-883a-7cdf13866b62"),
                        AttributeId = Guid.Parse("B627C3B1-48DA-4A10-8E84-436532E3CF1E"),
                        ProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Value = "Yes, with stereo speakers"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("7e912183-b47f-4462-ae88-64d8fcda21b8"),
                        AttributeId = Guid.Parse("3A79F080-A507-4016-89FE-608EF1A41E88"),
                        ProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Value = "Nano-SIM and eSIM or Dual SIM"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("6f882e4d-732a-4300-ad52-43dee1b997a9"),
                        AttributeId = Guid.Parse("D9D56DA3-BF2A-4FA0-957C-C6E586A99400"),
                        ProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Value = "228 g / 229 g (mmWave) (8.04 oz)"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("80c74fbc-b652-4e97-8e7d-ecb83b97c569"),
                        AttributeId = Guid.Parse("35E47E74-1D9A-4F8A-AF64-DFAB49030BB5"),
                        ProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Value = "Glass front (Gorilla Glass Victus+), glass back (Gorilla Glass Victus+), aluminum frame"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("7ec2095b-5deb-4c57-864c-44aa4fa4a04b"),
                        AttributeId = Guid.Parse("4DDAC538-909E-4E06-ACC9-E0763D7C59D4"),
                        ProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Value = "163.3 x 77.9 x 8.9 mm (6.43 x 3.07 x 0.35 in)"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("9f2d2ee3-9f56-44f5-8930-723b05f9ec61"),
                        AttributeId = Guid.Parse("BC9A2A55-FA69-4526-AA89-07A3B5B748BC"),
                        ProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Value = "128GB 6GB RAM, 256GB 6GB RAM, 512GB 6GB RAM, 1TB 6GB RAM"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("70a60b25-313d-46df-9f10-caa46189597d"),
                        AttributeId = Guid.Parse("79BBA792-9E11-44FA-842C-D2CA0919AB75"),
                        ProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Value = "No"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("d20652bb-b895-4857-8598-1a21bf72254f"),
                        AttributeId = Guid.Parse("72BA299F-9AB6-4429-B083-1C673811672F"),
                        ProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Value = "Dynamic AMOLED 2X, 120Hz, HDR10+, 1750 nits (peak)"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("19e06fdb-5b94-41d2-adb2-03a0a317a86c"),
                        AttributeId = Guid.Parse("75CF4E98-7BA8-4190-98C9-2086D1B90A63"),
                        ProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Value = "Corning Gorilla Glass Victus+"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("bde36691-20ca-49c4-b55f-a229a667db66"),
                        AttributeId = Guid.Parse("841DE7A5-82CE-4F64-AEB2-23E1EA70DD2B"),
                        ProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Value = "1440 x 3088 pixels (~500 ppi density)"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("d72bce98-5810-423b-8967-6c88f7a61087"),
                        AttributeId = Guid.Parse("D8ECB7AE-6539-4FD9-9F72-51A37BA754C4"),
                        ProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Value = "6.8 inches, 114.7 cm2 (~90.2% screen-to-body ratio)"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("92e1cc21-c11b-4f5b-a9d4-66b4d437534e"),
                        AttributeId = Guid.Parse("C50D9A30-E567-4553-85DB-7BB40125433F"),
                        ProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Value = "5.3, A2DP, LE"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("d0f8d9f3-a077-46a2-ab57-10541bdfd126"),
                        AttributeId = Guid.Parse("3A95802A-B1FB-4CA6-88C1-87448981A019"),
                        ProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Value = "USB Type-C 3.2, OTG"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("e9c5ad22-ea67-459f-a20f-e24d2343fe5f"),
                        AttributeId = Guid.Parse("522FFA4D-D907-43E2-BB23-96FA7FEF24B1"),
                        ProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Value = "Yes"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("3777c617-2946-46af-8f72-fec8e6840c15"),
                        AttributeId = Guid.Parse("600FA0C8-D3DE-4C78-9E2E-0B79D2A794F1"),
                        ProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Value = "Wi-Fi 802.11 a/b/g/n/ac/6e, dual-band, Wi-Fi Direct"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("6963bfdf-5744-4143-bc9c-96c5c12a71c1"),
                        AttributeId = Guid.Parse("C3F8CD5F-168D-4F4D-ADA3-1344CB7E75E7"),
                        ProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Value = "GPS, GLONASS, BDS, GALILEO"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("0d6cbb25-2327-4271-9c83-1860d4bc4f3e"),
                        AttributeId = Guid.Parse("52832EB3-A80E-4828-B0EC-19B458FE1DF7"),
                        ProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Value = "Yes"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("bbe15c3b-e7f8-4c68-bdae-49014deffceb"),
                        AttributeId = Guid.Parse("409F8969-4650-4B2F-94F0-A5BDFF20CA0C"),
                        ProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Value = "Yes"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("89831729-7778-4bd4-babc-b402c1f54d61"),
                        AttributeId = Guid.Parse("11D6F0D6-D9BC-4B33-897A-BC66E6B8BF5D"),
                        ProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Value = "No"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("2bf0c423-25b2-413d-9e40-79d1b8a3c6c6"),
                        AttributeId = Guid.Parse("F60DA4E8-2D75-4A5D-B6D1-EAD215939720"),
                        ProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Value = "No"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("6e5f26a3-5c24-4c71-a681-646fb63069c5"),
                        AttributeId = Guid.Parse("99E02E27-604B-4487-A6EE-FEF0DFD90051"),
                        ProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Value = "Yes"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("5eb258f8-f42d-4268-9869-abdb9943b459"),
                        AttributeId = Guid.Parse("80BBADDC-4D0A-44C7-BCE0-041BA78139F9"),
                        ProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Value = "Li-Ion 5000 mAh, non-removable"
                    },
                    new ProductAttributeValue
                    {
                        Id = Guid.Parse("e6434c4c-5195-42af-94b7-77d7def0e05b"),
                        AttributeId = Guid.Parse("7B314B22-4695-4140-9861-E44D08624052"),
                        ProductId = Guid.Parse("8c1acba8-0487-47fe-9491-e8334dc94d23"),
                        Value = "45W wired, PD3.0"
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
                        CreatedDate = DateTime.Now,
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
