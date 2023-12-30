using Application;
using Application.Booking.Dtos;
using Application.MercadoPago;
using NuGet.Frameworks;
using NUnit.Framework;
using Payments.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.UnitTests
{
    public class MercadoPagoTests
    {
        [Test]
        public void ShouldReturn_MercadoPagoAdapter_Provider()
        {
            var factory = new PaymentProcessorFactory();

            var provider = factory.GetPaymentProcessor(SupportedPaymentProviders.MercadoPago);

            Assert.That(provider.GetType().Equals(typeof(MercadoPagoAdapter)));
        }

        [Test]
        public async Task Should_FailWhenPaymentIntentionStringIsInvalid()
        {
            var factory = new PaymentProcessorFactory();

            var provider = factory.GetPaymentProcessor(SupportedPaymentProviders.MercadoPago);

            var res = await provider.CapturePayment("");

            Assert.False(res.Success);
            Assert.That(ErrorCodes.PAYMENT_INVALID_PAYMENT_INTENTION.Equals(res.ErrorCode));
            Assert.That("The selected payment intention is invalid".Equals(res.Message));
        }

        [Test]
        public async Task Should_SuccessfullyProcessPayment()
        {
            var factory = new PaymentProcessorFactory();

            var provider = factory.GetPaymentProcessor(SupportedPaymentProviders.MercadoPago);

            var res = await provider.CapturePayment("https://mercadopago/pay");

            Assert.IsTrue(res.Success);
            Assert.That("Payment successfully processed".Equals(res.Message));
            Assert.NotNull(res.Data);
            Assert.NotNull(res.Data.CreatedData);
            Assert.NotNull(res.Data.PaymentId);
        }
    }
}
