namespace Byook.Models
{
    public class Order
    {
        [Comment("주문번호")]
        [StringLength(18)]
        public string OrderId { get; set; } = string.Empty;

        [Comment("아이디")]
        [StringLength(15)]
        public string ConsumerId { get; set; } = string.Empty;

        [Comment("상품번호")]
        [StringLength(11)]
        public string ProductId { get; set; } = string.Empty;

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
    }
}