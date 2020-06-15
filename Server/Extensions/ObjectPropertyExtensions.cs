using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FeatureDBPortal.Server.Extensions
{
    public static class ObjectPropertyExtensions
    {
        public static T GetPropertyValue<T>(this object @object, string propertyName)
        {
            return (T)@object.GetType().GetProperty(propertyName).GetValue(@object, null);
        }
    }
}
