using System;
using System.Collections.Generic;
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
        public List<Guid> ChildrenIds { get; set; }

        /// <summary>
        /// Gets or sets the node's ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the node type's internal name
        /// </summary>
        public string NodeTypeInternalName { get; set; }

        /// <summary>
        /// Gets or sets the parent node's ID
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// Gets or sets the position in the visual editor
        /// </summary>
        public Vector2 PositionInEditor { get; set; }

        /// <summary>
        /// Gets or sets the dictionary of properties of the node
        /// </summary>
        public Dictionary<string, object> Properties { get; set; }

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
        public NodeEditorInstanceMetadata(string nodeTypeInternalName, Dictionary<string, object> properties, Vector2 positionInEditor, Guid? parentId)
        {
            this.ChildrenIds = new List<Guid>();
            this.Id = Guid.NewGuid();
            this.NodeTypeInternalName = nodeTypeInternalName;
            this.ParentId = parentId ?? Guid.Empty;
            this.PositionInEditor = positionInEditor;
            this.Properties = properties;
        }

        #endregion
    }
}
