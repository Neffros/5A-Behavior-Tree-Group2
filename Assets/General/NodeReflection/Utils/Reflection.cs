using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NodeReflection.Utils
{
    /// <summary>
    /// Utilitary reflection methods for the node reflection engine
    /// </summary>
    public static class Reflection
    {
        #region Private Static Fields

        private static readonly Type exposedAttributeType = typeof(ExposedAttribute);
        private static readonly Type nodeAttributeType = typeof(NodeTagAttribute);

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Gets the ExposedAttribute of the property type
        /// </summary>
        /// <param name="propertyType">Property to check</param>
        /// <returns>An ExposedAttribute</returns>
        public static ExposedAttribute GetExposedAttribute(PropertyInfo propertyType)
        {
            return Attribute.GetCustomAttribute(propertyType, Reflection.exposedAttributeType, true) as ExposedAttribute;
        }

        /// <summary>
        /// Returns an enumerable holding exposed properties in the given class
        /// </summary>
        /// <param name="classType">Class to check</param>
        /// <returns>An enumerable of properties</returns>
        public static IEnumerable<PropertyInfo> GetExposedProperties(Type classType)
        {
            return classType
                .GetProperties()
                .Where(property => property.GetCustomAttributes(true).OfType<ExposedAttribute>().Any());
        }

        /// <summary>
        /// Gets the NodeAttribute of the type
        /// </summary>
        /// <param name="classType">Class to check</param>
        /// <returns>A NodeAttribute</returns>
        public static NodeTagAttribute GetNodeAttribute(Type classType)
        {
            return Attribute.GetCustomAttribute(classType, Reflection.nodeAttributeType, true) as NodeTagAttribute;
        }

        /// <summary>
        /// Returns an enumerable holding node classes in the given assembly
        /// </summary>
        /// <param name="assembly">Assembly to check</param>
        /// <returns>An enumerable of class types</returns>
        public static IEnumerable<Type> GetNodeClasses(Assembly assembly)
        {
            return assembly
                .GetTypes()
                .Where(type => type.GetCustomAttributes(true).OfType<NodeTagAttribute>().Any());
        }

        /// <summary>
        /// Returns an enumerable holding node classes in every assembly
        /// </summary>
        /// <returns>An enumerable of class types</returns>
        public static IEnumerable<Type> GetNodeClasses()
        {
            return AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(type => type.GetCustomAttributes(true).OfType<NodeTagAttribute>().Any());
        }

        #endregion
    }
}
