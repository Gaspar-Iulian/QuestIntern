using Ecommerce.Api.Models;
using Ecommerce.Api.Repositories;
using Ecommerce.Api.Requests;
using Ecommerce.Api.Responses;

namespace Ecommerce.Api.Services;

public sealed class CheckoutService(
    IProductRepository productRepository,
    IOrderRepository orderRepository) : ICheckoutService
{
    public async Task<(CheckoutResponse? Response, IReadOnlyList<string> Errors)> PlaceOrderAsync(
        CheckoutRequest request,
        CancellationToken cancellationToken)
    {
        var errors = ValidateRequest(request);

        if (errors.Count > 0)
        {
            return (null, errors);
        }

        var requestedItems = request.Items
            .GroupBy(item => item.ProductId)
            .Select(group => new CheckoutItemRequest
            {
                ProductId = group.Key,
                Quantity = group.Sum(item => item.Quantity)
            })
            .ToList();

        var productIds = requestedItems.Select(item => item.ProductId).ToArray();
        var products = await productRepository.GetByIdsAsync(productIds, cancellationToken);
        var productsById = products.ToDictionary(product => product.Id);

        var orderItems = new List<OrderItemDraft>();

        foreach (var requestedItem in requestedItems)
        {
            if (!productsById.TryGetValue(requestedItem.ProductId, out var product))
            {
                errors.Add($"Product {requestedItem.ProductId} was not found.");
                continue;
            }

            if (requestedItem.Quantity > product.StockQuantity)
            {
                errors.Add($"Product '{product.Name}' has only {product.StockQuantity} items in stock.");
                continue;
            }

            orderItems.Add(new OrderItemDraft
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Quantity = requestedItem.Quantity,
                UnitPrice = product.Price
            });
        }

        if (errors.Count > 0)
        {
            return (null, errors);
        }

        var totalAmount = orderItems.Sum(item => item.LineTotal);
        var orderDraft = new OrderDraft
        {
            UserId = request.UserId,
            ShippingFullName = request.ShippingFullName.Trim(),
            ShippingAddressLine1 = request.ShippingAddressLine1.Trim(),
            ShippingCity = request.ShippingCity.Trim(),
            ShippingPostalCode = request.ShippingPostalCode.Trim(),
            ShippingCountry = request.ShippingCountry.Trim(),
            TotalAmount = totalAmount,
            Items = orderItems
        };

        var createdOrder = await orderRepository.CreateAsync(orderDraft, cancellationToken);

        return (new CheckoutResponse
        {
            OrderId = createdOrder.Id,
            TotalAmount = createdOrder.TotalAmount,
            CreatedAtUtc = createdOrder.CreatedAtUtc,
            Message = "Order placed successfully.",
            Items = orderItems.Select(item => new CheckoutItemResponse
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                LineTotal = item.LineTotal
            }).ToList()
        }, []);
    }

    private static List<string> ValidateRequest(CheckoutRequest request)
    {
        var errors = new List<string>();

        if (request.Items.Count == 0)
        {
            errors.Add("Cart must contain at least one item.");
        }

        foreach (var item in request.Items)
        {
            if (item.ProductId <= 0)
            {
                errors.Add("Every item must have a valid product id.");
            }

            if (item.Quantity <= 0)
            {
                errors.Add("Every item must have a quantity greater than zero.");
            }
        }

        return errors;
    }
}
