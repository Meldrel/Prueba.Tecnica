using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prueba.Tecnica.Domain.Event.Args;
using Prueba.Tecnica.Domain.IRepository;
using Serilog;
using System.Diagnostics.CodeAnalysis;

namespace Prueba.Tecnica.Infrastructure.Background
{
    [ExcludeFromCodeCoverage]
    public class ExpirationDateBackgroundService : BackgroundService
    {
        public event EventHandler<ItemExpiredEventArgs>? ItemExpired;

        private readonly ILogger logger;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly PeriodicTimer timer = new(TimeSpan.FromSeconds(30));

        /// <summary>
        /// Background service que comprueba periódicamente (cada 30 segundos) si ha expirado algún item.
        /// </summary>
        /// <param name="logger">Serilog para trazas</param>
        /// <param name="serviceScopeFactory">Scope factory para usar el RepositoryItem cuando sea necesario</param>
        public ExpirationDateBackgroundService(ILogger logger,
                                               IServiceScopeFactory serviceScopeFactory)
        {
            this.logger = logger.ForContext("Method", "ExpirationDateBackgroundService");
            this.serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.Information($"Execute ExpirationDateBackgroundService: {DateTime.UtcNow}");
            while (!stoppingToken.IsCancellationRequested
                && await timer.WaitForNextTickAsync(stoppingToken))
            {
                await CheckExpirationDate();
            }

            logger.Information($"End ExpirationDateBackgroundService");
        }

        /// <summary>
        /// Hace una consulta a BBDD para comprobar todos los artículos cuya fecha de expiración sea mayor que la actual
        /// Luego recorre aquellos que cumplen la condición, y lanza un evento. El evento se puede lanzar sólo una vez como una lista, en vez de forma individual por cada articulo. 
        /// </summary>
        /// <returns></returns>
        private async Task CheckExpirationDate()
        {
            using var scope = serviceScopeFactory.CreateScope();
            var itemRepository = scope.ServiceProvider.GetRequiredService<IItemRepository>();

            var items = await itemRepository
                              .GetAllItems()
                              .Where(x => x.ExpirationDate <= DateTime.UtcNow)
                              .ToListAsync();


            logger.Information($"Total Item expired: {items.Count}");

            foreach (var item in items)
                ItemExpired?.Invoke(this, new ItemExpiredEventArgs(item.Id, item.Name, item.ExpirationDate));
        }
    }
}
