using MySaaS.Domain.Entities;

namespace MySaaS.Application.Interfaces;

public interface ITenantRepository
{
    Task<List<Tenant>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Tenant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Tenant tenant, CancellationToken cancellationToken = default);
    Task UpdateAsync(Tenant tenant, CancellationToken cancellationToken = default);
    Task DeleteAsync(Tenant tenant, CancellationToken cancellationToken = default);
}
