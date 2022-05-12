using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BehaviorTreeSerializer.Data
{
    /// <summary>
    /// Contains the metadata of a node in the visual editor
    /// </summary>
    [Serializable]
    public class NodeEditorInstanceMetadata
    {
        #region Properties

        /// <summary>
        /// Gets or sets the list of chidren's IDs
        /// </summary>
        public List<string> ChildrenIds;

        /// <summary>
        /// Gets or sets the node's ID
        /// </summary>
        public string Id;

        /// <summary>
        /// Gets or sets the node type's internal name
        /// </summary>
        public string NodeTypeInternalName;

        /// <summary>
        /// Gets or sets the parent node's ID
        /// </summary>
        public string ParentId;

        /// <summary>
        /// Gets or sets the position in the visual editor
        /// </summary>
        public Vector2 PositionInEditor;

        /// <summary>
        /// Gets or sets the dictionary of properties of the node
        /// </summary>
        public SerializableDictionary<string, object> Properties { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public NodeEditorInstanceMetadata()
        {
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public NodeEditorInstanceMetadata(string nodeTypeInternalName, SerializableDictionary<string, object> properties, Vector2 positionInEditor, string parentId)
        {
            this.ChildrenIds = new List<string>();
            this.Id = Guid.NewGuid().ToString();
            this.NodeTypeInternalName = nodeTypeInternalName;
            this.ParentId = parentId;
            this.PositionInEditor = positionInEditor;
            this.Properties = properties;
        }

        #endregion
    }
}
