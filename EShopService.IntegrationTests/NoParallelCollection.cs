using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShopService_drugaedycja;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace EShopService.IntegrationTests
{
    [CollectionDefinition("NoParallel", DisableParallelization = true)]
    public class NoParallelCollection : ICollectionFixture<WebApplicationFactory<Program>>
    {

    }
}
