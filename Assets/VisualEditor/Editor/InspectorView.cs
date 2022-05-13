﻿using BehaviorTreeSerializer.Data;
using NodeReflection.Data;
using NodeReflection.Enumerations;
using System;
using UnityEngine.UIElements;

namespace VisualEditor.Editor
{
    public class InspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

        private BehaviorTreeObject _tree;

        public InspectorView() { }

        public void UpdateSelection(BehaviorTreeObject tree, NodeEditorInstanceMetadata node)
        {
            this.Clear();

            this._tree = tree;

            foreach (var pair in node.PropertiesBool)
                this.Add(this.GenerateToggleField(node.Id, pair.Key, pair.Value));
            foreach (var pair in node.PropertiesFloat)
                this.Add(this.GenerateFloatField(node.Id, pair.Key, pair.Value));
            foreach (var pair in node.PropertiesInt)
                this.Add(this.GenerateIntField(node.Id, pair.Key, pair.Value));
            foreach (var pair in node.PropertiesString)
                this.Add(this.GenerateTextField(node.Id, pair.Key, pair.Value));
        }

        #region Private Methods

        /// <summary>
        /// Generates a float field for the inspector
        /// </summary>
        /// <param name="node">Node metadata reference</param>
        /// <param name="property">Property name</param>
        /// <param name="value">Property base value</param>
        /// <returns>A FloatField</returns>
        private VisualElement GenerateFloatField(string nodeId, string property, float value)
        {
            var floatField = new FloatField(property) { value = value };

            floatField.RegisterValueChangedCallback(eventArg => this._tree.SetNodeFloatProperty(nodeId, property, eventArg.newValue));
            return floatField;
        }

        /// <summary>
        /// Generates an integer field for the inspector
        /// </summary>
        /// <param name="node">Node metadata reference</param>
        /// <param name="property">Property name</param>
        /// <param name="value">Property base value</param>
        /// <returns>An IntegerField</returns>
        private VisualElement GenerateIntField(string nodeId, string property, int value)
        {
            var integerField = new IntegerField(property) { value = value };

            integerField.RegisterValueChangedCallback(eventArg => this._tree.SetNodeIntProperty(nodeId, property, eventArg.newValue));
            return integerField;
        }

        /// <summary>
        /// Generates a text field for the inspector
        /// </summary>
        /// <param name="node">Node metadata reference</param>
        /// <param name="property">Property name</param>
        /// <param name="value">Property base value</param>
        /// <returns>A TextField</returns>
        private VisualElement GenerateTextField(string nodeId, string property, string value)
        {
            var textField = new TextField(property) { value = value };

            textField.RegisterValueChangedCallback(eventArg => this._tree.SetNodeStringProperty(nodeId, property, eventArg.newValue));
            return textField;
        }

        /// <summary>
        /// Generates a toggle field for the inspector
        /// </summary>
        /// <param name="node">Node metadata reference</param>
        /// <param name="property">Property name</param>
        /// <param name="value">Property base value</param>
        /// <returns>A Toggle</returns>
        private VisualElement GenerateToggleField(string nodeId, string property, bool value)
        {
            var toggle = new Toggle(property) { value = value };

            toggle.RegisterValueChangedCallback(eventArg => this._tree.SetNodeBoolProperty(nodeId, property, eventArg.newValue));
            return toggle;
        }

        #endregion
    }
}