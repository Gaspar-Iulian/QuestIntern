using Ecommerce.Api.Models;
using Ecommerce.Api.Repositories;
using Ecommerce.Api.Requests;
using Ecommerce.Api.Services;

namespace Ecommerce.Api.Tests;

public sealed class CheckoutServiceTests
{
    [Fact]
    public async Task PlaceOrderAsync_CalculatesTotalFromProductRepositoryPrices()
    {
        var productRepository = new FakeProductRepository([
            new Product
            {
                Id = 1,
                Name = "Blue Top",
                Description = "Women blue cotton top.",
                Price = 29.99m,
                ImageUrl = "image-1",
                StockQuantity = 10
            },
            new Product
            {
                Id = 2,
                Name = "Men Tshirt",
                Description = "Comfortable casual tshirt.",
                Price = 19.99m,
                ImageUrl = "image-2",
                StockQuantity = 10
            }
        ]);
        var orderRepository = new FakeOrderRepository();
        var checkoutService = new CheckoutService(productRepository, orderRepository);

        var (response, errors) = await checkoutService.PlaceOrderAsync(
            new CheckoutRequest
            {
                UserId = 7,
                ShippingFullName = " Demo User ",
                ShippingAddressLine1 = " Test Street 10 ",
                ShippingCity = " Bucharest ",
                ShippingPostalCode = " 010101 ",
                ShippingCountry = " Romania ",
                Items =
                [
                    new CheckoutItemRequest { ProductId = 1, Quantity = 2 },
                    new CheckoutItemRequest { ProductId = 2, Quantity = 1 }
                ]
            },
            CancellationToken.None);

        Assert.Empty(errors);
        Assert.NotNull(response);
        Assert.Equal(79.97m, response.TotalAmount);
        Assert.Equal(79.97m, orderRepository.CreatedOrder?.TotalAmount);
        Assert.Equal("Demo User", orderRepository.CreatedOrder?.ShippingFullName);
        Assert.Collection(
            orderRepository.CreatedOrder!.Items,
            item =>
            {
                Assert.Equal(1, item.ProductId);
                Assert.Equal(2, item.Quantity);
                Assert.Equal(29.99m, item.UnitPrice);
                Assert.Equal(59.98m, item.LineTotal);
            },
            item =>
            {
                Assert.Equal(2, item.ProductId);
                Assert.Equal(1, item.Quantity);
                Assert.Equal(19.99m, item.UnitPrice);
                Assert.Equal(19.99m, item.LineTotal);
            });
    }

    [Fact]
    public async Task PlaceOrderAsync_WhenQuantityExceedsStock_ReturnsErrorAndDoesNotCreateOrder()
    {
        var productRepository = new FakeProductRepository([
            new Product
            {
                Id = 1,
                Name = "Blue Top",
                Description = "Women blue cotton top.",
                Price = 29.99m,
                ImageUrl = "image-1",
                StockQuantity = 1
            }
        ]);
        var orderRepository = new FakeOrderRepository();
        var checkoutService = new CheckoutService(productRepository, orderRepository);

        var (response, errors) = await checkoutService.PlaceOrderAsync(
            CreateRequest([new CheckoutItemRequest { ProductId = 1, Quantity = 2 }]),
            CancellationToken.None);

        Assert.Null(response);
        Assert.Contains("Product 'Blue Top' has only 1 items in stock.", errors);
        Assert.Null(orderRepository.CreatedOrder);
    }

    [Fact]
    public async Task PlaceOrderAsync_WhenProductDoesNotExist_ReturnsErrorAndDoesNotCreateOrder()
    {
        var productRepository = new FakeProductRepository([]);
        var orderRepository = new FakeOrderRepository();
        var checkoutService = new CheckoutService(productRepository, orderRepository);

        var (response, errors) = await checkoutService.PlaceOrderAsync(
            CreateRequest([new CheckoutItemRequest { ProductId = 99, Quantity = 1 }]),
            CancellationToken.None);

        Assert.Null(response);
        Assert.Contains("Product 99 was not found.", errors);
        Assert.Null(orderRepository.CreatedOrder);
    }

    private static CheckoutRequest CreateRequest(IReadOnlyList<CheckoutItemRequest> items)
    {
        return new CheckoutRequest
        {
            ShippingFullName = "Demo User",
            ShippingAddressLine1 = "Test Street 10",
            ShippingCity = "Bucharest",
            ShippingPostalCode = "010101",
            ShippingCountry = "Romania",
            Items = items
        };
    }

    private sealed class FakeProductRepository(IReadOnlyList<Product> products) : IProductRepository
    {
        public Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(products);
        }

        public Task<IReadOnlyList<Product>> GetByIdsAsync(IReadOnlyCollection<int> productIds, CancellationToken cancellationToken)
        {
            var selectedProducts = products
                .Where(product => productIds.Contains(product.Id))
                .ToList();

            return Task.FromResult<IReadOnlyList<Product>>(selectedProducts);
        }
    }

    private sealed class FakeOrderRepository : IOrderRepository
    {
        public OrderDraft? CreatedOrder { get; private set; }

        public Task<OrderCreated> CreateAsync(OrderDraft order, CancellationToken cancellationToken)
        {
            CreatedOrder = order;

            return Task.FromResult(new OrderCreated
            {
                Id = 123,
                TotalAmount = order.TotalAmount,
                CreatedAtUtc = new DateTime(2026, 4, 21, 12, 0, 0, DateTimeKind.Utc)
            });
        }
    }
}
