namespace Byook.Models;

public class Seller
{
    [Comment("사업자등록번호")]
    [Required(ErrorMessage = "아이디를 입력해주세요.")]
    [StringLength(11)]
    public string SellerId { get; set; } = string.Empty;

    [Comment("비밀번호")]
    [Required(ErrorMessage = "비밀번호를 입력해주세요.")]
    [StringLength(512)]
    public string Password { get; set; } = string.Empty;

    [Comment("대표자명")]
    [StringLength(5, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Phone]
    [Comment("핸드폰번호")]
    [StringLength(11)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Comment("상호명")]
    [StringLength(30)]
    public string TradeName { get; set; } = string.Empty;

    [Comment("판매자 주소")]
    [StringLength(30)]
    public string Address { get; set; } = string.Empty;

    [NotMapped]
    [ForeignKey("SellerId")]
    public ICollection<Product>? Products { get; set; }
}