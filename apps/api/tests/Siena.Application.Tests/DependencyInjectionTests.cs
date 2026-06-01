using Microsoft.Extensions.DependencyInjection;
using Siena.Application;

namespace Siena.Application.Tests;

public sealed class DependencyInjectionTests
{
    [Fact]
    public void AddApplication_ReturnsTheSameServiceCollection()
    {
        var services = new ServiceCollection();

        var result = services.AddApplication();

        Assert.Same(services, result);
    }
}
