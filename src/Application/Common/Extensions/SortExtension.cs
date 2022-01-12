// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Razor.Application.Common.Extensions
{
    public static class SortExtension
    {
        public static IQueryable<T> OrderByWithCheck<T>(this IQueryable<T> source, string sort, string order)
        {
            var type = typeof(T);
            var property = type.GetProperty(sort);
            if (!PredicateBuilder.CheckProperty<T>(sort))
            {
                return source;
            }
            if (order.ToUpper() == "DESC")
            {
                return OrderByMemberDescending(source, sort);
            }
            return OrderByMember(source, sort);
        }
        public static IOrderedQueryable<T> OrderByMember<T>(this IQueryable<T> source, string memberPath)
        {
            return source.OrderByMemberUsing(memberPath, "OrderBy");
        }
        public static IOrderedQueryable<T> OrderByMemberDescending<T>(this IQueryable<T> source, string memberPath)
        {
            return source.OrderByMemberUsing(memberPath, "OrderByDescending");
        }
        private static IOrderedQueryable<T> OrderByMemberUsing<T>(this IQueryable<T> source, string memberPath, string method)
        {
            var parameter = Expression.Parameter(typeof(T), "item");
            var member = memberPath.Split('.')
                .Aggregate((Expression)parameter, Expression.PropertyOrField);
            var keySelector = Expression.Lambda(member, parameter);
            var methodCall = Expression.Call(
                typeof(Queryable), method, new[] { parameter.Type, member.Type },
                source.Expression, Expression.Quote(keySelector));
            return (IOrderedQueryable<T>)source.Provider.CreateQuery(methodCall);
        }

        public static (IEnumerable<T>,bool) OrderByDynamic<T>(this IEnumerable<T> items, string sortby, string sort_direction)
        {
            var property = typeof(T).GetProperty(sortby);
            if (property == null) return (items,false);
            var result = typeof(SortExtension)
                .GetMethod("OrderByDynamic_Private", BindingFlags.NonPublic | BindingFlags.Static)
                .MakeGenericMethod(typeof(T), property.PropertyType)
                .Invoke(null, new object[] { items, sortby, sort_direction });

            return ((IEnumerable<T>)result,false);
        }

        private static IEnumerable<T> OrderByDynamic_Private<T, TKey>(IEnumerable<T> items, string sortby, string sort_direction)
        {
            var parameter = Expression.Parameter(typeof(T), "x");

            Expression<Func<T, TKey>> property_access_expression =
                Expression.Lambda<Func<T, TKey>>(
                    Expression.Property(parameter, sortby),
                    parameter);

            if (sort_direction == "asc")
            {
                return items.OrderBy(property_access_expression.Compile());
            }

            if (sort_direction == "desc")
            {
                return items.OrderByDescending(property_access_expression.Compile());
            }

            throw new Exception("Invalid Sort Direction");
        }
    }

    public class OrderingMethodFinder : ExpressionVisitor
    {
        bool _orderingMethodFound = false;

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var name = node.Method.Name;

            if (node.Method.DeclaringType == typeof(Queryable) && (
                name.StartsWith("OrderBy", StringComparison.Ordinal) ||
                name.StartsWith("ThenBy", StringComparison.Ordinal)))
            {
                _orderingMethodFound = true;
            }

            return base.VisitMethodCall(node);
        }

        public static bool OrderMethodExists(Expression expression)
        {
            var visitor = new OrderingMethodFinder();
            visitor.Visit(expression);
            return visitor._orderingMethodFound;
        }
    }
}
