using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BfsApi;
using Bricknode.Soap.Sdk.Services;

namespace Bricknode_Broker_Utilities.Services
{
    public class BfsDataService
    {
        private readonly IBfsAccountService _bfsAccountService;
        private readonly IBfsAssetService _bfsAssetService;
        private readonly IBfsTransactionService _bfsTransactionService;

        public BfsDataService(IBfsAccountService bfsAccountService, IBfsAssetService bfsAssetService, IBfsTransactionService bfsTransactionService)
        {
            _bfsAccountService = bfsAccountService;
            _bfsAssetService = bfsAssetService;
            _bfsTransactionService = bfsTransactionService;
        }

        public async Task<GetAccountResponseRow> GetAccountByNumber(int accountNumber, string bfsInstanceKey)
        {
            return (await _bfsAccountService.GetAccountsAsync(new GetAccountsArgs()
            {
                AccountNos = new[] { accountNumber.ToString() }
            }, bfsInstanceKey)).Result.Single();
        }

        public async Task<GetCashResponseRow[]> GetAvailableCashAssets(string bfsInstanceKey)
        {
            return (await _bfsAssetService.GetCashAsync(new GetCashArgs()
            {
                InstrumentStatuses = new[] { 1 }
            }, bfsInstanceKey)).Result;
        }

        public async Task<CreateBusinessTransactionResponse> CreateTransaction(SuperTransaction superTransaction,
            string bfsInstanceKey) =>
            await _bfsTransactionService.CreateBusinessTransactionsAsync(new[] { superTransaction },
                bfsInstanceKey);
    }
}
