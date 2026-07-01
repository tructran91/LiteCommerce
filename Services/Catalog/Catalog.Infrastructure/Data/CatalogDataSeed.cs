using Catalog.Core.Entities;
using Catalog.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Data
{
    public static class CatalogDataSeed
    {
        public static async Task SeedAsync(CatalogContext context)
        {
            // Check if database already has data
            if (await context.Brands.AnyAsync())
            {
                return;
            }

            // 1. Seed Brands (no FK)
            var brands = new List<Brand>
            {
                new() { Id = Guid.Parse("5fb5f20c-bf57-4907-bc13-08de78272836"), Name = "Apple", Slug = "apple", IsPublished = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("75138068-7176-4afb-bc14-08de78272836"), Name = "Samsung", Slug = "samsung", IsPublished = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("72c3519f-8830-4e90-bc15-08de78272836"), Name = "Dell", Slug = "dell", IsPublished = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("ffaf754f-f5c2-4853-bc16-08de78272836"), Name = "Hp", Slug = "hp", IsPublished = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("718b4a11-1b02-44c6-bc17-08de78272836"), Name = "Lenovo", Slug = "lenovo", IsPublished = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("d0efbf1c-8d93-43cb-bc18-08de78272836"), Name = "Nokia", Slug = "nokia", IsPublished = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("47096813-224e-42c7-bc19-08de78272836"), Name = "Oppo", Slug = "oppo", IsPublished = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("29364927-6439-473f-bc1a-08de78272836"), Name = "Asus", Slug = "asus", IsPublished = true, CreatedDate = DateTime.UtcNow }
            };
            await context.Brands.AddRangeAsync(brands);

            // 2. Seed ProductAttributeGroups (no FK)
            var attributeGroups = new List<ProductAttributeGroup>
            {
                new() { Id = Guid.Parse("206e76f1-1f4d-4c0b-84b2-38431548ea49"), Name = "General", CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("b8dd305a-abf7-41f1-83fe-3ab7432926c9"), Name = "Screen", CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("4d6e8118-5cf2-4de6-ba04-3cf53425f910"), Name = "Camera", CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("58b6906e-8589-42ca-8d8e-ea85867d4cb7"), Name = "Connectivity", CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("b0c12b2c-7f15-4dea-90d0-08de7828f656"), Name = "Battery", CreatedDate = DateTime.UtcNow }
            };
            await context.ProductAttributeGroups.AddRangeAsync(attributeGroups);

            // 3. Seed ProductOptions (no FK)
            var productOptions = new List<ProductOption>
            {
                new() { Id = Guid.Parse("452d918b-b5bf-41b7-90e6-51cad397b292"), Name = "Color" },
                new() { Id = Guid.Parse("a900d061-715e-4682-b493-4a3f75f95b01"), Name = "Size" },
                new() { Id = Guid.Parse("646dceee-5eca-46a0-8a5f-db8d280dab4a"), Name = "Warranty" }
            };
            await context.ProductOptions.AddRangeAsync(productOptions);

            // 4. Seed Categories (FK to Medias - null, and self-reference)
            var categories = new List<Category>
            {
                new() { Id = Guid.Parse("8ac51586-431d-4642-8435-5926cb6c04f4"), Name = "Accessories", Slug = "accessories", IsPublished = true, IncludeInMenu = true, DisplayOrder = 0, MetaTitle = "Accessories - Shop all accessories", MetaKeywords = "accessories, gadgets, tech", MetaDescription = "Browse a wide selection of accessories including gadgets, tech gear, and more.", OgTitle = "Accessories - Shop the latest tech accessories", OgDescription = "Discover the best accessories for your devices", OgImage = "/images/accessories.jpg", OgUrl = "/categories/accessories", CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("f64602e6-d373-42ef-a3be-4360449bada1"), Name = "Computers", Slug = "computers", IsPublished = true, IncludeInMenu = true, DisplayOrder = 0, MetaTitle = "Computers - High-performance computers", MetaKeywords = "computers, laptops, desktops, high-performance", MetaDescription = "Explore a variety of high-performance computers including laptops, desktops, and gaming rigs.", OgTitle = "Computers - Powerful machines for every need", OgDescription = "Browse through our selection of powerful computers for work, gaming, and more", OgImage = "/images/computers.jpg", OgUrl = "/categories/computers", CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("7e226ecb-6fc1-4602-bf0b-d14d02a14555"), Name = "Phones", Slug = "phones", IsPublished = true, IncludeInMenu = true, DisplayOrder = 0, MetaTitle = "Phones - Latest mobile phones", MetaKeywords = "phones, smartphones, mobile phones", MetaDescription = "Shop for the latest smartphones and mobile phones at great prices.", OgTitle = "Phones - Shop the latest smartphones", OgDescription = "Browse through our collection of the latest mobile phones", OgImage = "/images/phones.jpg", OgUrl = "/categories/phones", CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("c3f05c5c-3fa3-4c6d-81c8-e33526e39511"), Name = "Cables", Slug = "cables", ParentId = Guid.Parse("8ac51586-431d-4642-8435-5926cb6c04f4"), IsPublished = true, IncludeInMenu = true, DisplayOrder = 0, MetaTitle = "Cables - Find the best cables", MetaKeywords = "cables, charging cables, tech accessories", MetaDescription = "Shop high-quality cables for your gadgets and devices.", OgTitle = "Cables - The best selection of cables online", OgDescription = "Browse our extensive collection of charging and data cables", OgImage = "/images/cables.jpg", OgUrl = "/categories/cables", CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("d8c4a2d8-3f4d-4d3c-923c-f4f46482ab7a"), Name = "Headphones", Slug = "headphones", ParentId = Guid.Parse("8ac51586-431d-4642-8435-5926cb6c04f4"), IsPublished = true, IncludeInMenu = true, DisplayOrder = 0, MetaTitle = "Headphones - Quality sound for all", MetaKeywords = "headphones, audio, music", MetaDescription = "Shop for high-quality headphones and audio accessories.", OgTitle = "Headphones - Superior sound quality", OgDescription = "Discover top-rated headphones for music lovers", OgImage = "/images/headphones.jpg", OgUrl = "/categories/headphones", CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("633b7e0d-e227-450e-a3a2-574125aa6024"), Name = "USB Drives", Slug = "usb-drives", ParentId = Guid.Parse("8ac51586-431d-4642-8435-5926cb6c04f4"), IsPublished = true, IncludeInMenu = true, DisplayOrder = 0, MetaTitle = "USB Drives - Portable storage solutions", MetaKeywords = "usb drives, storage, portable storage", MetaDescription = "Browse and buy USB drives for secure and portable data storage.", OgTitle = "USB Drives - Shop portable storage devices", OgDescription = "Find the best USB drives for your storage needs", OgImage = "/images/usb-drives.jpg", OgUrl = "/categories/usb-drives", CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("acfb3c87-502b-44a0-b39b-6b644c57ffb6"), Name = "Gaming", Slug = "gaming", ParentId = Guid.Parse("f64602e6-d373-42ef-a3be-4360449bada1"), IsPublished = true, IncludeInMenu = true, DisplayOrder = 0, MetaTitle = "Gaming - Top gaming equipment", MetaKeywords = "gaming, gaming laptops, gaming desktops", MetaDescription = "Find the best gaming equipment including gaming laptops and desktops.", OgTitle = "Gaming - Shop top gaming gear", OgDescription = "Explore gaming laptops, desktops, and accessories", OgImage = "/images/gaming.jpg", OgUrl = "/categories/gaming", CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("761d55da-023c-41e0-ace7-c7316602e0f6"), Name = "MacBook", Slug = "macbook", ParentId = Guid.Parse("f64602e6-d373-42ef-a3be-4360449bada1"), IsPublished = true, IncludeInMenu = true, DisplayOrder = 0, MetaTitle = "MacBook - Apple laptops", MetaKeywords = "MacBook, Apple laptops, MacBook Pro", MetaDescription = "Shop MacBook and MacBook Pro models for powerful performance and sleek design.", OgTitle = "MacBook - The best Apple laptops", OgDescription = "Explore MacBook models for premium performance and design", OgImage = "/images/macbook.jpg", OgUrl = "/categories/macbook", CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("5a6895a9-ce31-4a37-8300-60bb6a939c2c"), Name = "iPhone", Slug = "iphone", ParentId = Guid.Parse("7e226ecb-6fc1-4602-bf0b-d14d02a14555"), IsPublished = true, IncludeInMenu = true, DisplayOrder = 0, MetaTitle = "iPhone - The latest iPhone models", MetaKeywords = "iPhone, Apple, smartphones", MetaDescription = "Shop for the latest iPhone models and accessories.", OgTitle = "iPhone - Buy the latest iPhone", OgDescription = "Explore the latest iPhone models from Apple", OgImage = "/images/iphone.jpg", OgUrl = "/categories/iphone", CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("a5924453-4608-427f-9751-717f217ea569"), Name = "Basic Phones", Slug = "basic-phones", ParentId = Guid.Parse("7e226ecb-6fc1-4602-bf0b-d14d02a14555"), IsPublished = true, IncludeInMenu = true, DisplayOrder = 0, MetaTitle = "Basic Phones - Affordable mobile solutions", MetaKeywords = "basic phones, mobile, affordable phones", MetaDescription = "Browse affordable and reliable basic phones for simple communication.", OgTitle = "Basic Phones - Shop simple and reliable mobile phones", OgDescription = "Find durable and affordable basic mobile phones", OgImage = "/images/basic-phones.jpg", OgUrl = "/categories/basic-phones", CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("d5f3b0e4-350d-4188-a0d1-28ebc5cc3aea"), Name = "Gaming", Slug = "gaming", ParentId = Guid.Parse("7e226ecb-6fc1-4602-bf0b-d14d02a14555"), IsPublished = true, IncludeInMenu = true, DisplayOrder = 0, MetaTitle = "Gaming - Explore the ultimate gaming devices", MetaKeywords = "gaming, gaming devices, gaming phones", MetaDescription = "Discover high-performance gaming phones and devices for gamers.", OgTitle = "Gaming - High-performance gaming phones", OgDescription = "Explore the latest gaming phones and devices for the ultimate gaming experience.", OgImage = "/images/gaming.jpg", OgUrl = "/categories/gaming", CreatedDate = DateTime.UtcNow }
            };
            await context.Categories.AddRangeAsync(categories);

            // 5. Seed ProductAttributes (FK to ProductAttributeGroups)
            var attributes = new List<ProductAttribute>
            {
                new() { Id = Guid.Parse("6dda4bd4-538c-4a56-bb9a-804e4e477456"), Name = "CPU", GroupId = Guid.Parse("206e76f1-1f4d-4c0b-84b2-38431548ea49"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("18bbcb21-0c25-4543-af2a-b855e87e01f4"), Name = "OS", GroupId = Guid.Parse("206e76f1-1f4d-4c0b-84b2-38431548ea49"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("022c909e-bcf6-40ce-a490-e781557b9b96"), Name = "GPU", GroupId = Guid.Parse("206e76f1-1f4d-4c0b-84b2-38431548ea49"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("0993dc9d-38b9-4c9a-a315-ec6e63a540ea"), Name = "RAM", GroupId = Guid.Parse("206e76f1-1f4d-4c0b-84b2-38431548ea49"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("0ae35309-b38e-4002-a418-8c3f2d5ff66b"), Name = "Storage capacity", GroupId = Guid.Parse("206e76f1-1f4d-4c0b-84b2-38431548ea49"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("74d4f4b8-64f7-46ee-9c15-3ee3743514bf"), Name = "Widescreen", GroupId = Guid.Parse("b8dd305a-abf7-41f1-83fe-3ab7432926c9"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("7d637bdf-7801-40c2-b813-4e3c68a0502b"), Name = "Display technology", GroupId = Guid.Parse("b8dd305a-abf7-41f1-83fe-3ab7432926c9"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("67054b20-deb6-46dd-9e8f-edaf0b55a551"), Name = "Screen resolution", GroupId = Guid.Parse("b8dd305a-abf7-41f1-83fe-3ab7432926c9"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("d2641e35-ade1-48ac-a660-1fa70c648757"), Name = "SIM", GroupId = Guid.Parse("58b6906e-8589-42ca-8d8e-ea85867d4cb7"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("9f898dbd-1c78-4890-ba23-a2703def84f6"), Name = "Mobile network", GroupId = Guid.Parse("58b6906e-8589-42ca-8d8e-ea85867d4cb7"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("2f16e475-7ccf-4bfd-ab37-46fe0dd725bf"), Name = "Wifi", GroupId = Guid.Parse("58b6906e-8589-42ca-8d8e-ea85867d4cb7"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("1416eaa3-2f64-4fb9-966a-86c6dd76166a"), Name = "Bluetooth", GroupId = Guid.Parse("58b6906e-8589-42ca-8d8e-ea85867d4cb7"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("775a1c47-acd5-4794-a859-09f9aa6cc901"), Name = "Other connections", GroupId = Guid.Parse("58b6906e-8589-42ca-8d8e-ea85867d4cb7"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("fe158671-2d0c-4775-ace8-0017f3e21f02"), Name = "Main camera", GroupId = Guid.Parse("4d6e8118-5cf2-4de6-ba04-3cf53425f910"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("8d5fa038-27c6-4184-9b4c-eb50ec7ceb00"), Name = "Sub camera", GroupId = Guid.Parse("4d6e8118-5cf2-4de6-ba04-3cf53425f910"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("23be21d4-3622-4206-52d1-08de783534be"), Name = "Battery capacity", GroupId = Guid.Parse("b0c12b2c-7f15-4dea-90d0-08de7828f656"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("9c073c84-a0ea-4a18-52d2-08de783534be"), Name = "Battery type", GroupId = Guid.Parse("b0c12b2c-7f15-4dea-90d0-08de7828f656"), CreatedDate = DateTime.UtcNow }
            };
            await context.ProductAttributes.AddRangeAsync(attributes);

            // 6. Seed Products (FK to Brands and Medias)
            var products = new List<Product>
            {
                new() 
                { 
                    Id = Guid.Parse("7eb16dfc-78cf-427c-d6a1-08de78387ceb"), 
                    Name = "iPhone 17 Pro Max 256GB", 
                    Slug = "iphone-17-pro-max-256gb",
                    ShortDescription = "<ul><li>Apple A19 Pro 6-core chip</li><li>RAM: 12 GB</li><li>Capacity: 256 GB</li><li>Rear camera: Main 48 MP &amp; Secondary 48 MP, 48 MP</li><li>Front camera: 18 MP</li><li>37-hour battery life, 40W charging</li></ul>",
                    Description = "<p>Key features of the iPhone 17 Pro Max:</p><ul><li>Solid unibody aluminum design, featuring the largest screen ever.</li><li>Brightest and largest 120Hz ProMotion display for super smooth images and immersive movie viewing.</li><li>Professional photography with a 48MP triple camera system.</li><li>Utilizes the Apple A19 Pro chip, ensuring incredibly fast performance and effortless processing.</li><li>Incredible battery life, the longest-lasting iPhone ever, allowing for up to 37 hours of video playback.</li></ul>",
                    IsPublished = true,
                    Price = 38000000.00m,
                    BrandId = Guid.Parse("5fb5f20c-bf57-4907-bc13-08de78272836"),
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Giá iPhone 17 Pro Max 256GB giảm đến 5tr khi thanh toán qua Kredivo",
                    MetaKeywords = "iphone 17 pro max, iphone 17 pro max 2025, giá iphone 17 pro max, apple iphone 17 pro max, iphone 17 pro max 256gb",
                    MetaDescription = "iPhone 17 Pro Max (256GB, 512GB, 1TB, 2TB) giá tốt, có màu cam vũ trụ, xanh đậm, thu cũ giảm đến 3tr, giảm đến 5tr khi thanh toán qua Kredivo, trả chậm 0%. Mua ngay!"
                },
                new() 
                { 
                    Id = Guid.Parse("98770860-2a59-42c4-d6a2-08de78387ceb"), 
                    Name = "iPhone 16 Pro Max 256GB", 
                    Slug = "iphone-16-pro-max-256gb",
                    ShortDescription = "<ul><li>Apple A18 Pro 6-core chip</li><li>RAM: 8 GB</li><li>Capacity: 256 GB</li><li>Rear camera: Main 48 MP &amp; Secondary 48 MP, 12 MP</li><li>Front camera: 12 MP</li><li>33-hour battery life, 20W charging.</li></ul>",
                    Description = "<h3 class=\"ql-align-justify\"><strong>Overview of iPhone 16 Pro Max and iPhone 16 Pro</strong></h3><p class=\"ql-align-justify\">The iPhone 16 Pro and iPhone 16 Pro Max share many similarities but also have some important differences. Both use a titanium frame with a frosted glass finish and support IP68 water resistance. In terms of color, both versions come in four options: Natural Titanium, White Titanium, Black Titanium, and Desert Titanium.</p><p class=\"ql-align-justify\">Both models are equipped with an Action Button and a Camera Control button for quick camera control. The iPhone 16 Pro Max has a 6.9-inch Super Retina XDR OLED display, larger than the 6.3-inch screen of the iPhone 16 Pro. Both devices have a maximum brightness of 2000 nits and use the A18 Pro chip for powerful performance.</p><p class=\"ql-align-justify\">The iPhone 16 Pro Max boasts better battery life with 33 hours of video playback, compared to 27 hours for the iPhone 16 Pro. Storage on the iPhone 16 Pro Max starts at 256 GB, while the iPhone 16 Pro offers an additional 128 GB option.</p><p><br></p>",
                    IsPublished = true,
                    Price = 31590000.00m,
                    BrandId = Guid.Parse("5fb5f20c-bf57-4907-bc13-08de78272836"),
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "iPhone 16 Pro Max giá tốt, giảm đến 4.2tr, BH 1 năm",
                    MetaKeywords = "điện thoại iphone 16 pro max, iphone 16 pro max, iphone 16 pro max 256gb",
                    MetaDescription = "iPhone 16 Pro Max 256GB giá tốt, giảm ngay 4tr, thu cũ trợ giá đến 2tr, bảo hành chính hãng 1 năm, trả chậm 0% lãi suất, hư gì đổi nấy 12 tháng. Mua ngay!"
                },
                new() 
                { 
                    Id = Guid.Parse("012d01f5-1629-4a51-d6a3-08de78387ceb"), 
                    Name = "Samsung Galaxy S25 FE 5G 8GB/128GB", 
                    Slug = "samsung-galaxy-s25-fe-5g-8gb128gb",
                    ShortDescription = "<ul><li>Chip Exynos 2400 10 nhân</li><li>RAM: 8 GB</li><li>Dung lượng: 128 GB</li><li>Camera sau: Chính 50 MP &amp; Phụ 12 MP, 8 MP</li><li>Camera trước: 12 MP</li><li>Pin 4900 mAh, Sạc 45 W</li></ul>",
                    Description = "<p class=\"ql-align-justify\">Samsung Galaxy S25 FE không chỉ là bản nâng cấp phần cứng, mà còn là dấu mốc quan trọng cho trải nghiệm di động tương lai. Thiết bị kết hợp hiệu năng mạnh mẽ với trí tuệ nhân tạo Galaxy AI, mang đến một trợ lý cá nhân thông minh, luôn thấu hiểu và chủ động hỗ trợ. Đồng thời, đây cũng là mẫu FE mỏng nhẹ nhất, kết hợp thiết kế tinh tế cùng nhiều tính năng hiện đại.</p>",
                    IsPublished = true,
                    Price = 14050000.00m,
                    BrandId = Guid.Parse("75138068-7176-4afb-bc14-08de78272836"),
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Samsung Galaxy S25 FE 5G 8GB/128GB giảm ngay 2tr, góp 0%",
                    MetaKeywords = "Samsung Galaxy S25 FE 5G 8GB/128GB, s25 fe, Samsung galaxy s25 fe, glx s25, glx s25 fe, Samsung s25, Samsung s25 fe, s25 fe, s25fe, s25 fe, s25 fe, galaxy s25fe",
                    MetaDescription = "Mua Samsung Galaxy S25 FE 5G 8GB/128GB giá tốt, giảm đến 2 triệu, thu cũ trợ giá đến 1.5tr, trả chậm 0% lãi suất - trả trước 0đ, hư gì đổi nấy 12 tháng. Mua ngay!"
                },
                new() 
                { 
                    Id = Guid.Parse("6ff755bd-47ac-4397-d6a4-08de78387ceb"), 
                    Name = "Laptop Dell 15 DC15250 - DC5I5357W1 (i5 1334U, 16GB, 512GB, Full HD 120Hz, OfficeH24+365, Win11)", 
                    Slug = "laptop-dell-15-dc15250---dc5i5357w1-i5-1334u-16gb-512gb-full-hd-120hz-officeh24365-win11",
                    Description = "<p>Chiếc laptop Dell 15 DC15250 i5 1334U (DC5I5357W1) là sản phẩm lý tưởng cho học sinh, sinh viên và nhân viên văn phòng, thậm chí đáp ứng tốt nhu cầu thiết kế đồ họa cơ bản. Với hiệu năng ổn định, thiết kế thanh lịch và màn hình sắc nét, chiếc laptop này hứa hẹn mang đến trải nghiệm tuyệt vời trong công việc và giải trí, là một lựa chọn đáng cân nhắc trong phân khúc giá.</p>",
                    IsPublished = true,
                    Price = 17490000.00m,
                    BrandId = Guid.Parse("72c3519f-8830-4e90-bc15-08de78272836"),
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Dell 15 DC15250 i5 1334U (DC5I5357W1) giá tốt, bảo hành 1 năm",
                    MetaKeywords = "Dell 15 DC15250 i5 1334U/16GB/512GB/120Hz/OfficeHS24+365/Win11 (DC5I5357W1), Dell 15 DC15250 i5 1334U (DC5I5357W1), Dell 15 DC15250 i5 1334U (DC5I5357W1), Laptop Dell 15 DC15250 i5 1334U/16GB/512GB/120Hz/OfficeHS24+365/Win11 (DC5I5357W1), giá Dell 15 DC15250 i5 1334U/16GB/512GB/120Hz/OfficeHS24+365/Win11 (DC5I5357W1), thông tin Dell 15 DC15250 i5 1334U/16GB/512GB/120Hz/OfficeHS24+365/Win11 (DC5I5357W1)",
                    MetaDescription = "Laptop Dell 15 DC15250 i5 1334U (DC5I5357W1) giá tốt, trả chậm 0%. Giảm đến 10% qua Kredivo, hư gì đổi nấy 12 tháng, bảo hành chính hãng 1 năm. Mua ngay!"
                }
            };
            await context.Products.AddRangeAsync(products);

            // 7. Seed ProductCategories (FK to Categories and Products)
            var productCategories = new List<ProductCategory>
            {
                new() { Id = Guid.Parse("48a26b27-90a3-4348-d973-08de78387cfa"), CategoryId = Guid.Parse("5a6895a9-ce31-4a37-8300-60bb6a939c2c"), ProductId = Guid.Parse("7eb16dfc-78cf-427c-d6a1-08de78387ceb"), DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("589d7cf8-4ec4-4000-d974-08de78387cfa"), CategoryId = Guid.Parse("7e226ecb-6fc1-4602-bf0b-d14d02a14555"), ProductId = Guid.Parse("7eb16dfc-78cf-427c-d6a1-08de78387ceb"), DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("a973c00d-78fd-4d37-d975-08de78387cfa"), CategoryId = Guid.Parse("7e226ecb-6fc1-4602-bf0b-d14d02a14555"), ProductId = Guid.Parse("98770860-2a59-42c4-d6a2-08de78387ceb"), DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("f2d30039-00e3-4dd0-d976-08de78387cfa"), CategoryId = Guid.Parse("5a6895a9-ce31-4a37-8300-60bb6a939c2c"), ProductId = Guid.Parse("98770860-2a59-42c4-d6a2-08de78387ceb"), DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("3c548a37-8de6-460f-d977-08de78387cfa"), CategoryId = Guid.Parse("7e226ecb-6fc1-4602-bf0b-d14d02a14555"), ProductId = Guid.Parse("012d01f5-1629-4a51-d6a3-08de78387ceb"), DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("09739461-a944-4d12-d978-08de78387cfa"), CategoryId = Guid.Parse("d5f3b0e4-350d-4188-a0d1-28ebc5cc3aea"), ProductId = Guid.Parse("012d01f5-1629-4a51-d6a3-08de78387ceb"), DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("0404d76a-b7a5-4b46-d979-08de78387cfa"), CategoryId = Guid.Parse("acfb3c87-502b-44a0-b39b-6b644c57ffb6"), ProductId = Guid.Parse("6ff755bd-47ac-4397-d6a4-08de78387ceb"), DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("19a78608-0c04-4a8d-d97a-08de78387cfa"), CategoryId = Guid.Parse("f64602e6-d373-42ef-a3be-4360449bada1"), ProductId = Guid.Parse("6ff755bd-47ac-4397-d6a4-08de78387ceb"), DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow }
            };
            await context.ProductCategories.AddRangeAsync(productCategories);

            // 9. Seed ProductAttributeValues (FK to ProductAttributes and Products)
            var attributeValues = GetProductAttributeValues();
            await context.ProductAttributeValues.AddRangeAsync(attributeValues);

            // 10. Seed ProductTemplates (no FK)
            var productTemplates = new List<ProductTemplate>
            {
                new() { Id = Guid.Parse("ba82cd8d-198d-4106-bb8a-08de7829c8d9"), Name = "Phone", CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("40e153ad-35e8-4032-b848-08de783a4442"), Name = "Laptop", CreatedDate = DateTime.UtcNow }
            };
            await context.ProductTemplates.AddRangeAsync(productTemplates);

            // 11. Seed ProductTemplateProductAttributes (FK to ProductTemplates and ProductAttributes)
            var templateAttributes = GetProductTemplateProductAttributes();
            await context.ProductTemplateProductAttributes.AddRangeAsync(templateAttributes);

            await context.SaveChangesAsync();
        }

        private static List<ProductAttributeValue> GetProductAttributeValues()
        {
            return new List<ProductAttributeValue>
            {
                // iPhone 17 Pro Max
                new() { Id = Guid.Parse("aabda6f6-f1d6-482c-9b5e-08de78387cf8"), AttributeId = Guid.Parse("fe158671-2d0c-4775-ace8-0017f3e21f02"), ProductId = Guid.Parse("7eb16dfc-78cf-427c-d6a1-08de78387ceb"), Value = "48 MP" },
                new() { Id = Guid.Parse("c561d87e-fa22-4aa3-9b5f-08de78387cf8"), AttributeId = Guid.Parse("23be21d4-3622-4206-52d1-08de783534be"), ProductId = Guid.Parse("7eb16dfc-78cf-427c-d6a1-08de78387ceb"), Value = "37 hours" },
                new() { Id = Guid.Parse("ffb18944-15d6-44a0-9b60-08de78387cf8"), AttributeId = Guid.Parse("9c073c84-a0ea-4a18-52d2-08de783534be"), ProductId = Guid.Parse("7eb16dfc-78cf-427c-d6a1-08de78387ceb"), Value = "Li-Ion" },
                new() { Id = Guid.Parse("ca1cb073-04f8-44cd-9b61-08de78387cf8"), AttributeId = Guid.Parse("d2641e35-ade1-48ac-a660-1fa70c648757"), ProductId = Guid.Parse("7eb16dfc-78cf-427c-d6a1-08de78387ceb"), Value = "1 Nano SIM & 1 eSIM" },
                new() { Id = Guid.Parse("e37c071d-438e-4fa5-9b62-08de78387cf8"), AttributeId = Guid.Parse("74d4f4b8-64f7-46ee-9c15-3ee3743514bf"), ProductId = Guid.Parse("7eb16dfc-78cf-427c-d6a1-08de78387ceb"), Value = "6.9 inch" },
                new() { Id = Guid.Parse("d195f13f-71c1-421a-9b63-08de78387cf8"), AttributeId = Guid.Parse("2f16e475-7ccf-4bfd-ab37-46fe0dd725bf"), ProductId = Guid.Parse("7eb16dfc-78cf-427c-d6a1-08de78387ceb"), Value = "Wi-Fi 7" },
                new() { Id = Guid.Parse("53113407-e269-481a-9b64-08de78387cf8"), AttributeId = Guid.Parse("7d637bdf-7801-40c2-b813-4e3c68a0502b"), ProductId = Guid.Parse("7eb16dfc-78cf-427c-d6a1-08de78387ceb"), Value = "OLED" },
                new() { Id = Guid.Parse("46d80778-d406-4171-9b65-08de78387cf8"), AttributeId = Guid.Parse("6dda4bd4-538c-4a56-bb9a-804e4e477456"), ProductId = Guid.Parse("7eb16dfc-78cf-427c-d6a1-08de78387ceb"), Value = "Apple A19 Pro 6-core" },
                new() { Id = Guid.Parse("a64b43b8-869d-46b6-9b66-08de78387cf8"), AttributeId = Guid.Parse("1416eaa3-2f64-4fb9-966a-86c6dd76166a"), ProductId = Guid.Parse("7eb16dfc-78cf-427c-d6a1-08de78387ceb"), Value = "v6.0" },
                new() { Id = Guid.Parse("aec9352e-8bdb-4759-9b67-08de78387cf8"), AttributeId = Guid.Parse("0ae35309-b38e-4002-a418-8c3f2d5ff66b"), ProductId = Guid.Parse("7eb16dfc-78cf-427c-d6a1-08de78387ceb"), Value = "256 GB" },
                new() { Id = Guid.Parse("06c07373-12d9-4efb-9b68-08de78387cf8"), AttributeId = Guid.Parse("9f898dbd-1c78-4890-ba23-a2703def84f6"), ProductId = Guid.Parse("7eb16dfc-78cf-427c-d6a1-08de78387ceb"), Value = "5G support" },
                new() { Id = Guid.Parse("c29970dd-fe7d-4360-9b69-08de78387cf8"), AttributeId = Guid.Parse("18bbcb21-0c25-4543-af2a-b855e87e01f4"), ProductId = Guid.Parse("7eb16dfc-78cf-427c-d6a1-08de78387ceb"), Value = "iOS 26" },
                new() { Id = Guid.Parse("7fd61861-c186-4b92-9b6a-08de78387cf8"), AttributeId = Guid.Parse("022c909e-bcf6-40ce-a490-e781557b9b96"), ProductId = Guid.Parse("7eb16dfc-78cf-427c-d6a1-08de78387ceb"), Value = "Apple GPU with 6 cores" },
                new() { Id = Guid.Parse("b12aca5d-8e3d-4e16-9b6b-08de78387cf8"), AttributeId = Guid.Parse("8d5fa038-27c6-4184-9b4c-eb50ec7ceb00"), ProductId = Guid.Parse("7eb16dfc-78cf-427c-d6a1-08de78387ceb"), Value = "18 MP" },
                new() { Id = Guid.Parse("d7de914b-a9fd-4278-9b6c-08de78387cf8"), AttributeId = Guid.Parse("0993dc9d-38b9-4c9a-a315-ec6e63a540ea"), ProductId = Guid.Parse("7eb16dfc-78cf-427c-d6a1-08de78387ceb"), Value = "12 GB" },
                new() { Id = Guid.Parse("5d5bccbc-9db0-4bfa-9b6d-08de78387cf8"), AttributeId = Guid.Parse("67054b20-deb6-46dd-9e8f-edaf0b55a551"), ProductId = Guid.Parse("7eb16dfc-78cf-427c-d6a1-08de78387ceb"), Value = "Super Retina XDR (1320 x 2868 Pixels)" },

                // iPhone 16 Pro Max
                new() { Id = Guid.Parse("1b6b82e4-fbff-4c7f-9b6e-08de78387cf8"), AttributeId = Guid.Parse("fe158671-2d0c-4775-ace8-0017f3e21f02"), ProductId = Guid.Parse("98770860-2a59-42c4-d6a2-08de78387ceb"), Value = "48 MP" },
                new() { Id = Guid.Parse("38d3ee42-98bb-4a62-9b6f-08de78387cf8"), AttributeId = Guid.Parse("23be21d4-3622-4206-52d1-08de783534be"), ProductId = Guid.Parse("98770860-2a59-42c4-d6a2-08de78387ceb"), Value = "33 hours" },
                new() { Id = Guid.Parse("d6547840-5136-4521-9b70-08de78387cf8"), AttributeId = Guid.Parse("9c073c84-a0ea-4a18-52d2-08de783534be"), ProductId = Guid.Parse("98770860-2a59-42c4-d6a2-08de78387ceb"), Value = "Li-Ion" },
                new() { Id = Guid.Parse("4c70ef30-bd85-4023-9b71-08de78387cf8"), AttributeId = Guid.Parse("d2641e35-ade1-48ac-a660-1fa70c648757"), ProductId = Guid.Parse("98770860-2a59-42c4-d6a2-08de78387ceb"), Value = "1 Nano SIM & 1 eSIM" },
                new() { Id = Guid.Parse("12a2aedb-f2f1-4c16-9b72-08de78387cf8"), AttributeId = Guid.Parse("74d4f4b8-64f7-46ee-9c15-3ee3743514bf"), ProductId = Guid.Parse("98770860-2a59-42c4-d6a2-08de78387ceb"), Value = "6.9 inch" },
                new() { Id = Guid.Parse("5d3311d5-4cca-46ff-9b73-08de78387cf8"), AttributeId = Guid.Parse("2f16e475-7ccf-4bfd-ab37-46fe0dd725bf"), ProductId = Guid.Parse("98770860-2a59-42c4-d6a2-08de78387ceb"), Value = "Wi-Fi 7" },
                new() { Id = Guid.Parse("d6f8057a-2bee-4cdd-9b74-08de78387cf8"), AttributeId = Guid.Parse("7d637bdf-7801-40c2-b813-4e3c68a0502b"), ProductId = Guid.Parse("98770860-2a59-42c4-d6a2-08de78387ceb"), Value = "OLED" },
                new() { Id = Guid.Parse("6b30d669-8722-4e65-9b75-08de78387cf8"), AttributeId = Guid.Parse("6dda4bd4-538c-4a56-bb9a-804e4e477456"), ProductId = Guid.Parse("98770860-2a59-42c4-d6a2-08de78387ceb"), Value = "Apple A18 Pro 6-core" },
                new() { Id = Guid.Parse("ebc99e77-7cd2-4e17-9b76-08de78387cf8"), AttributeId = Guid.Parse("1416eaa3-2f64-4fb9-966a-86c6dd76166a"), ProductId = Guid.Parse("98770860-2a59-42c4-d6a2-08de78387ceb"), Value = "v5.3" },
                new() { Id = Guid.Parse("b91963dc-cbab-4143-9b77-08de78387cf8"), AttributeId = Guid.Parse("0ae35309-b38e-4002-a418-8c3f2d5ff66b"), ProductId = Guid.Parse("98770860-2a59-42c4-d6a2-08de78387ceb"), Value = "256 GB" },
                new() { Id = Guid.Parse("f4fbcc2e-9629-4bbc-9b78-08de78387cf8"), AttributeId = Guid.Parse("9f898dbd-1c78-4890-ba23-a2703def84f6"), ProductId = Guid.Parse("98770860-2a59-42c4-d6a2-08de78387ceb"), Value = "5G support" },
                new() { Id = Guid.Parse("ba374d89-592d-46be-9b79-08de78387cf8"), AttributeId = Guid.Parse("18bbcb21-0c25-4543-af2a-b855e87e01f4"), ProductId = Guid.Parse("98770860-2a59-42c4-d6a2-08de78387ceb"), Value = "iOS 18" },
                new() { Id = Guid.Parse("87b14fbf-dbf1-4035-9b7a-08de78387cf8"), AttributeId = Guid.Parse("022c909e-bcf6-40ce-a490-e781557b9b96"), ProductId = Guid.Parse("98770860-2a59-42c4-d6a2-08de78387ceb"), Value = "Apple GPU with 6 cores" },
                new() { Id = Guid.Parse("9f51a18f-d7aa-4c58-9b7b-08de78387cf8"), AttributeId = Guid.Parse("8d5fa038-27c6-4184-9b4c-eb50ec7ceb00"), ProductId = Guid.Parse("98770860-2a59-42c4-d6a2-08de78387ceb"), Value = "12 MP" },
                new() { Id = Guid.Parse("0c9fbb38-e19f-4b17-9b7c-08de78387cf8"), AttributeId = Guid.Parse("0993dc9d-38b9-4c9a-a315-ec6e63a540ea"), ProductId = Guid.Parse("98770860-2a59-42c4-d6a2-08de78387ceb"), Value = "8 GB" },
                new() { Id = Guid.Parse("ff45814b-b33d-459b-9b7d-08de78387cf8"), AttributeId = Guid.Parse("67054b20-deb6-46dd-9e8f-edaf0b55a551"), ProductId = Guid.Parse("98770860-2a59-42c4-d6a2-08de78387ceb"), Value = "Super Retina XDR (1320 x 2868 Pixels)" },

                // Samsung Galaxy S25 FE
                new() { Id = Guid.Parse("bee87102-384f-4d1e-9b7e-08de78387cf8"), AttributeId = Guid.Parse("fe158671-2d0c-4775-ace8-0017f3e21f02"), ProductId = Guid.Parse("012d01f5-1629-4a51-d6a3-08de78387ceb"), Value = "50 MP" },
                new() { Id = Guid.Parse("1d5f0f0a-0ccc-4dd4-9b7f-08de78387cf8"), AttributeId = Guid.Parse("23be21d4-3622-4206-52d1-08de783534be"), ProductId = Guid.Parse("012d01f5-1629-4a51-d6a3-08de78387ceb"), Value = "4900 mAh" },
                new() { Id = Guid.Parse("f8b56827-9db6-40ed-9b80-08de78387cf8"), AttributeId = Guid.Parse("9c073c84-a0ea-4a18-52d2-08de783534be"), ProductId = Guid.Parse("012d01f5-1629-4a51-d6a3-08de78387ceb"), Value = "Updating" },
                new() { Id = Guid.Parse("2a348e32-8cce-4d1d-9b81-08de78387cf8"), AttributeId = Guid.Parse("d2641e35-ade1-48ac-a660-1fa70c648757"), ProductId = Guid.Parse("012d01f5-1629-4a51-d6a3-08de78387ceb"), Value = "2 Nano SIMs + 1 eSIM" },
                new() { Id = Guid.Parse("568fdda6-e390-4c6d-9b82-08de78387cf8"), AttributeId = Guid.Parse("74d4f4b8-64f7-46ee-9c15-3ee3743514bf"), ProductId = Guid.Parse("012d01f5-1629-4a51-d6a3-08de78387ceb"), Value = "6.7 inch" },
                new() { Id = Guid.Parse("7c3195f8-9e4a-42de-9b83-08de78387cf8"), AttributeId = Guid.Parse("2f16e475-7ccf-4bfd-ab37-46fe0dd725bf"), ProductId = Guid.Parse("012d01f5-1629-4a51-d6a3-08de78387ceb"), Value = "Support" },
                new() { Id = Guid.Parse("5221cef6-0af2-4d64-9b84-08de78387cf8"), AttributeId = Guid.Parse("7d637bdf-7801-40c2-b813-4e3c68a0502b"), ProductId = Guid.Parse("012d01f5-1629-4a51-d6a3-08de78387ceb"), Value = "Dynamic AMOLED 2X" },
                new() { Id = Guid.Parse("6c298a75-daa6-4e83-9b85-08de78387cf8"), AttributeId = Guid.Parse("6dda4bd4-538c-4a56-bb9a-804e4e477456"), ProductId = Guid.Parse("012d01f5-1629-4a51-d6a3-08de78387ceb"), Value = "Exynos 2400 10-core" },
                new() { Id = Guid.Parse("10c95e38-c19a-487d-9b86-08de78387cf8"), AttributeId = Guid.Parse("1416eaa3-2f64-4fb9-966a-86c6dd76166a"), ProductId = Guid.Parse("012d01f5-1629-4a51-d6a3-08de78387ceb"), Value = "Support" },
                new() { Id = Guid.Parse("89b6c69e-b1cb-45fe-9b87-08de78387cf8"), AttributeId = Guid.Parse("0ae35309-b38e-4002-a418-8c3f2d5ff66b"), ProductId = Guid.Parse("012d01f5-1629-4a51-d6a3-08de78387ceb"), Value = " 128 GB" },
                new() { Id = Guid.Parse("27803b04-b78a-4537-9b88-08de78387cf8"), AttributeId = Guid.Parse("9f898dbd-1c78-4890-ba23-a2703def84f6"), ProductId = Guid.Parse("012d01f5-1629-4a51-d6a3-08de78387ceb"), Value = "5G support" },
                new() { Id = Guid.Parse("0fd4221b-c1fd-4e64-9b89-08de78387cf8"), AttributeId = Guid.Parse("18bbcb21-0c25-4543-af2a-b855e87e01f4"), ProductId = Guid.Parse("012d01f5-1629-4a51-d6a3-08de78387ceb"), Value = "Android 16" },
                new() { Id = Guid.Parse("b5a7dc28-f558-468f-9b8a-08de78387cf8"), AttributeId = Guid.Parse("8d5fa038-27c6-4184-9b4c-eb50ec7ceb00"), ProductId = Guid.Parse("012d01f5-1629-4a51-d6a3-08de78387ceb"), Value = "12 MP, 8 MP" },
                new() { Id = Guid.Parse("8c46cdc7-f53d-4d9f-9b8b-08de78387cf8"), AttributeId = Guid.Parse("0993dc9d-38b9-4c9a-a315-ec6e63a540ea"), ProductId = Guid.Parse("012d01f5-1629-4a51-d6a3-08de78387ceb"), Value = "8 GB" },
                new() { Id = Guid.Parse("be973be0-7cc9-4578-9b8c-08de78387cf8"), AttributeId = Guid.Parse("67054b20-deb6-46dd-9e8f-edaf0b55a551"), ProductId = Guid.Parse("012d01f5-1629-4a51-d6a3-08de78387ceb"), Value = "Full HD+ (1080 x 2340 Pixels)" },

                // Dell Laptop
                new() { Id = Guid.Parse("c33c95ac-c668-4592-9b8d-08de78387cf8"), AttributeId = Guid.Parse("23be21d4-3622-4206-52d1-08de783534be"), ProductId = Guid.Parse("6ff755bd-47ac-4397-d6a4-08de78387ceb"), Value = "3-cell Li-ion, 41 Wh" },
                new() { Id = Guid.Parse("967f5c3a-e81b-4fd2-9b8e-08de78387cf8"), AttributeId = Guid.Parse("74d4f4b8-64f7-46ee-9c15-3ee3743514bf"), ProductId = Guid.Parse("6ff755bd-47ac-4397-d6a4-08de78387ceb"), Value = "15.6 inch" },
                new() { Id = Guid.Parse("b6e2089b-c792-4e24-9b8f-08de78387cf8"), AttributeId = Guid.Parse("2f16e475-7ccf-4bfd-ab37-46fe0dd725bf"), ProductId = Guid.Parse("6ff755bd-47ac-4397-d6a4-08de78387ceb"), Value = "Wi-Fi 6 (802.11ax)" },
                new() { Id = Guid.Parse("f29e151c-f3ad-46e2-9b90-08de78387cf8"), AttributeId = Guid.Parse("7d637bdf-7801-40c2-b813-4e3c68a0502b"), ProductId = Guid.Parse("6ff755bd-47ac-4397-d6a4-08de78387ceb"), Value = "Anti-Glare" },
                new() { Id = Guid.Parse("a642986b-b9ea-4a63-9b91-08de78387cf8"), AttributeId = Guid.Parse("6dda4bd4-538c-4a56-bb9a-804e4e477456"), ProductId = Guid.Parse("6ff755bd-47ac-4397-d6a4-08de78387ceb"), Value = "Intel Core i5 Raptor Lake - 1334U" },
                new() { Id = Guid.Parse("b1e87a40-b4ad-41c2-9b92-08de78387cf8"), AttributeId = Guid.Parse("0ae35309-b38e-4002-a418-8c3f2d5ff66b"), ProductId = Guid.Parse("6ff755bd-47ac-4397-d6a4-08de78387ceb"), Value = "512 GB NVMe PCIe SSD" },
                new() { Id = Guid.Parse("8a22d709-8b3c-47b8-9b93-08de78387cf8"), AttributeId = Guid.Parse("18bbcb21-0c25-4543-af2a-b855e87e01f4"), ProductId = Guid.Parse("6ff755bd-47ac-4397-d6a4-08de78387ceb"), Value = "Windows 11 Home SL + Office Home 2024 lifetime license + Microsoft 365 Basic" },
                new() { Id = Guid.Parse("acc32c6a-ae06-478e-9b94-08de78387cf8"), AttributeId = Guid.Parse("022c909e-bcf6-40ce-a490-e781557b9b96"), ProductId = Guid.Parse("6ff755bd-47ac-4397-d6a4-08de78387ceb"), Value = "Integrated graphics card - Intel UHD Graphics" },
                new() { Id = Guid.Parse("a524b4a3-88b8-4f63-9b95-08de78387cf8"), AttributeId = Guid.Parse("0993dc9d-38b9-4c9a-a315-ec6e63a540ea"), ProductId = Guid.Parse("6ff755bd-47ac-4397-d6a4-08de78387ceb"), Value = "16 GB" },
                new() { Id = Guid.Parse("48054d10-3d18-4d0d-9b96-08de78387cf8"), AttributeId = Guid.Parse("67054b20-deb6-46dd-9e8f-edaf0b55a551"), ProductId = Guid.Parse("6ff755bd-47ac-4397-d6a4-08de78387ceb"), Value = "Full HD (1920 x 1080)" }
            };
        }

        private static List<ProductTemplateProductAttribute> GetProductTemplateProductAttributes()
        {
            return new List<ProductTemplateProductAttribute>
            {
                // Phone Template
                new() { Id = Guid.Parse("d05a00aa-f303-4eac-89b4-324685d25418"), ProductTemplateId = Guid.Parse("ba82cd8d-198d-4106-bb8a-08de7829c8d9"), ProductAttributeId = Guid.Parse("9f898dbd-1c78-4890-ba23-a2703def84f6"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("944d7c80-e2df-4352-a64a-6fcb09996991"), ProductTemplateId = Guid.Parse("ba82cd8d-198d-4106-bb8a-08de7829c8d9"), ProductAttributeId = Guid.Parse("d2641e35-ade1-48ac-a660-1fa70c648757"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("2e785611-930b-4c0c-964d-a58cb38edce0"), ProductTemplateId = Guid.Parse("ba82cd8d-198d-4106-bb8a-08de7829c8d9"), ProductAttributeId = Guid.Parse("1416eaa3-2f64-4fb9-966a-86c6dd76166a"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("b10ca819-1f24-473b-a43b-3d9d1fe75f8b"), ProductTemplateId = Guid.Parse("ba82cd8d-198d-4106-bb8a-08de7829c8d9"), ProductAttributeId = Guid.Parse("2f16e475-7ccf-4bfd-ab37-46fe0dd725bf"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("355509ae-7500-46f6-9925-15b00eae3530"), ProductTemplateId = Guid.Parse("ba82cd8d-198d-4106-bb8a-08de7829c8d9"), ProductAttributeId = Guid.Parse("0ae35309-b38e-4002-a418-8c3f2d5ff66b"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("16db56d1-fcce-4cab-8205-164da7ca092a"), ProductTemplateId = Guid.Parse("ba82cd8d-198d-4106-bb8a-08de7829c8d9"), ProductAttributeId = Guid.Parse("6dda4bd4-538c-4a56-bb9a-804e4e477456"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("1b29f2e0-8a41-4e8b-af3b-4bc387d3b20d"), ProductTemplateId = Guid.Parse("ba82cd8d-198d-4106-bb8a-08de7829c8d9"), ProductAttributeId = Guid.Parse("022c909e-bcf6-40ce-a490-e781557b9b96"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("7bc36be6-e99f-40dc-84bf-1460c70a38f2"), ProductTemplateId = Guid.Parse("ba82cd8d-198d-4106-bb8a-08de7829c8d9"), ProductAttributeId = Guid.Parse("18bbcb21-0c25-4543-af2a-b855e87e01f4"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("e12bb179-6128-49b2-acab-191e9f79aad5"), ProductTemplateId = Guid.Parse("ba82cd8d-198d-4106-bb8a-08de7829c8d9"), ProductAttributeId = Guid.Parse("0993dc9d-38b9-4c9a-a315-ec6e63a540ea"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("b13dd242-0099-4263-8b01-d4b2ef60b8d8"), ProductTemplateId = Guid.Parse("ba82cd8d-198d-4106-bb8a-08de7829c8d9"), ProductAttributeId = Guid.Parse("fe158671-2d0c-4775-ace8-0017f3e21f02"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("fc1c3f58-66cc-4000-9b1e-d3de952bf546"), ProductTemplateId = Guid.Parse("ba82cd8d-198d-4106-bb8a-08de7829c8d9"), ProductAttributeId = Guid.Parse("8d5fa038-27c6-4184-9b4c-eb50ec7ceb00"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("98eb02cf-8148-4ccd-b921-5a5d5d660af0"), ProductTemplateId = Guid.Parse("ba82cd8d-198d-4106-bb8a-08de7829c8d9"), ProductAttributeId = Guid.Parse("67054b20-deb6-46dd-9e8f-edaf0b55a551"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("d6880f97-8378-4d05-ad3c-b04ac18ef777"), ProductTemplateId = Guid.Parse("ba82cd8d-198d-4106-bb8a-08de7829c8d9"), ProductAttributeId = Guid.Parse("74d4f4b8-64f7-46ee-9c15-3ee3743514bf"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("a962442a-f4cf-4c5f-b756-81d99e5d4f41"), ProductTemplateId = Guid.Parse("ba82cd8d-198d-4106-bb8a-08de7829c8d9"), ProductAttributeId = Guid.Parse("7d637bdf-7801-40c2-b813-4e3c68a0502b"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("9e446248-6d9f-412d-87ae-8df3c95fb50c"), ProductTemplateId = Guid.Parse("ba82cd8d-198d-4106-bb8a-08de7829c8d9"), ProductAttributeId = Guid.Parse("23be21d4-3622-4206-52d1-08de783534be"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("811d0aa8-d539-405c-9064-7d7dc44eb4d8"), ProductTemplateId = Guid.Parse("ba82cd8d-198d-4106-bb8a-08de7829c8d9"), ProductAttributeId = Guid.Parse("9c073c84-a0ea-4a18-52d2-08de783534be"), CreatedDate = DateTime.UtcNow },

                // Laptop Template
                new() { Id = Guid.Parse("d627f839-87d6-4183-bbb0-6d46c8114072"), ProductTemplateId = Guid.Parse("40e153ad-35e8-4032-b848-08de783a4442"), ProductAttributeId = Guid.Parse("23be21d4-3622-4206-52d1-08de783534be"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("c5bb8dec-62c0-4f0a-8d49-2c6ad4f46d0f"), ProductTemplateId = Guid.Parse("40e153ad-35e8-4032-b848-08de783a4442"), ProductAttributeId = Guid.Parse("2f16e475-7ccf-4bfd-ab37-46fe0dd725bf"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("9598fbc1-2515-4e2e-85ee-9626c6409c47"), ProductTemplateId = Guid.Parse("40e153ad-35e8-4032-b848-08de783a4442"), ProductAttributeId = Guid.Parse("6dda4bd4-538c-4a56-bb9a-804e4e477456"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("5191cca3-9591-4643-93c4-9ae773f9bfd3"), ProductTemplateId = Guid.Parse("40e153ad-35e8-4032-b848-08de783a4442"), ProductAttributeId = Guid.Parse("022c909e-bcf6-40ce-a490-e781557b9b96"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("bfb61a8f-818e-41cd-bc68-28e316a2d280"), ProductTemplateId = Guid.Parse("40e153ad-35e8-4032-b848-08de783a4442"), ProductAttributeId = Guid.Parse("18bbcb21-0c25-4543-af2a-b855e87e01f4"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("8316195d-5d5b-4dcc-90f1-882054b8a700"), ProductTemplateId = Guid.Parse("40e153ad-35e8-4032-b848-08de783a4442"), ProductAttributeId = Guid.Parse("0993dc9d-38b9-4c9a-a315-ec6e63a540ea"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("dc9fb700-cd7f-4028-9239-5a823515504e"), ProductTemplateId = Guid.Parse("40e153ad-35e8-4032-b848-08de783a4442"), ProductAttributeId = Guid.Parse("0ae35309-b38e-4002-a418-8c3f2d5ff66b"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("ac76f5ea-3d99-4ca7-b7b4-94139e801771"), ProductTemplateId = Guid.Parse("40e153ad-35e8-4032-b848-08de783a4442"), ProductAttributeId = Guid.Parse("7d637bdf-7801-40c2-b813-4e3c68a0502b"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("3c9868bf-6a5d-4ec3-8a94-a18809c1e78f"), ProductTemplateId = Guid.Parse("40e153ad-35e8-4032-b848-08de783a4442"), ProductAttributeId = Guid.Parse("67054b20-deb6-46dd-9e8f-edaf0b55a551"), CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("4cce6fd9-29e1-4583-88d2-daf9b1fcbf32"), ProductTemplateId = Guid.Parse("40e153ad-35e8-4032-b848-08de783a4442"), ProductAttributeId = Guid.Parse("74d4f4b8-64f7-46ee-9c15-3ee3743514bf"), CreatedDate = DateTime.UtcNow }
            };
        }
    }
}
