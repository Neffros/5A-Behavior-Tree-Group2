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

        private static readonly Type exposedInVisualEditorAttributeType = typeof(ExposedInVisualEditorAttribute);
        private static readonly Type visualNodeAttributeType = typeof(VisualNodeAttribute);

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Gets the ExposedAttribute of the property type
        /// </summary>
        /// <param name="propertyType">Property to check</param>
        /// <returns>An ExposedAttribute</returns>
        public static ExposedInVisualEditorAttribute GetExposedAttribute(PropertyInfo propertyType)
        {
            return Attribute.GetCustomAttribute(propertyType, Reflection.exposedInVisualEditorAttributeType) as ExposedInVisualEditorAttribute;
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
                .Where(property => property.GetCustomAttributes().OfType<ExposedInVisualEditorAttribute>().Any());
        }

        /// <summary>
        /// Gets the NodeAttribute of the type
        /// </summary>
        /// <param name="classType">Class to check</param>
        /// <returns>A NodeAttribute</returns>
        public static VisualNodeAttribute GetNodeAttribute(Type classType)
        {
            return Attribute.GetCustomAttribute(classType, Reflection.visualNodeAttributeType) as VisualNodeAttribute;
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
                .Where(type => type.GetCustomAttributes().OfType<VisualNodeAttribute>().Any());
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
                .Where(type => type.GetCustomAttributes().OfType<VisualNodeAttribute>().Any());
        }

        #endregion
    }
}
