using ClearBank.DeveloperTest.Data.Interfaces;
using ClearBank.DeveloperTest.Factory.Interface;
using ClearBank.DeveloperTest.Types;
using System;
using System.Configuration;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountDataStore _accountDataStore;
        private readonly IAccountDataStore _backupAccountDataStore;
        private readonly IPaymentFactory _paymentFactory;

        private const string AppSettingDataStoreType = "DataStoreType";
        private const string BackupDataStoreType = "Backup";

        public PaymentService(IAccountDataStore accountDataStore, IAccountDataStore backupAccountDataStore, IPaymentFactory paymentFactory)
        {
            _accountDataStore = accountDataStore;
            _backupAccountDataStore = backupAccountDataStore;
            _paymentFactory = paymentFactory;
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var accountDataStore = GetAccountDataStore();

            var account = accountDataStore.GetAccount(request.DebtorAccountNumber);

            var result = new MakePaymentResult { Success = false };

            if (account == null)
            {
                return result;
            }

            result.Success = _paymentFactory.GetStrategy(request.PaymentScheme)?.DoesPaymentConditionMatch(request, account) ?? false;

            if (result.Success)
            {
                UpdateAccount(request, account, accountDataStore);
            }

            return result;
        }

        private void UpdateAccount(MakePaymentRequest request, Account account, IAccountDataStore accountDataStore)
        {
            account.Balance -= request.Amount;

            accountDataStore.UpdateAccount(account);
        }

        private IAccountDataStore GetAccountDataStore()
        {
            var dataStoreType = ConfigurationManager.AppSettings[AppSettingDataStoreType];
            
            return dataStoreType.Equals(BackupDataStoreType, StringComparison.InvariantCultureIgnoreCase)
                ? _backupAccountDataStore : _accountDataStore;
        }
    }
}
