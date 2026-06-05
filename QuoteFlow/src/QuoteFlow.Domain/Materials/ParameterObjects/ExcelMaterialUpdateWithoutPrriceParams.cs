using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.Materials.ParameterObjects;

public class ExcelMaterialUpdateWithoutPrriceParams
{
    [Required]
    public Guid? Id { get; set; } // từ DTO

    [StringLength(MaterialConsts.GolfaCodeMaxLength)]
    public string? GolfaCode { get; set; } // -> MaterialCode

    [StringLength(MaterialConsts.ModelMaxLength)]
    public string? Model { get; set; } // -> ModelName

    public DateTime? RegistrationDate { get; set; } // -> RegistrationDate
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }

    [StringLength(MaterialConsts.Spec1MaxLength)]
    public string? Spec1 { get; set; }

    [StringLength(MaterialConsts.Spec2MaxLength)]
    public string? Spec2 { get; set; }

    [StringLength(MaterialConsts.Spec3MaxLength)]
    public string? Spec3 { get; set; }

    [StringLength(MaterialConsts.Spec4MaxLength)]
    public string? Spec4 { get; set; }

    [StringLength(MaterialConsts.Description_ENMaxLength)]
    public string? Description_EN { get; set; } // -> DescriptionEN

    [StringLength(MaterialConsts.Description_VNMaxLength)]
    public string? Description_VN { get; set; } // -> DescriptionVN

    [StringLength(MaterialConsts.SupplierCodeMaxLength)]
    public string? SupplierCode { get; set; } // -> Supplier

    public string? SupplierBUCode { get; set; } // -> SupplierBU
    public Guid? SupplierBUId { get; set; }

    public string? Factory_Text { get; set; } // -> Factory

    [StringLength(MaterialConsts.MaterialTypeMaxLength)]
    public string? MaterialType { get; set; }

    [StringLength(MaterialConsts.UnitMaxLength)]
    public string? Unit { get; set; }

    public string? Material_SEC_Classification { get; set; } // optional extra

    public string? Material_Group { get; set; } // -> MaterialGroupId
    public string? MaterialGroup { get; set; } // -> MaterialGroup

    [StringLength(MaterialConsts.SAPMatGroupMaxLength)]
    public string? SAPMatGroup { get; set; } // -> SAPGroup

    [StringLength(MaterialConsts.ProductHierarchyDescriptionMaxLength)]
    public string? ProductHierarchyDescription { get; set; } // -> ProductHierarchy_Description

    [StringLength(MaterialConsts.CountryOfOriginMaxLength)]
    public string? CountryOfOrigin { get; set; }

    public int? ReferenceLeadTime { get; set; }

    public int? WarrantyTime { get; set; }

    [StringLength(MaterialConsts.InventoryCategoryMaxLength)]
    public string? InventoryCategory { get; set; }

    public int? Maxlot { get; set; } // -> MaxLot

    public int? StockWarning { get; set; }

    public int? StockQty { get; set; } // -> DTO

    [StringLength(MaterialConsts.HS_CodeMaxLength)]
    public string? HS_Code { get; set; } // -> HSCode
    [StringLength(MaterialConsts.CargoNoteMaxLength)]
    public virtual string? CargoNote { get; set; }
    [StringLength(MaterialConsts.WeightMaxLength)]
    public virtual string? Weight { get; set; }
    [StringLength(MaterialConsts.SizeMaxLength)]
    public virtual string? Size { get; set; }
    [StringLength(MaterialConsts.QRCodeMaxLength)]
    public virtual string? QRCode { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
}
