[![Payment Gateway Build and Test](https://github.com/KevWK314/Checkout.PaymentGateway/actions/workflows/buildAndTest.yml/badge.svg)](https://github.com/KevWK314/Checkout.PaymentGateway/actions/workflows/buildAndTest.yml)

# Checkout.com Payment Gateway

This implementation of a Payment Gateway has been written by Kevin Kavanagh as part of the interview process with Checkout.com as per the received spec. 

It is written using C# and I have chosen .NET 6 as the target framework. 

## How to run

I have written all the logic into a single ASP.NET api. To run the app you should just have to set `Checkout.PaymentGateway.Api` as the startup project and hit F5.

Here is a request example to process a payment:
```
curl -X POST \
  'https://localhost:7219/api/payment?merchantkey=merchantkey1' \
  --header 'Accept: */*' \
  --header 'Content-Type: application/json' \
  -d '{
    "amount": 23.5,
    "currency": "GBP",
    "from": {
        "cardHolderName": "Jimmy",
        "cardNumber": "1234123412341234",
        "cvv": "123",
        "expiryMonth": 12,
        "expiryYear": 22
    }
}'
```

And here is a request example to get a payment:
```
curl -X GET \
  'https://localhost:7219/api/payment/ac815b44-eeac-4147-ad60-a7a63c322787?merchantkey=merchantkey2' \
  --header 'Accept: */*' \
  --header 'Content-Type: application/json' \
  -d '{
    "amount": 23.5,
    "currency": "GBP",
    "from": {
        "cardHolderName": "Jimmy",
        "cardNumber": "1234123412341234",
        "cvv": "123",
        "expiryMonth": 12,
        "expiryYear": 22
    }
}'
```

## Mocked Data

I have mocked 4 Merchants with key values of `merchantkey1, merchantkey2, merchantkey3, merchantkey4` where every payment request with `merchantkey4` will fail (return a success value of false).
Along with that I have mocked the Bank response which would result when sending a payment request to the Bank. 

## Deployment

I haven't spent any time on implementing a deployment strategy. But as it is a .NET 6 service I would think it could be containerised and deployed to whatever platform using whatever technology makes the most sense.

## Assumptions

- MerchantClient: I've made the assumption that Merchants would be identified by a secret key (this was raised in a previous round of the interview process), but the lookup would exist in another service. MerchantClient is mocking the call to another service.
- When mocking the Bank I wasn't sure how to pass the merchant details on the bank request. For lack of a better option I've used an "account". It's just meant to represent the merchants bank information in the transaction.
- I've kept error handling simple. 
  - If the payment fails because of the Acquiring Bank, the failure is indicated in the payment as success = false. 
  - If the payment fails because of some other unexpected error (invalid merchant), the error field on the response will be populated and the payment will be empty.
- I have used coding conventions from my most previous role. This includes interface definitions in the same file as the implementation, not adding 'Async' to all async methods, and probably others. Joining a new team would mean using the teams conventions.

## Improvements

- I haven't added any logging to File (using Serilog or similar). But with Microsoft Logging Extensions, this would be a trivial process if required.
- In general I've kept logging to an absolute minimum. Debug and information logging would have to be reviewed as a team to decide what information we would want to hand.
- I've put getting the merchant logic into the Handlers. But it might be better putting it into an ActionFilter that is applied to the Controller.
- I'm only handling a valid response from the Bank (with a success or failed status). I have no doubt there are scenarios where the bank request could fail catastrophically and that should be well thought through and handled. As mentioned in a previous interview, we would want some kind of Circuit Breaker (i.e. Polly). You would also want considerable logging and monitoring around exceptions.
- This implementation does not have validation on the request, mostly because I ran out of time. Depending on the complicated nature of the validation, we would either use Data Annotations on the incoming request or create custom validator classes.
- I haven't unit tested everything. I spent more time on the Integration tests. I have added a unit test to show the approach I would have taken. But given the type and size of service, I felt I would get more value from integration tests.
- I haven't had to add any configuration, but if that were required I would update appSettings.json and create config types where required.
