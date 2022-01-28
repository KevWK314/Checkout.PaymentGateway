using Microsoft.Extensions.Logging;

namespace Checkout.PaymentGateway.Api.Test.Unit.Handler;

public class GetPaymentHandlerTests
{
    private readonly IMerchantClient _merchantClient;
    private readonly IPaymentRepository _paymentRepository;
    private readonly GetPaymentHandler _sut;

    public GetPaymentHandlerTests()
    {
        _merchantClient = Substitute.For<IMerchantClient>();
        _paymentRepository = Substitute.For<IPaymentRepository>();

        _sut = new GetPaymentHandler(Substitute.For<ILogger<GetPaymentHandler>>(), _merchantClient, _paymentRepository);
    }

    [Fact]
    public async Task Process_WhenInvalidMerchant_ShouldReturnError()
    {
        _merchantClient.GetMerchant("secret1").Returns(Task.FromResult(new Merchant { IsActive = false }));

        var response = await _sut.Process("secret1", "paymentId1");

        response.Payment.Should().BeNull();
        response.Error.Should().Be("Invalid merchant.");

        await _merchantClient.Received(1).GetMerchant("secret1");
        await _paymentRepository.Received(0).GetPayment("paymentId1");
    }

    [Fact]
    public async Task Process_WhenNoMerchant_ShouldReturnError()
    {
        _merchantClient.GetMerchant("secret1").Returns(Task.FromResult((Merchant)null));

        var response = await _sut.Process("secret1", "paymentId1");

        response.Payment.Should().BeNull();
        response.Error.Should().Be("Invalid merchant.");

        await _merchantClient.Received(1).GetMerchant("secret1");
        await _paymentRepository.Received(0).GetPayment("paymentId1");
    }

    [Fact]
    public async Task Process_WhenNonMatchingMerchant_ShouldReturnError()
    {
        _merchantClient.GetMerchant("secret1").Returns(
            Task.FromResult(
                new Merchant
                {
                    Id = "merchantId1",
                    IsActive = true
                }));

        _paymentRepository.GetPayment("paymentId1").Returns(
            Task.FromResult(
                new Payment
                {
                    Id = "paymentId1",
                    MerchantId = "otherMerchantId"
                }));

        var response = await _sut.Process("secret1", "paymentId1");

        response.Payment.Should().BeNull();
        response.Error.Should().Be("Payment was not found.");

        await _merchantClient.Received(1).GetMerchant("secret1");
        await _paymentRepository.Received(1).GetPayment("paymentId1");
    }

    [Fact]
    public async Task Process_WhenPaymentNotFound_ShouldReturnError()
    {
        _merchantClient.GetMerchant("secret1").Returns(
            Task.FromResult(
                new Merchant
                {
                    Id = "merchantId1",
                    IsActive = true
                }));

        _paymentRepository.GetPayment("paymentId1").Returns(Task.FromResult((Payment)null));

        var response = await _sut.Process("secret1", "paymentId1");

        response.Payment.Should().BeNull();
        response.Error.Should().Be("Payment was not found.");

        await _merchantClient.Received(1).GetMerchant("secret1");
        await _paymentRepository.Received(1).GetPayment("paymentId1");
    }

    [Fact]
    public async Task Process_ShouldReturnPayment()
    {
        _merchantClient.GetMerchant("secret1").Returns(
            Task.FromResult(
                new Merchant
                {
                    Id = "merchantId1",
                    IsActive = true
                }));

        var payment = new Payment { Id = "paymentId1", MerchantId = "merchantId1" };
        _paymentRepository.GetPayment("paymentId1").Returns(Task.FromResult(payment));

        var response = await _sut.Process("secret1", "paymentId1");

        response.Error.Should().BeNull();
        response.Payment.Should().Be(payment);

        await _merchantClient.Received(1).GetMerchant("secret1");
        await _paymentRepository.Received(1).GetPayment("paymentId1");
    }
}
