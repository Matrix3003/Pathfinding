using System;
using UnityEditor;
using UnityEngine;

namespace EditorForge.CustomHeader
{
    internal sealed class Header : MonoBehaviour
    {
        #if UNITY_EDITOR
        [field:SerializeField] public Color TextColor { get; set; } = Color.white; 
        [field:SerializeField] public Color BackgroundColor { get; set; } = Color.red;

        private void OnValidate() => EditorApplication.RepaintHierarchyWindow();

        #endif
    }
}
