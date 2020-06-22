using System;
using System.Linq.Expressions;

namespace FeatureDBPortal.Server.Utils
{
    public static class PropertyExpressionBuilder
    {
        public static Expression<Func<TObject, TOutput>> Build<TObject, TOutput>(string propertyName)
        {
            var x = Expression.Parameter(typeof(TObject), "x");
            var body = Expression.PropertyOrField(x, propertyName);
            var lambda = Expression.Lambda<Func<TObject, TOutput>>(body, x);

            return lambda;
        }
    }
}
