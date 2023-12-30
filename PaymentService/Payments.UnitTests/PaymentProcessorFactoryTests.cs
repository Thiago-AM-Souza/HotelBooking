using Application;
using Application.Booking.Dtos;
using Application.MercadoPago;
using NUnit.Framework;
using Payments.Application;

namespace Payments.UnitTests
{
    public class PaymentProcessorFactoryTests
    {
        [Test]
        public void ShouldReturn_NotImplementedPaymentProvider_WhenAskingForStripeProvider()
        {
            var factory = new PaymentProcessorFactory();

            var provider = factory.GetPaymentProcessor(SupportedPaymentProviders.Stripe);

            Assert.That(provider.GetType().Equals(typeof(NotImplementedPaymentProvider)));
        }

        [Test]
        public void ShouldReturn_MercadoPagoAdapter_Provider()
        {
            var factory = new PaymentProcessorFactory();

            var provider = factory.GetPaymentProcessor(SupportedPaymentProviders.MercadoPago);

            Assert.That(provider.GetType().Equals(typeof(MercadoPagoAdapter)));
        }

        public async Task ShouldReturnFalse_WhenCapturingPaymentFor_NotImplementedPaymentProvider()
        {
            var factory = new PaymentProcessorFactory();

            var provider = factory.GetPaymentProcessor(SupportedPaymentProviders.Stripe);

            var res = await provider.CapturePayment("myprovider.com/pay");

            Assert.False(res.Success);
            Assert.That(ErrorCodes.PAYMENT_PROVIDER_NOT_IMPLEMENTED.Equals(res.ErrorCode));
            Assert.That("The selected payment is not avaliable at the moment".Equals(res.Message));
        }
    }
}