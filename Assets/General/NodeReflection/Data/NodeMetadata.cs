using NodeReflection.Enumerations;
using System;
using System.Collections.Generic;

namespace NodeReflection.Data
{
    /// <summary>
    /// Contains all the metadata of a node type
    /// </summary>
    public struct NodeMetadata
    {
        #region Properties

        /// <summary>
        /// Gets the description of the node
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets if the children should be displayed as a block
        /// </summary>
        public bool DisplayAsBlock { get; private set; }

        /// <summary>
        /// Gets the internal name of the node
        /// </summary>
        public string InternalName { get; private set; }

        /// <summary>
        /// Gets the name of the node
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the dictionary of exposed properties' default values
        /// </summary>
        public Dictionary<string, object> NameToDefaultValue { get; private set; }

        /// <summary>
        /// Gets the dictionary of exposed properties' types
        /// </summary>
        public Dictionary<string, ExposedPropertyTypeEnum> NameToType { get; private set; }

        /// <summary>
        /// Gets the type of node to instantiate
        /// </summary>
        public Type NodeType { get; private set; }

        /// <summary>
        /// Gets the path of the node category
        /// </summary>
        public string Path { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="nodeData">Contains display information</param>
        /// <param name="classType">Node type to instantiate</param>
        /// <param name="nameToType">Properties of the node</param>
        public NodeMetadata(VisualNodeAttribute nodeData, Type classType, Dictionary<string, ExposedPropertyTypeEnum> nameToType, Dictionary<string, object> nameToDefaultValue)
        {
            this.Description = nodeData.Description;
            this.DisplayAsBlock = nodeData.DisplayAsBlock;
            this.InternalName = classType.Name;
            this.Name = string.IsNullOrWhiteSpace(nodeData.Name) ? classType.Name : nodeData.Name;
            this.NameToDefaultValue = nameToDefaultValue;
            this.NameToType = nameToType;
            this.NodeType = classType;
            this.Path = nodeData.Path;
        }

        #endregion
    }
}
