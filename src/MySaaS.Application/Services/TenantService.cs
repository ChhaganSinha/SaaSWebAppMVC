using MySaaS.Application.Interfaces;
using MySaaS.Domain.Entities;

namespace MySaaS.Application.Services;

public class TenantService(ITenantRepository tenantRepository) : ITenantService
{
    public Task<List<Tenant>> GetAllAsync(CancellationToken cancellationToken = default)
        => tenantRepository.GetAllAsync(cancellationToken);

    public Task<Tenant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => tenantRepository.GetByIdAsync(id, cancellationToken);

    public async Task<Tenant> CreateAsync(Tenant tenant, CancellationToken cancellationToken = default)
    {
        tenant.UpdatedAt = DateTimeOffset.UtcNow;
        await tenantRepository.AddAsync(tenant, cancellationToken);
        return tenant;
    }

    public async Task UpdateAsync(Tenant tenant, CancellationToken cancellationToken = default)
    {
        tenant.UpdatedAt = DateTimeOffset.UtcNow;
        await tenantRepository.UpdateAsync(tenant, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var tenant = await tenantRepository.GetByIdAsync(id, cancellationToken);
        if (tenant is null)
        {
            return;
        }

        await tenantRepository.DeleteAsync(tenant, cancellationToken);
    }
}
