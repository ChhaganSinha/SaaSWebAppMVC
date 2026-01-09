using Microsoft.EntityFrameworkCore;
using MySaaS.Application.Interfaces;
using MySaaS.Domain.Entities;
using MySaaS.Infrastructure.Data;

namespace MySaaS.Infrastructure.Repositories;

public class TenantRepository(SaaSAppDbContext dbContext) : ITenantRepository
{
    public Task<List<Tenant>> GetAllAsync(CancellationToken cancellationToken = default)
        => dbContext.Tenants.AsNoTracking().OrderBy(t => t.Name).ToListAsync(cancellationToken);

    public Task<Tenant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => dbContext.Tenants.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

    public async Task AddAsync(Tenant tenant, CancellationToken cancellationToken = default)
    {
        dbContext.Tenants.Add(tenant);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Tenant tenant, CancellationToken cancellationToken = default)
    {
        dbContext.Tenants.Update(tenant);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Tenant tenant, CancellationToken cancellationToken = default)
    {
        dbContext.Tenants.Remove(tenant);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
