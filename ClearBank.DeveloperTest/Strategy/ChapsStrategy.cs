using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Strategy
{
    public class ChapsStrategy : PaymentStrategy
    {
        public static PaymentScheme paymentScheme => PaymentScheme.Chaps;

        public override bool DoesPaymentConditionMatch(MakePaymentRequest request, Account account)
        {
            if (!IsPaymentTypeAllowed(account, AllowedPaymentSchemes.Chaps))
            {
                return false;
            }

            if (account.Status != AccountStatus.Live)
            {
                return false;
            }

            return true;
        }
    }
}
