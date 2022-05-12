using System;

namespace NodeReflection
{
    /// <summary>
    /// Custom attribute for custom node
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class VisualNodeAttribute
        : Attribute
    {
        #region Public Properties

        /// <summary>
        /// Gets the description of the node
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets if the children should be displayed as a block
        /// </summary>
        public bool DisplayAsBlock { get; private set; }

        /// <summary>
        /// Gets the name of the node
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the path of the node category
        /// </summary>
        public string Path { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="name">Node name, for display purposes</param>
        /// <param name="description">Node description</param>
        /// <param name="displayAsBlock">Indicates if the children should be displayed as a block</param>
        /// <param name="path">Path of the node in the editor</param>
        public VisualNodeAttribute(string name = "", string description = "", bool displayAsBlock = false, string path = "Custom")
        {
            this.Description = description;
            this.DisplayAsBlock = displayAsBlock;
            this.Name = name;
            this.Path = path;
        }

        #endregion
    }
}
