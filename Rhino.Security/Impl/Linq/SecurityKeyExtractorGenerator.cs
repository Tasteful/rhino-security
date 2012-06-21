using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate.Hql.Ast;
using NHibernate.Linq;
using NHibernate.Linq.Functions;
using NHibernate.Linq.Visitors;

namespace Rhino.Security.Impl.Linq
{
	internal class SecurityKeyExtractorGenerator : BaseHqlGeneratorForMethod
	{
		public SecurityKeyExtractorGenerator()
		{
			SupportedMethods = new[]
				{
					ReflectionHelper.GetMethodDefinition<Guid>(x => x.EqualsEntitySecurityKey(null)),
					ReflectionHelper.GetMethodDefinition<Guid?>(x => x.EqualsEntitySecurityKey(null)),
				};
		}

		public override HqlTreeNode BuildHql(MethodInfo method, Expression targetObject, ReadOnlyCollection<Expression> arguments, HqlTreeBuilder treeBuilder, IHqlExpressionVisitor visitor)
		{
			return treeBuilder.Equality(
				visitor.Visit(arguments[0]).AsExpression(),
				visitor.Visit(Expression.MakeMemberAccess(arguments[1], Security.GetSecurityKeyMemberInfo(arguments[1].Type))).AsExpression());
		}
	}
}