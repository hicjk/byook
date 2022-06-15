namespace byook.Models;

public record Consumer
{
    public string ConsumerId { get; set; } = string.Empty;

    [Required]
    [StringLength(512)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [StringLength(5, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(11)]
    public string PhoneNumber { get; set; } = string.Empty;

    //[Required]
    [StringLength(30)]
    public string Address { get; set; } = string.Empty;
}