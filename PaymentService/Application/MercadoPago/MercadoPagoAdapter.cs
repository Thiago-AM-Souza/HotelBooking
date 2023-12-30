using Application.MercadoPago.Exceptions;
using Application.Payment;
using Application.Payment.Dtos;
using Application.Payment.Ports;
using Application.Payment.Responses;

namespace Application.MercadoPago
{
    public class MercadoPagoAdapter : IPaymentProcessor
    {
        public Task<PaymentResponse> CapturePayment(string paymentIntention)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(paymentIntention))
                {
                    throw new InvalidPaymentIntentionException();
                }

                paymentIntention += "/success";

                var dto = new PaymentStateDto
                {
                    CreatedData = DateTime.Now,
                    Message = $"Successfully paid {paymentIntention}",
                    PaymentId = 123,
                    Status = Status.Success,
                };

                var response = new PaymentResponse()
                {
                    Success = true,
                    Data = dto,
                    Message = "Payment successfully processed",
                };

                return Task.FromResult(response);
            }
            catch (Exception)
            {
                var resp = new PaymentResponse()
                {
                    Success = false,
                    ErrorCode = ErrorCodes.PAYMENT_INVALID_PAYMENT_INTENTION,
                    Message = "The selected payment intention is invalid",
                };

                return Task.FromResult(resp);
            }
        }
    }
}
