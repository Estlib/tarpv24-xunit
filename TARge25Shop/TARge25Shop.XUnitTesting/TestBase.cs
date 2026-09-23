using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using TARge25Shop.ApplicationServices.Services;
using TARge25Shop.Controllers;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Data;

namespace TARge25Shop.XUnitTesting
{
    public abstract class TestBase
    {
        private readonly object InMemoryEventId;

        protected IServiceProvider serviceProvider { get; set; }

        protected TestBase()
        {
            var services = new ServiceCollection();
            SetupServices(services);
            serviceProvider = services.BuildServiceProvider();
        }
        /// <summary>
        /// Seame üles testide läbiviimiseks vajalikud kontrollerid mujalt projektist
        /// See meetod annab ka mälusoleva andmebaasi, mida testideks kasutada,
        /// VIPER-tüüpi projektis toimib kui "program.cs" analoog, ent lühidal kujul.
        /// </summary>
        /// <param name="services">kollektor kuhu asetame kontrolleri instantsid</param>
        public virtual void SetupServices(ServiceCollection services)
        {
            //services.AddScoped<ISpaceshipServices, SpaceshipServices>();
            //liidese-teenuse asemel, adresseeri kontrollerit:
            services.AddScoped<SpaceshipController>();
            services.AddScoped<IHostEnvironment, MockIHostEnvironment>();

            services.AddDbContext<TARge25ShopContext>
                (x =>
                {
                    x.UseInMemoryDatabase("TEST");
                    x.ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                });

            RegisterMacros(services);
        }
        public void Dispose()
        {

        }
        /// <summary>
        /// Leia üles kindel teenus, teenusepakkujalt.
        /// serviceProvider omab kontrollerite instantse, ning GetService hangib selle 
        /// X tüüpi kontrolleri
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T Svc<T>()
        {
            return serviceProvider.GetService<T>();
        }
        private void RegisterMacros(ServiceCollection services)
        {
            var macroBaseType = typeof(IMacros);

            var macros = macroBaseType.Assembly.GetTypes()
                .Where(t => macroBaseType.IsAssignableFrom(t)
                && !t.IsInterface && !t.IsAbstract);
            foreach (var macro in macros)
            {
                services.AddSingleton(macro);
            }
        }
    }
}
