using Microsoft.EntityFrameworkCore;
using MySaaS.Domain.Entities;
using MySaaS.Infrastructure.Data;
using MySaaS.Infrastructure.Repositories;
using Xunit;

namespace MySaaS.IntegrationTests.Data;

public class TenantRepositoryTests
{
    [Fact]
    public async Task AddAsync_PersistsTenant()
    {
        var options = new DbContextOptionsBuilder<SaaSAppDbContext>()
            .UseInMemoryDatabase(databaseName: "TenantRepositoryTests")
            .Options;

        await using var dbContext = new SaaSAppDbContext(options);
        var repository = new TenantRepository(dbContext);

        var tenant = new Tenant
        {
            Name = "Northwind",
            Slug = "northwind",
            ContactEmail = "ops@northwind.example",
            IsActive = true
        };

        await repository.AddAsync(tenant);
        var stored = await repository.GetByIdAsync(tenant.Id);

        Assert.NotNull(stored);
        Assert.Equal("Northwind", stored!.Name);
    }
}
