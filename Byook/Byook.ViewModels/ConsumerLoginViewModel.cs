using System.ComponentModel.DataAnnotations;

namespace Byook.ViewModels;

public class ConsumerLoginViewModel
{
    [Required]
    public string ConsumerId { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}