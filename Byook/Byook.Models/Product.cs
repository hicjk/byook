

namespace Byook.Models
{

    public class Product
    {
        [Key]
        [Comment("상품번호")]
        public int Id { get; set; }

        [Comment("사업자등록번호")]
        [StringLength(11)]
        public string SellerId { get; set; } = string.Empty;

        [Comment("등록날짜")]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        [Required]
        [Comment("제품명")]
        [StringLength(100)]
        public string Comment { get; set; } = string.Empty;

        [Required]
        [Comment("가격")]
        [Range(100, 1000000)]
        public int Price { get; set; }

        [Required]
        [Comment("무게")]
        [Range(50, 999)]
        public int Weight { get; set; }

        [ValidateNever]
        public string ImageUrl { get; set; } = string.Empty;
    }
}