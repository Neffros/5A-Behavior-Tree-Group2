﻿using System;

namespace NodeReflection
{
    /// <summary>
    /// Custom attribute for node's properties
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ExposedInVisualEditorAttribute
        : Attribute
    {
        #region Public Properties

        /// <summary>
        /// Gets the name of the node
        /// </summary>
        public string Name { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="name">Property name, for display purposes</param>
        public ExposedInVisualEditorAttribute(string name = "")
        {
            this.Name = name;
        }

        #endregion
    }
}
