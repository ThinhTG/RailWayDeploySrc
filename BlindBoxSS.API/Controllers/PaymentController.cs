using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using Net.payOS;
using Services.Payment;
using Services.Request;

namespace BlindBoxSS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // Update the CreatePaymentLink method to use the correct type
        [HttpPost("createPayment")]
        public async Task<IActionResult> CreatePaymentLink([FromBody] CreatePaymentLinkRequest body)
        {
            if (body == null)
            {
                return BadRequest(new Response(-1, "Request body is null", null));
            }

            try
            {
                var createPayment = await _paymentService.CreatePaymentLinkAsync(body);
                return Ok(new Response(0, "success", createPayment));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return Ok(new Response(-1, "fail", null));
            }
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder([FromRoute] int orderId)
        {
            try
            {
                var paymentLinkInformation = await _paymentService.GetPaymentLinkInformationAsync(orderId);
                return Ok(new Response(0, "Ok", paymentLinkInformation));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return Ok(new Response(-1, "fail", null));
            }
        }

        [HttpPost("confirm-webhook")]
        public async Task<IActionResult> ConfirmWebhook(ConfirmWebhook body)
        {
            try
            {
                await _paymentService.ConfirmWebhookAsync(body.webhook_url);
                return Ok(new Response(0, "Ok", null));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return Ok(new Response(-1, "fail", null));
            }
        }

        [HttpPost("payos_transfer_handler")]
        public IActionResult PayOSTransferHandler(WebhookType body)
        {
            try
            {
                var data = _paymentService.VerifyPaymentWebhookData(body);

                if (data.description == "Ma giao dich thu nghiem" || data.description == "BlindBoxQR123")
                {
                    return Ok(new Response(0, "Ok", null));
                }
                return Ok(new Response(0, "Ok", null));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Ok(new Response(-1, "fail", null));
            }
        }
    }
}