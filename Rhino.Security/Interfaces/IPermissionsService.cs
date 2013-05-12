using Rhino.Security.Model;

namespace Rhino.Security.Interfaces
{
	/// <summary>
	/// Allow to retrieve and remove permissions
	/// on users, user groups, entities groups and entities.
	/// </summary>
	public interface IPermissionsService
	{
		/// <summary>
		/// Gets the permissions for the specified user
		/// </summary>
		/// <param name="user">The user.</param>
		/// <returns></returns>
		Permission[] GetPermissionsFor(IUser user);

		/// <summary>
		/// Gets the permissions for the specified user and entity
		/// </summary>
		/// <typeparam name="TEntity">The type of the entity.</typeparam>
		/// <param name="user">The user.</param>
		/// <param name="entity">The entity.</param>
		/// <returns></returns>
		Permission[] GetPermissionsFor<TEntity>(IUser user, TEntity entity) where TEntity : class;

		/// <summary>
		/// Gets the permissions for the specified entity
		/// </summary>
		/// <param name="user">The user.</param>
		/// <param name="operationName">Name of the operation.</param>
		/// <returns></returns>
		Permission[] GetGlobalPermissionsFor(IUser user, string operationName) ;

		/// <summary>
		/// Gets all permissions for the specified operation
		/// </summary>
		/// <param name="operationName">Name of the operation.</param>
		/// <returns></returns>
		Permission[] GetPermissionsFor(string operationName) ;

		/// <summary>
		/// Gets the permissions for the specified entity
		/// </summary>
		/// <typeparam name="TEntity">The type of the entity.</typeparam>
		/// <param name="user">The user.</param>
		/// <param name="entity">The entity.</param>
		/// <param name="operationName">Name of the operation.</param>
		/// <returns></returns>
		Permission[] GetPermissionsFor<TEntity>(IUser user, TEntity entity, string operationName) where TEntity : class;

		/// <summary>
		/// Gets the permissions for the specified entity
		/// </summary>
		/// <typeparam name="TEntity">The type of the entity.</typeparam>
		/// <param name="entity">The entity.</param>
		/// <returns></returns>
		Permission[] GetPermissionsFor<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
		/// Get the specific global permissions for the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="operationName">Name of the operation.</param>
        /// <returns>Permission[][].</returns>
		Permission[] GetSpecificGlobalPermissionsFor(IUser user, string operationName);

        /// <summary>
		/// Get the specific global permissions for the specified users group.
        /// </summary>
        /// <param name="usersGroup">The users group.</param>
        /// <param name="operationName">Name of the operation.</param>
        /// <returns>Permission[][].</returns>
		Permission[] GetSpecificGlobalPermissionsFor(UsersGroup usersGroup, string operationName);

        /// <summary>
		/// Get the specific permissions for the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the T entity.</typeparam>
        /// <param name="user">The user.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="operationName">Name of the operation.</param>
        /// <returns>Permission[][].</returns>
		Permission[] GetSpecificPermissionsFor<TEntity>(IUser user, TEntity entity, string operationName) where TEntity : class;

        /// <summary>
		/// Get the specific permissions for the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the T entity.</typeparam>
        /// <param name="usersGroup">The users group.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="operationName">Name of the operation.</param>
        /// <returns>Permission[][].</returns>
		Permission[] GetSpecificPermissionsFor<TEntity>(UsersGroup usersGroup, TEntity entity, string operationName) where TEntity : class;

		/// <summary>
		/// Get the specific permissions for the specified entity.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <param name="entitiesGroup">The entities group.</param>
		/// <param name="operationName">Name of the operation.</param>
		/// <returns>Permission[][].</returns>
		Permission[] GetSpecificPermissionsFor(IUser user, EntitiesGroup entitiesGroup, string operationName);

		/// <summary>
		/// Get the specific permissions for the specified entity.
		/// </summary>
		/// <param name="usersGroup">The users group.</param>
		/// <param name="entitiesGroup">The entities group.</param>
		/// <param name="operationName">Name of the operation.</param>
		/// <returns>Permission[][].</returns>
		Permission[] GetSpecificPermissionsFor(UsersGroup usersGroup, EntitiesGroup entitiesGroup, string operationName);
	}
}
