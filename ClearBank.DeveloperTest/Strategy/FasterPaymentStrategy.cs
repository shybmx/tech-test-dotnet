using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Strategy
{
    public class FasterPaymentStrategy : PaymentStrategy
    {
        public static PaymentScheme paymentScheme => PaymentScheme.FasterPayments;

        public override bool DoesPaymentConditionMatch(MakePaymentRequest request, Account account)
        {
            if (!IsPaymentTypeAllowed(account, AllowedPaymentSchemes.FasterPayments))
            {
                return false;
            }

            if (account.Balance < request.Amount)
            {
                return false;
            }

            return true;
        }
    }
}
