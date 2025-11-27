using ClearBank.DeveloperTest.Strategy.Interface;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Strategy
{
    public abstract class PaymentStrategy : IPaymentStrategy
    {
        public PaymentScheme paymentScheme { get; set; }

        public abstract bool DoesPaymentConditionMatch(MakePaymentRequest request, Account account);
        public bool IsPaymentTypeAllowed(Account account, AllowedPaymentSchemes allowedPaymentSchemes)
        {
            return account.AllowedPaymentSchemes.HasFlag(allowedPaymentSchemes);
        }
    }
}
