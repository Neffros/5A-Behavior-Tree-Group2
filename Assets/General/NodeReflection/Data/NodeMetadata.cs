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
        /// Gets the dictionary of exposed properties' names and types
        /// </summary>
        public Dictionary<string, ExposedPropertyTypeEnum> NameToType { get; private set; }

        /// <summary>
        /// Gets the type of node to instantiate
        /// </summary>
        public Type NodeType { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="nodeData">Contains display information</param>
        /// <param name="classType">Node type to instantiate</param>
        /// <param name="nameToType">Properties of the node</param>
        public NodeMetadata(NodeTagAttribute nodeData, Type classType, Dictionary<string, ExposedPropertyTypeEnum> nameToType)
        {
            this.Description = nodeData.Description;
            this.DisplayAsBlock = nodeData.DisplayAsBlock;
            this.InternalName = classType.Name;
            this.Name = string.IsNullOrWhiteSpace(nodeData.Name) ? classType.Name : nodeData.Name;
            this.NameToType = nameToType;
            this.NodeType = classType;
        }

        #endregion
    }
}
