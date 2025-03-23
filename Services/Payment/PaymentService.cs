using DAO.Contracts;
using Net.payOS;
using Net.payOS.Types;
using Services.AccountService;
using Services.OrderS;
using Services.Payment;
using Services.Request;
public class PaymentService : IPaymentService
{
    private readonly PayOS _payOS;
    private readonly IOrderService _orderSV;
    private readonly IAccountService _accountSV;

    public PaymentService(PayOS payOS, IOrderService orderService, IAccountService accountService)
    {
        _payOS = payOS;
        _orderSV = orderService;
        _accountSV = accountService;
    }

    public async Task<CreatePaymentResult> CreatePaymentLinkAsync(CreatePaymentLinkRequest request)
    {
        var order = await _orderSV.GetByIdAsync(request.orderId);
        if (order.OrderCode != null)
        {
            var checkOrderCode = long.Parse(order.OrderCode.ToString());
            var checking = await _payOS.getPaymentLinkInformation(checkOrderCode);
            if (checking.status == "CANCELLED" || checking.status == "PENDING")
            {
                order.OrderCode = null;
                await _orderSV.UpdateAsync(order);
            }

            if (checking.status == "PAID")
            {
                order.PaymentConfirmed = true;
                await _orderSV.UpdateAsync(order);
                throw new Exception("Order has been paid");
            }

            if (checking.status == "PROCESSING")
            {
                throw new Exception("Order is processing");
            }
        }
        int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
        ItemData item = new ItemData(request.orderId.ToString(), 1, request.price);
        var descriptions = request.description = $"Payment {request.orderId}";
        List<ItemData> items = new List<ItemData> { item };
        var expiredAt = DateTimeOffset.Now.AddMinutes(15).ToUnixTimeSeconds();
        PaymentData paymentDataPayment = new PaymentData(orderCode, request.price, descriptions, items, request.cancelUrl, request.returnUrl, null, null, null, null, null, expiredAt);
        try
        {
            var createdLink = await _payOS.createPaymentLink(paymentDataPayment);

            order.OrderCode = orderCode; 
            await _orderSV.UpdateAsync(order);
            return createdLink;

        }
        catch (Exception ex)
        {
            throw new Exception();
        }


    }

    public async Task<CreatePaymentResult> CreatePaymentLinkDepositAsync(CreatePaymentLinkRequestV2 request)
    {
        int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));

        var account = await _accountSV.GetByIdAsync(request.accountId);
        if (account.orderCode != null)
        {
            var checkOrderCode = long.Parse(account.orderCode.ToString());
            var checking = await _payOS.getPaymentLinkInformation(checkOrderCode);
            if (checking.status == "CANCELLED" || checking.status == "PENDING")
            {
                var newacount = new UserRequestAndResponse.UpdateOrderCodeRequest
                {
                    orderCode = null
                };
                var parseID = Guid.Parse(request.accountId);
                await _accountSV.UpdateAsync(parseID, newacount);
            }
            if (checking.status == "PAID")
            {
                var newacount = new UserRequestAndResponse.UpdateOrderCodeRequest
                {
                    orderCode = null
                };
                var parseID = Guid.Parse(request.accountId);
                await _accountSV.UpdateAsync(parseID, newacount);
                throw new Exception("Deposit has been paid");
            }
            if (checking.status == "PROCESSING")
            {
                throw new Exception("Deposit is processing");
            }
        }

        ItemData item = new ItemData(request.accountId, 1, request.price);
        var descriptions = request.description = $"Deposit {request.price}";
        List<ItemData> items = new List<ItemData> { item };
        var expiredAt = DateTimeOffset.Now.AddMinutes(15).ToUnixTimeSeconds();
        PaymentData paymentDataPayment = new PaymentData(orderCode, request.price, descriptions, items, request.cancelUrl, request.returnUrl, null, null, null, null, null, expiredAt);
        try
        {
            var createdLink = await _payOS.createPaymentLink(paymentDataPayment);

            account.orderCode = orderCode;
            var newacount = new UserRequestAndResponse.UpdateOrderCodeRequest
            {
                orderCode = orderCode
            };
            var parseID = Guid.Parse(request.accountId);
            await _accountSV.UpdateAsync(parseID, newacount);
            return createdLink;

        }
        catch (Exception ex)
        {
            throw new Exception();
        }

    }

    public async Task<CreatePaymentResult> CreatePaymentLinkMBAsync(CreatePaymentLinkRequestMB request)
    {
        var order = await _orderSV.GetByIdAsync(request.orderId);
        if (order.OrderCode != null)
        {
            var checkOrderCode = long.Parse(order.OrderCode.ToString());
            var checking = await _payOS.getPaymentLinkInformation(checkOrderCode);
            if (checking.status == "CANCELLED" || checking.status == "PENDING")
            {
                order.OrderCode = null;
                await _orderSV.UpdateAsync(order);
            }

            if (checking.status == "PAID")
            {
                order.PaymentConfirmed = true;
                await _orderSV.UpdateAsync(order);
                throw new Exception("Order has been paid");
            }

            if (checking.status == "PROCESSING")
            {
                throw new Exception("Order is processing");
            }
        }
        int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
        ItemData item = new ItemData(request.orderId.ToString(), 1, request.price);
        var descriptions = request.description = $"Payment {request.orderId}";
        List<ItemData> items = new List<ItemData> { item };
        var expiredAt = DateTimeOffset.Now.AddMinutes(15).ToUnixTimeSeconds();
        PaymentData paymentDataPayment = new PaymentData(orderCode, request.price, descriptions, items, request.cancelUrl, request.returnUrl, null, null, null, null, null, expiredAt);
        try
        {
            var createdLink = await _payOS.createPaymentLink(paymentDataPayment);

            order.OrderCode = orderCode;
            await _orderSV.UpdateAsync(order);
            return createdLink;

        }
        catch (Exception ex)
        {
            throw new Exception();
        }


    }

    public async Task<CreatePaymentResult> CreatePaymentLinkDepositMBAsync(CreatePaymentLinkRequestMBV2 request)
    {
        int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));

        var parseID = Guid.Parse(request.accountId);
        var account = await _accountSV.GetByIdAsync(request.accountId);
        if (account.orderCode != null)
        {
            var checkOrderCode = long.Parse(account.orderCode.ToString());
            var checking = await _payOS.getPaymentLinkInformation(checkOrderCode);
            if (checking.status == "CANCELLED" || checking.status == "PENDING")
            {
                var newacount = new UserRequestAndResponse.UpdateOrderCodeRequest
                {
                    orderCode = null
                };
                await _accountSV.UpdateAsync(parseID, newacount);
            }
            if (checking.status == "PAID")
            {
                var newacount = new UserRequestAndResponse.UpdateOrderCodeRequest
                {
                    orderCode = null
                };
                await _accountSV.UpdateAsync(parseID, newacount);
                throw new Exception("Deposit has been paid");
            }
            if (checking.status == "PROCESSING")
            {
                throw new Exception("Deposit is processing");
            }
        }

        ItemData item = new ItemData(request.accountId, 1, request.price);
        var descriptions = request.description = $"Deposit {request.price}";
        List<ItemData> items = new List<ItemData> { item };
        var expiredAt = DateTimeOffset.Now.AddMinutes(15).ToUnixTimeSeconds();
        PaymentData paymentDataPayment = new PaymentData(orderCode, request.price, descriptions, items, request.cancelUrl, request.returnUrl, null, null, null, null, null, expiredAt);
        try
        {
            var createdLink = await _payOS.createPaymentLink(paymentDataPayment);

            account.orderCode = orderCode;
            var newacount = new UserRequestAndResponse.UpdateOrderCodeRequest
            {
                orderCode = orderCode
            };
            await _accountSV.UpdateAsync(parseID, newacount);
            return createdLink;

        }
        catch (Exception ex)
        {
            throw new Exception();
        }

    }

    public async Task<PaymentLinkInformation> GetPaymentLinkInformationAsync(int orderCode)
    {
        var checkOrderCode = long.Parse(orderCode.ToString());
        return await _payOS.getPaymentLinkInformation(checkOrderCode);
    }

    public async Task ConfirmWebhookAsync(string webhookUrl)
    {
        await _payOS.confirmWebhook(webhookUrl);
    }

    public WebhookData VerifyPaymentWebhookData(WebhookType webhookType)
    {
        return _payOS.verifyPaymentWebhookData(webhookType);
    }
}