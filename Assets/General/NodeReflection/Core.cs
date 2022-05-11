using NodeReflection.Data;
using NodeReflection.Utils;
using System.Collections.Generic;

namespace NodeReflection
{
    /// <summary>
    /// Provides the metadata from the node types in the current assembly
    /// </summary>
    public class Engine
    {
        // TODO : NodeTypeAdded, NodeTypeDeleted and NodeTypeModified events

        #region Properties

        /// <summary>
        /// Gets the current nodes types' metadata
        /// </summary>
        public Dictionary<string, NodeMetadata> Metadata { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the metadata
        /// </summary>
        public Dictionary<string, NodeMetadata> Update()
        {
            this.Metadata = StateGeneration.GetNodeMetadataObjects();

            return this.Metadata;
        }

        #endregion
    }
}
