using System;

namespace Rhino.Security
{
    /// <summary>
    /// Dependency resolver is used to resolve dependencies
    /// </summary>
    public static class DependencyResolver
    {
        private static Func<Type, object> _instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>``0.</returns>
        public static T GetInstance<T>()
        {
            return (T)GetInstance(typeof (T));
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="System.Exception">DependencyResolver is not initiated.</exception>
        public static object GetInstance(Type type)
        {
            if (_instance == null)
            {
                throw new Exception("DependencyResolver is not initiated.");
            }
            return _instance(type);
        }

        /// <summary>
        /// Sets the dependency resolver.
        /// </summary>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        public static void SetDependencyResolver(Func<Type, object> dependencyResolver)
        {
            _instance = dependencyResolver;
        }
    }
}