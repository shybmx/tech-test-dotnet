### Shahzad Emambaccus 

The original payment service had a few issues which made it hard to test and maintain, so I refactored it using solid principles and make it much easier to test and read.

1. I introduced interfaces to decouple the components, making it easier to swap implementations for testing or future changes.
2. Created a factory with strategy pattern to handle different payment methods, allowing for easy addition of new methods without modifying existing code. Within the factory itself, it has different strategies for each payment method. Each with their own implementation of the payment process. This can be expanded upon in the future if the validation logic of the different payment methods changes. 
3. The Payment service takes in 3 different interfaces, as the back up account and the standard account both use the same methods, it made sense to have them implement the same interface. Then added the factory interface to handle the different payment methods.

## Results
- The refactored payment service is now more modular, testable, and maintainable. New payment methods can be added with minimal changes to existing code, and the use of interfaces allows for easier mocking during unit tests.
- Unit tests were created for each component, ensuring that individual parts of the system work as expected. This improves reliability and helps catch bugs early in the development process.
- Overall, the refactoring has led to a cleaner architecture that adheres to solid principles, making future enhancements and maintenance more straightforward.
- No more duplicated code. 
- Readable and easy to follow.
