using MySaaS.Domain.Entities;
using Xunit;

namespace MySaaS.UnitTests.Domain;

public class SubscriptionStatusTests
{
    [Fact]
    public void SubscriptionDefaultsToActive()
    {
        var subscription = new Subscription();

        Assert.Equal(SubscriptionStatus.Active, subscription.Status);
    }
}
