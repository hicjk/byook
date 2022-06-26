namespace byook.Models;

public class User
{
    [StringLength(11)]
    public string Id { get; set; } = string.Empty;

    [StringLength(512)]
    public string Password { get; set; } = string.Empty;
}