using System;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Linq.Functions;
using NHibernate.Util;
using Environment = NHibernate.Cfg.Environment;

namespace Rhino.Security.Impl.Linq
{
	internal class SecurityLinqToHqlGeneratorsRegistry : ILinqToHqlGeneratorsRegistry
	{
		private static ILinqToHqlGeneratorsRegistry _inner;

		internal static void SetLinqToHqlGeneratorsRegistry(Configuration cfg)
		{
			string registry;
			if (cfg.Properties.TryGetValue(Environment.LinqToHqlGeneratorsRegistry, out registry))
			{
				try
				{
					_inner = (ILinqToHqlGeneratorsRegistry)Environment.BytecodeProvider.ObjectsFactory.CreateInstance(ReflectHelper.ClassForName(registry));
				}
				catch (Exception e)
				{
					throw new HibernateException("Could not instantiate LinqToHqlGeneratorsRegistry: " + registry, e);
				}
			}
			else
			{
				_inner = new DefaultLinqToHqlGeneratorsRegistry();
			}
			cfg.SetProperty(Environment.LinqToHqlGeneratorsRegistry, typeof(SecurityLinqToHqlGeneratorsRegistry).AssemblyQualifiedName);
		}

		public SecurityLinqToHqlGeneratorsRegistry()
		{
			this.Merge(new SecurityKeyExtractorGenerator());
		}

		bool ILinqToHqlGeneratorsRegistry.TryGetGenerator(MethodInfo method, out IHqlGeneratorForMethod generator)
		{
			return _inner.TryGetGenerator(method, out generator);
		}

		bool ILinqToHqlGeneratorsRegistry.TryGetGenerator(MemberInfo property, out IHqlGeneratorForProperty generator)
		{
			return _inner.TryGetGenerator(property, out generator);
		}

		void ILinqToHqlGeneratorsRegistry.RegisterGenerator(MethodInfo method, IHqlGeneratorForMethod generator)
		{
			_inner.RegisterGenerator(method, generator);
		}

		void ILinqToHqlGeneratorsRegistry.RegisterGenerator(MemberInfo property, IHqlGeneratorForProperty generator)
		{
			_inner.RegisterGenerator(property, generator);
		}

		void ILinqToHqlGeneratorsRegistry.RegisterGenerator(IRuntimeMethodHqlGenerator generator)
		{
			_inner.RegisterGenerator(generator);
		}
	}
}