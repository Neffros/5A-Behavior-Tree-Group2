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
    public static class Engine
    {
        #region Public Static Properties

        /// <summary>
        /// Gets the current nodes types' metadata
        /// </summary>
        public static Dictionary<string, NodeMetadata> Metadata { get; private set; }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Generates tree from the scriptable object
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Node GenerateTree(BehaviorTreeObject data)
        {
            if (Engine.Metadata == null)
                throw new Exception("Metadata not initialized");

            return Engine.CreateNodeInstance(data, data.RootId);
        }

        /// <summary>
        /// Returns the property dictionary of the wanted node type
        /// </summary>
        /// <param name="internalName">Internal name of the node type</param>
        /// <returns>A dictionary of properties</returns>
        public static Dictionary<string, object> GetProperties(string internalName)
        {
            if (Engine.Metadata == null)
                throw new Exception("Metadata not initialized");

            if (!Engine.Metadata.ContainsKey(internalName))
                throw new Exception("Type not existing");

            return Engine.Metadata[internalName]
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
        public static Dictionary<string, NodeMetadata> Update()
        {
            Engine.Metadata = StateGeneration.GetNodeMetadataObjects();

            return Engine.Metadata;
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Instantiates a node according to the instance metadata in the tree
        /// </summary>
        /// <param name="tree">Behavior tree metadata object</param>
        /// <param name="nodeId">ID of the node to instantiate</param>
        /// <returns>The instantiated node and children</returns>
        private static Node CreateNodeInstance(BehaviorTreeObject tree, Guid nodeId)
        {
            if (!tree.IdToNode.ContainsKey(nodeId))
                throw new Exception("Node not found");

            var data = tree.IdToNode[nodeId];

            if (!Engine.Metadata.ContainsKey(data.NodeTypeInternalName))
                throw new Exception("Node type not found");

            NodeMetadata metadata = Engine.Metadata[data.NodeTypeInternalName];

            foreach (var (property, value) in data.Properties)
            {
                if (!metadata.NameToType.ContainsKey(property))
                    throw new Exception("Property not found");

                switch (metadata.NameToType[property])
                {
                    case ExposedPropertyTypeEnum.BOOL when value is not bool:
                        throw new Exception("Property uncompatible");
                    case ExposedPropertyTypeEnum.FLOAT when value is not float:
                        throw new Exception("Property uncompatible");
                    case ExposedPropertyTypeEnum.INT when value is not int:
                        throw new Exception("Property uncompatible");
                    case ExposedPropertyTypeEnum.STRING when value is not string:
                        throw new Exception("Property uncompatible");
                    default:
                        throw new Exception("Unmanaged property type");
                }
            }

            Node node = (Node)Activator.CreateInstance(metadata.NodeType);

            foreach (var (property, value) in data.Properties)
                metadata.NodeType.GetProperty(property).SetValue(node, value);

            foreach (var child in data.ChildrenIds)
                node.Attach(Engine.CreateNodeInstance(tree, child));

            return node;
        }

        #endregion
    }
}
