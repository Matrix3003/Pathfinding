using UnityEditor;
using UnityEngine;

namespace EditorForge.CustomHeader
{
    #if UNITY_EDITOR
        [InitializeOnLoad]
        internal static class StyleRender 
        {
            static StyleRender()
            {
                EditorApplication.hierarchyWindowItemOnGUI += RenderObject; 
            }
    
            private static void RenderObject(int instanceID, Rect selectionRect)
            {
                if (EditorUtility.InstanceIDToObject(instanceID) is not GameObject obj)
                    return;
    
                if (obj.TryGetComponent<Header>(out var headlerObject))
                {
                    DrawObjectStyle(headlerObject); 
                }
    
                void DrawObjectStyle(Header header)
                {
                    EditorGUI.DrawRect(selectionRect, header.BackgroundColor); 
                    EditorGUI.LabelField(selectionRect, obj.name.ToUpper(), new GUIStyle() 
                    {
                        alignment = TextAnchor.MiddleCenter,
                        fontStyle = FontStyle.Bold,
                        normal = new GUIStyleState() 
                        {
                            textColor = header.TextColor 
                        }
                    });
                }
            }
    #endif
    }
}
