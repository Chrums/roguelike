using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.Tilemaps;

namespace Fizz6.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Tilemap), true)]
    public class TilemapEditorExt : UnityEditor.Editor
    {
        private UnityEditor.Editor defaultEditor = null;
        private Tilemap tilemap = null;

        public override void OnInspectorGUI()
        {
            if (!defaultEditor) return;
            defaultEditor.OnInspectorGUI();
            if (GUILayout.Button("Compress Bounds")) tilemap.CompressBounds();
        }

        private void OnEnable()
        {
            defaultEditor = CreateEditor(targets, Type.GetType("UnityEditor.TilemapEditor, UnityEditor"));
            tilemap = target as Tilemap;
            TryInvoke(nameof(OnEnable));
        }

        private void OnDisable()
        {
            TryInvoke(nameof(OnDisable));
            DestroyImmediate(defaultEditor);
        }

        private void OnSceneGUI() => TryInvoke(nameof(OnSceneGUI));

        private void TryInvoke(string methodName)
        {
            if (!defaultEditor) return;
            var method = defaultEditor
                .GetType()
                .GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            method?.Invoke(defaultEditor, null);
        }
    }
}