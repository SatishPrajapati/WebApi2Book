using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using log4net.Config;
using NHibernate;
using Ninject;
using Ninject.Activation;
using Ninject.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi2Book.Common;
using WebApi2Book.Common.Logging;
using WebApi2Book.Data.SqlServer.Mapping;
using NHibernate.Context;

namespace WebApi2Book.Web.Api
{
    public class NinjectConfigurator
    {
        public void Configure(IKernel container)
        {
            AddBindings(container);
        }

        private void AddBindings(IKernel container)
        {
            ConfigureLog4Net(container);
            ConfigureNHibernate(container);
            container.Bind<IDateTime>().To<DateTimeAdapter>().InSingletonScope();
        }

        private void ConfigureLog4Net(IKernel container)
        {
            XmlConfigurator.Configure();
            var logManager = new LogManagerAdapter();
            container.Bind<ILogManager>().ToConstant(logManager);
        }
        private void ConfigureNHibernate(IKernel container)
        {
            var sessionFactory = Fluently.Configure()
                    .Database
                    (
                        MsSqlConfiguration.MsSql2012.ConnectionString
                            (
                                c => c.FromConnectionStringWithKey("WebApi2BookDb")
                            )
                    )
                    .CurrentSessionContext("web")
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<TaskMap>())
                    .BuildSessionFactory();
            container.Bind<ISessionFactory>().ToConstant(sessionFactory);

            container.Bind<ISession>().ToMethod(CreateSession).InRequestScope();
        }

        private ISession CreateSession(IContext context)
        {
            var sessionFactory = context.Kernel.Get<ISessionFactory>();
            if (CurrentSessionContext.HasBind(sessionFactory) == false)
            {
                var session = sessionFactory.OpenSession();
                CurrentSessionContext.Bind(session);
            }
            return sessionFactory.GetCurrentSession();
        }
    }
}





