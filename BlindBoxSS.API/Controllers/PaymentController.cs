using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using Org.BouncyCastle.Bcpg;
using Services.Cache;
using Services.OrderS;
using Services.Payment;
using Services.Request;

[Route("api/payments")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly IResponseCacheService _responseCacheService;
    private readonly IOrderService _orderService;

    public PaymentController(IPaymentService paymentService, IResponseCacheService responseCacheService,IOrderService orderService)
    {
        _paymentService = paymentService;
        _responseCacheService = responseCacheService;
        _orderService = orderService;
    }

    // Tạo link thanh toán
    [HttpPost("createPayment")]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentLinkRequest body)
    {
        if (body == null) return BadRequest(new Response(-1, "Request body is null", null));
        var order = await _orderService.GetByIdAsync(body.orderId);
        var userId = order.AccountId;  // lấy accountId từ order

        try
        {
            var paymentLink = await _paymentService.CreatePaymentLinkAsync(body);
            await _responseCacheService.RemoveCacheResponseAsync($"/cart-management/managed-carts/{userId}");  // xóa Cache Cart khi tạo link thanh toán thành công
            return Ok(new Response(0, "Success", paymentLink));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new Response(-1, "Internal Server Error", null));
        }
    }

    [HttpPost("createDeposit")]
    public async Task<IActionResult> CreateDeposit([FromBody] CreatePaymentLinkRequestV2 body)
    {
        if (body == null) return BadRequest(new Response(-1, "Request body is null", null));

        try
        {
            var paymentLink = await _paymentService.CreatePaymentLinkDepositAsync(body);
          
            return Ok(new Response(0, "Success", paymentLink));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new Response(-1, "Internal Server Error", null));
        }
    }

    // Tạo link thanh toán
    [HttpPost("createPaymentMB")]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentLinkRequestMB body)
    {
        if (body == null) return BadRequest(new Response(-1, "Request body is null", null));

        try
        {
            var paymentLink = await _paymentService.CreatePaymentLinkMBAsync(body);
            return Ok(new Response(0, "Success", paymentLink));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new Response(-1, "Internal Server Error", null));
        }
    }

    [HttpPost("createDepositMB")]
    public async Task<IActionResult> CreateDeposit([FromBody] CreatePaymentLinkRequestMBV2 body)
    {
        if (body == null) return BadRequest(new Response(-1, "Request body is null", null));

        try
        {
            var paymentLink = await _paymentService.CreatePaymentLinkDepositMBAsync(body);
            return Ok(new Response(0, "Success", paymentLink));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new Response(-1, "Internal Server Error", null));
        }
    }

    // Lấy thông tin thanh toán theo orderCode
    [HttpGet("{orderCode}")]
    public async Task<IActionResult> GetPayment(int orderCode)
    {
        try
        {
            var paymentInfo = await _paymentService.GetPaymentLinkInformationAsync(orderCode);
            return Ok(new Response(0, "Success", paymentInfo));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new Response(-1, "Internal Server Error", null));
        }
    }

    //// Xác nhận Webhook
    //[HttpPost("webhook/confirm")]
    //public async Task<IActionResult> ConfirmWebhook([FromBody] ConfirmWebhook body)
    //{
    //    try
    //    {
    //        await _paymentService.ConfirmWebhookAsync(body.webhook_url);
    //        return Ok(new Response(0, "Success", null));
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine(ex);
    //        return StatusCode(500, new Response(-1, "Internal Server Error", null));
    //    }
    //}

    //// Xử lý Webhook từ PayOS
    //[HttpPost("webhook/handle-transfer")]
    //public IActionResult HandlePayOSTransfer([FromBody] WebhookType body)
    //{
    //    try
    //    {
    //        var data = _paymentService.VerifyPaymentWebhookData(body);

    //        if (data.description == "Ma giao dich thu nghiem" || data.description == "BlindBoxQR123")
    //        {
    //            return Ok(new Response(0, "Success", null));
    //        }

    //        return Ok(new Response(0, "Success", null));
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine(ex);
    //        return StatusCode(500, new Response(-1, "Internal Server Error", null));
    //    }
    //}
}
