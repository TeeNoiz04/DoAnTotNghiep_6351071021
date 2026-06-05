namespace QuoteFlow.Seeders;

public class MaterialSeeder
{
    //public List<MaterialDto> Generate(int count, int? seed = null)
    //{
    //    var statuses = new[] { "ACTIVE", "DISCONTINUTE", "DEACTIVE" };
    //    if (seed.HasValue)
    //        Randomizer.Seed = new Random(seed.Value);
    //    var faker = new Faker<MaterialDto>("en")
    //        .CustomInstantiator(f =>
    //        {
    //            return new MaterialDto
    //            {
    //                Id = f.Random.Guid(),
    //                GolfaCode = $"GF-{f.Random.AlphaNumeric(6).ToUpper()}",
    //                Model = $"Model-{f.Random.Int(1000, 9999)}",
    //                ValidFrom = f.Date.Past(2),
    //                ValidTo = f.Date.Future(2),
    //                SAP_Code = f.Random.Replace("SAP-####"),
    //                Spec1 = f.Commerce.ProductAdjective(),
    //                Spec2 = f.Commerce.ProductMaterial(),
    //                Spec3 = f.Commerce.Product(),
    //                Spec4 = f.Random.Word(),
    //                Description_EN = f.Commerce.ProductDescription(),
    //                Description_VN = f.Commerce.ProductDescription(),
    //                MaterialType = f.Commerce.Department(),
    //                Unit = "pcs",
    //                Material_SEC_Classification = f.Random.Word(),
    //                Material_Group = f.Random.Guid(),
    //                SAPMatGroup = f.Random.AlphaNumeric(5),
    //                Product_Hierarchy = f.Random.AlphaNumeric(8),
    //                ProductHierarchyDescription = f.Commerce.Product(),
    //                CountryOfOrigin = f.Address.Country(),
    //                ReferenceLeadTime = f.Random.Int(1, 60).ToString(),
    //                WarrantyTime = f.Random.Int(1, 24),
    //                InventoryCategory = f.Commerce.Categories(1)[0],
    //                Maxlot = f.Random.Int(10, 100),
    //                StockWarning = f.Random.Int(5, 20),
    //                VAT = f.Random.Decimal(0, 20),
    //                HS_Code = f.Random.Replace("HS####"),
    //                 = f.Random.Guid(),
    //                SupplierBU = f.Company.CompanyName(),
    //                Factory_nvarchar = f.Company.CompanySuffix(),
    //                BU = f.Random.Guid(),
    //                Input_Price = f.Random.Decimal(10, 1000),
    //                InputCurrency = f.Random.Guid(),
    //                INCOTERMS = "FOB",
    //                EPA = f.Random.Bool(),
    //                ImportDuty = f.Random.Decimal(0, 10),
    //                AppliedExchangeRate = f.Random.Decimal(22000, 25000),
    //                LandedCost = f.Random.Decimal(1000, 5000),
    //                MaxSalesOfferPrice = f.Random.Decimal(1000, 5000),
    //                MaxMangerOfferPrice = f.Random.Decimal(1000, 5000),
    //                Standard_Price = f.Random.Decimal(500, 2000),
    //                SellingPrice1 = f.Random.Decimal(1000, 2000),
    //                SellingPrice2 = f.Random.Decimal(2000, 3000),
    //                SellingPrice3 = f.Random.Decimal(3000, 4000),
    //                SellingPrice4 = f.Random.Decimal(4000, 5000),
    //                SellingPrice5 = f.Random.Decimal(5000, 6000),
    //                MaterialStatus = f.PickRandom(statuses),
    //                DestinationDate = f.Date.Future(),
    //                RegistrationDate = f.Date.Recent(),
    //                IndeactiveDate = f.Date.Recent(),
    //                Description_Group = f.Commerce.ProductAdjective(),
    //                Origin = f.Address.CountryCode(),
    //                Kind = f.Commerce.ProductMaterial(),
    //                Factory = f.Random.Guid(),
    //                Vendor = f.Random.Guid(),
    //                LeadTime = f.Random.Int(1, 30),
    //                RefExchangeRate = f.Random.Decimal(22000, 25000),
    //                Note = f.Commerce.ProductDescription(),
    //                ConcurrencyStamp = Guid.NewGuid().ToString(),
    //                MaterialHistory = new List<MaterialHistoryDto>
    //                {
    //                    new MaterialHistoryDto
    //                    {
    //                        Id = f.Random.Guid(),
    //                        MaterialId = f.Random.Guid(),
    //                        Action = "Created",
    //                        Note = "Initial creation",
    //                        ConcurrencyStamp = Guid.NewGuid().ToString(),
    //                        CreationTime = f.Date.Past()
    //                    },
    //                    new MaterialHistoryDto
    //                    {
    //                        Id = f.Random.Guid(),
    //                        MaterialId = f.Random.Guid(),
    //                        Action = "Updated",
    //                        Note = "Updated description",
    //                        ConcurrencyStamp = Guid.NewGuid().ToString(),
    //                        CreationTime = f.Date.Recent()
    //                    }
    //                },
    //                MaterialStock = new List<MaterialStockDto>
    //                {
    //                    new MaterialStockDto
    //                    {
    //                        Id = f.Random.Guid(),
    //                        MaterialId = f.Random.Guid(),
    //                        StockCategoryId = f.Random.Guid(),
    //                        GolfaCode = $"GF-{f.Random.AlphaNumeric(6).ToUpper()}",
    //                        Qty = f.Random.Int(0, 500),
    //                        Locked = f.Random.Int(0, 50),
    //                        LockStockKeeping = f.Random.Int(0, 20),
    //                        LockStockSO = f.Random.Int(0, 20),
    //                        Available_Qty = f.Random.Int(0, 400),
    //                        Note = f.Commerce.ProductDescription(),
    //                        ConcurrencyStamp = Guid.NewGuid().ToString(),
    //                        CreationTime = f.Date.Past(),
    //                        StockCategory = new StockCategoryDto
    //                        {
    //                            Id = f.Random.Guid(),
    //                            StockCode = $"SC-{f.Random.AlphaNumeric(4).ToUpper()}",
    //                            StockName = f.Commerce.Department(),
    //                            MainStock = f.Random.Bool(),
    //                            DamagedStock = f.Random.Bool(),
    //                            SortOrder = f.Random.Int(1, 10),
    //                            IsDeactive = f.Random.Bool(),
    //                            Note = f.Commerce.ProductDescription(),
    //                            ConcurrencyStamp = Guid.NewGuid().ToString(),
    //                            CreationTime = f.Date.Past()
    //                        }
    //                    }
    //                }

    //            };
    //        });
    //    return faker.Generate(count);
    //}
    //public List<MaterialApprovalDto> GenerateApprovalList(int count, int? seed = null)
    //{
    //    if (seed.HasValue)
    //        Randomizer.Seed = new Random(seed.Value);

    //    var importTypeFileNameMap = new Dictionary<string, string>
    //{
    //    { "MATERIAL.PRICE", "ImportMaterial_Price" },
    //    { "MATERIAL.NEW", "ImportMaterial_New" }
    //};

    //    var statuses = new[] { "IN_PROGRESS", "APPROVED", "REJECTED", "CANCELLED" };

    //    var faker = new Faker<MaterialApprovalDto>("en")
    //        .CustomInstantiator(f =>
    //        {
    //            var importType = f.PickRandom(importTypeFileNameMap.Keys.ToList());
    //            var fileName = importTypeFileNameMap[importType];

    //            // Dùng chung 1 ngày cho FileName và RequestNo
    //            var baseDate = f.Date.Between(DateTime.Now.AddMonths(-3), DateTime.Now.AddMonths(1));

    //            return new MaterialApprovalDto
    //            {
    //                Id = f.Random.Guid(),
    //                ImportType = importType,
    //                FileName = $"{fileName}_{baseDate:yyyy.MM.dd.HH.mm.ss}.xlsx",
    //                Note = f.Commerce.ProductDescription(),
    //                Status = f.PickRandom(statuses),
    //                RequestNo = $"{importType}_{baseDate:dd.MM}_{f.Random.Number(0, 999):D3}",
    //                Approvers = string.Join(", ", f.Make(f.Random.Number(1, 3), () => f.Name.FullName())),
    //                Description = f.Company.CatchPhrase(),

    //                CreationTime = f.Date.Past(1),
    //                CreatorId = f.Random.Guid(),
    //                CreatorName = f.Name.FullName(),
    //                LastModificationTime = f.Date.Recent(30),
    //                LastModifierId = f.Random.Guid(),
    //                LastModifierName = f.Name.FullName()
    //            };
    //        });

    //    return faker.Generate(count);
    //}

    //public List<MaterialApprovalDetailDto> GenerateApprovalListDetail(int count, Guid? importGuid, int? seed = null)
    //{

    //    if (seed.HasValue)
    //        Randomizer.Seed = new Random(seed.Value);

    //    var faker = new Faker<MaterialApprovalDetailDto>("en")
    //        .CustomInstantiator(f =>
    //        {
    //            var id = f.Random.Guid();

    //            return new MaterialApprovalDetailDto
    //            {
    //                Id = id,
    //                GolfaCode = $"GF-{f.Random.AlphaNumeric(6).ToUpper()}",
    //                Model = $"Model-{f.Random.Int(1000, 9999)}",
    //                ValidFrom = f.Date.Past(2),
    //                ValidTo = f.Date.Future(2),
    //                SAP_Code = f.Random.Replace("SAP-####"),
    //                Spec1 = f.Commerce.ProductAdjective(),
    //                Spec2 = f.Commerce.ProductMaterial(),
    //                Spec3 = f.Commerce.Product(),
    //                Spec4 = f.Random.Word(),
    //                Description_EN = f.Commerce.ProductDescription(),
    //                Description_VN = f.Commerce.ProductDescription(),
    //                MaterialType = f.Commerce.Department(),
    //                Unit = "pcs",
    //                Material_SEC_Classification = f.Random.Word(),
    //                Material_Group = f.Random.Guid(),
    //                SAPMatGroup = f.Random.AlphaNumeric(5),
    //                Product_Hierarchy = f.Random.AlphaNumeric(8),
    //                ProductHierarchyDescription = f.Commerce.Product(),
    //                CountryOfOrigin = f.Address.Country(),
    //                ReferenceLeadTime = f.Random.Int(1, 60).ToString(),
    //                WarrantyTime = f.Random.Int(1, 24),
    //                InventoryCategory = f.Commerce.Categories(1)[0],
    //                Maxlot = f.Random.Int(10, 100),
    //                StockWarning = f.Random.Int(5, 20),
    //                VAT = f.Random.Decimal(0, 20),
    //                HS_Code = f.Random.Replace("HS####"),
    //                Supplier = f.Random.Guid(),
    //                SupplierBU = f.Company.CompanyName(),
    //                Factory_nvarchar = f.Company.CompanySuffix(),
    //                BU = f.Random.Guid(),
    //                Input_Price = f.Random.Decimal(10, 1000),
    //                InputCurrency = f.Random.Guid(),
    //                INCOTERMS = "FOB",
    //                EPA = f.Random.Bool(),
    //                ImportDuty = f.Random.Decimal(0, 10),
    //                AppliedExchangeRate = f.Random.Decimal(22000, 25000),
    //                LandedCost = f.Random.Decimal(1000, 5000),
    //                MaxSalesOfferPrice = f.Random.Decimal(3000, 10000),
    //                MaxMangerOfferPrice = f.Random.Decimal(2000, 9000),
    //                Standard_Price = f.Random.Decimal(500, 2000),
    //                SellingPrice1 = f.Random.Decimal(1000, 2000),
    //                SellingPrice2 = f.Random.Decimal(2000, 3000),
    //                SellingPrice3 = f.Random.Decimal(3000, 4000),
    //                SellingPrice4 = f.Random.Decimal(4000, 5000),
    //                SellingPrice5 = f.Random.Decimal(5000, 6000),
    //                MaterialStatus = "Approved",
    //                DestinationDate = f.Date.Future(),
    //                RegistrationDate = f.Date.Recent(),
    //                IndeactiveDate = null,
    //                Description_Group = f.Commerce.ProductAdjective(),
    //                Origin = f.Address.CountryCode(),
    //                Kind = f.Commerce.ProductMaterial(),
    //                Factory = f.Random.Guid(),
    //                Vendor = f.Random.Guid(),
    //                LeadTime = f.Random.Int(1, 30),
    //                RefExchangeRate = f.Random.Decimal(22000, 25000),
    //                Note = f.Commerce.ProductDescription(),
    //                ImportGuid = f.PickRandom(importGuid),
    //                ConcurrencyStamp = Guid.NewGuid().ToString(),

    //                MaterialHistory = new List<MaterialHistoryDto>
    //                {
    //                    new MaterialHistoryDto
    //                    {
    //                        Id = f.Random.Guid(),
    //                        MaterialId = id,
    //                        Action = "Created",
    //                        Note = "Initial creation",
    //                        ConcurrencyStamp = Guid.NewGuid().ToString(),
    //                        CreationTime = f.Date.Past()
    //                    },
    //                    new MaterialHistoryDto
    //                    {
    //                        Id = f.Random.Guid(),
    //                        MaterialId = id,
    //                        Action = "Updated",
    //                        Note = "Updated description",
    //                        ConcurrencyStamp = Guid.NewGuid().ToString(),
    //                        CreationTime = f.Date.Recent()
    //                    }
    //                },

    //                MaterialStock = new List<MaterialStockDto>
    //                {
    //                    new MaterialStockDto
    //                    {
    //                        Id = f.Random.Guid(),
    //                        MaterialId = id,
    //                        StockCategoryId = f.Random.Guid(),
    //                        GolfaCode = $"GF-{f.Random.AlphaNumeric(6).ToUpper()}",
    //                        Qty = f.Random.Int(0, 500),
    //                        Locked = f.Random.Int(0, 50),
    //                        LockStockKeeping = f.Random.Int(0, 20),
    //                        LockStockSO = f.Random.Int(0, 20),
    //                        Available_Qty = f.Random.Int(0, 400),
    //                        Note = f.Commerce.ProductDescription(),
    //                        ConcurrencyStamp = Guid.NewGuid().ToString(),
    //                        CreationTime = f.Date.Past(),

    //                        StockCategory = new StockCategoryDto
    //                        {
    //                            Id = f.Random.Guid(),
    //                            StockCode = $"SC-{f.Random.AlphaNumeric(4).ToUpper()}",
    //                            StockName = f.Commerce.Department(),
    //                            MainStock = f.Random.Bool(),
    //                            DamagedStock = f.Random.Bool(),
    //                            SortOrder = f.Random.Int(1, 10),
    //                            IsDeactive = f.Random.Bool(),
    //                            Note = f.Commerce.ProductDescription(),
    //                            ConcurrencyStamp = Guid.NewGuid().ToString(),
    //                            CreationTime = f.Date.Past()
    //                        }
    //                    }
    //                }
    //            };
    //        });

    //    return faker.Generate(count);
    //}



}

