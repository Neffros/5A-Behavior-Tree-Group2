using BehaviorTree;
using NodeReflection;
using System;
using System.Linq;
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
        private SerializableDictionary<string, NodeEditorInstanceMetadata> _idToNode;

        [SerializeField, HideInInspector]
        private bool _initialized;

        [SerializeField, Tooltip("ID of the root node")]
        private string _rootId;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the dictionary of nodes by their IDs
        /// </summary>
        public SerializableDictionary<string, NodeEditorInstanceMetadata> IdToNode => this._idToNode;

        /// <summary>
        /// Gets or sets the ID of the root node
        /// </summary>
        public string RootId => this._rootId;

        #endregion

        #region Unity Callbacks

        /// <summary>
        /// Fired on serialized field edition in inspector
        /// </summary>
        private void Awake()
        {
            this.Initialize();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds child ID to parent node's children IDs list
        /// </summary>
        /// <param name="parentId">ID of the parent</param>
        /// <param name="childId">ID of the child</param>
        public void AddChild(string parentId, string childId)
        {
            if (!this.IdToNode.ContainsKey(parentId))
                throw new Exception("Parent not found");
            if (!this.IdToNode.ContainsKey(childId))
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
        public NodeEditorInstanceMetadata AddNode(string nodeTypeInternalName, SerializableDictionary<string, object> properties, Vector2 positionInEditor, string parentId)
        {
            var node = new NodeEditorInstanceMetadata(nodeTypeInternalName, properties, positionInEditor, parentId);

            this.IdToNode.Add(node.Id, node);

            if (!string.IsNullOrEmpty(parentId))
                this.AddChild(parentId, node.Id);

            return node;
        }

        /// <summary>
        /// Initializes this BehaviorTreeObject with default properties
        /// </summary>
        /// <returns>This instance</returns>
        public BehaviorTreeObject Initialize()
        {
            if (this._initialized)
                return this;

            this._idToNode = new SerializableDictionary<string, NodeEditorInstanceMetadata>();
            
            var selectorName = typeof(Selector).Name;
            var root = this.AddNode(
                selectorName,
                new SerializableDictionary<string, object>(Engine.GetProperties(selectorName)),
                Vector2.zero,
                null
            );
            
            this._rootId = root.Id;
            this._initialized = true;

            return this;
        }

        /// <summary>
        /// Removes the child node from the parent
        /// </summary>
        /// <param name="parentId">ID of the parent node</param>
        /// <param name="childId">ID of the child node</param>
        public void RemoveChild(string parentId, string childId)
        {
            if (!this.IdToNode.ContainsKey(parentId))
                throw new Exception("Parent not found");

            if (!this.IdToNode[parentId].ChildrenIds.Contains(childId))
                throw new Exception("Child not found");

            this.IdToNode[parentId].ChildrenIds.Remove(childId);
            this.IdToNode[childId].ParentId = null;
        }

        /// <summary>
        /// Removes the node from the tree
        /// </summary>
        /// <param name="nodeId">ID of the node</param>
        public void RemoveNode(string nodeId)
        {
            if (!this.IdToNode.ContainsKey(nodeId))
                throw new Exception("Node not found");

            if (!string.IsNullOrEmpty(this.IdToNode[nodeId].ParentId))
                this.RemoveChild(this.IdToNode[nodeId].ParentId, nodeId);

            foreach (var childId in this.IdToNode[nodeId].ChildrenIds)
                this.IdToNode[childId].ParentId = null;

            this.IdToNode.Remove(nodeId);
        }

        /// <summary>
        /// Sets the property of a node
        /// </summary>
        /// <param name="nodeId">ID of the node</param>
        /// <param name="property">Property name</param>
        /// <param name="value">Value of the property</param>
        public void SetNodeProperty(string nodeId, string property, object value)
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
        /// Creates an initialized BehaviorTreeObject
        /// </summary>
        /// <returns>A BehaviorTreeObject</returns>
        public static BehaviorTreeObject Create() => ScriptableObject.CreateInstance<BehaviorTreeObject>().Initialize();

        #endregion
    }
}
