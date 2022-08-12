using System;
using System.Collections.Generic;
using System.Linq;
using Fizz6.Core;
using Fizz6.Core.Editor;
using UnityEditor;
using UnityEngine;

namespace Fizz6.SerializeImplementation.Editor
{
    [CustomPropertyDrawer(typeof(SerializeImplementationAttribute))]
    public class ImplementationAttributePropertyDrawer : PropertyDrawer
    {
        private const float PropertyPadding = 2.0f;
        
        private static readonly Color BorderColor = new Color32(36, 36, 36, 255);
        private static readonly Color ContainerColor = new Color32(65, 65, 65, 255);
        private const float ContainerBorder = 0.5f;
        private const float ContainerPadding = 4.0f;

        private bool foldout;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // base.OnGUI(position, property, label);

            var type = fieldInfo.FieldType;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                type = type.GenericTypeArguments.FirstOrDefault();
            }
            
            var assignableTypes = type
                .GetAssignableTypes()
                .Prepend(null)
                .ToArray();
            var assignableTypeNames = assignableTypes
                .Select(assignableType => assignableType?.FullName?.Replace("+", "/"))
                .ToArray();
            var typeNames = assignableTypes
                .Select(assignableType => assignableType?.Name ?? "None")
                .ToArray();

            var managedReferenceFullTypeName = property.GetManagedReferenceFullTypeName();
            var selectedIndex = Array.IndexOf(assignableTypeNames, managedReferenceFullTypeName);
            
            var controlPosition = new Rect(
                position.x, 
                position.y, 
                position.width, 
                EditorGUIUtility.singleLineHeight
            );

            using (var changeCheckScope = new EditorGUI.ChangeCheckScope())
            {
                var index = EditorGUI.Popup(controlPosition, label.text, selectedIndex, typeNames);
                if (changeCheckScope.changed)
                {
                    var changeType = assignableTypes[index];
                    property.managedReferenceValue = index > 0
                        ? Activator.CreateInstance(changeType) 
                        : null;
                }
            }

            var childrenSerializedProperties = property
                .GetChildren();

            if (!childrenSerializedProperties.Any()) return;

            controlPosition.y += PropertyPadding; // Between popup and container
            
            var containerHeight = GetContainerHeight(property);
            
            var borderPosition = new Rect(controlPosition.x, controlPosition.yMax, controlPosition.width, containerHeight);
            EditorGUI.DrawRect(borderPosition, BorderColor);

            controlPosition.x += ContainerBorder;
            controlPosition.width -= ContainerBorder * 2.0f;
            
            var containerPosition = new Rect(controlPosition.x, controlPosition.yMax + ContainerBorder, controlPosition.width, containerHeight - ContainerBorder * 2.0f);
            EditorGUI.DrawRect(containerPosition, ContainerColor);
            
            controlPosition.x += ContainerPadding;
            controlPosition.y += ContainerPadding;
            controlPosition.width -= ContainerPadding * 2.0f;

            foreach (var serializedProperty in childrenSerializedProperties)
            {
                controlPosition.y = controlPosition.yMax + PropertyPadding;
                controlPosition.height = EditorGUI.GetPropertyHeight(serializedProperty);
                EditorGUI.PropertyField(controlPosition, serializedProperty, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var propertyHeight = base.GetPropertyHeight(property, label);
            var containerHeight = GetContainerHeight(property);
            return containerHeight > 0.0f
                ? propertyHeight + PropertyPadding * 2.0f + containerHeight
                : propertyHeight;
        }

        private float GetContainerHeight(SerializedProperty property)
        {
            var childrenHeight = GetChildrenHeight(property);
            return childrenHeight > 0.0f
                ? childrenHeight + ContainerBorder * 2.0f + ContainerPadding * 2.0f
                : 0.0f;
        }

        private float GetChildrenHeight(SerializedProperty property)
        {
            var childrenSerializedProperties = property
                .GetChildren();
            return childrenSerializedProperties.Any()
                ? childrenSerializedProperties.Aggregate(
                    PropertyPadding, // Above children
                    (current, serializedProperty) => current + EditorGUI.GetPropertyHeight(serializedProperty) + PropertyPadding // Below each child
                )
                : 0.0f;
        }
    }
}