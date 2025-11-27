using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Strategy.Interface
{
    public interface IPaymentStrategy
    {
        PaymentScheme paymentScheme { get; }
        bool DoesPaymentConditionMatch(MakePaymentRequest request, Account account);
    }
}
