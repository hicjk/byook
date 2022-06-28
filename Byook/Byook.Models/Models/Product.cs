namespace Byook.Models;

public class Product
{
    [Key]
    [Comment("상품번호")]
    [StringLength(11)]
    public string ProductId { get; set; } = string.Empty;

    [Comment("사업자등록번호")]
    [StringLength(11)]
    public string SellerId { get; set; } = string.Empty;
    
    [Comment("등록날짜")]
    public DateTime CreateDate { get; set; }

    [Comment("제품명")]
    [StringLength(100)]
    public string Comment { get; set; } = string.Empty;

    [Comment("가격")]
    [Range(100, 1000000)]
    public int Price { get; set; }

    [Comment("무게")]
    [Range(50, 999)]
    public int Weight { get; set; }

    [Comment("원산지")]
    [StringLength(15)]
    public string Origin { get; set; } = string.Empty;

    [Comment("소 등록번호")]
    [StringLength(12)]
    public string CowSerialNumber { get; set; } = string.Empty;
}