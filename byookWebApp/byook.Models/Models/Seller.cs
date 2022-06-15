namespace byook.Models;

public record Seller
{
    [StringLength(11)]
    public string SellerId { get; set; } = string.Empty;

    [Required]
    [StringLength(512)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [StringLength(5, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    public string TradeName { get; set; } = string.Empty;

    //[Required]
    [StringLength(30)]
    public string Address { get; set; } = string.Empty;
}