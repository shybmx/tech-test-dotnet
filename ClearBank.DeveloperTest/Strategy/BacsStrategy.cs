using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Strategy
{
    public class BacsStrategy : PaymentStrategy
    {
        public static PaymentScheme _paymentScheme = PaymentScheme.Bacs;

        public override bool DoesPaymentConditionMatch(MakePaymentRequest request, Account account)
        {
            if (!IsPaymentTypeAllowed(account, AllowedPaymentSchemes.Bacs))
            {
                return false;
            }

            return true;
        }
    }
}
