﻿using Ninject;
using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

namespace WebApi2Book.Web.Common
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _container;
        public NinjectDependencyResolver(IKernel container)
        {
            _container = container;
        }
        public IKernel Container
        {
            get
            {
                return _container;
            }
        }
        public IDependencyScope BeginScope()
        {
            return this;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public object GetService(Type serviceType)
        {
            return Container.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Container.GetAll(serviceType);
        }
    }
}
