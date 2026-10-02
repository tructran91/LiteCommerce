using Catalog.Application.Database;
using Catalog.Core.Entities;

namespace Catalog.Infrastructure.Data.Seeding
{
    public class TechnologySeedProfile : ISeedProfile
    {
        public string Name => SeedProfiles.Technology;

        public async Task SeedAsync(CatalogContext context, CancellationToken cancellationToken = default)
        {
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
            await context.Brands.AddRangeAsync(brands, cancellationToken);

            var attributeGroups = new List<ProductAttributeGroup>
            {
                new() { Id = Guid.Parse("206e76f1-1f4d-4c0b-84b2-38431548ea49"), Name = "General", CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("b8dd305a-abf7-41f1-83fe-3ab7432926c9"), Name = "Screen", CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("4d6e8118-5cf2-4de6-ba04-3cf53425f910"), Name = "Camera", CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("58b6906e-8589-42ca-8d8e-ea85867d4cb7"), Name = "Connectivity", CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("b0c12b2c-7f15-4dea-90d0-08de7828f656"), Name = "Battery", CreatedDate = DateTime.UtcNow }
            };
            await context.ProductAttributeGroups.AddRangeAsync(attributeGroups, cancellationToken);

            var productOptions = new List<ProductOption>
            {
                new() { Id = Guid.Parse("452d918b-b5bf-41b7-90e6-51cad397b292"), Name = "Color" },
                new() { Id = Guid.Parse("a900d061-715e-4682-b493-4a3f75f95b01"), Name = "Size" },
                new() { Id = Guid.Parse("646dceee-5eca-46a0-8a5f-db8d280dab4a"), Name = "Warranty" }
            };
            await context.ProductOptions.AddRangeAsync(productOptions, cancellationToken);

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
                new() { Id = Guid.Parse("d5f3b0e4-350d-4188-a0d1-28ebc5cc3aea"), Name = "Gaming", Slug = "gaming", ParentId = Guid.Parse("7e226ecb-6fc1-4602-bf0b-d14d02a14555"), IsPublished = true, IncludeInMenu = true, DisplayOrder = 0, MetaTitle = "Gaming - Explore the ultimate gaming devices", MetaKeywords = "gaming, gaming devices, gaming phones", MetaDescription = "Discover high-performance gaming phones and devices for gamers.", OgTitle = "Gaming - High-performance gaming phones", OgDescription = "Explore the latest gaming phones and devices for the ultimate gaming experience.", OgImage = "/images/gaming.jpg", OgUrl = "/categories/gaming", CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("f2f2f2f2-f2f2-4f2f-8f2f-f2f2f2f2f2f2"), Name = "Laptops", Slug = "laptops", ParentId = Guid.Parse("f64602e6-d373-42ef-a3be-4360449bada1"), IsPublished = true, IncludeInMenu = true, DisplayOrder = 2, MetaTitle = "Laptops - Văn phòng & học tập", MetaKeywords = "laptop, ultrabook, laptop văn phòng", MetaDescription = "Laptop văn phòng, học tập từ Dell, HP, Lenovo.", OgTitle = "Laptops - Mỏng nhẹ, pin lâu", OgDescription = "Laptop cho công việc và học tập", OgImage = "/images/laptops.jpg", OgUrl = "/categories/laptops", CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("a3a3a3a3-a3a3-4a3a-8a3a-a3a3a3a3a3a3"), Name = "Android", Slug = "android", ParentId = Guid.Parse("7e226ecb-6fc1-4602-bf0b-d14d02a14555"), IsPublished = true, IncludeInMenu = true, DisplayOrder = 2, MetaTitle = "Điện thoại Android", MetaKeywords = "android, samsung, oppo", MetaDescription = "Smartphone Android từ Samsung, OPPO.", OgTitle = "Android - Đa dạng lựa chọn", OgDescription = "Khám phá điện thoại Android", OgImage = "/images/android.jpg", OgUrl = "/categories/android", CreatedDate = DateTime.UtcNow }
            };
            await context.Categories.AddRangeAsync(categories, cancellationToken);

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
            await context.ProductAttributes.AddRangeAsync(attributes, cancellationToken);

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
                },
                new()
                {
                    Id = Guid.Parse("a1a1a1a1-a1a1-4a1a-8a1a-a1a1a1a1a1a1"),
                    Name = "Samsung Galaxy S25 Ultra 5G 12GB/512GB",
                    Slug = "samsung-galaxy-s25-ultra-5g-12gb512gb",
                    ShortDescription = "<ul><li>Snapdragon 8 Elite</li><li>RAM 12 GB, 512 GB</li><li>Camera 200 MP, bút S Pen</li><li>Pin 5000 mAh, sạc 45 W</li></ul>",
                    Description = "<p>Galaxy S25 Ultra: khung titan, camera 200 MP với Galaxy AI, bút S Pen đa năng và chip Snapdragon 8 Elite mạnh nhất.</p>",
                    IsPublished = true,
                    Price = 33990000m,
                    BrandId = Guid.Parse("75138068-7176-4afb-bc14-08de78272836"),
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Samsung Galaxy S25 Ultra 5G giá tốt, góp 0%",
                    MetaKeywords = "samsung s25 ultra, galaxy s25 ultra, s25 ultra 512gb",
                    MetaDescription = "Mua Samsung Galaxy S25 Ultra 5G giá tốt, thu cũ trợ giá, trả chậm 0%, bảo hành chính hãng 1 năm. Mua ngay!"
                },
                new()
                {
                    Id = Guid.Parse("b2b2b2b2-b2b2-4b2b-8b2b-b2b2b2b2b2b2"),
                    Name = "OPPO Reno13 F 5G 8GB/256GB",
                    Slug = "oppo-reno13-f-5g-8gb256gb",
                    ShortDescription = "<ul><li>Snapdragon 6 Gen 1</li><li>RAM 8 GB, 256 GB</li><li>Camera 50 MP chống rung OIS</li><li>Pin 5800 mAh, sạc 45 W</li></ul>",
                    Description = "<p>OPPO Reno13 F: thiết kế mỏng nhẹ, camera chân dung AI, pin lớn dùng 2 ngày và kháng nước bụi IP69.</p>",
                    IsPublished = true,
                    Price = 9490000m,
                    BrandId = Guid.Parse("47096813-224e-42c7-bc19-08de78272836"),
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "OPPO Reno13 F 5G giá tốt, góp 0%",
                    MetaKeywords = "oppo reno13 f, reno13 f 5g, điện thoại oppo",
                    MetaDescription = "Mua OPPO Reno13 F 5G giá tốt, trả chậm 0%, bảo hành chính hãng 1 năm, hư gì đổi nấy 12 tháng. Mua ngay!"
                },
                new()
                {
                    Id = Guid.Parse("c3c3c3c3-c3c3-4c3c-8c3c-c3c3c3c3c3c3"),
                    Name = "Nokia 110 4G (2024)",
                    Slug = "nokia-110-4g-2024",
                    ShortDescription = "<ul><li>Màn hình TFT 2.4 inch</li><li>2 SIM, nghe gọi 4G</li><li>Pin 1000 mAh tháo rời</li><li>Nghe radio FM không cần tai nghe</li></ul>",
                    Description = "<p>Nokia 110 4G (2024): điện thoại phổ thông bền bỉ, pin dùng nhiều ngày, bàn phím lớn dễ bấm, phù hợp người lớn tuổi.</p>",
                    IsPublished = true,
                    Price = 890000m,
                    BrandId = Guid.Parse("d0efbf1c-8d93-43cb-bc18-08de78272836"),
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Nokia 110 4G (2024) giá rẻ, pin lâu",
                    MetaKeywords = "nokia 110 4g, điện thoại cục gạch, nokia phổ thông",
                    MetaDescription = "Mua Nokia 110 4G (2024) giá rẻ, pin dùng nhiều ngày, bảo hành chính hãng 1 năm. Mua ngay!"
                },
                new()
                {
                    Id = Guid.Parse("d4d4d4d4-d4d4-4d4d-8d4d-d4d4d4d4d4d4"),
                    Name = "Laptop Asus ROG Strix G16 G614JU i7-14650HX RTX 4050",
                    Slug = "laptop-asus-rog-strix-g16-g614ju-i7-14650hx-rtx4050",
                    ShortDescription = "<ul><li>Intel Core i7-14650HX</li><li>RTX 4050 6 GB, RAM 16 GB</li><li>SSD 512 GB, màn 16 inch 165 Hz</li></ul>",
                    Description = "<p>ROG Strix G16: laptop gaming với tản nhiệt 3 quạt, màn hình ROG Nebula 165 Hz và hiệu năng chiến game AAA mượt mà.</p>",
                    IsPublished = true,
                    Price = 34990000m,
                    BrandId = Guid.Parse("29364927-6439-473f-bc1a-08de78272836"),
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Asus ROG Strix G16 RTX 4050 giá tốt, BH 2 năm",
                    MetaKeywords = "rog strix g16, laptop gaming asus, rtx 4050 laptop",
                    MetaDescription = "Mua Asus ROG Strix G16 RTX 4050 giá tốt, trả chậm 0%, bảo hành chính hãng 2 năm. Mua ngay!"
                },
                new()
                {
                    Id = Guid.Parse("e5e5e5e5-e5e5-4e5e-8e5e-e5e5e5e5e5e5"),
                    Name = "Laptop Lenovo ThinkPad E14 Gen 6 Ryzen 7 7735U",
                    Slug = "laptop-lenovo-thinkpad-e14-gen-6-ryzen-7-7735u",
                    ShortDescription = "<ul><li>AMD Ryzen 7 7735U</li><li>RAM 16 GB, SSD 512 GB</li><li>Màn 14 inch WUXGA chống chói</li><li>Vỏ nhôm, đạt chuẩn quân đội</li></ul>",
                    Description = "<p>ThinkPad E14 Gen 6: laptop doanh nhân bền bỉ với bàn phím huyền thoại, bảo mật vân tay và pin dùng cả ngày làm việc.</p>",
                    IsPublished = true,
                    Price = 21990000m,
                    BrandId = Guid.Parse("718b4a11-1b02-44c6-bc17-08de78272836"),
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Lenovo ThinkPad E14 Gen 6 giá tốt, BH 1 năm",
                    MetaKeywords = "thinkpad e14, lenovo thinkpad, laptop doanh nhân",
                    MetaDescription = "Mua Lenovo ThinkPad E14 Gen 6 giá tốt, trả chậm 0%, bảo hành chính hãng 1 năm. Mua ngay!"
                },
                new()
                {
                    Id = Guid.Parse("f6f6f6f6-f6f6-4f6f-8f6f-f6f6f6f6f6f6"),
                    Name = "Laptop HP Pavilion 15-eg3048TU i5-1335U 16GB 512GB",
                    Slug = "laptop-hp-pavilion-15-eg3048tu-i5-1335u-16gb-512gb",
                    ShortDescription = "<ul><li>Intel Core i5-1335U</li><li>RAM 16 GB, SSD 512 GB</li><li>Màn 15.6 inch Full HD viền mỏng</li></ul>",
                    Description = "<p>HP Pavilion 15: laptop học tập văn phòng cân bằng giữa hiệu năng và giá, loa B&O và pin dùng 8 tiếng.</p>",
                    IsPublished = true,
                    Price = 16490000m,
                    BrandId = Guid.Parse("ffaf754f-f5c2-4853-bc16-08de78272836"),
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "HP Pavilion 15 i5-1335U giá tốt, góp 0%",
                    MetaKeywords = "hp pavilion 15, laptop hp, pavilion eg3048tu",
                    MetaDescription = "Mua HP Pavilion 15 i5-1335U giá tốt, trả chậm 0%, bảo hành chính hãng 1 năm. Mua ngay!"
                },
                new()
                {
                    Id = Guid.Parse("a7a7a7a7-a7a7-4a7a-8a7a-a7a7a7a7a7a7"),
                    Name = "Apple MacBook Air 13 inch M4 16GB/256GB",
                    Slug = "apple-macbook-air-13-m4-16gb256gb",
                    ShortDescription = "<ul><li>Chip Apple M4 10 nhân</li><li>RAM unified 16 GB, SSD 256 GB</li><li>Màn Liquid Retina 13.6 inch</li><li>Pin dùng đến 18 tiếng</li></ul>",
                    Description = "<p>MacBook Air M4: mỏng 11.3 mm, nhẹ 1.24 kg, hiệu năng AI vượt trội và thời lượng pin tốt nhất phân khúc ultrabook.</p>",
                    IsPublished = true,
                    Price = 27990000m,
                    BrandId = Guid.Parse("5fb5f20c-bf57-4907-bc13-08de78272836"),
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "MacBook Air M4 13 inch giá tốt, BH 1 năm",
                    MetaKeywords = "macbook air m4, macbook air 13, apple m4",
                    MetaDescription = "Mua MacBook Air M4 13 inch giá tốt, trả chậm 0%, bảo hành chính hãng 1 năm. Mua ngay!"
                },
                new()
                {
                    Id = Guid.Parse("b8b8b8b8-b8b8-4b8b-8b8b-b8b8b8b8b8b8"),
                    Name = "Tai nghe Asus ROG Cetra True Wireless",
                    Slug = "tai-nghe-asus-rog-cetra-true-wireless",
                    ShortDescription = "<ul><li>Chống ồn chủ động lai</li><li>Driver 10 mm, Bluetooth 5.3</li><li>Pin 27 tiếng kèm hộp sạc</li><li>Chế độ gaming độ trễ thấp</li></ul>",
                    Description = "<p>ROG Cetra True Wireless: tai nghe gaming không dây với ANC, âm thanh chi tiết và kháng nước IPX4 để dùng ngoài trời.</p>",
                    IsPublished = true,
                    Price = 2990000m,
                    BrandId = Guid.Parse("29364927-6439-473f-bc1a-08de78272836"),
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Tai nghe Asus ROG Cetra True Wireless chính hãng",
                    MetaKeywords = "rog cetra, tai nghe gaming, true wireless asus",
                    MetaDescription = "Mua tai nghe Asus ROG Cetra True Wireless chính hãng, bảo hành 2 năm. Mua ngay!"
                },
                new()
                {
                    Id = Guid.Parse("c9c9c9c9-c9c9-4c9c-8c9c-c9c9c9c9c9c9"),
                    Name = "Pin sạc dự phòng Samsung 10000mAh 25W",
                    Slug = "pin-sac-du-phong-samsung-10000mah-25w",
                    ShortDescription = "<ul><li>Dung lượng 10000 mAh</li><li>Sạc nhanh 25 W, 2 cổng USB-C</li><li>Vỏ nhôm nguyên khối</li></ul>",
                    Description = "<p>Pin sạc dự phòng Samsung 10000 mAh: sạc nhanh 25 W cho Galaxy, thiết kế mỏng nhẹ bỏ túi, an toàn nhiều lớp.</p>",
                    IsPublished = true,
                    Price = 890000m,
                    BrandId = Guid.Parse("75138068-7176-4afb-bc14-08de78272836"),
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Pin sạc dự phòng Samsung 10000mAh 25W chính hãng",
                    MetaKeywords = "sạc dự phòng samsung, pin 10000mah, battery pack",
                    MetaDescription = "Mua pin sạc dự phòng Samsung 10000mAh 25W chính hãng, bảo hành 1 năm. Mua ngay!"
                },
                new()
                {
                    Id = Guid.Parse("d0d0d0d0-d0d0-4d0d-8d0d-d0d0d0d0d0d0"),
                    Name = "USB Samsung FIT Plus 256GB USB 3.1",
                    Slug = "usb-samsung-fit-plus-256gb-usb31",
                    ShortDescription = "<ul><li>Dung lượng 256 GB</li><li>Chuẩn USB 3.1, đọc đến 400 MB/s</li><li>Vỏ kim loại chống nước, siêu nhỏ gọn</li></ul>",
                    Description = "<p>Samsung FIT Plus 256 GB: USB siêu nhỏ cắm là quên, tốc độ đọc 400 MB/s, lưu phim 4K và backup nhanh chóng.</p>",
                    IsPublished = true,
                    Price = 990000m,
                    BrandId = Guid.Parse("75138068-7176-4afb-bc14-08de78272836"),
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "USB Samsung FIT Plus 256GB chính hãng",
                    MetaKeywords = "usb samsung, fit plus 256gb, usb 3.1",
                    MetaDescription = "Mua USB Samsung FIT Plus 256GB chính hãng, bảo hành 5 năm. Mua ngay!"
                },
                new()
                {
                    Id = Guid.Parse("e1e1e1e1-e1e1-4e1e-8e1e-e1e1e1e1e1e1"),
                    Name = "Apple AirPods Pro 2 USB-C",
                    Slug = "apple-airpods-pro-2-usb-c",
                    ShortDescription = "<ul><li>Chip H2, chống ồn gấp 2 lần</li><li>Âm thanh Adaptive Transparency</li><li>Pin 30 tiếng kèm hộp MagSafe</li></ul>",
                    Description = "<p>AirPods Pro 2 USB-C: chống ồn chủ động mạnh gấp đôi thế hệ trước, âm trầm sâu và hộp sạc MagSafe tiện lợi.</p>",
                    IsPublished = true,
                    Price = 5490000m,
                    BrandId = Guid.Parse("5fb5f20c-bf57-4907-bc13-08de78272836"),
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "AirPods Pro 2 USB-C chính hãng, giá tốt",
                    MetaKeywords = "airpods pro 2, tai nghe apple, airpods usb-c",
                    MetaDescription = "Mua AirPods Pro 2 USB-C chính hãng, bảo hành 1 năm, trả chậm 0%. Mua ngay!"
                }
            };
            await context.Products.AddRangeAsync(products, cancellationToken);

            var productCategories = new List<ProductCategory>
            {
                new() { Id = Guid.Parse("48a26b27-90a3-4348-d973-08de78387cfa"), CategoryId = Guid.Parse("5a6895a9-ce31-4a37-8300-60bb6a939c2c"), ProductId = Guid.Parse("7eb16dfc-78cf-427c-d6a1-08de78387ceb"), DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("589d7cf8-4ec4-4000-d974-08de78387cfa"), CategoryId = Guid.Parse("7e226ecb-6fc1-4602-bf0b-d14d02a14555"), ProductId = Guid.Parse("7eb16dfc-78cf-427c-d6a1-08de78387ceb"), DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("a973c00d-78fd-4d37-d975-08de78387cfa"), CategoryId = Guid.Parse("7e226ecb-6fc1-4602-bf0b-d14d02a14555"), ProductId = Guid.Parse("98770860-2a59-42c4-d6a2-08de78387ceb"), DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("f2d30039-00e3-4dd0-d976-08de78387cfa"), CategoryId = Guid.Parse("5a6895a9-ce31-4a37-8300-60bb6a939c2c"), ProductId = Guid.Parse("98770860-2a59-42c4-d6a2-08de78387ceb"), DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("3c548a37-8de6-460f-d977-08de78387cfa"), CategoryId = Guid.Parse("7e226ecb-6fc1-4602-bf0b-d14d02a14555"), ProductId = Guid.Parse("012d01f5-1629-4a51-d6a3-08de78387ceb"), DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("09739461-a944-4d12-d978-08de78387cfa"), CategoryId = Guid.Parse("d5f3b0e4-350d-4188-a0d1-28ebc5cc3aea"), ProductId = Guid.Parse("012d01f5-1629-4a51-d6a3-08de78387ceb"), DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("0404d76a-b7a5-4b46-d979-08de78387cfa"), CategoryId = Guid.Parse("acfb3c87-502b-44a0-b39b-6b644c57ffb6"), ProductId = Guid.Parse("6ff755bd-47ac-4397-d6a4-08de78387ceb"), DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("19a78608-0c04-4d8d-d97a-08de78387cfa"), CategoryId = Guid.Parse("f64602e6-d373-42ef-a3be-4360449bada1"), ProductId = Guid.Parse("6ff755bd-47ac-4397-d6a4-08de78387ceb"), DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = Guid.Parse("7e226ecb-6fc1-4602-bf0b-d14d02a14555"), ProductId = Guid.Parse("a1a1a1a1-a1a1-4a1a-8a1a-a1a1a1a1a1a1"), DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = Guid.Parse("a3a3a3a3-a3a3-4a3a-8a3a-a3a3a3a3a3a3"), ProductId = Guid.Parse("a1a1a1a1-a1a1-4a1a-8a1a-a1a1a1a1a1a1"), DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = Guid.Parse("7e226ecb-6fc1-4602-bf0b-d14d02a14555"), ProductId = Guid.Parse("b2b2b2b2-b2b2-4b2b-8b2b-b2b2b2b2b2b2"), DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = Guid.Parse("a3a3a3a3-a3a3-4a3a-8a3a-a3a3a3a3a3a3"), ProductId = Guid.Parse("b2b2b2b2-b2b2-4b2b-8b2b-b2b2b2b2b2b2"), DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = Guid.Parse("7e226ecb-6fc1-4602-bf0b-d14d02a14555"), ProductId = Guid.Parse("c3c3c3c3-c3c3-4c3c-8c3c-c3c3c3c3c3c3"), DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = Guid.Parse("a5924453-4608-427f-9751-717f217ea569"), ProductId = Guid.Parse("c3c3c3c3-c3c3-4c3c-8c3c-c3c3c3c3c3c3"), DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = Guid.Parse("f64602e6-d373-42ef-a3be-4360449bada1"), ProductId = Guid.Parse("d4d4d4d4-d4d4-4d4d-8d4d-d4d4d4d4d4d4"), DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = Guid.Parse("acfb3c87-502b-44a0-b39b-6b644c57ffb6"), ProductId = Guid.Parse("d4d4d4d4-d4d4-4d4d-8d4d-d4d4d4d4d4d4"), DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = Guid.Parse("f64602e6-d373-42ef-a3be-4360449bada1"), ProductId = Guid.Parse("e5e5e5e5-e5e5-4e5e-8e5e-e5e5e5e5e5e5"), DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = Guid.Parse("f2f2f2f2-f2f2-4f2f-8f2f-f2f2f2f2f2f2"), ProductId = Guid.Parse("e5e5e5e5-e5e5-4e5e-8e5e-e5e5e5e5e5e5"), DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = Guid.Parse("f64602e6-d373-42ef-a3be-4360449bada1"), ProductId = Guid.Parse("f6f6f6f6-f6f6-4f6f-8f6f-f6f6f6f6f6f6"), DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = Guid.Parse("f2f2f2f2-f2f2-4f2f-8f2f-f2f2f2f2f2f2"), ProductId = Guid.Parse("f6f6f6f6-f6f6-4f6f-8f6f-f6f6f6f6f6f6"), DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = Guid.Parse("f64602e6-d373-42ef-a3be-4360449bada1"), ProductId = Guid.Parse("a7a7a7a7-a7a7-4a7a-8a7a-a7a7a7a7a7a7"), DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = Guid.Parse("761d55da-023c-41e0-ace7-c7316602e0f6"), ProductId = Guid.Parse("a7a7a7a7-a7a7-4a7a-8a7a-a7a7a7a7a7a7"), DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = Guid.Parse("8ac51586-431d-4642-8435-5926cb6c04f4"), ProductId = Guid.Parse("b8b8b8b8-b8b8-4b8b-8b8b-b8b8b8b8b8b8"), DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = Guid.Parse("d8c4a2d8-3f4d-4d3c-923c-f4f46482ab7a"), ProductId = Guid.Parse("b8b8b8b8-b8b8-4b8b-8b8b-b8b8b8b8b8b8"), DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = Guid.Parse("8ac51586-431d-4642-8435-5926cb6c04f4"), ProductId = Guid.Parse("c9c9c9c9-c9c9-4c9c-8c9c-c9c9c9c9c9c9"), DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = Guid.Parse("c3f05c5c-3fa3-4c6d-81c8-e33526e39511"), ProductId = Guid.Parse("c9c9c9c9-c9c9-4c9c-8c9c-c9c9c9c9c9c9"), DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = Guid.Parse("8ac51586-431d-4642-8435-5926cb6c04f4"), ProductId = Guid.Parse("d0d0d0d0-d0d0-4d0d-8d0d-d0d0d0d0d0d0"), DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = Guid.Parse("633b7e0d-e227-450e-a3a2-574125aa6024"), ProductId = Guid.Parse("d0d0d0d0-d0d0-4d0d-8d0d-d0d0d0d0d0d0"), DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = Guid.Parse("8ac51586-431d-4642-8435-5926cb6c04f4"), ProductId = Guid.Parse("e1e1e1e1-e1e1-4e1e-8e1e-e1e1e1e1e1e1"), DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = Guid.Parse("d8c4a2d8-3f4d-4d3c-923c-f4f46482ab7a"), ProductId = Guid.Parse("e1e1e1e1-e1e1-4e1e-8e1e-e1e1e1e1e1e1"), DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow }
            };
            await context.ProductCategories.AddRangeAsync(productCategories, cancellationToken);

            var attributeValues = GetProductAttributeValues();
            await context.ProductAttributeValues.AddRangeAsync(attributeValues, cancellationToken);

            var productTemplates = new List<ProductTemplate>
            {
                new() { Id = Guid.Parse("ba82cd8d-198d-4106-bb8a-08de7829c8d9"), Name = "Phone", CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.Parse("40e153ad-35e8-4032-b848-08de783a4442"), Name = "Laptop", CreatedDate = DateTime.UtcNow }
            };
            await context.ProductTemplates.AddRangeAsync(productTemplates, cancellationToken);

            var templateAttributes = GetProductTemplateProductAttributes();
            await context.ProductTemplateProductAttributes.AddRangeAsync(templateAttributes, cancellationToken);
        }

        private static List<ProductAttributeValue> GetProductAttributeValues()
        {
            return new List<ProductAttributeValue>
            {
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

                new() { Id = Guid.Parse("c33c95ac-c668-4592-9b8d-08de78387cf8"), AttributeId = Guid.Parse("23be21d4-3622-4206-52d1-08de783534be"), ProductId = Guid.Parse("6ff755bd-47ac-4397-d6a4-08de78387ceb"), Value = "3-cell Li-ion, 41 Wh" },
                new() { Id = Guid.Parse("967f5c3a-e81b-4fd2-9b8e-08de78387cf8"), AttributeId = Guid.Parse("74d4f4b8-64f7-46ee-9c15-3ee3743514bf"), ProductId = Guid.Parse("6ff755bd-47ac-4397-d6a4-08de78387ceb"), Value = "15.6 inch" },
                new() { Id = Guid.Parse("b6e2089b-c792-4e24-9b8f-08de78387cf8"), AttributeId = Guid.Parse("2f16e475-7ccf-4bfd-ab37-46fe0dd725bf"), ProductId = Guid.Parse("6ff755bd-47ac-4397-d6a4-08de78387ceb"), Value = "Wi-Fi 6 (802.11ax)" },
                new() { Id = Guid.Parse("f29e151c-f3ad-46e2-9b90-08de78387cf8"), AttributeId = Guid.Parse("7d637bdf-7801-40c2-b813-4e3c68a0502b"), ProductId = Guid.Parse("6ff755bd-47ac-4397-d6a4-08de78387ceb"), Value = "Anti-Glare" },
                new() { Id = Guid.Parse("a642986b-b9ea-4a63-9b91-08de78387cf8"), AttributeId = Guid.Parse("6dda4bd4-538c-4a56-bb9a-804e4e477456"), ProductId = Guid.Parse("6ff755bd-47ac-4397-d6a4-08de78387ceb"), Value = "Intel Core i5 Raptor Lake - 1334U" },
                new() { Id = Guid.Parse("b1e87a40-b4ad-41c2-9b92-08de78387cf8"), AttributeId = Guid.Parse("0ae35309-b38e-4002-a418-8c3f2d5ff66b"), ProductId = Guid.Parse("6ff755bd-47ac-4397-d6a4-08de78387ceb"), Value = "512 GB NVMe PCIe SSD" },
                new() { Id = Guid.Parse("8a22d709-8b3c-47b8-9b93-08de78387cf8"), AttributeId = Guid.Parse("18bbcb21-0c25-4543-af2a-b855e87e01f4"), ProductId = Guid.Parse("6ff755bd-47ac-4397-d6a4-08de78387ceb"), Value = "Windows 11 Home SL + Office Home 2024 lifetime license + Microsoft 365 Basic" },
                new() { Id = Guid.Parse("acc32c6a-ae06-478e-9b94-08de78387cf8"), AttributeId = Guid.Parse("022c909e-bcf6-40ce-a490-e781557b9b96"), ProductId = Guid.Parse("6ff755bd-47ac-4397-d6a4-08de78387ceb"), Value = "Integrated graphics card - Intel UHD Graphics" },
                new() { Id = Guid.Parse("a524b4a3-88b8-4f63-9b95-08de78387cf8"), AttributeId = Guid.Parse("0993dc9d-38b9-4c9a-a315-ec6e63a540ea"), ProductId = Guid.Parse("6ff755bd-47ac-4397-d6a4-08de78387ceb"), Value = "16 GB" },
                new() { Id = Guid.Parse("48054d10-3d18-4d0d-9b96-08de78387cf8"), AttributeId = Guid.Parse("67054b20-deb6-46dd-9e8f-edaf0b55a551"), ProductId = Guid.Parse("6ff755bd-47ac-4397-d6a4-08de78387ceb"), Value = "Full HD (1920 x 1080)" },

                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("fe158671-2d0c-4775-ace8-0017f3e21f02"), ProductId = Guid.Parse("a1a1a1a1-a1a1-4a1a-8a1a-a1a1a1a1a1a1"), Value = "200 MP" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("8d5fa038-27c6-4184-9b4c-eb50ec7ceb00"), ProductId = Guid.Parse("a1a1a1a1-a1a1-4a1a-8a1a-a1a1a1a1a1a1"), Value = "12 MP" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("23be21d4-3622-4206-52d1-08de783534be"), ProductId = Guid.Parse("a1a1a1a1-a1a1-4a1a-8a1a-a1a1a1a1a1a1"), Value = "5000 mAh" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("9c073c84-a0ea-4a18-52d2-08de783534be"), ProductId = Guid.Parse("a1a1a1a1-a1a1-4a1a-8a1a-a1a1a1a1a1a1"), Value = "Li-Ion" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("d2641e35-ade1-48ac-a660-1fa70c648757"), ProductId = Guid.Parse("a1a1a1a1-a1a1-4a1a-8a1a-a1a1a1a1a1a1"), Value = "2 Nano SIM + eSIM" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("9f898dbd-1c78-4890-ba23-a2703def84f6"), ProductId = Guid.Parse("a1a1a1a1-a1a1-4a1a-8a1a-a1a1a1a1a1a1"), Value = "5G support" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("2f16e475-7ccf-4bfd-ab37-46fe0dd725bf"), ProductId = Guid.Parse("a1a1a1a1-a1a1-4a1a-8a1a-a1a1a1a1a1a1"), Value = "Wi-Fi 7" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("1416eaa3-2f64-4fb9-966a-86c6dd76166a"), ProductId = Guid.Parse("a1a1a1a1-a1a1-4a1a-8a1a-a1a1a1a1a1a1"), Value = "v5.4" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("6dda4bd4-538c-4a56-bb9a-804e4e477456"), ProductId = Guid.Parse("a1a1a1a1-a1a1-4a1a-8a1a-a1a1a1a1a1a1"), Value = "Snapdragon 8 Elite" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("18bbcb21-0c25-4543-af2a-b855e87e01f4"), ProductId = Guid.Parse("a1a1a1a1-a1a1-4a1a-8a1a-a1a1a1a1a1a1"), Value = "Android 15" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("0993dc9d-38b9-4c9a-a315-ec6e63a540ea"), ProductId = Guid.Parse("a1a1a1a1-a1a1-4a1a-8a1a-a1a1a1a1a1a1"), Value = "12 GB" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("0ae35309-b38e-4002-a418-8c3f2d5ff66b"), ProductId = Guid.Parse("a1a1a1a1-a1a1-4a1a-8a1a-a1a1a1a1a1a1"), Value = "512 GB" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("74d4f4b8-64f7-46ee-9c15-3ee3743514bf"), ProductId = Guid.Parse("a1a1a1a1-a1a1-4a1a-8a1a-a1a1a1a1a1a1"), Value = "6.9 inch" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("7d637bdf-7801-40c2-b813-4e3c68a0502b"), ProductId = Guid.Parse("a1a1a1a1-a1a1-4a1a-8a1a-a1a1a1a1a1a1"), Value = "Dynamic AMOLED 2X" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("67054b20-deb6-46dd-9e8f-edaf0b55a551"), ProductId = Guid.Parse("a1a1a1a1-a1a1-4a1a-8a1a-a1a1a1a1a1a1"), Value = "QHD+ (3120 x 1440 Pixels)" },

                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("fe158671-2d0c-4775-ace8-0017f3e21f02"), ProductId = Guid.Parse("b2b2b2b2-b2b2-4b2b-8b2b-b2b2b2b2b2b2"), Value = "50 MP OIS" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("23be21d4-3622-4206-52d1-08de783534be"), ProductId = Guid.Parse("b2b2b2b2-b2b2-4b2b-8b2b-b2b2b2b2b2b2"), Value = "5800 mAh" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("d2641e35-ade1-48ac-a660-1fa70c648757"), ProductId = Guid.Parse("b2b2b2b2-b2b2-4b2b-8b2b-b2b2b2b2b2b2"), Value = "2 Nano SIM" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("9f898dbd-1c78-4890-ba23-a2703def84f6"), ProductId = Guid.Parse("b2b2b2b2-b2b2-4b2b-8b2b-b2b2b2b2b2b2"), Value = "5G support" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("2f16e475-7ccf-4bfd-ab37-46fe0dd725bf"), ProductId = Guid.Parse("b2b2b2b2-b2b2-4b2b-8b2b-b2b2b2b2b2b2"), Value = "Wi-Fi 5" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("1416eaa3-2f64-4fb9-966a-86c6dd76166a"), ProductId = Guid.Parse("b2b2b2b2-b2b2-4b2b-8b2b-b2b2b2b2b2b2"), Value = "v5.1" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("6dda4bd4-538c-4a56-bb9a-804e4e477456"), ProductId = Guid.Parse("b2b2b2b2-b2b2-4b2b-8b2b-b2b2b2b2b2b2"), Value = "Snapdragon 6 Gen 1" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("18bbcb21-0c25-4543-af2a-b855e87e01f4"), ProductId = Guid.Parse("b2b2b2b2-b2b2-4b2b-8b2b-b2b2b2b2b2b2"), Value = "Android 15" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("0993dc9d-38b9-4c9a-a315-ec6e63a540ea"), ProductId = Guid.Parse("b2b2b2b2-b2b2-4b2b-8b2b-b2b2b2b2b2b2"), Value = "8 GB" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("0ae35309-b38e-4002-a418-8c3f2d5ff66b"), ProductId = Guid.Parse("b2b2b2b2-b2b2-4b2b-8b2b-b2b2b2b2b2b2"), Value = "256 GB" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("74d4f4b8-64f7-46ee-9c15-3ee3743514bf"), ProductId = Guid.Parse("b2b2b2b2-b2b2-4b2b-8b2b-b2b2b2b2b2b2"), Value = "6.67 inch" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("7d637bdf-7801-40c2-b813-4e3c68a0502b"), ProductId = Guid.Parse("b2b2b2b2-b2b2-4b2b-8b2b-b2b2b2b2b2b2"), Value = "AMOLED" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("67054b20-deb6-46dd-9e8f-edaf0b55a551"), ProductId = Guid.Parse("b2b2b2b2-b2b2-4b2b-8b2b-b2b2b2b2b2b2"), Value = "Full HD+ (1080 x 2400 Pixels)" },

                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("fe158671-2d0c-4775-ace8-0017f3e21f02"), ProductId = Guid.Parse("c3c3c3c3-c3c3-4c3c-8c3c-c3c3c3c3c3c3"), Value = "QVGA" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("23be21d4-3622-4206-52d1-08de783534be"), ProductId = Guid.Parse("c3c3c3c3-c3c3-4c3c-8c3c-c3c3c3c3c3c3"), Value = "1000 mAh" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("9c073c84-a0ea-4a18-52d2-08de783534be"), ProductId = Guid.Parse("c3c3c3c3-c3c3-4c3c-8c3c-c3c3c3c3c3c3"), Value = "Li-Ion (removable)" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("d2641e35-ade1-48ac-a660-1fa70c648757"), ProductId = Guid.Parse("c3c3c3c3-c3c3-4c3c-8c3c-c3c3c3c3c3c3"), Value = "2 Nano SIM" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("9f898dbd-1c78-4890-ba23-a2703def84f6"), ProductId = Guid.Parse("c3c3c3c3-c3c3-4c3c-8c3c-c3c3c3c3c3c3"), Value = "4G" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("18bbcb21-0c25-4543-af2a-b855e87e01f4"), ProductId = Guid.Parse("c3c3c3c3-c3c3-4c3c-8c3c-c3c3c3c3c3c3"), Value = "S30+" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("74d4f4b8-64f7-46ee-9c15-3ee3743514bf"), ProductId = Guid.Parse("c3c3c3c3-c3c3-4c3c-8c3c-c3c3c3c3c3c3"), Value = "2.4 inch" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("1416eaa3-2f64-4fb9-966a-86c6dd76166a"), ProductId = Guid.Parse("c3c3c3c3-c3c3-4c3c-8c3c-c3c3c3c3c3c3"), Value = "v5.0" },

                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("6dda4bd4-538c-4a56-bb9a-804e4e477456"), ProductId = Guid.Parse("d4d4d4d4-d4d4-4d4d-8d4d-d4d4d4d4d4d4"), Value = "Intel Core i7-14650HX" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("022c909e-bcf6-40ce-a490-e781557b9b96"), ProductId = Guid.Parse("d4d4d4d4-d4d4-4d4d-8d4d-d4d4d4d4d4d4"), Value = "RTX 4050 6GB GDDR6" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("0993dc9d-38b9-4c9a-a315-ec6e63a540ea"), ProductId = Guid.Parse("d4d4d4d4-d4d4-4d4d-8d4d-d4d4d4d4d4d4"), Value = "16 GB DDR5" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("0ae35309-b38e-4002-a418-8c3f2d5ff66b"), ProductId = Guid.Parse("d4d4d4d4-d4d4-4d4d-8d4d-d4d4d4d4d4d4"), Value = "512 GB NVMe PCIe SSD" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("18bbcb21-0c25-4543-af2a-b855e87e01f4"), ProductId = Guid.Parse("d4d4d4d4-d4d4-4d4d-8d4d-d4d4d4d4d4d4"), Value = "Windows 11 Home" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("74d4f4b8-64f7-46ee-9c15-3ee3743514bf"), ProductId = Guid.Parse("d4d4d4d4-d4d4-4d4d-8d4d-d4d4d4d4d4d4"), Value = "16 inch" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("7d637bdf-7801-40c2-b813-4e3c68a0502b"), ProductId = Guid.Parse("d4d4d4d4-d4d4-4d4d-8d4d-d4d4d4d4d4d4"), Value = "ROG Nebula Anti-Glare" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("67054b20-deb6-46dd-9e8f-edaf0b55a551"), ProductId = Guid.Parse("d4d4d4d4-d4d4-4d4d-8d4d-d4d4d4d4d4d4"), Value = "WUXGA (1920 x 1200) 165Hz" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("2f16e475-7ccf-4bfd-ab37-46fe0dd725bf"), ProductId = Guid.Parse("d4d4d4d4-d4d4-4d4d-8d4d-d4d4d4d4d4d4"), Value = "Wi-Fi 6E" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("1416eaa3-2f64-4fb9-966a-86c6dd76166a"), ProductId = Guid.Parse("d4d4d4d4-d4d4-4d4d-8d4d-d4d4d4d4d4d4"), Value = "v5.3" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("23be21d4-3622-4206-52d1-08de783534be"), ProductId = Guid.Parse("d4d4d4d4-d4d4-4d4d-8d4d-d4d4d4d4d4d4"), Value = "90Wh 4-cell" },

                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("6dda4bd4-538c-4a56-bb9a-804e4e477456"), ProductId = Guid.Parse("e5e5e5e5-e5e5-4e5e-8e5e-e5e5e5e5e5e5"), Value = "AMD Ryzen 7 7735U" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("022c909e-bcf6-40ce-a490-e781557b9b96"), ProductId = Guid.Parse("e5e5e5e5-e5e5-4e5e-8e5e-e5e5e5e5e5e5"), Value = "AMD Radeon 680M" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("0993dc9d-38b9-4c9a-a315-ec6e63a540ea"), ProductId = Guid.Parse("e5e5e5e5-e5e5-4e5e-8e5e-e5e5e5e5e5e5"), Value = "16 GB LPDDR5" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("0ae35309-b38e-4002-a418-8c3f2d5ff66b"), ProductId = Guid.Parse("e5e5e5e5-e5e5-4e5e-8e5e-e5e5e5e5e5e5"), Value = "512 GB NVMe SSD" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("18bbcb21-0c25-4543-af2a-b855e87e01f4"), ProductId = Guid.Parse("e5e5e5e5-e5e5-4e5e-8e5e-e5e5e5e5e5e5"), Value = "Windows 11 Pro" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("74d4f4b8-64f7-46ee-9c15-3ee3743514bf"), ProductId = Guid.Parse("e5e5e5e5-e5e5-4e5e-8e5e-e5e5e5e5e5e5"), Value = "14 inch" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("7d637bdf-7801-40c2-b813-4e3c68a0502b"), ProductId = Guid.Parse("e5e5e5e5-e5e5-4e5e-8e5e-e5e5e5e5e5e5"), Value = "IPS Anti-glare" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("67054b20-deb6-46dd-9e8f-edaf0b55a551"), ProductId = Guid.Parse("e5e5e5e5-e5e5-4e5e-8e5e-e5e5e5e5e5e5"), Value = "WUXGA (1920 x 1200)" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("2f16e475-7ccf-4bfd-ab37-46fe0dd725bf"), ProductId = Guid.Parse("e5e5e5e5-e5e5-4e5e-8e5e-e5e5e5e5e5e5"), Value = "Wi-Fi 6E" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("1416eaa3-2f64-4fb9-966a-86c6dd76166a"), ProductId = Guid.Parse("e5e5e5e5-e5e5-4e5e-8e5e-e5e5e5e5e5e5"), Value = "v5.3" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("23be21d4-3622-4206-52d1-08de783534be"), ProductId = Guid.Parse("e5e5e5e5-e5e5-4e5e-8e5e-e5e5e5e5e5e5"), Value = "57Wh" },

                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("6dda4bd4-538c-4a56-bb9a-804e4e477456"), ProductId = Guid.Parse("f6f6f6f6-f6f6-4f6f-8f6f-f6f6f6f6f6f6"), Value = "Intel Core i5-1335U" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("022c909e-bcf6-40ce-a490-e781557b9b96"), ProductId = Guid.Parse("f6f6f6f6-f6f6-4f6f-8f6f-f6f6f6f6f6f6"), Value = "Intel Iris Xe" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("0993dc9d-38b9-4c9a-a315-ec6e63a540ea"), ProductId = Guid.Parse("f6f6f6f6-f6f6-4f6f-8f6f-f6f6f6f6f6f6"), Value = "16 GB DDR4" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("0ae35309-b38e-4002-a418-8c3f2d5ff66b"), ProductId = Guid.Parse("f6f6f6f6-f6f6-4f6f-8f6f-f6f6f6f6f6f6"), Value = "512 GB NVMe SSD" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("18bbcb21-0c25-4543-af2a-b855e87e01f4"), ProductId = Guid.Parse("f6f6f6f6-f6f6-4f6f-8f6f-f6f6f6f6f6f6"), Value = "Windows 11 Home" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("74d4f4b8-64f7-46ee-9c15-3ee3743514bf"), ProductId = Guid.Parse("f6f6f6f6-f6f6-4f6f-8f6f-f6f6f6f6f6f6"), Value = "15.6 inch" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("7d637bdf-7801-40c2-b813-4e3c68a0502b"), ProductId = Guid.Parse("f6f6f6f6-f6f6-4f6f-8f6f-f6f6f6f6f6f6"), Value = "IPS micro-edge" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("67054b20-deb6-46dd-9e8f-edaf0b55a551"), ProductId = Guid.Parse("f6f6f6f6-f6f6-4f6f-8f6f-f6f6f6f6f6f6"), Value = "Full HD (1920 x 1080)" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("2f16e475-7ccf-4bfd-ab37-46fe0dd725bf"), ProductId = Guid.Parse("f6f6f6f6-f6f6-4f6f-8f6f-f6f6f6f6f6f6"), Value = "Wi-Fi 6E" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("23be21d4-3622-4206-52d1-08de783534be"), ProductId = Guid.Parse("f6f6f6f6-f6f6-4f6f-8f6f-f6f6f6f6f6f6"), Value = "43Wh 3-cell" },

                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("6dda4bd4-538c-4a56-bb9a-804e4e477456"), ProductId = Guid.Parse("a7a7a7a7-a7a7-4a7a-8a7a-a7a7a7a7a7a7"), Value = "Apple M4 10-core" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("022c909e-bcf6-40ce-a490-e781557b9b96"), ProductId = Guid.Parse("a7a7a7a7-a7a7-4a7a-8a7a-a7a7a7a7a7a7"), Value = "Apple GPU 10-core" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("0993dc9d-38b9-4c9a-a315-ec6e63a540ea"), ProductId = Guid.Parse("a7a7a7a7-a7a7-4a7a-8a7a-a7a7a7a7a7a7"), Value = "16 GB unified" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("0ae35309-b38e-4002-a418-8c3f2d5ff66b"), ProductId = Guid.Parse("a7a7a7a7-a7a7-4a7a-8a7a-a7a7a7a7a7a7"), Value = "256 GB SSD" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("18bbcb21-0c25-4543-af2a-b855e87e01f4"), ProductId = Guid.Parse("a7a7a7a7-a7a7-4a7a-8a7a-a7a7a7a7a7a7"), Value = "macOS Sequoia" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("74d4f4b8-64f7-46ee-9c15-3ee3743514bf"), ProductId = Guid.Parse("a7a7a7a7-a7a7-4a7a-8a7a-a7a7a7a7a7a7"), Value = "13.6 inch" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("7d637bdf-7801-40c2-b813-4e3c68a0502b"), ProductId = Guid.Parse("a7a7a7a7-a7a7-4a7a-8a7a-a7a7a7a7a7a7"), Value = "Liquid Retina" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("67054b20-deb6-46dd-9e8f-edaf0b55a551"), ProductId = Guid.Parse("a7a7a7a7-a7a7-4a7a-8a7a-a7a7a7a7a7a7"), Value = "2560 x 1664 Pixels" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("2f16e475-7ccf-4bfd-ab37-46fe0dd725bf"), ProductId = Guid.Parse("a7a7a7a7-a7a7-4a7a-8a7a-a7a7a7a7a7a7"), Value = "Wi-Fi 6E" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("1416eaa3-2f64-4fb9-966a-86c6dd76166a"), ProductId = Guid.Parse("a7a7a7a7-a7a7-4a7a-8a7a-a7a7a7a7a7a7"), Value = "v5.3" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("23be21d4-3622-4206-52d1-08de783534be"), ProductId = Guid.Parse("a7a7a7a7-a7a7-4a7a-8a7a-a7a7a7a7a7a7"), Value = "Up to 18 hours" },

                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("1416eaa3-2f64-4fb9-966a-86c6dd76166a"), ProductId = Guid.Parse("b8b8b8b8-b8b8-4b8b-8b8b-b8b8b8b8b8b8"), Value = "v5.3" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("23be21d4-3622-4206-52d1-08de783534be"), ProductId = Guid.Parse("b8b8b8b8-b8b8-4b8b-8b8b-b8b8b8b8b8b8"), Value = "27 hours with case" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("9c073c84-a0ea-4a18-52d2-08de783534be"), ProductId = Guid.Parse("b8b8b8b8-b8b8-4b8b-8b8b-b8b8b8b8b8b8"), Value = "Li-Ion" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("775a1c47-acd5-4794-a859-09f9aa6cc901"), ProductId = Guid.Parse("b8b8b8b8-b8b8-4b8b-8b8b-b8b8b8b8b8b8"), Value = "USB-C charging" },

                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("23be21d4-3622-4206-52d1-08de783534be"), ProductId = Guid.Parse("c9c9c9c9-c9c9-4c9c-8c9c-c9c9c9c9c9c9"), Value = "10000 mAh" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("9c073c84-a0ea-4a18-52d2-08de783534be"), ProductId = Guid.Parse("c9c9c9c9-c9c9-4c9c-8c9c-c9c9c9c9c9c9"), Value = "Li-Polymer" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("775a1c47-acd5-4794-a859-09f9aa6cc901"), ProductId = Guid.Parse("c9c9c9c9-c9c9-4c9c-8c9c-c9c9c9c9c9c9"), Value = "USB-C 25W, 2 ports" },

                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("0ae35309-b38e-4002-a418-8c3f2d5ff66b"), ProductId = Guid.Parse("d0d0d0d0-d0d0-4d0d-8d0d-d0d0d0d0d0d0"), Value = "256 GB" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("775a1c47-acd5-4794-a859-09f9aa6cc901"), ProductId = Guid.Parse("d0d0d0d0-d0d0-4d0d-8d0d-d0d0d0d0d0d0"), Value = "USB 3.1 Gen 1" },

                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("1416eaa3-2f64-4fb9-966a-86c6dd76166a"), ProductId = Guid.Parse("e1e1e1e1-e1e1-4e1e-8e1e-e1e1e1e1e1e1"), Value = "v5.3" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("23be21d4-3622-4206-52d1-08de783534be"), ProductId = Guid.Parse("e1e1e1e1-e1e1-4e1e-8e1e-e1e1e1e1e1e1"), Value = "30 hours with case" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("9c073c84-a0ea-4a18-52d2-08de783534be"), ProductId = Guid.Parse("e1e1e1e1-e1e1-4e1e-8e1e-e1e1e1e1e1e1"), Value = "Li-Ion" },
                new() { Id = Guid.NewGuid(), AttributeId = Guid.Parse("775a1c47-acd5-4794-a859-09f9aa6cc901"), ProductId = Guid.Parse("e1e1e1e1-e1e1-4e1e-8e1e-e1e1e1e1e1e1"), Value = "USB-C + MagSafe" }
            };
        }

        private static List<ProductTemplateProductAttribute> GetProductTemplateProductAttributes()
        {
            return new List<ProductTemplateProductAttribute>
            {
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
