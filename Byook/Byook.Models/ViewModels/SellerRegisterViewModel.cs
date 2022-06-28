namespace Byook.ViewModels;

public record SellerRegisterViewModel
{
    public Seller? Seller { get; set; }

    public string OtherAddress { get; set; } = string.Empty;
}