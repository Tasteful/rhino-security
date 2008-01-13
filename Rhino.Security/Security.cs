namespace Rhino.Security
{
    using System;
    using Castle.ActiveRecord;
    using Castle.ActiveRecord.Framework;
    using Castle.ActiveRecord.Framework.Internal;
    using Castle.Core.Logging;
    using Commons;

    /// <summary>
    /// This class allows to configure the security system
    /// </summary>
    public class Security
    {
        /// <summary>
        /// A flag that determains how the security tables should be treated.
        /// In a separate schema or using a naming convention.
        /// The default is to put them in a separate schema.
        /// </summary>
        public static bool UseSecuritySchema = true;

        /// <summary>
        /// Prepares to change all internal reference in the security system
        /// from IUser to the user implementation of the project
        /// </summary>
        public static void PrepareForActiveRecordInitialization<TUser>()
            where TUser : IUser
        {
            ActiveRecordStarter.ModelsCreated +=
                delegate(ActiveRecordModelCollection models, IConfigurationSource source)
                {
                    Type userType = typeof(TUser);
                    foreach (ActiveRecordModel model in models)
                    {
                        if (model.Type.Assembly != typeof(IUser).Assembly)
                            continue;
                        if (UseSecuritySchema)
                            model.ActiveRecordAtt.Schema = "security";
                        else
                            model.ActiveRecordAtt.Table = "security_" + model.ActiveRecordAtt.Table;
                        foreach (BelongsToModel belongsToModel in model.BelongsTo)
                        {
                            if (belongsToModel.Property.PropertyType == typeof(IUser))
                            {
                                belongsToModel.BelongsToAtt.Type = userType;
                            }
                        }
                        foreach (HasAndBelongsToManyModel hasAndBelongsToManyModel in model.HasAndBelongsToMany)
                        {
                            if (UseSecuritySchema)
                            {
                                hasAndBelongsToManyModel.HasManyAtt.Schema = "security";
                            }
                            else
                            {
                                hasAndBelongsToManyModel.HasManyAtt.Table = "security_" +
                                                                            hasAndBelongsToManyModel.HasManyAtt.Table;
                            }
                            if (hasAndBelongsToManyModel.HasManyAtt.MapType == typeof(IUser))
                            {
                                hasAndBelongsToManyModel.HasManyAtt.MapType = userType;
                            }
                        }
                        foreach (HasManyModel hasManyModel in model.HasMany)
                        {
                            if (hasManyModel.HasManyAtt.MapType == typeof(IUser))
                            {
                                hasManyModel.HasManyAtt.MapType = userType;
                            }
                        }
                    }
                };
        }

        /// <summary>
        /// Gets the logger for the security system.
        /// </summary>
        /// <value>The logger.</value>
        public static ILogger Logger
        {
            get
            {
                return IoC.TryResolve<ILogger>(new NullLogger());

            }
        }

        /// <summary>
        /// Extracts the key from the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Guid ExtractKey<TEntity>(TEntity entity) 
            where TEntity : class
        {
            Guard.Against<ArgumentNullException>(entity == null, "Entity cannot be null");
            IEntitySecurityKeyExtractor<TEntity> keyExtractor = IoC.Resolve<IEntitySecurityKeyExtractor<TEntity>>();
            return keyExtractor.GetSecurityKeyFor(entity);
        }
    }
}