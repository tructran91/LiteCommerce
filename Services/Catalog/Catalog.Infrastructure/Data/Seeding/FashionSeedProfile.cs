using Catalog.Application.Database;
using Catalog.Core.Entities;

namespace Catalog.Infrastructure.Data.Seeding
{
    public class FashionSeedProfile : ISeedProfile
    {
        public string Name => SeedProfiles.Fashion;

        public async Task SeedAsync(CatalogContext context, CancellationToken cancellationToken = default)
        {
            var nikeId = Guid.Parse("c20c3ba2-c1b6-406a-84d7-d8af5cddf0be");
            var adidasId = Guid.Parse("6476eadc-5aae-4605-adec-3d846107d60b");
            var zaraId = Guid.Parse("a8f423e2-6fda-42a0-b2cf-aad9f7730350");
            var hmId = Guid.Parse("8a799787-459d-40be-be95-6fc7a61dfc9a");
            var uniqloId = Guid.Parse("6b848879-6c67-44de-9be3-b336e68600f4");
            var levisId = Guid.Parse("58263e1b-d9cd-4e69-b793-82c6c3696c19");

            await context.Brands.AddRangeAsync(new List<Brand>
            {
                new() { Id = nikeId, Name = "Nike", Slug = "nike", IsPublished = true, CreatedDate = DateTime.UtcNow },
                new() { Id = adidasId, Name = "Adidas", Slug = "adidas", IsPublished = true, CreatedDate = DateTime.UtcNow },
                new() { Id = zaraId, Name = "Zara", Slug = "zara", IsPublished = true, CreatedDate = DateTime.UtcNow },
                new() { Id = hmId, Name = "H&M", Slug = "hm", IsPublished = true, CreatedDate = DateTime.UtcNow },
                new() { Id = uniqloId, Name = "Uniqlo", Slug = "uniqlo", IsPublished = true, CreatedDate = DateTime.UtcNow },
                new() { Id = levisId, Name = "Levi's", Slug = "levis", IsPublished = true, CreatedDate = DateTime.UtcNow }
            }, cancellationToken);

            var generalGroupId = Guid.Parse("e087a664-4fb4-4e5a-8e10-d9b6163fccdd");
            var materialGroupId = Guid.Parse("ed56aa28-92a6-4361-a339-0e9586ac446b");
            var sizeFitGroupId = Guid.Parse("40401cd5-4def-42a2-85ad-1ebfa0e39a50");

            await context.ProductAttributeGroups.AddRangeAsync(new List<ProductAttributeGroup>
            {
                new() { Id = generalGroupId, Name = "General", CreatedDate = DateTime.UtcNow },
                new() { Id = materialGroupId, Name = "Material", CreatedDate = DateTime.UtcNow },
                new() { Id = sizeFitGroupId, Name = "Size & Fit", CreatedDate = DateTime.UtcNow }
            }, cancellationToken);

            await context.ProductOptions.AddRangeAsync(new List<ProductOption>
            {
                new() { Id = Guid.Parse("31f3dca0-0103-4a07-999a-541cb5c9146f"), Name = "Color" },
                new() { Id = Guid.Parse("156c8217-1692-4305-b6a5-223c7237eb98"), Name = "Size" },
                new() { Id = Guid.Parse("1bf305ba-bb27-4a20-9d4e-6e71dcf0e1dd"), Name = "Material" }
            }, cancellationToken);

            var menId = Guid.Parse("d9ad0686-d5d8-485d-be26-edebe727fc32");
            var womenId = Guid.Parse("986fa362-cb4b-4fa4-b1e2-5f03748908f0");
            var kidsId = Guid.Parse("b8944b80-5b77-4b92-ade7-5daf9833ef80");
            var shoesId = Guid.Parse("4cb10c91-5680-41bb-822d-90c00d863f20");
            var accessoriesId = Guid.Parse("780de891-d88c-4eb2-a782-d8666d2def78");
            var shirtsId = Guid.Parse("21799b5f-a14c-4dab-a11d-fa5237f4b738");
            var jeansId = Guid.Parse("a1c726b6-812c-47d8-9378-5bed61c478f6");
            var dressesId = Guid.Parse("8fd57f4b-05ea-4eaa-bc7c-4caceb0e9ef5");
            var handbagsId = Guid.Parse("be494954-b4bd-4c45-b835-de69cae991be");
            var sneakersId = Guid.Parse("05bfdf68-5db4-46c0-acf7-8684d3260167");
            var topsId = Guid.Parse("b69c183b-475f-4391-82a9-ff54ff11b251");
            var skirtsId = Guid.Parse("b9555d53-c93a-481d-a5a8-1704ed8a65e0");
            var jacketsId = Guid.Parse("85611283-716f-432b-a608-4a3310670789");
            var sandalsId = Guid.Parse("a2d37989-4385-476c-94a9-d8b7556007dc");
            var capsId = Guid.Parse("ac92409c-456a-4101-95f4-113ae1607295");

            await context.Categories.AddRangeAsync(new List<Category>
            {
                new() { Id = menId, Name = "Men", Slug = "men", IsPublished = true, IncludeInMenu = true, DisplayOrder = 0, MetaTitle = "Men's fashion", MetaDescription = "Shop the latest men's fashion.", CreatedDate = DateTime.UtcNow },
                new() { Id = womenId, Name = "Women", Slug = "women", IsPublished = true, IncludeInMenu = true, DisplayOrder = 1, MetaTitle = "Women's fashion", MetaDescription = "Shop the latest women's fashion.", CreatedDate = DateTime.UtcNow },
                new() { Id = kidsId, Name = "Kids", Slug = "kids", IsPublished = true, IncludeInMenu = true, DisplayOrder = 2, MetaTitle = "Kids' fashion", MetaDescription = "Shop the latest kids' fashion.", CreatedDate = DateTime.UtcNow },
                new() { Id = shoesId, Name = "Shoes", Slug = "shoes", IsPublished = true, IncludeInMenu = true, DisplayOrder = 3, MetaTitle = "Shoes for everyone", MetaDescription = "Shop sneakers, sandals and more.", CreatedDate = DateTime.UtcNow },
                new() { Id = accessoriesId, Name = "Accessories", Slug = "accessories", IsPublished = true, IncludeInMenu = true, DisplayOrder = 4, MetaTitle = "Fashion accessories", MetaDescription = "Bags, belts, hats and more.", CreatedDate = DateTime.UtcNow },
                new() { Id = shirtsId, Name = "Shirts", Slug = "shirts", ParentId = menId, IsPublished = true, IncludeInMenu = true, DisplayOrder = 0, MetaTitle = "Men's shirts", MetaDescription = "T-shirts, shirts and polos for men.", CreatedDate = DateTime.UtcNow },
                new() { Id = jeansId, Name = "Jeans", Slug = "jeans", ParentId = menId, IsPublished = true, IncludeInMenu = true, DisplayOrder = 1, MetaTitle = "Men's jeans", MetaDescription = "Slim, skinny and regular jeans.", CreatedDate = DateTime.UtcNow },
                new() { Id = dressesId, Name = "Dresses", Slug = "dresses", ParentId = womenId, IsPublished = true, IncludeInMenu = true, DisplayOrder = 0, MetaTitle = "Women's dresses", MetaDescription = "Midi, maxi and summer dresses.", CreatedDate = DateTime.UtcNow },
                new() { Id = handbagsId, Name = "Handbags", Slug = "handbags", ParentId = womenId, IsPublished = true, IncludeInMenu = true, DisplayOrder = 1, MetaTitle = "Women's handbags", MetaDescription = "Totes, shoulder bags and clutches.", CreatedDate = DateTime.UtcNow },
                new() { Id = sneakersId, Name = "Sneakers", Slug = "sneakers", ParentId = shoesId, IsPublished = true, IncludeInMenu = true, DisplayOrder = 0, MetaTitle = "Sneakers", MetaDescription = "Classic and running sneakers.", CreatedDate = DateTime.UtcNow },
                new() { Id = topsId, Name = "Tops", Slug = "tops", ParentId = womenId, IsPublished = true, IncludeInMenu = true, DisplayOrder = 2, MetaTitle = "Women's tops", MetaDescription = "Blouses, shirts and tops for women.", CreatedDate = DateTime.UtcNow },
                new() { Id = skirtsId, Name = "Skirts", Slug = "skirts", ParentId = womenId, IsPublished = true, IncludeInMenu = true, DisplayOrder = 3, MetaTitle = "Women's skirts", MetaDescription = "Mini, midi and maxi skirts.", CreatedDate = DateTime.UtcNow },
                new() { Id = jacketsId, Name = "Jackets", Slug = "jackets", ParentId = menId, IsPublished = true, IncludeInMenu = true, DisplayOrder = 2, MetaTitle = "Men's jackets", MetaDescription = "Denim, down and casual jackets.", CreatedDate = DateTime.UtcNow },
                new() { Id = sandalsId, Name = "Sandals", Slug = "sandals", ParentId = shoesId, IsPublished = true, IncludeInMenu = true, DisplayOrder = 1, MetaTitle = "Sandals & slides", MetaDescription = "Slides and sandals for summer.", CreatedDate = DateTime.UtcNow },
                new() { Id = capsId, Name = "Caps", Slug = "caps", ParentId = accessoriesId, IsPublished = true, IncludeInMenu = true, DisplayOrder = 0, MetaTitle = "Caps & hats", MetaDescription = "Caps, hats and beanies.", CreatedDate = DateTime.UtcNow }
            }, cancellationToken);

            var colorId = Guid.Parse("575dd91c-af2b-44a1-b1af-7d6a72a7f52d");
            var genderId = Guid.Parse("0edcf70c-5d5a-48b2-b1f0-c7f656ecb1ec");
            var seasonId = Guid.Parse("e23905f3-2412-4d9d-a755-45f0fd9d781b");
            var patternId = Guid.Parse("92306cf0-5d35-4887-adeb-6ed223b7fe1e");
            var materialId = Guid.Parse("91466df5-9b63-4be8-bb5c-7fb299452c80");
            var fabricId = Guid.Parse("ecb3bc51-ed5b-404f-ab95-ce34af90e67c");
            var sizeId = Guid.Parse("62ce01d7-a825-46b4-9bc2-f2161cdadd2c");
            var fitId = Guid.Parse("10547f76-987c-492d-8bb5-aa8a1aca31d1");

            await context.ProductAttributes.AddRangeAsync(new List<ProductAttribute>
            {
                new() { Id = colorId, Name = "Color", GroupId = generalGroupId, CreatedDate = DateTime.UtcNow },
                new() { Id = genderId, Name = "Gender", GroupId = generalGroupId, CreatedDate = DateTime.UtcNow },
                new() { Id = seasonId, Name = "Season", GroupId = generalGroupId, CreatedDate = DateTime.UtcNow },
                new() { Id = patternId, Name = "Pattern", GroupId = generalGroupId, CreatedDate = DateTime.UtcNow },
                new() { Id = materialId, Name = "Material", GroupId = materialGroupId, CreatedDate = DateTime.UtcNow },
                new() { Id = fabricId, Name = "Fabric", GroupId = materialGroupId, CreatedDate = DateTime.UtcNow },
                new() { Id = sizeId, Name = "Size", GroupId = sizeFitGroupId, CreatedDate = DateTime.UtcNow },
                new() { Id = fitId, Name = "Fit", GroupId = sizeFitGroupId, CreatedDate = DateTime.UtcNow }
            }, cancellationToken);

            var af1Id = Guid.Parse("81e49fcd-9fb4-42c5-b249-b761e221ec6f");
            var jeans511Id = Guid.Parse("97e1a478-2486-47e8-ab95-b5d2553abc9d");
            var midiDressId = Guid.Parse("8eb20a83-312a-4b89-9aaf-65c7cc6fb482");
            var supimaTeeId = Guid.Parse("9829112a-1dee-4a07-9f9a-5eea50221bb7");
            var sambaId = Guid.Parse("0e78c473-e6b6-4ca0-bbce-185cbed1adc6");
            var milerId = Guid.Parse("86ce82ab-164a-4efe-b5f8-5fc68f0db60b");
            var hoodieId = Guid.Parse("f58a62eb-5ebf-4866-ade8-c45e4c193705");
            var blouseId = Guid.Parse("c0af743d-7657-4d6c-84f8-8f97134ed0c6");
            var skirtId = Guid.Parse("a48656a0-32b8-472d-b0d9-b1a5e07c04e3");
            var truckerId = Guid.Parse("d17699ea-a6be-4b66-89f8-b75128345e8a");
            var parkaId = Guid.Parse("7ff3f0ce-13f9-428c-bf07-4ced4cbdecfd");
            var benassiId = Guid.Parse("f5a2fdca-ac75-4a5b-b0be-ca835f77b78b");
            var hatId = Guid.Parse("9e2cb3fa-9a01-476d-b33f-5a04333b5b7f");
            var toteId = Guid.Parse("3691d6da-7e86-4a0a-ba20-0b7117648f43");
            var kidsTeeId = Guid.Parse("21d10766-5aca-413a-b408-5d94832d6380");

            await context.Products.AddRangeAsync(new List<Product>
            {
                new()
                {
                    Id = af1Id,
                    Name = "Nike Air Force 1 '07 White",
                    Slug = "nike-air-force-1-07-white",
                    ShortDescription = "<ul><li>Classic leather upper</li><li>Perforated toe box</li><li>Air-Sole cushioning</li></ul>",
                    Description = "<p>The Nike Air Force 1 '07 is an icon of street style: crisp white leather, timeless silhouette and all-day comfort.</p>",
                    IsPublished = true,
                    Price = 3290000m,
                    BrandId = nikeId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Nike Air Force 1 '07 White chính hãng",
                    MetaKeywords = "nike air force 1, af1 white, giày nike",
                    MetaDescription = "Mua Nike Air Force 1 '07 White chính hãng, bảo hành 12 tháng, trả góp 0%. Mua ngay!"
                },
                new()
                {
                    Id = jeans511Id,
                    Name = "Levi's 511 Slim Fit Jeans",
                    Slug = "levis-511-slim-fit-jeans",
                    ShortDescription = "<ul><li>Slim fit, sits below waist</li><li>Stretch denim</li><li>Zip fly</li></ul>",
                    Description = "<p>Levi's 511 Slim jeans in dark wash: a modern slim silhouette with room to move, made from durable stretch denim.</p>",
                    IsPublished = true,
                    Price = 2190000m,
                    BrandId = levisId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Quần Levi's 511 Slim Fit chính hãng",
                    MetaKeywords = "levis 511, quần jeans levis, slim fit jeans",
                    MetaDescription = "Mua quần Levi's 511 Slim Fit chính hãng, đủ size 28-36, đổi trả 30 ngày. Mua ngay!"
                },
                new()
                {
                    Id = midiDressId,
                    Name = "Zara Floral Midi Dress",
                    Slug = "zara-floral-midi-dress",
                    ShortDescription = "<ul><li>Floral print</li><li>V-neck, short sleeves</li><li>Smocked waist</li></ul>",
                    Description = "<p>Zara floral midi dress with a flowy skirt and smocked waist — perfect for summer days and weekend getaways.</p>",
                    IsPublished = true,
                    Price = 1290000m,
                    BrandId = zaraId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Váy Zara Floral Midi chính hãng",
                    MetaKeywords = "váy zara, floral midi dress, đầm hoa",
                    MetaDescription = "Mua váy Zara Floral Midi chính hãng, đủ size XS-XL, đổi trả 30 ngày. Mua ngay!"
                },
                new()
                {
                    Id = supimaTeeId,
                    Name = "Uniqlo Supima Cotton T-Shirt",
                    Slug = "uniqlo-supima-cotton-t-shirt",
                    ShortDescription = "<ul><li>100% Supima cotton</li><li>Crew neck</li><li>Regular fit</li></ul>",
                    Description = "<p>Uniqlo Supima cotton T-shirt: premium long-staple cotton with a smooth hand feel and high color retention.</p>",
                    IsPublished = true,
                    Price = 399000m,
                    BrandId = uniqloId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Áo thun Uniqlo Supima Cotton chính hãng",
                    MetaKeywords = "áo thun uniqlo, supima cotton, áo phông",
                    MetaDescription = "Mua áo thun Uniqlo Supima Cotton chính hãng, nhiều màu, đủ size S-XXL. Mua ngay!"
                },
                new()
                {
                    Id = sambaId,
                    Name = "Adidas Samba OG Cloud White",
                    Slug = "adidas-samba-og-cloud-white",
                    ShortDescription = "<ul><li>Full-grain leather upper</li><li>Suede T-toe overlay</li><li>Gum rubber outsole</li></ul>",
                    Description = "<p>Adidas Samba OG: the timeless indoor-football icon with a soft leather upper and classic gum sole.</p>",
                    IsPublished = true,
                    Price = 2990000m,
                    BrandId = adidasId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Giày Adidas Samba OG chính hãng",
                    MetaKeywords = "adidas samba, samba og, giày samba",
                    MetaDescription = "Mua Adidas Samba OG chính hãng, đủ size 40-44, bảo hành 12 tháng. Mua ngay!"
                },
                new()
                {
                    Id = milerId,
                    Name = "Nike Dri-FIT Miler Running T-Shirt",
                    Slug = "nike-dri-fit-miler-running-t-shirt",
                    ShortDescription = "<ul><li>Dri-FIT moisture-wicking mesh</li><li>Reflective details for night runs</li><li>Standard fit, crew neck</li></ul>",
                    Description = "<p>Nike Dri-FIT Miler: lightweight running tee that keeps you dry and visible on every run.</p>",
                    IsPublished = true,
                    Price = 899000m,
                    BrandId = nikeId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Áo chạy bộ Nike Dri-FIT Miler chính hãng",
                    MetaKeywords = "nike dri-fit, áo chạy bộ, miler tee",
                    MetaDescription = "Mua áo chạy bộ Nike Dri-FIT Miler chính hãng, đủ size S-XXL. Mua ngay!"
                },
                new()
                {
                    Id = hoodieId,
                    Name = "Adidas Adicolor Classics Trefoil Hoodie",
                    Slug = "adidas-adicolor-classics-trefoil-hoodie",
                    ShortDescription = "<ul><li>French terry fleece</li><li>Trefoil logo embroidery</li><li>Kangaroo pocket, drawstring hood</li></ul>",
                    Description = "<p>Adicolor Trefoil hoodie: streetwear classic in soft fleece with the iconic embroidered Trefoil.</p>",
                    IsPublished = true,
                    Price = 1790000m,
                    BrandId = adidasId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Áo hoodie Adidas Adicolor chính hãng",
                    MetaKeywords = "adidas hoodie, adicolor, áo hoodie",
                    MetaDescription = "Mua áo hoodie Adidas Adicolor chính hãng, đủ size S-XXL. Mua ngay!"
                },
                new()
                {
                    Id = blouseId,
                    Name = "H&M Oversized Cotton Blouse",
                    Slug = "hm-oversized-cotton-blouse",
                    ShortDescription = "<ul><li>Airy viscose blend</li><li>Oversized silhouette, V-neck</li><li>Dropped shoulders</li></ul>",
                    Description = "<p>H&M oversized blouse: breezy everyday top that pairs with jeans, skirts or tailored trousers.</p>",
                    IsPublished = true,
                    Price = 599000m,
                    BrandId = hmId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Áo kiểu H&M Oversized chính hãng",
                    MetaKeywords = "áo kiểu hm, oversized blouse, áo nữ",
                    MetaDescription = "Mua áo kiểu H&M Oversized chính hãng, đủ size XS-XL. Mua ngay!"
                },
                new()
                {
                    Id = skirtId,
                    Name = "Zara Satin Slip Skirt",
                    Slug = "zara-satin-slip-skirt",
                    ShortDescription = "<ul><li>Fluid satin finish</li><li>Elastic back waist</li><li>Midi length, side slit</li></ul>",
                    Description = "<p>Zara satin slip skirt: elegant midi with a lustrous drape, easy to dress up or down.</p>",
                    IsPublished = true,
                    Price = 799000m,
                    BrandId = zaraId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Chân váy satin Zara chính hãng",
                    MetaKeywords = "chân váy zara, slip skirt, váy satin",
                    MetaDescription = "Mua chân váy satin Zara chính hãng, đủ size XS-XL. Mua ngay!"
                },
                new()
                {
                    Id = truckerId,
                    Name = "Levi's Type III Trucker Jacket",
                    Slug = "levis-type-iii-trucker-jacket",
                    ShortDescription = "<ul><li>100% cotton denim</li><li>Iconic pointed flaps, shank buttons</li><li>Regular fit, hip length</li></ul>",
                    Description = "<p>Levi's Type III Trucker: the original denim jacket since 1967, better with every wear.</p>",
                    IsPublished = true,
                    Price = 2590000m,
                    BrandId = levisId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Áo khoác Levi's Type III Trucker chính hãng",
                    MetaKeywords = "levis trucker, áo khoác jean, denim jacket",
                    MetaDescription = "Mua áo khoác Levi's Type III Trucker chính hãng, đủ size S-XXL. Mua ngay!"
                },
                new()
                {
                    Id = parkaId,
                    Name = "Uniqlo Ultra Light Down Parka",
                    Slug = "uniqlo-ultra-light-down-parka",
                    ShortDescription = "<ul><li>90% premium down fill</li><li>Packs into its own pocket</li><li>Water-repellent shell</li></ul>",
                    Description = "<p>Uniqlo Ultra Light Down parka: featherlight warmth that compresses to pocket size for travel.</p>",
                    IsPublished = true,
                    Price = 2290000m,
                    BrandId = uniqloId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Áo phao Uniqlo Ultra Light Down chính hãng",
                    MetaKeywords = "uniqlo ultra light down, áo phao lông vũ, parka",
                    MetaDescription = "Mua áo phao Uniqlo Ultra Light Down chính hãng, đủ size S-XXL. Mua ngay!"
                },
                new()
                {
                    Id = benassiId,
                    Name = "Nike Benassi JDI Slides",
                    Slug = "nike-benassi-jdi-slides",
                    ShortDescription = "<ul><li>Soft synthetic strap with Swoosh</li><li>Phylon foam sole</li><li>Textured footbed</li></ul>",
                    Description = "<p>Nike Benassi JDI: poolside essential with plush cushioning and the classic Swoosh logo.</p>",
                    IsPublished = true,
                    Price = 790000m,
                    BrandId = nikeId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Dép Nike Benassi JDI chính hãng",
                    MetaKeywords = "nike benassi, dép nike, slides",
                    MetaDescription = "Mua dép Nike Benassi JDI chính hãng, đủ size 39-44. Mua ngay!"
                },
                new()
                {
                    Id = hatId,
                    Name = "H&M Straw Sun Hat",
                    Slug = "hm-straw-sun-hat",
                    ShortDescription = "<ul><li>Woven paper straw</li><li>Wide brim, inner drawstring</li><li>Unlined, breathable</li></ul>",
                    Description = "<p>H&M straw sun hat: wide-brim summer protection with a natural woven texture.</p>",
                    IsPublished = true,
                    Price = 449000m,
                    BrandId = hmId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Mũ cói H&M Straw Sun Hat chính hãng",
                    MetaKeywords = "mũ cói hm, sun hat, mũ đi biển",
                    MetaDescription = "Mua mũ cói H&M Straw Sun Hat chính hãng, freesize. Mua ngay!"
                },
                new()
                {
                    Id = toteId,
                    Name = "Zara Structured Tote Bag",
                    Slug = "zara-structured-tote-bag",
                    ShortDescription = "<ul><li>Structured silhouette</li><li>Top handles + detachable strap</li><li>Magnetic closure, inner pocket</li></ul>",
                    Description = "<p>Zara structured tote: polished everyday bag that fits a 13-inch laptop and daily essentials.</p>",
                    IsPublished = true,
                    Price = 1990000m,
                    BrandId = zaraId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Túi tote Zara Structured chính hãng",
                    MetaKeywords = "túi zara, tote bag, structured tote",
                    MetaDescription = "Mua túi tote Zara Structured chính hãng, bảo hành 12 tháng. Mua ngay!"
                },
                new()
                {
                    Id = kidsTeeId,
                    Name = "Uniqlo Kids AIRism Cotton T-Shirt",
                    Slug = "uniqlo-kids-airism-cotton-t-shirt",
                    ShortDescription = "<ul><li>AIRism breathable mesh</li><li>Quick-drying, odor control</li><li>Tagless, kid-friendly fit</li></ul>",
                    Description = "<p>Uniqlo Kids AIRism tee: cool and dry comfort for active kids, in fun everyday colors.</p>",
                    IsPublished = true,
                    Price = 299000m,
                    BrandId = uniqloId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Áo thun trẻ em Uniqlo AIRism chính hãng",
                    MetaKeywords = "uniqlo kids, áo trẻ em, airism tee",
                    MetaDescription = "Mua áo thun trẻ em Uniqlo AIRism chính hãng, size 110-150. Mua ngay!"
                }
            }, cancellationToken);

            await context.ProductCategories.AddRangeAsync(new List<ProductCategory>
            {
                new() { Id = Guid.NewGuid(), CategoryId = shoesId, ProductId = af1Id, DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = sneakersId, ProductId = af1Id, DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = menId, ProductId = jeans511Id, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = jeansId, ProductId = jeans511Id, DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = womenId, ProductId = midiDressId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = dressesId, ProductId = midiDressId, DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = menId, ProductId = supimaTeeId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = shirtsId, ProductId = supimaTeeId, DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = shoesId, ProductId = sambaId, DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = sneakersId, ProductId = sambaId, DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = menId, ProductId = milerId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = shirtsId, ProductId = milerId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = menId, ProductId = hoodieId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = shirtsId, ProductId = hoodieId, DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = womenId, ProductId = blouseId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = topsId, ProductId = blouseId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = womenId, ProductId = skirtId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = skirtsId, ProductId = skirtId, DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = menId, ProductId = truckerId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = jacketsId, ProductId = truckerId, DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = menId, ProductId = parkaId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = jacketsId, ProductId = parkaId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = shoesId, ProductId = benassiId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = sandalsId, ProductId = benassiId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = accessoriesId, ProductId = hatId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = capsId, ProductId = hatId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = womenId, ProductId = toteId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = handbagsId, ProductId = toteId, DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = kidsId, ProductId = kidsTeeId, DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow }
            }, cancellationToken);

            await context.ProductAttributeValues.AddRangeAsync(new List<ProductAttributeValue>
            {
                new() { Id = Guid.NewGuid(), AttributeId = colorId, ProductId = af1Id, Value = "White" },
                new() { Id = Guid.NewGuid(), AttributeId = sizeId, ProductId = af1Id, Value = "42" },
                new() { Id = Guid.NewGuid(), AttributeId = materialId, ProductId = af1Id, Value = "Leather" },
                new() { Id = Guid.NewGuid(), AttributeId = genderId, ProductId = af1Id, Value = "Unisex" },
                new() { Id = Guid.NewGuid(), AttributeId = seasonId, ProductId = af1Id, Value = "All-season" },

                new() { Id = Guid.NewGuid(), AttributeId = colorId, ProductId = jeans511Id, Value = "Dark Blue" },
                new() { Id = Guid.NewGuid(), AttributeId = sizeId, ProductId = jeans511Id, Value = "32" },
                new() { Id = Guid.NewGuid(), AttributeId = materialId, ProductId = jeans511Id, Value = "Denim" },
                new() { Id = Guid.NewGuid(), AttributeId = fabricId, ProductId = jeans511Id, Value = "98% Cotton, 2% Elastane" },
                new() { Id = Guid.NewGuid(), AttributeId = fitId, ProductId = jeans511Id, Value = "Slim" },
                new() { Id = Guid.NewGuid(), AttributeId = genderId, ProductId = jeans511Id, Value = "Men" },

                new() { Id = Guid.NewGuid(), AttributeId = colorId, ProductId = midiDressId, Value = "Floral" },
                new() { Id = Guid.NewGuid(), AttributeId = sizeId, ProductId = midiDressId, Value = "M" },
                new() { Id = Guid.NewGuid(), AttributeId = materialId, ProductId = midiDressId, Value = "Polyester" },
                new() { Id = Guid.NewGuid(), AttributeId = patternId, ProductId = midiDressId, Value = "Floral" },
                new() { Id = Guid.NewGuid(), AttributeId = fitId, ProductId = midiDressId, Value = "Regular" },
                new() { Id = Guid.NewGuid(), AttributeId = genderId, ProductId = midiDressId, Value = "Women" },
                new() { Id = Guid.NewGuid(), AttributeId = seasonId, ProductId = midiDressId, Value = "Summer" },

                new() { Id = Guid.NewGuid(), AttributeId = colorId, ProductId = supimaTeeId, Value = "White" },
                new() { Id = Guid.NewGuid(), AttributeId = sizeId, ProductId = supimaTeeId, Value = "L" },
                new() { Id = Guid.NewGuid(), AttributeId = materialId, ProductId = supimaTeeId, Value = "Supima Cotton" },
                new() { Id = Guid.NewGuid(), AttributeId = fabricId, ProductId = supimaTeeId, Value = "100% Cotton" },
                new() { Id = Guid.NewGuid(), AttributeId = fitId, ProductId = supimaTeeId, Value = "Regular" },
                new() { Id = Guid.NewGuid(), AttributeId = genderId, ProductId = supimaTeeId, Value = "Men" },

                new() { Id = Guid.NewGuid(), AttributeId = colorId, ProductId = sambaId, Value = "Cloud White" },
                new() { Id = Guid.NewGuid(), AttributeId = sizeId, ProductId = sambaId, Value = "42" },
                new() { Id = Guid.NewGuid(), AttributeId = materialId, ProductId = sambaId, Value = "Leather" },
                new() { Id = Guid.NewGuid(), AttributeId = genderId, ProductId = sambaId, Value = "Unisex" },
                new() { Id = Guid.NewGuid(), AttributeId = seasonId, ProductId = sambaId, Value = "All-season" },

                new() { Id = Guid.NewGuid(), AttributeId = colorId, ProductId = milerId, Value = "Black" },
                new() { Id = Guid.NewGuid(), AttributeId = sizeId, ProductId = milerId, Value = "L" },
                new() { Id = Guid.NewGuid(), AttributeId = materialId, ProductId = milerId, Value = "Polyester" },
                new() { Id = Guid.NewGuid(), AttributeId = fabricId, ProductId = milerId, Value = "Dri-FIT mesh" },
                new() { Id = Guid.NewGuid(), AttributeId = fitId, ProductId = milerId, Value = "Regular" },
                new() { Id = Guid.NewGuid(), AttributeId = genderId, ProductId = milerId, Value = "Men" },
                new() { Id = Guid.NewGuid(), AttributeId = seasonId, ProductId = milerId, Value = "Summer" },

                new() { Id = Guid.NewGuid(), AttributeId = colorId, ProductId = hoodieId, Value = "Collegiate Green" },
                new() { Id = Guid.NewGuid(), AttributeId = sizeId, ProductId = hoodieId, Value = "M" },
                new() { Id = Guid.NewGuid(), AttributeId = materialId, ProductId = hoodieId, Value = "Cotton fleece" },
                new() { Id = Guid.NewGuid(), AttributeId = fabricId, ProductId = hoodieId, Value = "70% Cotton, 30% Polyester" },
                new() { Id = Guid.NewGuid(), AttributeId = fitId, ProductId = hoodieId, Value = "Loose" },
                new() { Id = Guid.NewGuid(), AttributeId = genderId, ProductId = hoodieId, Value = "Unisex" },
                new() { Id = Guid.NewGuid(), AttributeId = seasonId, ProductId = hoodieId, Value = "All-season" },

                new() { Id = Guid.NewGuid(), AttributeId = colorId, ProductId = blouseId, Value = "White" },
                new() { Id = Guid.NewGuid(), AttributeId = sizeId, ProductId = blouseId, Value = "M" },
                new() { Id = Guid.NewGuid(), AttributeId = materialId, ProductId = blouseId, Value = "Viscose" },
                new() { Id = Guid.NewGuid(), AttributeId = patternId, ProductId = blouseId, Value = "Solid" },
                new() { Id = Guid.NewGuid(), AttributeId = fitId, ProductId = blouseId, Value = "Oversized" },
                new() { Id = Guid.NewGuid(), AttributeId = genderId, ProductId = blouseId, Value = "Women" },
                new() { Id = Guid.NewGuid(), AttributeId = seasonId, ProductId = blouseId, Value = "Summer" },

                new() { Id = Guid.NewGuid(), AttributeId = colorId, ProductId = skirtId, Value = "Champagne" },
                new() { Id = Guid.NewGuid(), AttributeId = sizeId, ProductId = skirtId, Value = "S" },
                new() { Id = Guid.NewGuid(), AttributeId = materialId, ProductId = skirtId, Value = "Satin polyester" },
                new() { Id = Guid.NewGuid(), AttributeId = patternId, ProductId = skirtId, Value = "Solid" },
                new() { Id = Guid.NewGuid(), AttributeId = fitId, ProductId = skirtId, Value = "Regular" },
                new() { Id = Guid.NewGuid(), AttributeId = genderId, ProductId = skirtId, Value = "Women" },

                new() { Id = Guid.NewGuid(), AttributeId = colorId, ProductId = truckerId, Value = "Medium Wash" },
                new() { Id = Guid.NewGuid(), AttributeId = sizeId, ProductId = truckerId, Value = "M" },
                new() { Id = Guid.NewGuid(), AttributeId = materialId, ProductId = truckerId, Value = "Denim" },
                new() { Id = Guid.NewGuid(), AttributeId = fabricId, ProductId = truckerId, Value = "100% Cotton" },
                new() { Id = Guid.NewGuid(), AttributeId = fitId, ProductId = truckerId, Value = "Regular" },
                new() { Id = Guid.NewGuid(), AttributeId = genderId, ProductId = truckerId, Value = "Men" },
                new() { Id = Guid.NewGuid(), AttributeId = seasonId, ProductId = truckerId, Value = "All-season" },

                new() { Id = Guid.NewGuid(), AttributeId = colorId, ProductId = parkaId, Value = "Black" },
                new() { Id = Guid.NewGuid(), AttributeId = sizeId, ProductId = parkaId, Value = "L" },
                new() { Id = Guid.NewGuid(), AttributeId = materialId, ProductId = parkaId, Value = "90% Down" },
                new() { Id = Guid.NewGuid(), AttributeId = fabricId, ProductId = parkaId, Value = "100% Nylon shell" },
                new() { Id = Guid.NewGuid(), AttributeId = fitId, ProductId = parkaId, Value = "Regular" },
                new() { Id = Guid.NewGuid(), AttributeId = genderId, ProductId = parkaId, Value = "Men" },
                new() { Id = Guid.NewGuid(), AttributeId = seasonId, ProductId = parkaId, Value = "Winter" },

                new() { Id = Guid.NewGuid(), AttributeId = colorId, ProductId = benassiId, Value = "Black" },
                new() { Id = Guid.NewGuid(), AttributeId = sizeId, ProductId = benassiId, Value = "43" },
                new() { Id = Guid.NewGuid(), AttributeId = materialId, ProductId = benassiId, Value = "Synthetic" },
                new() { Id = Guid.NewGuid(), AttributeId = genderId, ProductId = benassiId, Value = "Men" },
                new() { Id = Guid.NewGuid(), AttributeId = seasonId, ProductId = benassiId, Value = "Summer" },

                new() { Id = Guid.NewGuid(), AttributeId = colorId, ProductId = hatId, Value = "Natural" },
                new() { Id = Guid.NewGuid(), AttributeId = sizeId, ProductId = hatId, Value = "One size" },
                new() { Id = Guid.NewGuid(), AttributeId = materialId, ProductId = hatId, Value = "Paper straw" },
                new() { Id = Guid.NewGuid(), AttributeId = patternId, ProductId = hatId, Value = "Woven" },
                new() { Id = Guid.NewGuid(), AttributeId = genderId, ProductId = hatId, Value = "Women" },
                new() { Id = Guid.NewGuid(), AttributeId = seasonId, ProductId = hatId, Value = "Summer" },

                new() { Id = Guid.NewGuid(), AttributeId = colorId, ProductId = toteId, Value = "Brown" },
                new() { Id = Guid.NewGuid(), AttributeId = sizeId, ProductId = toteId, Value = "One size" },
                new() { Id = Guid.NewGuid(), AttributeId = materialId, ProductId = toteId, Value = "Faux leather" },
                new() { Id = Guid.NewGuid(), AttributeId = patternId, ProductId = toteId, Value = "Solid" },
                new() { Id = Guid.NewGuid(), AttributeId = genderId, ProductId = toteId, Value = "Women" },
                new() { Id = Guid.NewGuid(), AttributeId = seasonId, ProductId = toteId, Value = "All-season" },

                new() { Id = Guid.NewGuid(), AttributeId = colorId, ProductId = kidsTeeId, Value = "Navy" },
                new() { Id = Guid.NewGuid(), AttributeId = sizeId, ProductId = kidsTeeId, Value = "130" },
                new() { Id = Guid.NewGuid(), AttributeId = materialId, ProductId = kidsTeeId, Value = "AIRism mesh" },
                new() { Id = Guid.NewGuid(), AttributeId = fitId, ProductId = kidsTeeId, Value = "Regular" },
                new() { Id = Guid.NewGuid(), AttributeId = genderId, ProductId = kidsTeeId, Value = "Kids" },
                new() { Id = Guid.NewGuid(), AttributeId = seasonId, ProductId = kidsTeeId, Value = "Summer" }
            }, cancellationToken);

            var clothingTemplateId = Guid.Parse("efb76d54-015f-4e6f-8807-996afc49acc4");
            var shoesTemplateId = Guid.Parse("6d1cd852-f2bf-49c3-87ac-d9011e44fa2a");

            await context.ProductTemplates.AddRangeAsync(new List<ProductTemplate>
            {
                new() { Id = clothingTemplateId, Name = "Clothing", CreatedDate = DateTime.UtcNow },
                new() { Id = shoesTemplateId, Name = "Shoes", CreatedDate = DateTime.UtcNow }
            }, cancellationToken);

            var clothingAttributes = new[] { colorId, sizeId, materialId, fabricId, fitId, patternId, genderId, seasonId };
            var shoesAttributes = new[] { colorId, sizeId, materialId, genderId, seasonId };

            await context.ProductTemplateProductAttributes.AddRangeAsync(
                clothingAttributes
                    .Select(a => new ProductTemplateProductAttribute { Id = Guid.NewGuid(), ProductTemplateId = clothingTemplateId, ProductAttributeId = a, CreatedDate = DateTime.UtcNow })
                    .Concat(shoesAttributes
                        .Select(a => new ProductTemplateProductAttribute { Id = Guid.NewGuid(), ProductTemplateId = shoesTemplateId, ProductAttributeId = a, CreatedDate = DateTime.UtcNow }))
                    .ToList(), cancellationToken);
        }
    }
}
