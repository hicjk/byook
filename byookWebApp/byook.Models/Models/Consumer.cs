namespace byook.Models;

public class Consumer : User
{
    [StringLength(5, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [StringLength(11)]
    public string PhoneNumber { get; set; } = string.Empty;

    [StringLength(30)]
    public string Address { get; set; } = string.Empty;
}