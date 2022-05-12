using BehaviorTree;
using BehaviorTreeSerializer.Data;
using NodeReflection.Data;
using NodeReflection.Enumerations;
using NodeReflection.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NodeReflection
{
    /// <summary>
    /// Provides the metadata from the node types in the current assembly
    /// </summary>
    public class Engine
    {
        #region Properties

        /// <summary>
        /// Gets the current nodes types' metadata
        /// </summary>
        public Dictionary<string, NodeMetadata> Metadata { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the property dictionary of the wanted node type
        /// </summary>
        /// <param name="internalName">Internal name of the node type</param>
        /// <returns>A dictionary of properties</returns>
        public Dictionary<string, object> GetProperties(string internalName)
        {
            if (this.Metadata == null)
                return null;

            if (!this.Metadata.ContainsKey(internalName))
                throw new Exception("Type not existing");

            return this.Metadata[internalName]
                .NameToType
                .Select((pair) =>
                {
                    switch (pair.Value)
                    {
                        case ExposedPropertyTypeEnum.BOOL:
                            return new KeyValuePair<string, object>(pair.Key, false);
                        case ExposedPropertyTypeEnum.FLOAT:
                            return new KeyValuePair<string, object>(pair.Key, 0);
                        case ExposedPropertyTypeEnum.INT:
                            return new KeyValuePair<string, object>(pair.Key, 0);
                        case ExposedPropertyTypeEnum.STRING:
                            return new KeyValuePair<string, object>(pair.Key, string.Empty);
                        default:
                            throw new Exception("Unmanaged property type");
                    }
                })
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        /// <summary>
        /// Updates the metadata
        /// </summary>
        public Dictionary<string, NodeMetadata> Update()
        {
            this.Metadata = StateGeneration.GetNodeMetadataObjects();

            return this.Metadata;
        }

        #endregion
    }
}
