namespace Checkout.PaymentGateway.Api.Client;

public interface IMerchantClient
{
    Task<Merchant> GetMerchant(string secretKey);
}

/// <summary>
/// This class is used to request Merchant details from a Merchant Service.
/// I've mocked this class using preconfigured merchants.
/// </summary>
public class MerchantClient : IMerchantClient
{
    private static readonly Merchant NotFoundMerchant = new Merchant
    {
        Id = "NotFound",
        IsActive = false
    };

    private Dictionary<string, Merchant> _merchants = new Dictionary<string, Merchant>
    {
        { "merchantkey1", new Merchant { Id = "09D3E1DD-BCC4-4457-A987-38B837B29D22", Name = "Happy Toy Shop", BankAccount = "53554743", IsActive = true } },
        { "merchantkey2", new Merchant { Id = "7495BDF4-6268-4BD9-AAFE-6DB1A313D25A", Name = "We Make Scarves", BankAccount = "93847652", IsActive = true } },
        { "merchantkey3", new Merchant { Id = "1223CC0A-5743-463D-84B3-EF5DDFCF8338", Name = "The Flower Shop", BankAccount = "28594955", IsActive = true } },
        { "merchantkey4", new Merchant { Id = "75EC028C-0064-4165-86A8-C791635D209F", Name = "The Grocer", BankAccount = "Invalid", IsActive = true } },
    };

    public Task<Merchant> GetMerchant(string secretKey)
    {
        if (_merchants.TryGetValue(secretKey, out var merchant))
        {
            return Task.FromResult(merchant);
        }

        return Task.FromResult(NotFoundMerchant);
    }
}
