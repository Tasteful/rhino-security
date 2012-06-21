using System;

namespace Rhino.Security.Impl.Linq
{
	internal static class SecurityKeyExtractorExtenssions
	{
		public static bool EqualsEntitySecurityKey(this Guid entitySecurityKey, object entity)
		{
			return Security.ExtractKey(entity) == entitySecurityKey;
		}

		public static bool EqualsEntitySecurityKey(this Guid? entitySecurityKey, object entity)
		{
			return Security.ExtractKey(entity) == entitySecurityKey;
		}
	}
}
