using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Runtime.Serialization;
using System.Reflection;
using Microsoft.Practices.Unity;

namespace Beauty.Web
{
    public class UnityDependencyResolver : UnityContainer, IDependencyResolver
    {
        public object GetService(Type serviceType)
        {
            //
            //  Unity "Resolve" method throws exception so we need to wrap it up with non throwing method (like MVC expects)
            //
            return TryGetService(serviceType);
        }

        public object TryGetService(Type serviceType)
        {
            try
            {
   
                return this.Resolve(serviceType);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            //
            //  Unity "Resolve" method throws exception so we need to wrap it up with non throwing method (like MVC expects)
            //
            return TryGetServices(serviceType);
        }

        public IEnumerable<object> TryGetServices(Type serviceType)
        {
            try
            {
                return this.ResolveAll(serviceType);
            }
            catch (Exception)
            {
                return new object[0];
            }
        }
    }
}