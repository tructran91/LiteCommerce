using Catalog.Application.Database;
using Catalog.Core.Entities;

namespace Catalog.Infrastructure.Data.Seeding
{
    public class FurnitureSeedProfile : ISeedProfile
    {
        public string Name => SeedProfiles.Furniture;

        public async Task SeedAsync(CatalogContext context, CancellationToken cancellationToken = default)
        {
            var ikeaId = Guid.Parse("f2000000-0000-4000-8000-000000000001");
            var hoaPhatId = Guid.Parse("f2000000-0000-4000-8000-000000000002");
            var mohoId = Guid.Parse("f2000000-0000-4000-8000-000000000003");
            var xuanHoaId = Guid.Parse("f2000000-0000-4000-8000-000000000004");
            var noithat190Id = Guid.Parse("f2000000-0000-4000-8000-000000000005");
            var nhaXinhId = Guid.Parse("f2000000-0000-4000-8000-000000000006");

            await context.Brands.AddRangeAsync(new List<Brand>
            {
                new() { Id = ikeaId, Name = "IKEA", Slug = "ikea", IsPublished = true, CreatedDate = DateTime.UtcNow },
                new() { Id = hoaPhatId, Name = "Hòa Phát", Slug = "hoa-phat", IsPublished = true, CreatedDate = DateTime.UtcNow },
                new() { Id = mohoId, Name = "MOHO", Slug = "moho", IsPublished = true, CreatedDate = DateTime.UtcNow },
                new() { Id = xuanHoaId, Name = "Xuân Hòa", Slug = "xuan-hoa", IsPublished = true, CreatedDate = DateTime.UtcNow },
                new() { Id = noithat190Id, Name = "Nội thất 190", Slug = "noi-that-190", IsPublished = true, CreatedDate = DateTime.UtcNow },
                new() { Id = nhaXinhId, Name = "Nhà Xinh", Slug = "nha-xinh", IsPublished = true, CreatedDate = DateTime.UtcNow }
            }, cancellationToken);

            var generalGroupId = Guid.Parse("f2000000-0000-4000-8000-000000000007");
            var materialGroupId = Guid.Parse("f2000000-0000-4000-8000-000000000008");
            var dimensionsGroupId = Guid.Parse("f2000000-0000-4000-8000-000000000009");

            await context.ProductAttributeGroups.AddRangeAsync(new List<ProductAttributeGroup>
            {
                new() { Id = generalGroupId, Name = "General", CreatedDate = DateTime.UtcNow },
                new() { Id = materialGroupId, Name = "Material", CreatedDate = DateTime.UtcNow },
                new() { Id = dimensionsGroupId, Name = "Dimensions", CreatedDate = DateTime.UtcNow }
            }, cancellationToken);

            await context.ProductOptions.AddRangeAsync(new List<ProductOption>
            {
                new() { Id = Guid.NewGuid(), Name = "Color" },
                new() { Id = Guid.NewGuid(), Name = "Size" },
                new() { Id = Guid.NewGuid(), Name = "Material" }
            }, cancellationToken);

            var livingId = Guid.Parse("f2000000-0000-4000-8000-000000000010");
            var bedroomId = Guid.Parse("f2000000-0000-4000-8000-000000000011");
            var diningId = Guid.Parse("f2000000-0000-4000-8000-000000000012");
            var officeId = Guid.Parse("f2000000-0000-4000-8000-000000000013");
            var decorId = Guid.Parse("f2000000-0000-4000-8000-000000000014");
            var sofasId = Guid.Parse("f2000000-0000-4000-8000-000000000015");
            var coffeeTablesId = Guid.Parse("f2000000-0000-4000-8000-000000000016");
            var bedsId = Guid.Parse("f2000000-0000-4000-8000-000000000017");
            var wardrobesId = Guid.Parse("f2000000-0000-4000-8000-000000000018");
            var diningSetsId = Guid.Parse("f2000000-0000-4000-8000-000000000019");
            var desksId = Guid.Parse("f2000000-0000-4000-8000-000000000020");

            await context.Categories.AddRangeAsync(new List<Category>
            {
                new() { Id = livingId, Name = "Living Room", Slug = "living-room", IsPublished = true, IncludeInMenu = true, DisplayOrder = 0, MetaTitle = "Living room furniture", MetaDescription = "Sofas, coffee tables and TV stands.", CreatedDate = DateTime.UtcNow },
                new() { Id = bedroomId, Name = "Bedroom", Slug = "bedroom", IsPublished = true, IncludeInMenu = true, DisplayOrder = 1, MetaTitle = "Bedroom furniture", MetaDescription = "Beds, wardrobes and nightstands.", CreatedDate = DateTime.UtcNow },
                new() { Id = diningId, Name = "Dining Room", Slug = "dining-room", IsPublished = true, IncludeInMenu = true, DisplayOrder = 2, MetaTitle = "Dining room furniture", MetaDescription = "Dining tables, chairs and sets.", CreatedDate = DateTime.UtcNow },
                new() { Id = officeId, Name = "Office", Slug = "office", IsPublished = true, IncludeInMenu = true, DisplayOrder = 3, MetaTitle = "Office furniture", MetaDescription = "Desks and office storage.", CreatedDate = DateTime.UtcNow },
                new() { Id = decorId, Name = "Decor", Slug = "decor", IsPublished = true, IncludeInMenu = true, DisplayOrder = 4, MetaTitle = "Home decor", MetaDescription = "Lamps, rugs and decorations.", CreatedDate = DateTime.UtcNow },
                new() { Id = sofasId, Name = "Sofas", Slug = "sofas", ParentId = livingId, IsPublished = true, IncludeInMenu = true, DisplayOrder = 0, MetaTitle = "Sofas", MetaDescription = "2-seat, 3-seat and corner sofas.", CreatedDate = DateTime.UtcNow },
                new() { Id = coffeeTablesId, Name = "Coffee Tables", Slug = "coffee-tables", ParentId = livingId, IsPublished = true, IncludeInMenu = true, DisplayOrder = 1, MetaTitle = "Coffee tables", MetaDescription = "Wood, glass and stone coffee tables.", CreatedDate = DateTime.UtcNow },
                new() { Id = bedsId, Name = "Beds", Slug = "beds", ParentId = bedroomId, IsPublished = true, IncludeInMenu = true, DisplayOrder = 0, MetaTitle = "Beds", MetaDescription = "Wooden and upholstered beds.", CreatedDate = DateTime.UtcNow },
                new() { Id = wardrobesId, Name = "Wardrobes", Slug = "wardrobes", ParentId = bedroomId, IsPublished = true, IncludeInMenu = true, DisplayOrder = 1, MetaTitle = "Wardrobes", MetaDescription = "Sliding and hinged wardrobes.", CreatedDate = DateTime.UtcNow },
                new() { Id = diningSetsId, Name = "Dining Sets", Slug = "dining-sets", ParentId = diningId, IsPublished = true, IncludeInMenu = true, DisplayOrder = 0, MetaTitle = "Dining sets", MetaDescription = "Tables, chairs and full sets.", CreatedDate = DateTime.UtcNow },
                new() { Id = desksId, Name = "Desks", Slug = "desks", ParentId = officeId, IsPublished = true, IncludeInMenu = true, DisplayOrder = 0, MetaTitle = "Desks", MetaDescription = "Work, study and computer desks.", CreatedDate = DateTime.UtcNow }
            }, cancellationToken);

            var colorId = Guid.Parse("f2000000-0000-4000-8000-000000000021");
            var styleId = Guid.Parse("f2000000-0000-4000-8000-000000000022");
            var materialId = Guid.Parse("f2000000-0000-4000-8000-000000000023");
            var surfaceId = Guid.Parse("f2000000-0000-4000-8000-000000000024");
            var widthId = Guid.Parse("f2000000-0000-4000-8000-000000000025");
            var depthId = Guid.Parse("f2000000-0000-4000-8000-000000000026");
            var heightId = Guid.Parse("f2000000-0000-4000-8000-000000000027");
            var loadId = Guid.Parse("f2000000-0000-4000-8000-000000000028");

            await context.ProductAttributes.AddRangeAsync(new List<ProductAttribute>
            {
                new() { Id = colorId, Name = "Color", GroupId = generalGroupId, CreatedDate = DateTime.UtcNow },
                new() { Id = styleId, Name = "Style", GroupId = generalGroupId, CreatedDate = DateTime.UtcNow },
                new() { Id = materialId, Name = "Material", GroupId = materialGroupId, CreatedDate = DateTime.UtcNow },
                new() { Id = surfaceId, Name = "Surface finish", GroupId = materialGroupId, CreatedDate = DateTime.UtcNow },
                new() { Id = widthId, Name = "Width (cm)", GroupId = dimensionsGroupId, CreatedDate = DateTime.UtcNow },
                new() { Id = depthId, Name = "Depth (cm)", GroupId = dimensionsGroupId, CreatedDate = DateTime.UtcNow },
                new() { Id = heightId, Name = "Height (cm)", GroupId = dimensionsGroupId, CreatedDate = DateTime.UtcNow },
                new() { Id = loadId, Name = "Max load (kg)", GroupId = dimensionsGroupId, CreatedDate = DateTime.UtcNow }
            }, cancellationToken);

            var soderhamnId = Guid.Parse("f2000000-0000-4000-8000-000000000029");
            var fynId = Guid.Parse("f2000000-0000-4000-8000-000000000030");
            var lackId = Guid.Parse("f2000000-0000-4000-8000-000000000031");
            var viennaId = Guid.Parse("f2000000-0000-4000-8000-000000000032");
            var cataniaId = Guid.Parse("f2000000-0000-4000-8000-000000000033");
            var ca3aId = Guid.Parse("f2000000-0000-4000-8000-000000000034");
            var brimnesId = Guid.Parse("f2000000-0000-4000-8000-000000000035");
            var dalatId = Guid.Parse("f2000000-0000-4000-8000-000000000036");
            var at120Id = Guid.Parse("f2000000-0000-4000-8000-000000000037");
            var hp140Id = Guid.Parse("f2000000-0000-4000-8000-000000000038");
            var at140Id = Guid.Parse("f2000000-0000-4000-8000-000000000039");
            var bhsId = Guid.Parse("f2000000-0000-4000-8000-000000000040");
            var hektarId = Guid.Parse("f2000000-0000-4000-8000-000000000041");
            var retroTableId = Guid.Parse("f2000000-0000-4000-8000-000000000042");
            var mollyId = Guid.Parse("f2000000-0000-4000-8000-000000000043");

            await context.Products.AddRangeAsync(new List<Product>
            {
                new()
                {
                    Id = soderhamnId,
                    Name = "Sofa IKEA SÖDERHAMN 3 chỗ Fridtuna xám",
                    Slug = "sofa-ikea-soderhamn-3-cho-fridtuna-xam",
                    ShortDescription = "<ul><li>Sofa 3 chỗ ngồi sâu, êm</li><li>Vải Fridtuna tháo giặt được</li><li>Chân gỗ sồi đặc</li></ul>",
                    Description = "<p>SÖDERHAMN: sofa module với đệm ngồi sâu, tựa linh hoạt và vỏ bọc tháo rời giặt máy — càng dùng càng êm.</p>",
                    IsPublished = true,
                    Price = 24990000m,
                    BrandId = ikeaId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Sofa IKEA SÖDERHAMN 3 chỗ chính hãng",
                    MetaKeywords = "sofa ikea, soderhamn, sofa 3 chỗ",
                    MetaDescription = "Mua sofa IKEA SÖDERHAMN 3 chỗ chính hãng, bảo hành 10 năm. Mua ngay!"
                },
                new()
                {
                    Id = fynId,
                    Name = "Sofa vải MOHO Fyn 3 chỗ xám tro",
                    Slug = "sofa-moho-fyn-3-cho-xam-tro",
                    ShortDescription = "<ul><li>Sofa 3 chỗ khung gỗ cao su</li><li>Vải bố chống xù, tháo giặt</li><li>Nệm mút D40 êm bền</li></ul>",
                    Description = "<p>Sofa MOHO Fyn: dáng hiện đại gọn gàng cho chung cư, khung gỗ tự nhiên và vải bố thoáng mát.</p>",
                    IsPublished = true,
                    Price = 8990000m,
                    BrandId = mohoId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Sofa MOHO Fyn 3 chỗ giá tốt",
                    MetaKeywords = "sofa moho, sofa fyn, sofa vải",
                    MetaDescription = "Mua sofa vải MOHO Fyn 3 chỗ giá tốt, bảo hành 5 năm, freeship lắp đặt. Mua ngay!"
                },
                new()
                {
                    Id = lackId,
                    Name = "Bàn trà IKEA LACK 55x55 trắng",
                    Slug = "ban-tra-ikea-lack-55x55-trang",
                    ShortDescription = "<ul><li>Mặt bàn 55x55 cm</li><li>Nhẹ, dễ di chuyển</li><li>Phối mọi phong cách</li></ul>",
                    Description = "<p>Bàn trà LACK: thiết kế tối giản kinh điển, giá dễ chịu, dùng làm bàn trà hay táp-lơ đều hợp.</p>",
                    IsPublished = true,
                    Price = 499000m,
                    BrandId = ikeaId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Bàn trà IKEA LACK 55x55 chính hãng",
                    MetaKeywords = "bàn trà ikea, lack, bàn sofa",
                    MetaDescription = "Mua bàn trà IKEA LACK 55x55 chính hãng, bảo hành 2 năm. Mua ngay!"
                },
                new()
                {
                    Id = viennaId,
                    Name = "Giường gỗ MOHO Vienna 1m6",
                    Slug = "giuong-go-moho-vienna-1m6",
                    ShortDescription = "<ul><li>Gỗ cao su tự nhiên</li><li>Nệm 1m6 x 2m, đầu giường cao</li><li>Thanh nan cong chịu lực tốt</li></ul>",
                    Description = "<p>Giường MOHO Vienna: gỗ cao su bền chắc, vân gỗ đẹp, đầu giường vát cong phong cách Scandinavian.</p>",
                    IsPublished = true,
                    Price = 8490000m,
                    BrandId = mohoId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Giường gỗ MOHO Vienna 1m6 giá tốt",
                    MetaKeywords = "giường moho, giường gỗ 1m6, vienna",
                    MetaDescription = "Mua giường gỗ MOHO Vienna 1m6 giá tốt, bảo hành 5 năm. Mua ngay!"
                },
                new()
                {
                    Id = cataniaId,
                    Name = "Giường bọc vải Nhà Xinh Catania 1m8",
                    Slug = "giuong-boc-vai-nha-xinh-catania-1m8",
                    ShortDescription = "<ul><li>Bọc vải nỉ cao cấp</li><li>Nệm 1m8 x 2m, đầu giường b Button</li><li>Khung gỗ thông chắc chắn</li></ul>",
                    Description = "<p>Giường Catania: đầu giường bọc nệm êm ái để tựa đọc sách, vải nỉ sang trọng cho phòng ngủ hiện đại.</p>",
                    IsPublished = true,
                    Price = 12990000m,
                    BrandId = nhaXinhId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Giường Nhà Xinh Catania 1m8 chính hãng",
                    MetaKeywords = "giường nhà xinh, catania, giường bọc vải",
                    MetaDescription = "Mua giường bọc vải Nhà Xinh Catania 1m8 chính hãng, bảo hành 2 năm. Mua ngay!"
                },
                new()
                {
                    Id = ca3aId,
                    Name = "Tủ quần áo Xuân Hòa CA-3A 3 buồng",
                    Slug = "tu-quan-ao-xuan-hoa-ca-3a-3-buong",
                    ShortDescription = "<ul><li>Thép cán nguội dày dặn</li><li>3 buồng + đợt + suốt treo</li><li>Khóa an toàn</li></ul>",
                    Description = "<p>Tủ CA-3A: tủ thép 3 buồng rộng rãi cho gia đình, sơn tĩnh điện chống gỉ, dùng bền hàng chục năm.</p>",
                    IsPublished = true,
                    Price = 4590000m,
                    BrandId = xuanHoaId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Tủ quần áo Xuân Hòa CA-3A giá tốt",
                    MetaKeywords = "tủ xuân hòa, ca-3a, tủ thép 3 buồng",
                    MetaDescription = "Mua tủ quần áo Xuân Hòa CA-3A 3 buồng giá tốt, bảo hành 1 năm. Mua ngay!"
                },
                new()
                {
                    Id = brimnesId,
                    Name = "Tủ áo IKEA BRIMNES 3 cửa trắng",
                    Slug = "tu-ao-ikea-brimnes-3-cua-trang",
                    ShortDescription = "<ul><li>3 cửa gương + kệ linh hoạt</li><li>Gương soi toàn thân</li><li>Chân đế dễ vệ sinh gầm</li></ul>",
                    Description = "<p>Tủ BRIMNES: cửa gương giúp phòng trông rộng hơn, bên trong chia ngăn treo và xếp thông minh.</p>",
                    IsPublished = true,
                    Price = 6990000m,
                    BrandId = ikeaId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Tủ áo IKEA BRIMNES 3 cửa chính hãng",
                    MetaKeywords = "tủ ikea, brimnes, tủ gương",
                    MetaDescription = "Mua tủ áo IKEA BRIMNES 3 cửa chính hãng, bảo hành 10 năm. Mua ngay!"
                },
                new()
                {
                    Id = dalatId,
                    Name = "Bộ bàn ăn MOHO Dalat 4 ghế",
                    Slug = "bo-ban-an-moho-dalat-4-ghe",
                    ShortDescription = "<ul><li>Bàn + 4 ghế gỗ cao su</li><li>Mặt bàn chống thấm, dễ lau</li><li>Ghế tựa cong ôm lưng</li></ul>",
                    Description = "<p>Bộ bàn ăn Dalat: gỗ cao su màu óc chó ấm cúng, đủ chỗ cho gia đình 4 người sum họp.</p>",
                    IsPublished = true,
                    Price = 9990000m,
                    BrandId = mohoId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Bộ bàn ăn MOHO Dalat 4 ghế giá tốt",
                    MetaKeywords = "bàn ăn moho, dalat 4 ghế, bộ bàn ăn gỗ",
                    MetaDescription = "Mua bộ bàn ăn MOHO Dalat 4 ghế giá tốt, bảo hành 5 năm. Mua ngay!"
                },
                new()
                {
                    Id = at120Id,
                    Name = "Bàn ăn Hòa Phát AT120 mặt laminate",
                    Slug = "ban-an-hoa-phat-at120-mat-laminate",
                    ShortDescription = "<ul><li>Mặt bàn 1m2 chống xước</li><li>Chân thép hộp sơn tĩnh điện</li><li>Ngồi thoải mái 4-6 người</li></ul>",
                    Description = "<p>Bàn ăn AT120: mặt laminate vân gỗ chống thấm, chân thép vững chãi — lựa chọn kinh tế cho mọi nhà.</p>",
                    IsPublished = true,
                    Price = 2890000m,
                    BrandId = hoaPhatId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Bàn ăn Hòa Phát AT120 giá rẻ",
                    MetaKeywords = "bàn ăn hòa phát, at120, bàn ăn 1m2",
                    MetaDescription = "Mua bàn ăn Hòa Phát AT120 giá rẻ, bảo hành 1 năm. Mua ngay!"
                },
                new()
                {
                    Id = hp140Id,
                    Name = "Bàn làm việc Hòa Phát HP140 1m4",
                    Slug = "ban-lam-viec-hoa-phat-hp140-1m4",
                    ShortDescription = "<ul><li>Mặt bàn MFC 1m4 rộng rãi</li><li>Kệ CPU + khay bàn phím</li><li>Yếm tôn che chắn gọn gàng</li></ul>",
                    Description = "<p>Bàn HP140: bàn văn phòng tiêu chuẩn với kệ để CPU, khay phím trượt và lỗ luồn dây tiện lợi.</p>",
                    IsPublished = true,
                    Price = 2150000m,
                    BrandId = hoaPhatId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Bàn làm việc Hòa Phát HP140 giá tốt",
                    MetaKeywords = "bàn hòa phát, hp140, bàn văn phòng",
                    MetaDescription = "Mua bàn làm việc Hòa Phát HP140 1m4 giá tốt, bảo hành 1 năm. Mua ngay!"
                },
                new()
                {
                    Id = at140Id,
                    Name = "Bàn nhân viên Nội thất 190 AT140",
                    Slug = "ban-nhan-vien-noi-that-190-at140",
                    ShortDescription = "<ul><li>Mặt bàn 1m4 x 70 cm</li><li>Chân sắt hộp 30x50 mm</li><li>Lắp ghép module văn phòng</li></ul>",
                    Description = "<p>Bàn AT140 của Nội thất 190: giải pháp setup văn phòng đồng bộ, cứng cáp và tiết kiệm chi phí.</p>",
                    IsPublished = true,
                    Price = 1990000m,
                    BrandId = noithat190Id,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Bàn nhân viên Nội thất 190 AT140 giá rẻ",
                    MetaKeywords = "bàn 190, at140, bàn nhân viên",
                    MetaDescription = "Mua bàn nhân viên Nội thất 190 AT140 giá rẻ, bảo hành 1 năm. Mua ngay!"
                },
                new()
                {
                    Id = bhsId,
                    Name = "Bàn học sinh Xuân Hòa BHS-14",
                    Slug = "ban-hoc-sinh-xuan-hoa-bhs-14",
                    ShortDescription = "<ul><li>Kèm giá sách 2 tầng</li><li>Mặt bàn chống lóa</li><li>Chiều cao phù hợp học sinh</li></ul>",
                    Description = "<p>Bàn học BHS-14: liền giá sách tiện lợi, mặt bàn rộng để vừa laptop và sách vở, khung thép chắc chắn.</p>",
                    IsPublished = true,
                    Price = 1650000m,
                    BrandId = xuanHoaId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Bàn học sinh Xuân Hòa BHS-14 giá tốt",
                    MetaKeywords = "bàn học xuân hòa, bhs-14, bàn học sinh",
                    MetaDescription = "Mua bàn học sinh Xuân Hòa BHS-14 giá tốt, bảo hành 1 năm. Mua ngay!"
                },
                new()
                {
                    Id = hektarId,
                    Name = "Đèn sàn IKEA HEKTAR xám đậm",
                    Slug = "den-san-ikea-hektar-xam-dam",
                    ShortDescription = "<ul><li>Chao đèn kim loại xoay hướng</li><li>Cần đèn điều chỉnh cao thấp</li><li>Dùng bóng E27 tiết kiệm điện</li></ul>",
                    Description = "<p>Đèn sàn HEKTAR: phong cách công nghiệp, ánh sáng tập trung lý tưởng cho góc đọc sách và làm việc.</p>",
                    IsPublished = true,
                    Price = 1899000m,
                    BrandId = ikeaId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Đèn sàn IKEA HEKTAR chính hãng",
                    MetaKeywords = "đèn ikea, hektar, đèn sàn",
                    MetaDescription = "Mua đèn sàn IKEA HEKTAR chính hãng, bảo hành 2 năm. Mua ngay!"
                },
                new()
                {
                    Id = retroTableId,
                    Name = "Bàn trà MOHO Retro mặt đá 110cm",
                    Slug = "ban-tra-moho-retro-mat-da-110cm",
                    ShortDescription = "<ul><li>Mặt đá nhân tạo vân mây</li><li>Chân gỗ sồi vát côn</li><li>Kèm tầng để tạp chí</li></ul>",
                    Description = "<p>Bàn trà Retro: mặt đá mát lạnh sang trọng, chân gỗ sồi thanh mảnh — điểm nhấn cho phòng khách.</p>",
                    IsPublished = true,
                    Price = 3290000m,
                    BrandId = mohoId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Bàn trà MOHO Retro mặt đá giá tốt",
                    MetaKeywords = "bàn trà moho, bàn đá, coffee table",
                    MetaDescription = "Mua bàn trà MOHO Retro mặt đá giá tốt, bảo hành 5 năm. Mua ngay!"
                },
                new()
                {
                    Id = mollyId,
                    Name = "Ghế ăn Nhà Xinh Molly bọc vải",
                    Slug = "ghe-an-nha-xinh-molly-boc-vai",
                    ShortDescription = "<ul><li>Nệm ngồi + tựa bọc vải</li><li>Chân gỗ sồi tự nhiên</li><li>Ngồi êm, dễ phối bàn ăn</li></ul>",
                    Description = "<p>Ghế Molly: dáng gọn thanh lịch, nệm êm cho bữa ăn dài và chân gỗ sồi bền đẹp theo thời gian.</p>",
                    IsPublished = true,
                    Price = 1290000m,
                    BrandId = nhaXinhId,
                    CreatedDate = DateTime.UtcNow,
                    MetaTitle = "Ghế ăn Nhà Xinh Molly chính hãng",
                    MetaKeywords = "ghế nhà xinh, molly, ghế ăn bọc vải",
                    MetaDescription = "Mua ghế ăn Nhà Xinh Molly bọc vải chính hãng, bảo hành 2 năm. Mua ngay!"
                }
            }, cancellationToken);

            await context.ProductCategories.AddRangeAsync(new List<ProductCategory>
            {
                new() { Id = Guid.NewGuid(), CategoryId = livingId, ProductId = soderhamnId, DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = sofasId, ProductId = soderhamnId, DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = livingId, ProductId = fynId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = sofasId, ProductId = fynId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = livingId, ProductId = lackId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = coffeeTablesId, ProductId = lackId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = bedroomId, ProductId = viennaId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = bedsId, ProductId = viennaId, DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = bedroomId, ProductId = cataniaId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = bedsId, ProductId = cataniaId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = bedroomId, ProductId = ca3aId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = wardrobesId, ProductId = ca3aId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = bedroomId, ProductId = brimnesId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = wardrobesId, ProductId = brimnesId, DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = diningId, ProductId = dalatId, DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = diningSetsId, ProductId = dalatId, DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = diningId, ProductId = at120Id, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = diningSetsId, ProductId = at120Id, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = officeId, ProductId = hp140Id, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = desksId, ProductId = hp140Id, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = officeId, ProductId = at140Id, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = desksId, ProductId = at140Id, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = officeId, ProductId = bhsId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = desksId, ProductId = bhsId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = decorId, ProductId = hektarId, DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = livingId, ProductId = retroTableId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = coffeeTablesId, ProductId = retroTableId, DisplayOrder = 0, IsFeaturedProduct = true, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = diningId, ProductId = mollyId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), CategoryId = diningSetsId, ProductId = mollyId, DisplayOrder = 0, IsFeaturedProduct = false, CreatedDate = DateTime.UtcNow }
            }, cancellationToken);

            await context.ProductAttributeValues.AddRangeAsync(new List<ProductAttributeValue>
            {
                new() { Id = Guid.NewGuid(), AttributeId = colorId, ProductId = soderhamnId, Value = "Xám Fridtuna" },
                new() { Id = Guid.NewGuid(), AttributeId = styleId, ProductId = soderhamnId, Value = "Scandinavian" },
                new() { Id = Guid.NewGuid(), AttributeId = materialId, ProductId = soderhamnId, Value = "Vải polyester, khung gỗ" },
                new() { Id = Guid.NewGuid(), AttributeId = surfaceId, ProductId = soderhamnId, Value = "Vải bọc tháo giặt" },
                new() { Id = Guid.NewGuid(), AttributeId = widthId, ProductId = soderhamnId, Value = "193" },
                new() { Id = Guid.NewGuid(), AttributeId = depthId, ProductId = soderhamnId, Value = "99" },
                new() { Id = Guid.NewGuid(), AttributeId = heightId, ProductId = soderhamnId, Value = "83" },
                new() { Id = Guid.NewGuid(), AttributeId = loadId, ProductId = soderhamnId, Value = "320" },

                new() { Id = Guid.NewGuid(), AttributeId = colorId, ProductId = fynId, Value = "Xám tro" },
                new() { Id = Guid.NewGuid(), AttributeId = styleId, ProductId = fynId, Value = "Hiện đại" },
                new() { Id = Guid.NewGuid(), AttributeId = materialId, ProductId = fynId, Value = "Gỗ cao su, vải bố" },
                new() { Id = Guid.NewGuid(), AttributeId = surfaceId, ProductId = fynId, Value = "Vải bọc" },
                new() { Id = Guid.NewGuid(), AttributeId = widthId, ProductId = fynId, Value = "210" },
                new() { Id = Guid.NewGuid(), AttributeId = depthId, ProductId = fynId, Value = "85" },
                new() { Id = Guid.NewGuid(), AttributeId = heightId, ProductId = fynId, Value = "85" },
                new() { Id = Guid.NewGuid(), AttributeId = loadId, ProductId = fynId, Value = "300" },

                new() { Id = Guid.NewGuid(), AttributeId = colorId, ProductId = lackId, Value = "Trắng" },
                new() { Id = Guid.NewGuid(), AttributeId = styleId, ProductId = lackId, Value = "Tối giản" },
                new() { Id = Guid.NewGuid(), AttributeId = materialId, ProductId = lackId, Value = "Ván ép tổ ong" },
                new() { Id = Guid.NewGuid(), AttributeId = surfaceId, ProductId = lackId, Value = "Phủ foil" },
                new() { Id = Guid.NewGuid(), AttributeId = widthId, ProductId = lackId, Value = "55" },
                new() { Id = Guid.NewGuid(), AttributeId = depthId, ProductId = lackId, Value = "55" },
                new() { Id = Guid.NewGuid(), AttributeId = heightId, ProductId = lackId, Value = "45" },
                new() { Id = Guid.NewGuid(), AttributeId = loadId, ProductId = lackId, Value = "25" },

                new() { Id = Guid.NewGuid(), AttributeId = colorId, ProductId = viennaId, Value = "Nâu tự nhiên" },
                new() { Id = Guid.NewGuid(), AttributeId = styleId, ProductId = viennaId, Value = "Scandinavian" },
                new() { Id = Guid.NewGuid(), AttributeId = materialId, ProductId = viennaId, Value = "Gỗ cao su" },
                new() { Id = Guid.NewGuid(), AttributeId = surfaceId, ProductId = viennaId, Value = "Sơn PU mờ" },
                new() { Id = Guid.NewGuid(), AttributeId = widthId, ProductId = viennaId, Value = "160" },
                new() { Id = Guid.NewGuid(), AttributeId = depthId, ProductId = viennaId, Value = "200" },
                new() { Id = Guid.NewGuid(), AttributeId = heightId, ProductId = viennaId, Value = "110" },
                new() { Id = Guid.NewGuid(), AttributeId = loadId, ProductId = viennaId, Value = "300" },

                new() { Id = Guid.NewGuid(), AttributeId = colorId, ProductId = cataniaId, Value = "Xám sáng" },
                new() { Id = Guid.NewGuid(), AttributeId = styleId, ProductId = cataniaId, Value = "Hiện đại" },
                new() { Id = Guid.NewGuid(), AttributeId = materialId, ProductId = cataniaId, Value = "Vải nỉ, gỗ thông" },
                new() { Id = Guid.NewGuid(), AttributeId = surfaceId, ProductId = cataniaId, Value = "Vải bọc" },
                new() { Id = Guid.NewGuid(), AttributeId = widthId, ProductId = cataniaId, Value = "180" },
                new() { Id = Guid.NewGuid(), AttributeId = depthId, ProductId = cataniaId, Value = "200" },
                new() { Id = Guid.NewGuid(), AttributeId = heightId, ProductId = cataniaId, Value = "120" },
                new() { Id = Guid.NewGuid(), AttributeId = loadId, ProductId = cataniaId, Value = "350" },

                new() { Id = Guid.NewGuid(), AttributeId = colorId, ProductId = ca3aId, Value = "Ghi sáng" },
                new() { Id = Guid.NewGuid(), AttributeId = styleId, ProductId = ca3aId, Value = "Công nghiệp" },
                new() { Id = Guid.NewGuid(), AttributeId = materialId, ProductId = ca3aId, Value = "Thép cán nguội" },
                new() { Id = Guid.NewGuid(), AttributeId = surfaceId, ProductId = ca3aId, Value = "Sơn tĩnh điện" },
                new() { Id = Guid.NewGuid(), AttributeId = widthId, ProductId = ca3aId, Value = "135" },
                new() { Id = Guid.NewGuid(), AttributeId = depthId, ProductId = ca3aId, Value = "45" },
                new() { Id = Guid.NewGuid(), AttributeId = heightId, ProductId = ca3aId, Value = "183" },
                new() { Id = Guid.NewGuid(), AttributeId = loadId, ProductId = ca3aId, Value = "120" },

                new() { Id = Guid.NewGuid(), AttributeId = colorId, ProductId = brimnesId, Value = "Trắng" },
                new() { Id = Guid.NewGuid(), AttributeId = styleId, ProductId = brimnesId, Value = "Scandinavian" },
                new() { Id = Guid.NewGuid(), AttributeId = materialId, ProductId = brimnesId, Value = "Ván dăm" },
                new() { Id = Guid.NewGuid(), AttributeId = surfaceId, ProductId = brimnesId, Value = "Melamine" },
                new() { Id = Guid.NewGuid(), AttributeId = widthId, ProductId = brimnesId, Value = "117" },
                new() { Id = Guid.NewGuid(), AttributeId = depthId, ProductId = brimnesId, Value = "50" },
                new() { Id = Guid.NewGuid(), AttributeId = heightId, ProductId = brimnesId, Value = "190" },
                new() { Id = Guid.NewGuid(), AttributeId = loadId, ProductId = brimnesId, Value = "100" },

                new() { Id = Guid.NewGuid(), AttributeId = colorId, ProductId = dalatId, Value = "Nâu óc chó" },
                new() { Id = Guid.NewGuid(), AttributeId = styleId, ProductId = dalatId, Value = "Scandinavian" },
                new() { Id = Guid.NewGuid(), AttributeId = materialId, ProductId = dalatId, Value = "Gỗ cao su" },
                new() { Id = Guid.NewGuid(), AttributeId = surfaceId, ProductId = dalatId, Value = "Sơn PU" },
                new() { Id = Guid.NewGuid(), AttributeId = widthId, ProductId = dalatId, Value = "120" },
                new() { Id = Guid.NewGuid(), AttributeId = depthId, ProductId = dalatId, Value = "70" },
                new() { Id = Guid.NewGuid(), AttributeId = heightId, ProductId = dalatId, Value = "75" },
                new() { Id = Guid.NewGuid(), AttributeId = loadId, ProductId = dalatId, Value = "200" },

                new() { Id = Guid.NewGuid(), AttributeId = colorId, ProductId = at120Id, Value = "Vân gỗ sồi" },
                new() { Id = Guid.NewGuid(), AttributeId = styleId, ProductId = at120Id, Value = "Hiện đại" },
                new() { Id = Guid.NewGuid(), AttributeId = materialId, ProductId = at120Id, Value = "Gỗ công nghiệp MFC" },
                new() { Id = Guid.NewGuid(), AttributeId = surfaceId, ProductId = at120Id, Value = "Melamine" },
                new() { Id = Guid.NewGuid(), AttributeId = widthId, ProductId = at120Id, Value = "120" },
                new() { Id = Guid.NewGuid(), AttributeId = depthId, ProductId = at120Id, Value = "60" },
                new() { Id = Guid.NewGuid(), AttributeId = heightId, ProductId = at120Id, Value = "75" },
                new() { Id = Guid.NewGuid(), AttributeId = loadId, ProductId = at120Id, Value = "100" },

                new() { Id = Guid.NewGuid(), AttributeId = colorId, ProductId = hp140Id, Value = "Vân gỗ sáng" },
                new() { Id = Guid.NewGuid(), AttributeId = styleId, ProductId = hp140Id, Value = "Hiện đại" },
                new() { Id = Guid.NewGuid(), AttributeId = materialId, ProductId = hp140Id, Value = "Gỗ MFC" },
                new() { Id = Guid.NewGuid(), AttributeId = surfaceId, ProductId = hp140Id, Value = "Melamine" },
                new() { Id = Guid.NewGuid(), AttributeId = widthId, ProductId = hp140Id, Value = "140" },
                new() { Id = Guid.NewGuid(), AttributeId = depthId, ProductId = hp140Id, Value = "70" },
                new() { Id = Guid.NewGuid(), AttributeId = heightId, ProductId = hp140Id, Value = "75" },
                new() { Id = Guid.NewGuid(), AttributeId = loadId, ProductId = hp140Id, Value = "120" },

                new() { Id = Guid.NewGuid(), AttributeId = colorId, ProductId = at140Id, Value = "Vân gỗ" },
                new() { Id = Guid.NewGuid(), AttributeId = styleId, ProductId = at140Id, Value = "Hiện đại" },
                new() { Id = Guid.NewGuid(), AttributeId = materialId, ProductId = at140Id, Value = "Gỗ MFC" },
                new() { Id = Guid.NewGuid(), AttributeId = surfaceId, ProductId = at140Id, Value = "Melamine" },
                new() { Id = Guid.NewGuid(), AttributeId = widthId, ProductId = at140Id, Value = "140" },
                new() { Id = Guid.NewGuid(), AttributeId = depthId, ProductId = at140Id, Value = "70" },
                new() { Id = Guid.NewGuid(), AttributeId = heightId, ProductId = at140Id, Value = "75" },
                new() { Id = Guid.NewGuid(), AttributeId = loadId, ProductId = at140Id, Value = "120" },

                new() { Id = Guid.NewGuid(), AttributeId = colorId, ProductId = bhsId, Value = "Trắng - Ghi" },
                new() { Id = Guid.NewGuid(), AttributeId = styleId, ProductId = bhsId, Value = "Hiện đại" },
                new() { Id = Guid.NewGuid(), AttributeId = materialId, ProductId = bhsId, Value = "MFC, thép sơn" },
                new() { Id = Guid.NewGuid(), AttributeId = surfaceId, ProductId = bhsId, Value = "Sơn tĩnh điện" },
                new() { Id = Guid.NewGuid(), AttributeId = widthId, ProductId = bhsId, Value = "100" },
                new() { Id = Guid.NewGuid(), AttributeId = depthId, ProductId = bhsId, Value = "55" },
                new() { Id = Guid.NewGuid(), AttributeId = heightId, ProductId = bhsId, Value = "75" },
                new() { Id = Guid.NewGuid(), AttributeId = loadId, ProductId = bhsId, Value = "80" },

                new() { Id = Guid.NewGuid(), AttributeId = colorId, ProductId = hektarId, Value = "Xám đậm" },
                new() { Id = Guid.NewGuid(), AttributeId = styleId, ProductId = hektarId, Value = "Công nghiệp" },
                new() { Id = Guid.NewGuid(), AttributeId = materialId, ProductId = hektarId, Value = "Thép" },
                new() { Id = Guid.NewGuid(), AttributeId = surfaceId, ProductId = hektarId, Value = "Sơn tĩnh điện" },
                new() { Id = Guid.NewGuid(), AttributeId = widthId, ProductId = hektarId, Value = "32" },
                new() { Id = Guid.NewGuid(), AttributeId = depthId, ProductId = hektarId, Value = "32" },
                new() { Id = Guid.NewGuid(), AttributeId = heightId, ProductId = hektarId, Value = "176" },

                new() { Id = Guid.NewGuid(), AttributeId = colorId, ProductId = retroTableId, Value = "Trắng vân mây" },
                new() { Id = Guid.NewGuid(), AttributeId = styleId, ProductId = retroTableId, Value = "Hiện đại" },
                new() { Id = Guid.NewGuid(), AttributeId = materialId, ProductId = retroTableId, Value = "Đá nhân tạo, gỗ sồi" },
                new() { Id = Guid.NewGuid(), AttributeId = surfaceId, ProductId = retroTableId, Value = "Đánh bóng" },
                new() { Id = Guid.NewGuid(), AttributeId = widthId, ProductId = retroTableId, Value = "110" },
                new() { Id = Guid.NewGuid(), AttributeId = depthId, ProductId = retroTableId, Value = "55" },
                new() { Id = Guid.NewGuid(), AttributeId = heightId, ProductId = retroTableId, Value = "40" },
                new() { Id = Guid.NewGuid(), AttributeId = loadId, ProductId = retroTableId, Value = "50" },

                new() { Id = Guid.NewGuid(), AttributeId = colorId, ProductId = mollyId, Value = "Be" },
                new() { Id = Guid.NewGuid(), AttributeId = styleId, ProductId = mollyId, Value = "Hiện đại" },
                new() { Id = Guid.NewGuid(), AttributeId = materialId, ProductId = mollyId, Value = "Vải, gỗ sồi" },
                new() { Id = Guid.NewGuid(), AttributeId = surfaceId, ProductId = mollyId, Value = "Vải bọc" },
                new() { Id = Guid.NewGuid(), AttributeId = widthId, ProductId = mollyId, Value = "46" },
                new() { Id = Guid.NewGuid(), AttributeId = depthId, ProductId = mollyId, Value = "52" },
                new() { Id = Guid.NewGuid(), AttributeId = heightId, ProductId = mollyId, Value = "82" },
                new() { Id = Guid.NewGuid(), AttributeId = loadId, ProductId = mollyId, Value = "120" }
            }, cancellationToken);

            var furnitureTemplateId = Guid.Parse("f2000000-0000-4000-8000-000000000044");
            var lightingTemplateId = Guid.Parse("f2000000-0000-4000-8000-000000000045");

            await context.ProductTemplates.AddRangeAsync(new List<ProductTemplate>
            {
                new() { Id = furnitureTemplateId, Name = "Furniture", CreatedDate = DateTime.UtcNow },
                new() { Id = lightingTemplateId, Name = "Lighting", CreatedDate = DateTime.UtcNow }
            }, cancellationToken);

            var furnitureAttributes = new[] { colorId, styleId, materialId, surfaceId, widthId, depthId, heightId, loadId };
            var lightingAttributes = new[] { colorId, styleId, materialId, heightId };

            await context.ProductTemplateProductAttributes.AddRangeAsync(
                furnitureAttributes
                    .Select(a => new ProductTemplateProductAttribute { Id = Guid.NewGuid(), ProductTemplateId = furnitureTemplateId, ProductAttributeId = a, CreatedDate = DateTime.UtcNow })
                    .Concat(lightingAttributes
                        .Select(a => new ProductTemplateProductAttribute { Id = Guid.NewGuid(), ProductTemplateId = lightingTemplateId, ProductAttributeId = a, CreatedDate = DateTime.UtcNow }))
                    .ToList(), cancellationToken);
        }
    }
}
