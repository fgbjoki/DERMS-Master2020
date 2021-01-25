using System.Collections.Generic;

namespace Common.ServiceLocator
{
    public class ServiceLocator
    {
        private static Dictionary<object, object> serviceContainer;

        static ServiceLocator()
        {
            serviceContainer = new Dictionary<object, object>();
        }

        public static T GetService<T>()
        {
            return (T)serviceContainer[typeof(T)];
        }

        public static void AddService(object service)
        {
            serviceContainer[service.GetType()] = service;
        }
    }
}
