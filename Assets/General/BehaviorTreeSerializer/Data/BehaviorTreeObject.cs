using BehaviorTree;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeSerializer.Data
{
    /// <summary>
    /// Represents the tree of nodes in the visual editor
    /// </summary>
    [CreateAssetMenu(fileName = "BehaviorTree", menuName = "Behavior Tree/Create Object", order = 1)]
    public class BehaviorTreeObject : ScriptableObject
    {
        #region Unity Fields

        [SerializeField, Tooltip("Maps a node to its ID")]
        private Dictionary<Guid, NodeEditorInstanceMetadata> _idToNode;

        [SerializeField, Tooltip("ID of the root node")]
        private Guid _rootId;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the dictionary of nodes by their IDs
        /// </summary>
        public Dictionary<Guid, NodeEditorInstanceMetadata> IdToNode { get; set; }

        /// <summary>
        /// Gets or sets the ID of the root node
        /// </summary>
        public Guid RootId => this._rootId;

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds child ID to parent node's children IDs list
        /// </summary>
        /// <param name="parentId">ID of the parent</param>
        /// <param name="childId">ID of the child</param>
        public void AddChild(Guid parentId, Guid childId)
        {
            if (!this.IdToNode.ContainsKey(parentId))
                throw new Exception("Parent not found");
            if (!this.IdToNode[parentId].ChildrenIds.Contains(childId))
                throw new Exception("Child not found");

            this.IdToNode[parentId].ChildrenIds.Add(childId);
        }

        /// <summary>
        /// Adds a node to the store and adds it as child of its parent if given
        /// </summary>
        /// <param name="nodeTypeInternalName">The node type to instantiate</param>
        /// <param name="properties">Properties of the node type with default values</param>
        /// <param name="positionInEditor">Position on the grid</param>
        /// <param name="parentId">ID of the parent node</param>
        /// <returns>The instantiated node metadata</returns>
        public NodeEditorInstanceMetadata AddNode(string nodeTypeInternalName, Dictionary<string, object> properties, Vector2 positionInEditor, Guid? parentId)
        {
            var node = new NodeEditorInstanceMetadata(nodeTypeInternalName, properties, positionInEditor, parentId);

            this.IdToNode.Add(node.Id, node);

            if (parentId.HasValue)
                this.AddChild(parentId.Value, node.Id);

            return node;
        }

        /// <summary>
        /// Removes the child node from the parent
        /// </summary>
        /// <param name="parentId">ID of the parent node</param>
        /// <param name="childId">ID of the child node</param>
        public void RemoveChild(Guid parentId, Guid childId)
        {
            if (!this.IdToNode.ContainsKey(parentId))
                throw new Exception("Parent not found");
            if (!this.IdToNode[parentId].ChildrenIds.Contains(childId))
                throw new Exception("Child not found");

            this.IdToNode[parentId].ChildrenIds.Remove(childId);
        }

        /// <summary>
        /// Removes the node from the tree
        /// </summary>
        /// <param name="nodeId">ID of the node</param>
        public void RemoveNode(Guid nodeId)
        {
            if (!this.IdToNode.ContainsKey(nodeId))
                throw new Exception("Node not found");

            if (!this.IdToNode[nodeId].ParentId.Equals(Guid.Empty))
                this.RemoveChild(this.IdToNode[nodeId].ParentId, nodeId);

            this.IdToNode.Remove(nodeId);
        }

        /// <summary>
        /// Sets the property of a node
        /// </summary>
        /// <param name="nodeId">ID of the node</param>
        /// <param name="property">Property name</param>
        /// <param name="value">Value of the property</param>
        public void SetNodeProperty(Guid nodeId, string property, object value)
        {
            if (!this.IdToNode.ContainsKey(nodeId))
                throw new Exception("Node not found");
            if (!this.IdToNode[nodeId].Properties.ContainsKey(property))
                throw new Exception("Property not found");

            this.IdToNode[nodeId].Properties[property] = value;
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Creates a BehaviorTreeObject with default properties
        /// </summary>
        /// <returns>A BehaviorTreeObject</returns>
        public static BehaviorTreeObject Create()
        {
            var tree = ScriptableObject.CreateInstance<BehaviorTreeObject>();
            var selectorName = typeof(Selector).Name;
            var root = new NodeEditorInstanceMetadata(
                selectorName,
                NodeReflection.Engine.GetProperties(selectorName),
                Vector2.zero,
                null
            );

            tree.IdToNode = new Dictionary<Guid, NodeEditorInstanceMetadata>();
            tree.IdToNode.Add(root.Id, root);
            tree._rootId = root.Id;

            return tree;
        }

        #endregion
    }
}
