using System;

namespace Rhino.Security
{
	/// <summary>
	/// Provide a way to get the security key from a secure entity.
	/// </summary>
	public interface ISecurityKey
	{
		/// <summary>
		/// Gets the security key.
		/// </summary>
		/// <value>
		/// The security key.
		/// </value>
		Guid SecurityKey { get; }
	}
}