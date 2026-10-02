using Catalog.Application.Database;
using Catalog.Core.Entities;

namespace Catalog.Infrastructure.Data.Seeding
{
    public class CosmeticsSeedProfile : ISeedProfile
    {
        public string Name => SeedProfiles.Cosmetics;

        public async Task SeedAsync(CatalogContext context, CancellationToken cancellationToken = default)
        {
            var lorealId = Guid.Parse("c0ffee00-0000-4000-8000-000000000001");
            var maybellineId = Guid.Parse("c0ffee00-0000-4000-8000-000000000002");
            var garnierId = Guid.Parse("c0ffee00-0000-4000-8000-000000000003");
            var innisfreeId = Guid.Parse("c0ffee00-0000-4000-8000-000000000004");
            var laRochePosayId = Guid.Parse("c0ffee00-0000-4000-8000-000000000005");
            var cocoonId = Guid.Parse("c0ffee00-0000-4000-8000-000000000006");

            await context.Brands.AddRangeAsync(new List<Brand>
            {
                new() { Id = lorealId, Name = "L'Oréal Paris", Slug = "loreal-paris", IsPublished = true, CreatedDate = DateTime.UtcNow },
                new() { Id = maybellineId, Name = "Maybelline New York", Slug = "maybelline-new-york", IsPublished = true, CreatedDate = DateTime.UtcNow },
                new() { Id = garnierId, Name = "Garnier", Slug = "garnier", IsPublished = true, CreatedDate = DateTime.UtcNow },
                new() { Id = innisfreeId, Name = "Innisfree", Slug = "innisfree", IsPublished = true, CreatedDate = DateTime.UtcNow },
                new() { Id = laRochePosayId, Name = "La Roche-Posay", Slug = "la-roche-posay", IsPublished = true, CreatedDate = DateTime.UtcNow },
                new() { Id = cocoonId, Name = "Cocoon", Slug = "cocoon", IsPublished = true, CreatedDate = DateTime.UtcNow }
            }, cancellationToken);

            var generalGroupId = Guid.Parse("c0ffee00-0000-4000-8000-000000000007");
            var formulaGroupId = Guid.Parse("c0ffee00-0000-4000-8000-000000000008");
            var packagingGroupId = Guid.Parse("c0ffee00-0000-4000-8000-000000000009");

            await context.ProductAttributeGroups.AddRangeAsync(new List<ProductAttributeGroup>
            {
                new() { Id = generalGroupId, Name = "General", CreatedDate = DateTime.UtcNow },
                new() { Id = formulaGroupId, Name = "Formula", CreatedDate = DateTime.UtcNow },
                new() { Id = packagingGroupId, Name = "Packaging", CreatedDate = DateTime.UtcNow }
            }, cancellationToken);

            await context.ProductOptions.AddRangeAsync(new List<ProductOption>
            {
                new() { Id = Guid.NewGuid(), Name = "Shade" },
                new() { Id = Guid.NewGuid(), Name = "Volume" },
                new() { Id = Guid.NewGuid(), Name = "Skin type" }
            }, cancellationToken);

            var skincareId = Guid.Parse("c0ffee00-0000-4000-8000-000000000010");
            var makeupId = Guid.Parse("c0ffee00-0000-4000-8000-000000000011");
            var hairId = Guid.Parse("c0ffee00-0000-4000-8000-000000000012");
            var bodyId = Guid.Parse("c0ffee00-0000-4000-8000-000000000013");
            var cleansersId = Guid.Parse("c0ffee00-0000-4000-8000-000000000014");
            var moisturizersId = Guid.Parse("c0ffee00-0000-4000-8000-000000000015");
            var serumsId = Guid.Parse("c0ffee00-0000-4000-8000-000000000016");
            var sunscreenId = Guid.Parse("c0ffee00-0000-4000-8000-000000000017");
            var lipstickId = Guid.Parse("c0ffee00-0000-4000-8000-000000000018");
            var foundationId = Guid.Parse("c0ffee00-0000-4000-8000-000000000019");
            var shampooId = Guid.Parse("c0ffee00-0000-4000-8000-000000000020");

            await context.Categories.AddRangeAsync(new List<Category>
            {
                new() { Id = skincareId, Name = "Skincare", Slug = "skincare", IsPublished = true, IncludeInMenu = true, DisplayOrder = 0, MetaTitle = "Skincare", MetaDescription = "Cleansers, serums, moisturizers and sunscreen.", CreatedDate = DateTime.UtcNow },
                new() { Id = makeupId, Name = "Makeup", Slug = "makeup", IsPublished = true, IncludeInMenu = true, DisplayOrder = 1, MetaTitle = "Makeup", MetaDescription = "Lipstick, foundation, mascara and more.", CreatedDate = DateTime.UtcNow },
                new() { Id = hairId, Name = "Hair Care", Slug = "hair-care", IsPublished = true, IncludeInMenu = true, DisplayOrder = 2, MetaTitle = "Hair care", MetaDescription = "Shampoo, conditioner and treatments.", CreatedDate = DateTime.UtcNow },
                new() { Id = bodyId, Name = "Body Care", Slug = "body-care", IsPublished = true, IncludeInMenu = true, DisplayOrder = 3, MetaTitle = "Body care", MetaDescription = "Body wash, scrubs and lotion.", CreatedDate = DateTime.UtcNow },
                new() { Id = cleansersId, Name = "Cleansers", Slug = "cleansers", ParentId = skincareId, IsPublished = true, IncludeInMenu = true, DisplayOrder = 0, MetaTitle = "Cleansers", MetaDescription = "Gel, foam and micellar cleansers.", CreatedDate = DateTime.UtcNow },
                new() { Id = moisturizersId, Name = "Moisturizers", Slug = "moisturizers", ParentId = skincareId, IsPublished = true, IncludeInMenu = true, DisplayOrder = 1, MetaTitle = "Moisturizers", MetaDescription = "Creams, gels and lotions.", CreatedDate = DateTime.UtcNow },
                new() { Id = serumsId, Name = "Serums", Slug = "serums", ParentId = skincareId, IsPublished = true, IncludeInMenu = true, DisplayOrder = 2, MetaTitle = "Serums", MetaDescription = "Concentrated treatment serums.", CreatedDate = DateTime.UtcNow },
                new() { Id = sunscreenId, Name = "Sunscreen", Slug = "sunscreen", ParentId = skincareId, IsPublished = true, IncludeInMenu = true, DisplayOrder = 3, MetaTitle = "Sunscreen", MetaDescription = "SPF protection for face and body.", CreatedDate = DateTime.UtcNow },
                new() { Id = lipstickId, Name = "Lipstick", Slug = "lipstick", ParentId = makeupId, IsPublished = true, IncludeInMenu = true, DisplayOrder = 0, MetaTitle = "Lipstick", MetaDescription = "Matte, satin and glossy lips.", CreatedDate = DateTime.UtcNow },
                new() { Id = foundationId, Name = "Foundation", Slug = "foundation", ParentId = makeupId, IsPublished = true, IncludeInMenu = true, DisplayOrder = 1, MetaTitle = "Foundation", MetaDescription = "Liquid, cushion and powder base.", CreatedDate = DateTime.UtcNow },
                new() { Id = shampooId, Name = "Shampoo", Slug = "shampoo", ParentId = hairId, IsPublished = true, IncludeInMenu = true, DisplayOrder = 0, MetaTitle = "Shampoo", MetaDescription = "Shampoo for every hair type.", CreatedDate = DateTime.UtcNow }
            }, cancellationToken);

            var shadeId = Guid.Parse("c0ffee00-0000-4000-8000-000000000021");
            var skinTypeId = Guid.Parse("c0ffee00-0000-4000-8000-000000000022");
            var volumeId = Guid.Parse("c0ffee00-0000-4000-8000-000000000023");
            var ingredientId = Guid.Parse("c0ffee00-0000-4000-8000-000000000024");
            var spfId = Guid.Parse("c0ffee00-0000-4000-8000-000000000025");
            var finishId = Guid.Parse("c0ffee00-0000-4000-8000-000000000026");
            var waterproofId = Guid.Parse("c0ffee00-0000-4000-8000-000000000027");
            var packagingId = Guid.Parse("c0ffee00-0000-4000-8000-000000000028");

            await context.ProductAttributes.AddRangeAsync(new List<ProductAttribute>
            {
                new() { Id = shadeId, Name = "Shade", GroupId = generalGroupId, CreatedDate = DateTime.UtcNow },
                new() { Id = skinTypeId, Name = "Skin type", GroupId = generalGroupId, CreatedDate = DateTime.UtcNow },
                new() { Id = volumeId, Name = "Volume", GroupId = generalGroupId, CreatedDate = DateTime.UtcNow },
                new() { Id = ingredientId, Name = "Key ingredient", GroupId = formulaGroupId, CreatedDate = DateTime.UtcNow },
                new() { Id = spfId, Name = "SPF", GroupId = formulaGroupId, CreatedDate = DateTime.UtcNow },
                new() { Id = finishId, Name = "Finish", GroupId = formulaGroupId, CreatedDate = DateTime.UtcNow },
                new() { Id = waterproofId, Name = "Waterproof", GroupId = formulaGroupId, CreatedDate = DateTime.UtcNow },
                new() { Id = packagingId, Name = "Packaging", GroupId = packagingGroupId, CreatedDate = DateTime.UtcNow }
            }, cancellationToken);

            var effaclarId = Guid.Parse("c0ffee00-0000-4000-8000-000000000029");
            var micellarId = Guid.Parse("c0ffee00-0000-4000-8000-000000000030");
            var cicaplastId = Guid.Parse("c0ffee00-0000-4000-8000-000000000031");
            var greenTeaId = Guid.Parse("c0ffee00-0000-4000-8000-000000000032");
            var biDaoId = Guid.Parse("c0ffee00-0000-4000-8000-000000000033");
            var tripleShieldId = Guid.Parse("c0ffee00-0000-4000-8000-000000000034");
            var superstayId = Guid.Parse("c0ffee00-0000-4000-8000-000000000035");
            var colorRicheId = Guid.Parse("c0ffee00-0000-4000-8000-000000000036");
            var infallibleId = Guid.Parse("c0ffee00-0000-4000-8000-000000000037");
            var fitMeId = Guid.Parse("c0ffee00-0000-4000-8000-000000000038");
            var lashId = Guid.Parse("c0ffee00-0000-4000-8000-000000000039");
            var shampooBuoiId = Guid.Parse("c0ffee00-0000-4000-8000-000000000040");
            var conditionerId = Guid.Parse("c0ffee00-0000-4000-8000-000000000041");
            var coffeeScrubId = Guid.Parse("c0ffee00-0000-4000-8000-000000000042");
            var thermalId = Guid.Parse("c0ffee00-0000-4000-8000-000000000043");

            await context.Products.AddRangeAsync(new List<Product>
            {
                new()
                {
                    Id = effaclarId,
                    Name = "Sữa rửa mặt La Roche-Posay Effaclar 200ml",
                    Slug = "sua-rua-mat-la-roche-posay-effaclar-200ml",
                    ShortDescription = "<ul><li>Cho da dầu, mụn</li><li>Niacinamide + Zinc PCA</li><li>pH 5.5, không xà phòng</li></ul>",
                    Description = "<p>Gel rửa mặt Effaclar: làm sạch bã nhờn, giảm bóng dầu mà không gây khô căng, dùng được hằng ngày sáng tối.</p>",
                    IsPublished = true,
                    Price = 489000m,
                    BrandId = laRochePosayId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Sữa rửa mặt La Roche-Posay Effaclar 200ml chính hãng",
                    MetaKeywords = "la roche-posay effaclar, sữa rửa mặt da dầu",
                    MetaDescription = "Mua sữa rửa mặt La Roche-Posay Effaclar 200ml chính hãng, cho da dầu mụn. Mua ngay!"
                },
                new()
                {
                    Id = micellarId,
                    Name = "Nước tẩy trang Garnier Micellar hồng 400ml",
                    Slug = "nuoc-tay-trang-garnier-micellar-hong-400ml",
                    ShortDescription = "<ul><li>Cho mọi loại da, kể cả nhạy cảm</li><li>Micellar + Glycerin</li><li>Không cần rửa lại với nước</li></ul>",
                    Description = "<p>Nước tẩy trang Garnier nắp hồng: lấy đi lớp trang điểm và bụi mịn nhẹ nhàng, da sạch mềm không nhờn rít.</p>",
                    IsPublished = true,
                    Price = 249000m,
                    BrandId = garnierId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Nước tẩy trang Garnier Micellar hồng 400ml chính hãng",
                    MetaKeywords = "garnier micellar, tẩy trang hồng, garnier 400ml",
                    MetaDescription = "Mua nước tẩy trang Garnier Micellar hồng 400ml chính hãng, giá tốt. Mua ngay!"
                },
                new()
                {
                    Id = cicaplastId,
                    Name = "Kem dưỡng La Roche-Posay Cicaplast B5+ 100ml",
                    Slug = "kem-duong-la-roche-posay-cicaplast-b5-100ml",
                    ShortDescription = "<ul><li>Phục hồi da kích ứng, sau nặn mụn</li><li>Panthenol 5% + Madecassoside</li><li>Không hương liệu, không paraben</li></ul>",
                    Description = "<p>Cicaplast B5+: kem dưỡng phục hồi hàng rào bảo vệ da, làm dịu mẩn đỏ và đẩy nhanh tái tạo da tổn thương.</p>",
                    IsPublished = true,
                    Price = 679000m,
                    BrandId = laRochePosayId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Kem dưỡng La Roche-Posay Cicaplast B5+ 100ml chính hãng",
                    MetaKeywords = "cicaplast b5, kem phục hồi da, la roche-posay",
                    MetaDescription = "Mua kem dưỡng La Roche-Posay Cicaplast B5+ 100ml chính hãng. Mua ngay!"
                },
                new()
                {
                    Id = greenTeaId,
                    Name = "Serum Innisfree Green Tea Seed 80ml",
                    Slug = "serum-innisfree-green-tea-seed-80ml",
                    ShortDescription = "<ul><li>Trà xanh Jeju + HA</li><li>Cấp ẩm sâu 100 giờ</li><li>Thấm nhanh, không dính</li></ul>",
                    Description = "<p>Green Tea Seed serum: tinh chất trà xanh đảo Jeju kết hợp Hyaluronic Acid cho làn da căng mọng, đủ ẩm.</p>",
                    IsPublished = true,
                    Price = 650000m,
                    BrandId = innisfreeId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Serum Innisfree Green Tea Seed 80ml chính hãng",
                    MetaKeywords = "innisfree green tea, serum trà xanh",
                    MetaDescription = "Mua serum Innisfree Green Tea Seed 80ml chính hãng, giá tốt. Mua ngay!"
                },
                new()
                {
                    Id = biDaoId,
                    Name = "Serum Cocoon bí đao N15 70ml",
                    Slug = "serum-cocoon-bi-dao-n15-70ml",
                    ShortDescription = "<ul><li>Cho da dầu, mụn ẩn</li><li>Bí đao + 4% Niacinamide (N15)</li><li>Thuần chay, không thử nghiệm động vật</li></ul>",
                    Description = "<p>Serum bí đao Cocoon: kiểm soát dầu, giảm mụn ẩn và thu nhỏ lỗ chân lông với công thức thuần Việt lành tính.</p>",
                    IsPublished = true,
                    Price = 315000m,
                    BrandId = cocoonId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Serum Cocoon bí đao N15 70ml chính hãng",
                    MetaKeywords = "cocoon bí đao, serum n15, serum da dầu",
                    MetaDescription = "Mua serum Cocoon bí đao N15 70ml chính hãng, thuần chay. Mua ngay!"
                },
                new()
                {
                    Id = tripleShieldId,
                    Name = "Kem chống nắng Innisfree Triple Shield SPF50+ 50ml",
                    Slug = "kem-chong-nang-innisfree-triple-shield-50ml",
                    ShortDescription = "<ul><li>SPF50+ PA++++</li><li>Nâng tone tự nhiên</li><li>Kiềm dầu, chống trôi</li></ul>",
                    Description = "<p>Triple Shield: chống nắng phổ rộng 3 lớp bảo vệ, finish ráo mịn hợp khí hậu nóng ẩm Việt Nam.</p>",
                    IsPublished = true,
                    Price = 429000m,
                    BrandId = innisfreeId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Kem chống nắng Innisfree Triple Shield SPF50+ chính hãng",
                    MetaKeywords = "innisfree sunscreen, triple shield, chống nắng innisfree",
                    MetaDescription = "Mua kem chống nắng Innisfree Triple Shield SPF50+ 50ml chính hãng. Mua ngay!"
                },
                new()
                {
                    Id = superstayId,
                    Name = "Son kem Maybelline SuperStay Matte Ink #20 Pioneer",
                    Slug = "son-kem-maybelline-superstay-matte-ink-20-pioneer",
                    ShortDescription = "<ul><li>Màu #20 Pioneer đỏ thuần</li><li>Lì đến 16 giờ, chống lem</li><li>Đầu cọ mũi tên viền môi chuẩn</li></ul>",
                    Description = "<p>SuperStay Matte Ink: son kem lì quốc dân với màu Pioneer đỏ thuần tôn da, ăn uống thoải mái không trôi.</p>",
                    IsPublished = true,
                    Price = 229000m,
                    BrandId = maybellineId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Son Maybelline SuperStay Matte Ink #20 chính hãng",
                    MetaKeywords = "superstay matte ink, son maybelline 20, pioneer",
                    MetaDescription = "Mua son kem Maybelline SuperStay Matte Ink #20 Pioneer chính hãng. Mua ngay!"
                },
                new()
                {
                    Id = colorRicheId,
                    Name = "Son L'Oréal Color Riche #297 Red Passion",
                    Slug = "son-loreal-color-riche-297-red-passion",
                    ShortDescription = "<ul><li>Màu #297 đỏ đam mê</li><li>Finish satin căng mọng</li><li>Dưỡng jojoba mềm môi</li></ul>",
                    Description = "<p>Color Riche Satin: son thỏi kinh điển với sắc đỏ quyền lực, chất son mịn mượt và dưỡng ẩm cả ngày.</p>",
                    IsPublished = true,
                    Price = 259000m,
                    BrandId = lorealId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Son L'Oréal Color Riche #297 chính hãng",
                    MetaKeywords = "loreal color riche, son 297, red passion",
                    MetaDescription = "Mua son L'Oréal Color Riche #297 Red Passion chính hãng. Mua ngay!"
                },
                new()
                {
                    Id = infallibleId,
                    Name = "Kem nền L'Oréal Infallible Fresh Wear #120",
                    Slug = "kem-nen-loreal-infallible-fresh-wear-120",
                    ShortDescription = "<ul><li>Tone #120 Vanilla cho da sáng</li><li>Lớp nền mỏng nhẹ 24h</li><li>SPF 25, kiềm dầu</li></ul>",
                    Description = "<p>Infallible Fresh Wear: kem nền lâu trôi cho lớp finish tự nhiên như da thật, không mốc nền cuối ngày.</p>",
                    IsPublished = true,
                    Price = 329000m,
                    BrandId = lorealId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Kem nền L'Oréal Infallible Fresh Wear #120 chính hãng",
                    MetaKeywords = "loreal infallible, kem nền 120, fresh wear",
                    MetaDescription = "Mua kem nền L'Oréal Infallible Fresh Wear #120 chính hãng. Mua ngay!"
                },
                new()
                {
                    Id = fitMeId,
                    Name = "Phấn phủ Maybelline Fit Me #120",
                    Slug = "phan-phu-maybelline-fit-me-120",
                    ShortDescription = "<ul><li>Tone #120 Classic Ivory</li><li>Kiềm dầu 12 giờ</li><li>Hạt phấn mịn, không cakey</li></ul>",
                    Description = "<p>Phấn phủ Fit Me: khóa nền kiềm dầu cả ngày với hạt phấn siêu mịn, kèm gương và bông tiện lợi.</p>",
                    IsPublished = true,
                    Price = 249000m,
                    BrandId = maybellineId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Phấn phủ Maybelline Fit Me #120 chính hãng",
                    MetaKeywords = "maybelline fit me, phấn phủ 120",
                    MetaDescription = "Mua phấn phủ Maybelline Fit Me #120 chính hãng, giá tốt. Mua ngay!"
                },
                new()
                {
                    Id = lashId,
                    Name = "Mascara Maybelline Lash Sensational Waterproof",
                    Slug = "mascara-maybelline-lash-sensational-waterproof",
                    ShortDescription = "<ul><li>Chổi cánh quạt tơi mi</li><li>Chống nước, không lem 24h</li><li>Dài và cong rõ rệt</li></ul>",
                    Description = "<p>Lash Sensational Waterproof: mi tơi dài từng sợi, cong vút cả ngày mưa hay đi bơi vẫn không lem.</p>",
                    IsPublished = true,
                    Price = 239000m,
                    BrandId = maybellineId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Mascara Maybelline Lash Sensational chính hãng",
                    MetaKeywords = "mascara maybelline, lash sensational",
                    MetaDescription = "Mua mascara Maybelline Lash Sensational Waterproof chính hãng. Mua ngay!"
                },
                new()
                {
                    Id = shampooBuoiId,
                    Name = "Dầu gội Cocoon tinh dầu bưởi 310ml",
                    Slug = "dau-goi-cocoon-tinh-dau-buoi-310ml",
                    ShortDescription = "<ul><li>Tinh dầu vỏ bưởi Pomelo</li><li>Giảm gãy rụng, kích mọc tóc</li><li>Không sulfate, thuần chay</li></ul>",
                    Description = "<p>Dầu gội bưởi Cocoon: làm sạch dịu nhẹ với tinh dầu vỏ bưởi nguyên chất, tóc chắc khỏe và thơm mát.</p>",
                    IsPublished = true,
                    Price = 255000m,
                    BrandId = cocoonId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Dầu gội Cocoon bưởi 310ml chính hãng",
                    MetaKeywords = "cocoon bưởi, dầu gội bưởi, cocoon shampoo",
                    MetaDescription = "Mua dầu gội Cocoon tinh dầu bưởi 310ml chính hãng. Mua ngay!"
                },
                new()
                {
                    Id = conditionerId,
                    Name = "Dầu xả Cocoon tinh dầu bưởi 310ml",
                    Slug = "dau-xa-cocoon-tinh-dau-buoi-310ml",
                    ShortDescription = "<ul><li>Tinh dầu bưởi + Vitamin B5</li><li>Tóc mềm mượt, dễ chải</li><li>Dùng kèm dầu gội cùng dòng</li></ul>",
                    Description = "<p>Dầu xả bưởi Cocoon: dưỡng ẩm thân và ngọn tóc, giảm xơ rối mà không gây bết dính da đầu.</p>",
                    IsPublished = true,
                    Price = 255000m,
                    BrandId = cocoonId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Dầu xả Cocoon bưởi 310ml chính hãng",
                    MetaKeywords = "cocoon conditioner, dầu xả bưởi",
                    MetaDescription = "Mua dầu xả Cocoon tinh dầu bưởi 310ml chính hãng. Mua ngay!"
                },
                new()
                {
                    Id = coffeeScrubId,
                    Name = "Tẩy da chết Cocoon cà phê Đắk Lắk 200ml",
                    Slug = "tay-da-chet-cocoon-ca-phe-dak-lak-200ml",
                    ShortDescription = "<ul><li>Hạt cà phê Đắk Lắk xay nhuyễn</li><li>Da sáng mịn sau 1 lần dùng</li><li>Mùi cà phê thư giãn</li></ul>",
                    Description = "<p>Tẩy da chết cà phê Cocoon: best-seller làm sáng da body, mờ thâm và thơm mùi cà phê đặc trưng.</p>",
                    IsPublished = true,
                    Price = 139000m,
                    BrandId = cocoonId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Tẩy da chết Cocoon cà phê 200ml chính hãng",
                    MetaKeywords = "cocoon cà phê, tẩy da chết body",
                    MetaDescription = "Mua tẩy da chết Cocoon cà phê Đắk Lắk 200ml chính hãng. Mua ngay!"
                },
                new()
                {
                    Id = thermalId,
                    Name = "Xịt khoáng La Roche-Posay 300ml",
                    Slug = "xit-khoang-la-roche-posay-300ml",
                    ShortDescription = "<ul><li>Nước khoáng Pháp giàu Selenium</li><li>Làm dịu da nhạy cảm, cháy nắng</li><li>Phun sương mịn khắp mặt</li></ul>",
                    Description = "<p>Xịt khoáng La Roche-Posay: cấp ẩm tức thì, làm dịu da kích ứng — vật bất ly thân của da nhạy cảm.</p>",
                    IsPublished = true,
                    Price = 459000m,
                    BrandId = laRochePosayId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Xịt khoáng La Roche-Posay 300ml chính hãng",
                    MetaKeywords = "xịt khoáng la roche-posay, thermal spring water",
                    MetaDescription = "Mua xịt khoáng La Roche-Posay 300ml chính hãng. Mua ngay!"
                }
            }, cancellationToken);

            await context.ProductCategories.AddRangeAsync(new List<ProductCategory>
            {
                new() { Id = Guid.NewGuid(), CategoryId = skincareId, ProductId = effaclarId, DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = cleansersId, ProductId = effaclarId, DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = skincareId, ProductId = micellarId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = cleansersId, ProductId = micellarId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = skincareId, ProductId = cicaplastId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = moisturizersId, ProductId = cicaplastId, DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = skincareId, ProductId = greenTeaId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = serumsId, ProductId = greenTeaId, DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = skincareId, ProductId = biDaoId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = serumsId, ProductId = biDaoId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = skincareId, ProductId = tripleShieldId, DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = sunscreenId, ProductId = tripleShieldId, DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = makeupId, ProductId = superstayId, DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = lipstickId, ProductId = superstayId, DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = makeupId, ProductId = colorRicheId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = lipstickId, ProductId = colorRicheId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = makeupId, ProductId = infallibleId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = foundationId, ProductId = infallibleId, DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = makeupId, ProductId = fitMeId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = foundationId, ProductId = fitMeId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = makeupId, ProductId = lashId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = hairId, ProductId = shampooBuoiId, DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = shampooId, ProductId = shampooBuoiId, DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = hairId, ProductId = conditionerId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = bodyId, ProductId = coffeeScrubId, DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = skincareId, ProductId = thermalId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow }
            }, cancellationToken);

            await context.ProductAttributeValues.AddRangeAsync(new List<ProductAttributeValue>
            {
                new() { Id = Guid.NewGuid(), AttributeId = skinTypeId, ProductId = effaclarId, Value = "Da dầu, mụn" },
                new() { Id = Guid.NewGuid(), AttributeId = volumeId, ProductId = effaclarId, Value = "200ml" },
                new() { Id = Guid.NewGuid(), AttributeId = ingredientId, ProductId = effaclarId, Value = "Niacinamide, Zinc PCA" },
                new() { Id = Guid.NewGuid(), AttributeId = spfId, ProductId = effaclarId, Value = "Không" },
                new() { Id = Guid.NewGuid(), AttributeId = packagingId, ProductId = effaclarId, Value = "Chai" },

                new() { Id = Guid.NewGuid(), AttributeId = skinTypeId, ProductId = micellarId, Value = "Mọi loại da" },
                new() { Id = Guid.NewGuid(), AttributeId = volumeId, ProductId = micellarId, Value = "400ml" },
                new() { Id = Guid.NewGuid(), AttributeId = ingredientId, ProductId = micellarId, Value = "Micellar, Glycerin" },
                new() { Id = Guid.NewGuid(), AttributeId = spfId, ProductId = micellarId, Value = "Không" },
                new() { Id = Guid.NewGuid(), AttributeId = packagingId, ProductId = micellarId, Value = "Chai" },

                new() { Id = Guid.NewGuid(), AttributeId = skinTypeId, ProductId = cicaplastId, Value = "Da kích ứng" },
                new() { Id = Guid.NewGuid(), AttributeId = volumeId, ProductId = cicaplastId, Value = "100ml" },
                new() { Id = Guid.NewGuid(), AttributeId = ingredientId, ProductId = cicaplastId, Value = "Panthenol 5%, Madecassoside" },
                new() { Id = Guid.NewGuid(), AttributeId = spfId, ProductId = cicaplastId, Value = "Không" },
                new() { Id = Guid.NewGuid(), AttributeId = packagingId, ProductId = cicaplastId, Value = "Tuýp" },

                new() { Id = Guid.NewGuid(), AttributeId = skinTypeId, ProductId = greenTeaId, Value = "Mọi loại da" },
                new() { Id = Guid.NewGuid(), AttributeId = volumeId, ProductId = greenTeaId, Value = "80ml" },
                new() { Id = Guid.NewGuid(), AttributeId = ingredientId, ProductId = greenTeaId, Value = "Trà xanh Jeju, HA" },
                new() { Id = Guid.NewGuid(), AttributeId = spfId, ProductId = greenTeaId, Value = "Không" },
                new() { Id = Guid.NewGuid(), AttributeId = packagingId, ProductId = greenTeaId, Value = "Chai vòi nhấn" },

                new() { Id = Guid.NewGuid(), AttributeId = skinTypeId, ProductId = biDaoId, Value = "Da dầu, mụn" },
                new() { Id = Guid.NewGuid(), AttributeId = volumeId, ProductId = biDaoId, Value = "70ml" },
                new() { Id = Guid.NewGuid(), AttributeId = ingredientId, ProductId = biDaoId, Value = "Bí đao, Niacinamide 4%" },
                new() { Id = Guid.NewGuid(), AttributeId = spfId, ProductId = biDaoId, Value = "Không" },
                new() { Id = Guid.NewGuid(), AttributeId = packagingId, ProductId = biDaoId, Value = "Chai nhỏ giọt" },

                new() { Id = Guid.NewGuid(), AttributeId = skinTypeId, ProductId = tripleShieldId, Value = "Mọi loại da" },
                new() { Id = Guid.NewGuid(), AttributeId = volumeId, ProductId = tripleShieldId, Value = "50ml" },
                new() { Id = Guid.NewGuid(), AttributeId = ingredientId, ProductId = tripleShieldId, Value = "Chiết xuất trà xanh" },
                new() { Id = Guid.NewGuid(), AttributeId = spfId, ProductId = tripleShieldId, Value = "SPF50+ PA++++" },
                new() { Id = Guid.NewGuid(), AttributeId = finishId, ProductId = tripleShieldId, Value = "Nâng tone tự nhiên" },
                new() { Id = Guid.NewGuid(), AttributeId = packagingId, ProductId = tripleShieldId, Value = "Tuýp" },

                new() { Id = Guid.NewGuid(), AttributeId = shadeId, ProductId = superstayId, Value = "#20 Pioneer" },
                new() { Id = Guid.NewGuid(), AttributeId = volumeId, ProductId = superstayId, Value = "5ml" },
                new() { Id = Guid.NewGuid(), AttributeId = ingredientId, ProductId = superstayId, Value = "Vitamin E" },
                new() { Id = Guid.NewGuid(), AttributeId = finishId, ProductId = superstayId, Value = "Lì (matte)" },
                new() { Id = Guid.NewGuid(), AttributeId = waterproofId, ProductId = superstayId, Value = "Có" },
                new() { Id = Guid.NewGuid(), AttributeId = packagingId, ProductId = superstayId, Value = "Thỏi son kem" },

                new() { Id = Guid.NewGuid(), AttributeId = shadeId, ProductId = colorRicheId, Value = "#297 Red Passion" },
                new() { Id = Guid.NewGuid(), AttributeId = volumeId, ProductId = colorRicheId, Value = "4.3g" },
                new() { Id = Guid.NewGuid(), AttributeId = ingredientId, ProductId = colorRicheId, Value = "Dầu jojoba" },
                new() { Id = Guid.NewGuid(), AttributeId = finishId, ProductId = colorRicheId, Value = "Satin" },
                new() { Id = Guid.NewGuid(), AttributeId = waterproofId, ProductId = colorRicheId, Value = "Không" },
                new() { Id = Guid.NewGuid(), AttributeId = packagingId, ProductId = colorRicheId, Value = "Thỏi xoay" },

                new() { Id = Guid.NewGuid(), AttributeId = shadeId, ProductId = infallibleId, Value = "#120 Vanilla" },
                new() { Id = Guid.NewGuid(), AttributeId = volumeId, ProductId = infallibleId, Value = "30ml" },
                new() { Id = Guid.NewGuid(), AttributeId = finishId, ProductId = infallibleId, Value = "Tự nhiên" },
                new() { Id = Guid.NewGuid(), AttributeId = waterproofId, ProductId = infallibleId, Value = "Không" },
                new() { Id = Guid.NewGuid(), AttributeId = spfId, ProductId = infallibleId, Value = "SPF 25" },
                new() { Id = Guid.NewGuid(), AttributeId = packagingId, ProductId = infallibleId, Value = "Chai vòi nhấn" },

                new() { Id = Guid.NewGuid(), AttributeId = shadeId, ProductId = fitMeId, Value = "#120 Classic Ivory" },
                new() { Id = Guid.NewGuid(), AttributeId = volumeId, ProductId = fitMeId, Value = "8.5g" },
                new() { Id = Guid.NewGuid(), AttributeId = finishId, ProductId = fitMeId, Value = "Lì, kiềm dầu" },
                new() { Id = Guid.NewGuid(), AttributeId = waterproofId, ProductId = fitMeId, Value = "Không" },
                new() { Id = Guid.NewGuid(), AttributeId = packagingId, ProductId = fitMeId, Value = "Hộp phấn kèm gương" },

                new() { Id = Guid.NewGuid(), AttributeId = shadeId, ProductId = lashId, Value = "Đen Blackest Black" },
                new() { Id = Guid.NewGuid(), AttributeId = volumeId, ProductId = lashId, Value = "9ml" },
                new() { Id = Guid.NewGuid(), AttributeId = waterproofId, ProductId = lashId, Value = "Có" },
                new() { Id = Guid.NewGuid(), AttributeId = packagingId, ProductId = lashId, Value = "Tuýp mascara" },

                new() { Id = Guid.NewGuid(), AttributeId = volumeId, ProductId = shampooBuoiId, Value = "310ml" },
                new() { Id = Guid.NewGuid(), AttributeId = ingredientId, ProductId = shampooBuoiId, Value = "Tinh dầu vỏ bưởi" },
                new() { Id = Guid.NewGuid(), AttributeId = packagingId, ProductId = shampooBuoiId, Value = "Chai vòi nhấn" },

                new() { Id = Guid.NewGuid(), AttributeId = volumeId, ProductId = conditionerId, Value = "310ml" },
                new() { Id = Guid.NewGuid(), AttributeId = ingredientId, ProductId = conditionerId, Value = "Tinh dầu bưởi, Vitamin B5" },
                new() { Id = Guid.NewGuid(), AttributeId = packagingId, ProductId = conditionerId, Value = "Chai vòi nhấn" },

                new() { Id = Guid.NewGuid(), AttributeId = skinTypeId, ProductId = coffeeScrubId, Value = "Mọi loại da" },
                new() { Id = Guid.NewGuid(), AttributeId = volumeId, ProductId = coffeeScrubId, Value = "200ml" },
                new() { Id = Guid.NewGuid(), AttributeId = ingredientId, ProductId = coffeeScrubId, Value = "Cà phê Đắk Lắk" },
                new() { Id = Guid.NewGuid(), AttributeId = packagingId, ProductId = coffeeScrubId, Value = "Hũ" },

                new() { Id = Guid.NewGuid(), AttributeId = skinTypeId, ProductId = thermalId, Value = "Da nhạy cảm" },
                new() { Id = Guid.NewGuid(), AttributeId = volumeId, ProductId = thermalId, Value = "300ml" },
                new() { Id = Guid.NewGuid(), AttributeId = ingredientId, ProductId = thermalId, Value = "Nước khoáng, Selenium" },
                new() { Id = Guid.NewGuid(), AttributeId = spfId, ProductId = thermalId, Value = "Không" },
                new() { Id = Guid.NewGuid(), AttributeId = packagingId, ProductId = thermalId, Value = "Chai xịt" }
            }, cancellationToken);

            var skincareTemplateId = Guid.Parse("c0ffee00-0000-4000-8000-000000000044");
            var makeupTemplateId = Guid.Parse("c0ffee00-0000-4000-8000-000000000045");

            await context.ProductTemplates.AddRangeAsync(new List<ProductTemplate>
            {
                new() { Id = skincareTemplateId, Name = "Skincare", CreatedDate = DateTime.UtcNow },
                new() { Id = makeupTemplateId, Name = "Makeup", CreatedDate = DateTime.UtcNow }
            }, cancellationToken);

            var skincareAttributes = new[] { skinTypeId, volumeId, ingredientId, spfId };
            var makeupAttributes = new[] { shadeId, volumeId, ingredientId, finishId, waterproofId };

            await context.ProductTemplateProductAttributes.AddRangeAsync(
                skincareAttributes
                    .Select(a => new ProductTemplateProductAttribute { Id = Guid.NewGuid(), ProductTemplateId = skincareTemplateId, ProductAttributeId = a, CreatedDate = DateTime.UtcNow })
                    .Concat(makeupAttributes
                        .Select(a => new ProductTemplateProductAttribute { Id = Guid.NewGuid(), ProductTemplateId = makeupTemplateId, ProductAttributeId = a, CreatedDate = DateTime.UtcNow }))
                    .ToList(), cancellationToken);
        }
    }
}
