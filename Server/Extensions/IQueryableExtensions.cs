using FeatureDBPortal.Server.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FeatureDBPortal.Server.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<TSource> WhereEqual<TSource, TProperty>(this IQueryable<TSource> query, Expression<Func<TSource, TProperty>> propertySelector, TProperty value)
        {
            var body2 = Expression.Equal(propertySelector.Body, Expression.Constant(value));
            var lambda = Expression.Lambda<Func<TSource, bool>>(body2, propertySelector.Parameters);
            return query.Where(lambda);
        }

        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> query, Expression<Func<TSource, bool>> predicate, bool condition)
        {
            if (condition)
                return query.Where(predicate);
            else
                return query;
        }

        //public static IQueryable<T> GetQuery<TEntity>(FeaturesContext context, string propertyName)
        //    where TEntity : class
        //{
        //    ConstructorInfo ci = typeof(TEntity).GetConstructor(new Type[0]);
        //    MethodInfo miFooGetName = typeof(TEntity).GetMethod("set_NameX");
        //    MethodInfo miBlogEntry = typeof(FeaturesContext).GetMethod("get_" + propertyName);

        //    ParameterExpression param = Expression.Parameter(typeof(FeaturesContext), "x");

        //    IQueryable<TEntity> result = Queryable.Select<FeaturesContext, TEntity>(
        //                                context.Application,
        //                                Expression.Lambda<Func<FeaturesContext, TEntity>>(
        //                                    Expression.MemberInit(
        //                                        Expression.New(ci, new Expression[0]),
        //                                        new MemberBinding[]{
        //                                    Expression.Bind(miFooGetName,
        //                                                    Expression.Property(param,
        //                                                    miBlogEntry))}
        //                                    ),
        //                                    param
        //                                )
        //                                );
        //    return result;
        //}
    }
}
