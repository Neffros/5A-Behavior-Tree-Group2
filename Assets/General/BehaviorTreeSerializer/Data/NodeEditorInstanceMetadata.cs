using NodeReflection;
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
        #region Public Fields

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
        /// Gets or sets the dictionary of bool properties of the node
        /// </summary>
        public SerializableDictionary<string, bool> PropertiesBool;

        /// <summary>
        /// Gets or sets the dictionary of float properties of the node
        /// </summary>
        public SerializableDictionary<string, float> PropertiesFloat;

        /// <summary>
        /// Gets or sets the dictionary of int properties of the node
        /// </summary>
        public SerializableDictionary<string, int> PropertiesInt;

        /// <summary>
        /// Gets or sets the dictionary of string properties of the node
        /// </summary>
        public SerializableDictionary<string, string> PropertiesString;

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
        public NodeEditorInstanceMetadata(string nodeTypeInternalName, Vector2 positionInEditor, string parentId)
        {
            this.ChildrenIds = new List<string>();
            this.Id = Guid.NewGuid().ToString();
            this.NodeTypeInternalName = nodeTypeInternalName;
            this.ParentId = parentId;
            this.PositionInEditor = positionInEditor;
            this.PropertiesBool = new SerializableDictionary<string, bool>(Engine.GetPropertiesBool(nodeTypeInternalName));
            this.PropertiesFloat = new SerializableDictionary<string, float>(Engine.GetPropertiesFloat(nodeTypeInternalName));
            this.PropertiesInt = new SerializableDictionary<string, int>(Engine.GetPropertiesInt(nodeTypeInternalName));
            this.PropertiesString = new SerializableDictionary<string, string>(Engine.GetPropertiesString(nodeTypeInternalName));
        }

        #endregion
    }
}
