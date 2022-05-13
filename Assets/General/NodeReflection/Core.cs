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
        public static Dictionary<string, NodeMetadata> Metadata
        {
            get
            {
                if (Engine._metadata == null)
                    Engine.Update();

                return Engine._metadata;
            }
            private set
            {
                Engine._metadata = value;
            }
        }

        #endregion

        #region Private Static Fields

        private static Dictionary<string, NodeMetadata> _metadata;

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Generates tree from the scriptable object
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Node GenerateTree(BehaviorTreeObject data)
        {
            return Engine.CreateNodeInstance(data, data.RootId);
        }

        /// <summary>
        /// Returns the bool properties of the wanted node type
        /// </summary>
        /// <param name="internalName">Internal name of the node type</param>
        /// <returns>A dictionary of bool properties</returns>
        public static Dictionary<string, bool> GetPropertiesBool(string internalName)
        {
            if (!Engine.Metadata.ContainsKey(internalName))
                throw new Exception("Type not existing");

            var metadata = Engine.Metadata[internalName];

            return metadata
                .NameToType
                .Where(pair => pair.Value == ExposedPropertyTypeEnum.BOOL)
                .Zip(metadata.NameToDefaultValue, (nameAndType, nameAndDefaultValue) => (nameAndType.Key, nameAndDefaultValue.Value))
                .Select((data) =>
                {
                    var (key, defaultValue) = data;

                    if (defaultValue != null && defaultValue is not bool)
                        throw new Exception("Default value uncompatible");
                    return new KeyValuePair<string, bool>(key, defaultValue != null && (bool)defaultValue);
                })
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        /// <summary>
        /// Returns the float properties of the wanted node type
        /// </summary>
        /// <param name="internalName">Internal name of the node type</param>
        /// <returns>A dictionary of float properties</returns>
        public static Dictionary<string, float> GetPropertiesFloat(string internalName)
        {
            if (!Engine.Metadata.ContainsKey(internalName))
                throw new Exception("Type not existing");

            var metadata = Engine.Metadata[internalName];

            return metadata
                .NameToType
                .Where(pair => pair.Value == ExposedPropertyTypeEnum.FLOAT)
                .Zip(metadata.NameToDefaultValue, (nameAndType, nameAndDefaultValue) => (nameAndType.Key, nameAndDefaultValue.Value))
                .Select((data) =>
                {
                    var (key, defaultValue) = data;

                    if (defaultValue != null && defaultValue is not float)
                        throw new Exception("Default value uncompatible");
                    return new KeyValuePair<string, float>(key, defaultValue == null ? 0f : (float)defaultValue);
                })
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        /// <summary>
        /// Returns the int properties of the wanted node type
        /// </summary>
        /// <param name="internalName">Internal name of the node type</param>
        /// <returns>A dictionary of int properties</returns>
        public static Dictionary<string, int> GetPropertiesInt(string internalName)
        {
            if (!Engine.Metadata.ContainsKey(internalName))
                throw new Exception("Type not existing");

            var metadata = Engine.Metadata[internalName];

            return metadata
                .NameToType
                .Where(pair => pair.Value == ExposedPropertyTypeEnum.INT)
                .Zip(metadata.NameToDefaultValue, (nameAndType, nameAndDefaultValue) => (nameAndType.Key, nameAndDefaultValue.Value))
                .Select((data) =>
                {
                    var (key, defaultValue) = data;

                    if (defaultValue != null && defaultValue is not int)
                        throw new Exception("Default value uncompatible");
                    return new KeyValuePair<string, int>(key, defaultValue == null ? 0 : (int)defaultValue);
                })
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        /// <summary>
        /// Returns the string properties of the wanted node type
        /// </summary>
        /// <param name="internalName">Internal name of the node type</param>
        /// <returns>A dictionary of string properties</returns>
        public static Dictionary<string, string> GetPropertiesString(string internalName)
        {
            if (!Engine.Metadata.ContainsKey(internalName))
                throw new Exception("Type not existing");

            var metadata = Engine.Metadata[internalName];

            return metadata
                .NameToType
                .Where(pair => pair.Value == ExposedPropertyTypeEnum.STRING)
                .Zip(metadata.NameToDefaultValue, (nameAndType, nameAndDefaultValue) => (nameAndType.Key, nameAndDefaultValue.Value))
                .Select((data) =>
                {
                    var (key, defaultValue) = data;

                    if (defaultValue != null && defaultValue is not string)
                        throw new Exception("Default value uncompatible");
                    return new KeyValuePair<string, string>(key, defaultValue == null ? string.Empty : (string)defaultValue);
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
        private static Node CreateNodeInstance(BehaviorTreeObject tree, string nodeId)
        {
            if (!tree.IdToNode.ContainsKey(nodeId))
                throw new Exception("Node not found");

            var data = tree.IdToNode[nodeId];

            if (!Engine.Metadata.ContainsKey(data.NodeTypeInternalName))
                throw new Exception("Node type not found");

            NodeMetadata metadata = Engine.Metadata[data.NodeTypeInternalName];

            foreach (var (property, _) in data.PropertiesBool)
            {
                if (!metadata.NameToType.ContainsKey(property))
                    throw new Exception("Bool property not found");
            }
            foreach (var (property, _) in data.PropertiesFloat)
            {
                if (!metadata.NameToType.ContainsKey(property))
                    throw new Exception("Bool property not found");
            }
            foreach (var (property, _) in data.PropertiesInt)
            {
                if (!metadata.NameToType.ContainsKey(property))
                    throw new Exception("Bool property not found");
            }
            foreach (var (property, _) in data.PropertiesString)
            {
                if (!metadata.NameToType.ContainsKey(property))
                    throw new Exception("Bool property not found");
            }

            Node node = (Node)Activator.CreateInstance(metadata.NodeType);

            foreach (var (property, value) in data.PropertiesBool)
                metadata.NodeType.GetProperty(property).SetValue(node, value);
            foreach (var (property, value) in data.PropertiesFloat)
                metadata.NodeType.GetProperty(property).SetValue(node, value);
            foreach (var (property, value) in data.PropertiesInt)
                metadata.NodeType.GetProperty(property).SetValue(node, value);
            foreach (var (property, value) in data.PropertiesString)
                metadata.NodeType.GetProperty(property).SetValue(node, value);

            foreach (var child in data.ChildrenIds)
                node.Attach(Engine.CreateNodeInstance(tree, child));

            return node;
        }

        #endregion
    }
}
