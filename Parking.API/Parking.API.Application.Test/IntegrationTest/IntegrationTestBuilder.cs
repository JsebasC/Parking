using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Security.Cryptography;

namespace Parking.API.Application.Test.IntegrationTest
{
    public class IntegrationTestBuilder : WebApplicationFactory<Program>
    {
        readonly Guid _id;

        public Guid Id => _id;

        /// <summary>
        /// Permite configurar el web host, Default :Development , Release : Production
        /// </summary>
        /// <param name="builder"></param>
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            base.ConfigureWebHost(builder);
        }

        /// <summary>
        ///  Al crear el Host podemos especificar dependencias distintas a las que se usarían en el proyecto en producción
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Configurar cualquier Mock o similares
            });

            return base.CreateHost(builder);
        }

        public IntegrationTestBuilder()
        {
            _id = Guid.NewGuid();
        }
    }

}
