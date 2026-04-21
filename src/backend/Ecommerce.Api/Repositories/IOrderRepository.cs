using Ecommerce.Api.Models;

namespace Ecommerce.Api.Repositories;

public interface IOrderRepository
{
    Task<OrderCreated> CreateAsync(OrderDraft order, CancellationToken cancellationToken);
}
