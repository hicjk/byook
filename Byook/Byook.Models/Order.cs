namespace Byook.Models
{
    public class Order
    {
        [Comment("주문번호")]
        [StringLength(18)]
        public string OrderId { get; set; } = string.Empty;

        [Comment("소비자")]
        [StringLength(15)]
        public string ConsumerId { get; set; } = string.Empty;

        [ValidateNever]
        [ForeignKey(nameof(ConsumerId))]
        public Consumer? Consumer { get; set; }

        [Comment("상품번호")]
        public int ProductId { get; set; }

        [ValidateNever]
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }
    }
}