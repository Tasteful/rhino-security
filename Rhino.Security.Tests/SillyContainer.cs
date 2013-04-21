using System;
using NHibernate;
using Rhino.Security.Interfaces;
using Rhino.Security.Services;

namespace Rhino.Security.Tests
{
    public static class SillyContainer
    {
        public static Func<ISession> SessionProvider;

    	public static object GetInstance(Type serviceType)
        {
            if (serviceType == typeof(IAuthorizationService))
                return new AuthorizationService(DependencyResolver.GetInstance<IPermissionsService>(),
                                                DependencyResolver.GetInstance<IAuthorizationRepository>(),
                                                DependencyResolver.GetInstance<ISession>());
            if (serviceType == typeof(IAuthorizationRepository))
                return new AuthorizationRepository(DependencyResolver.GetInstance<ISession>());
            if (serviceType == typeof(IPermissionsBuilderService))
                return new PermissionsBuilderService(DependencyResolver.GetInstance<ISession>(), DependencyResolver.GetInstance<IAuthorizationRepository>());
            if (serviceType == typeof(IPermissionsService))
                return new PermissionsService(DependencyResolver.GetInstance<IAuthorizationRepository>(), DependencyResolver.GetInstance<ISession>());
			if (serviceType == typeof(IEntityInformationExtractor<Account>))
                return new AccountInfromationExtractor(DependencyResolver.GetInstance<ISession>());
			if (serviceType == typeof(ISession))
				return SessionProvider();
            throw new DependencyResolverException();
        }
    }
}