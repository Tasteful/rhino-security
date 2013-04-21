using System;
using System.Collections.Generic;
using System.Reflection;
using Rhino.Security.Impl.Linq;
using NHibernate.Cfg;
using NHibernate.Event;
using Rhino.Security.Impl;
using Rhino.Security.Impl.MappingRewriting;
using Rhino.Security.Interfaces;

namespace Rhino.Security
{
	/// <summary>
	/// This class allows to configure the security system
	/// </summary>
	public class Security
	{
		private static readonly MethodInfo getSecurityKeyMethod = typeof (Security).GetMethod(
			"GetSecurityKeyPropertyInternal", BindingFlags.NonPublic | BindingFlags.Static);

		private static readonly Dictionary<Type, Func<string>> GetSecurityKeyPropertyCache =
			new Dictionary<Type, Func<string>>();

		/// <summary>
		/// Extracts the key from the specified entity.
		/// </summary>
		/// <typeparam name="TEntity">The type of the entity.</typeparam>
		/// <param name="entity">The entity.</param>
		/// <returns></returns>
		public static Guid ExtractKey<TEntity>(TEntity entity)
			where TEntity : class
		{
			Guard.Against<ArgumentNullException>(entity == null, "entity");

			var extractor = DependencyResolver.GetInstance<IEntityInformationExtractor<TEntity>>();
			return extractor.GetSecurityKeyFor(entity);
		}

		/// <summary>
		/// Extracts the key from the specified entity using the given object.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>If no IEntityInformationExtractor{TEntity} class is registered in the IoC container
		/// for the entity's runtime type this returns an empty Guid, otherwise the security
		/// key of the given entity.</returns>
		/// <remarks>It is recommended to use the ExtractKey{TEntity}(TEntity) method if possible 
		/// as this code uses reflection to extract the entity security key.</remarks>
		internal static Guid ExtractKey(object entity) {
			Guard.Against<ArgumentNullException>(entity == null, "entity");
			Type[] entityType = { entity.GetType() };
			Guard.Against<ArgumentException>(!entityType[0].IsClass, "Entity must be a class object");

			Type extractorType = typeof(IEntityInformationExtractor<>);
			Type genericExtractor = extractorType.MakeGenericType(entityType);
			object extractor;

			try {
                extractor = DependencyResolver.GetInstance(genericExtractor);
			}
			catch (DependencyResolverException) {
				// If no IEntityInformationExtractor is registered then the entity isn't 
				// secured by Rhino.Security. Ignore the error and return an empty Guid.
				return Guid.Empty;
			}

			object key = genericExtractor.InvokeMember("GetSecurityKeyFor", BindingFlags.InvokeMethod, null, extractor, new object[] { entity });
			
			return (Guid)key;
		}

		/// <summary>
		/// Gets a human readable description for the specified entity
		/// </summary>
		/// <typeparam name="TEntity">The type of the entity.</typeparam>
		/// <param name="entity">The entity.</param>
		/// <returns></returns>
		public static string GetDescription<TEntity>(TEntity entity) where TEntity : class
		{
            IEntityInformationExtractor<TEntity> extractor = DependencyResolver.GetInstance<IEntityInformationExtractor<TEntity>>();
			return extractor.GetDescription(ExtractKey(entity));
		}

		/// <summary>
		/// Gets the security key member info.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns><see cref="MemberInfo"/> for the entities security key.</returns>
		public static MemberInfo GetSecurityKeyMemberInfo(Type type)
		{
			return type.GetProperty(GetSecurityKeyProperty(type));
		}

		/// <summary>
		/// Gets the security key property for the specified entity type
		/// </summary>
		/// <param name="entityType">Type of the entity.</param>
		/// <returns></returns>
		public static string GetSecurityKeyProperty(Type entityType)
		{
			lock (GetSecurityKeyPropertyCache)
			{
				Func<string> func;
				if (GetSecurityKeyPropertyCache.TryGetValue(entityType, out func))
					return func();
				func = (Func<string>)
				       Delegate.CreateDelegate(typeof (Func<string>),getSecurityKeyMethod.MakeGenericMethod(entityType));
				GetSecurityKeyPropertyCache[entityType] = func;
				return func();
			}
		}

		internal static string GetSecurityKeyPropertyInternal<TEntity>()
		{
            return DependencyResolver.GetInstance<IEntityInformationExtractor<TEntity>>().SecurityKeyPropertyName;
		}

		///<summary>
		/// Setup NHibernate to include Rhino Security configuration
		///</summary>
		public static void Configure<TUserType>(Configuration cfg, SecurityTableStructure securityTableStructure)
			where TUserType : IUser 
		{
			cfg.AddAssembly(typeof (IUser).Assembly);
			new SchemaChanger(cfg, securityTableStructure).Change();
			new UserMapper(cfg, typeof(TUserType)).Map();
			SecurityLinqToHqlGeneratorsRegistry.SetLinqToHqlGeneratorsRegistry(cfg);
			cfg.SetListener(ListenerType.PreDelete, new DeleteEntityEventListener());
		}
	}
}