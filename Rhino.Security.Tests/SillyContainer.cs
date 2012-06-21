using System;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;
using NHibernate;
using Rhino.Security.Interfaces;
using Rhino.Security.Services;

namespace Rhino.Security.Tests
{
    public class SillyContainer : ServiceLocatorImplBase
    {
        public static Func<ISession> SessionProvider;

    	protected override object DoGetInstance(Type serviceType, string key)
        {
            if (serviceType == typeof(IAuthorizationService))
                return new AuthorizationService(GetInstance<IPermissionsService>(),
												GetInstance<IAuthorizationRepository>(),
												GetInstance<ISession>());
            if (serviceType == typeof(IAuthorizationRepository))
				return new AuthorizationRepository(GetInstance<ISession>());
            if (serviceType == typeof(IPermissionsBuilderService))
				return new PermissionsBuilderService(GetInstance<ISession>(), GetInstance<IAuthorizationRepository>());
            if (serviceType == typeof(IPermissionsService))
				return new PermissionsService(GetInstance<IAuthorizationRepository>(), GetInstance<ISession>());
			if (serviceType == typeof(IEntityInformationExtractor<Account>))
				return new AccountInfromationExtractor(GetInstance<ISession>());
			if (serviceType == typeof(ISession))
				return SessionProvider();
			throw new NotImplementedException();
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            throw new NotImplementedException();
        }
    }
}