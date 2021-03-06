using NodeReflection.Data;
using NodeReflection.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NodeReflection.Utils
{
    /// <summary>
    /// Utilitary state generation methods for the node reflection engine
    /// </summary>
    public static class StateGeneration
    {
        #region Private Static Fields

        private static Type boolType = typeof(bool);
        private static Type floatType = typeof(float);
        private static Type intType = typeof(int);
        private static Type stringType = typeof(string);

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Initializes and returns all node metadata objects by internal name in the given assembly
        /// </summary>
        /// <param name="assembly">Assembly to check</param>
        /// <returns>A dictionary of string to NodeMetadata</returns>
        public static Dictionary<string, NodeMetadata> GetNodeMetadataObjects(Assembly assembly)
        {
            return Reflection
                .GetNodeClasses(assembly)
                .Select(StateGeneration.CreateNodeMetadata)
                .ToDictionary((data) => data.InternalName);
        }

        /// <summary>
        /// Initializes and returns all node metadata objects by internal name in the given assembly
        /// </summary>
        /// <returns>A dictionary of string to NodeMetadata</returns>
        public static Dictionary<string, NodeMetadata> GetNodeMetadataObjects()
        {
            return Reflection
                .GetNodeClasses()
                .Select(StateGeneration.CreateNodeMetadata)
                .ToDictionary((data) => data.InternalName);
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Creates a node metadata object from a compatible type
        /// </summary>
        /// <param name="classType">Node class</param>
        /// <returns>A NodeMetadata</returns>
        private static NodeMetadata CreateNodeMetadata(Type classType)
        {
            var properties = Reflection.GetExposedProperties(classType);
            var nodeAttribute = Reflection.GetNodeAttribute(classType);
            var nameToType = new Dictionary<string, ExposedPropertyTypeEnum>();
            var nameToDefaultValue = new Dictionary<string, object>();

            foreach (var propertyType in properties)
            {
                var exposedAttribute = Reflection.GetExposedAttribute(propertyType);
                ExposedPropertyTypeEnum typeValue;

                if (propertyType.PropertyType.Equals(boolType))
                    typeValue = ExposedPropertyTypeEnum.BOOL;
                else if (propertyType.PropertyType.Equals(floatType))
                    typeValue = ExposedPropertyTypeEnum.FLOAT;
                else if (propertyType.PropertyType.Equals(intType))
                    typeValue = ExposedPropertyTypeEnum.INT;
                else if (propertyType.PropertyType.Equals(stringType))
                    typeValue = ExposedPropertyTypeEnum.STRING;
                else
                    continue;

                var name = string.IsNullOrWhiteSpace(exposedAttribute.Name) ? propertyType.Name : exposedAttribute.Name;

                nameToType.Add(name, typeValue);
                nameToDefaultValue.Add(name, exposedAttribute.DefaultValue);
            }

            return new NodeMetadata(nodeAttribute, classType, nameToType, nameToDefaultValue);
        }

        #endregion
    }
}
