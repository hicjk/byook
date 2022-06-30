namespace Byook.Models
{
    public class Consumer
    {
        [Comment("아이디")]
        [Required(ErrorMessage = "아이디를 입력해주세요.")]
        [StringLength(15)]
        public string ConsumerId { get; set; } = string.Empty;

        [Comment("비밀번호")]
        [Required(ErrorMessage = "비밀번호를 입력해주세요.")]
        [StringLength(512)]
        public string Password { get; set; } = string.Empty;

        [Comment("성명")]
        [StringLength(5, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Phone]
        [Comment("핸드폰 번호")]
        [StringLength(11)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Comment("구매자 주소")]
        [StringLength(30)]
        public string Address { get; set; } = string.Empty;
    }
}