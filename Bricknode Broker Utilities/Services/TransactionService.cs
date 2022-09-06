using System.Globalization;
using BfsApi;

namespace Bricknode_Broker_Utilities.Services;

public class TransactionService
{
    private readonly BfsDataService _bfsDataService;

    public TransactionService(BfsDataService bfsDataService)
    {
        _bfsDataService = bfsDataService;
    }

    /// <summary>
    ///     This method will deposit cash into an account within the Bricknode Broker instance
    /// </summary>
    /// <returns></returns>
    public async Task DepositCash(string bfsInstanceKey)
    {
        Console.WriteLine("Account number:");
        var accountNumber = int.Parse(Console.ReadLine() ?? string.Empty);

        var account = await _bfsDataService.GetAccountByNumber(accountNumber, bfsInstanceKey);

        Console.WriteLine("Amount to deposit:");
        var amount = decimal.Parse(Console.ReadLine() ?? string.Empty);

        Console.WriteLine("The following cash assets are available to deposit:");

        var cashAssets = await _bfsDataService.GetAvailableCashAssets(bfsInstanceKey);

        foreach (var cashAsset in cashAssets) Console.WriteLine($"{cashAsset.Key}");

        Console.WriteLine("");

        Console.WriteLine("Enter the three letter symbol for the cash asset that you would like to deposit:");

        var cashAssetKey = Console.ReadLine();

        var selectedCashAsset = cashAssets.Single(t => t.Key == cashAssetKey);

        Console.WriteLine("Enter trade date in the following format (YYYY-MM-DD)");
        var tradeDate =
            DateTime.ParseExact(Console.ReadLine() ?? string.Empty, "yyyy-MM-dd", CultureInfo.InvariantCulture);

        Console.WriteLine("Enter settle date in the following format (YYYY-MM-DD)");
        var settleDate =
            DateTime.ParseExact(Console.ReadLine() ?? string.Empty, "yyyy-MM-dd", CultureInfo.InvariantCulture);

        var superTransaction = CreateSuperTransaction(account, amount, selectedCashAsset, tradeDate, settleDate);

        var response = await _bfsDataService.CreateTransaction(superTransaction, bfsInstanceKey);

        if (response.Message == "OK")
        {
            Console.WriteLine("Success! The transaction was created.");
            return;
        }

        Console.WriteLine($"No transaction was created because of the following error: {response.Message}");
    }

    private SuperTransaction CreateSuperTransaction(GetAccountResponseRow account, decimal amount,
        GetCashResponseRow selectedCashAsset, DateTime tradeDate, DateTime settleDate)
    {
        return new SuperTransaction
        {
            Batch = Guid.NewGuid(),
            BusinessEventId = Guid.NewGuid(),
            BusinessTransactions = new[]
            {
                new BusinessTransaction
                {
                    Account = account.BrickId,
                    AmountAsset1 = amount,
                    Asset1 = selectedCashAsset.BrickId,
                    BusinessTransactionType = "Default_Transfer_Trade_Cash",
                    TradeDate = tradeDate
                },
                new BusinessTransaction
                {
                    Account = account.BrickId,
                    AmountAsset1 = amount,
                    Asset1 = selectedCashAsset.BrickId,
                    BusinessTransactionType = "Default_Transfer_Settle_Cash",
                    SettlementDate = settleDate,
                    ValueDate = settleDate
                }
            }
        };
    }
}