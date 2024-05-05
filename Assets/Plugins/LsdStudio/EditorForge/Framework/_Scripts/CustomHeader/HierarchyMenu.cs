using UnityEditor;
using UnityEngine;

namespace EditorForge.CustomHeader
{
    #if UNITY_EDITOR
        internal static class HierarchyMenu
        {
            private const string MANAGER_OBJ_NAME = "Managers";      
            private const string SETUP_OBJ_NAME = "Setup";      
            private const string ENVIROMENT_OBJ_NAME = "Enviroment";      
            private const string UI_OBJ_NAME = "UI";      
            private const string TESTS_OBJ_NAME = "Tests";      
            
            [MenuItem("GameObject/Editor Forge/Header/Managers")]
            static void CreateCustomHeadlerManager(MenuCommand menuCommand) => CreateObjectOnHierarchy(Color.red, menuCommand, MANAGER_OBJ_NAME); 
    
            [MenuItem("GameObject/Editor Forge/Header/Setup")]
            static void CreateCustomHeadlerSetup(MenuCommand menuCommand) => CreateObjectOnHierarchy(Color.yellow, menuCommand, SETUP_OBJ_NAME); 
    
            [MenuItem("GameObject/Editor Forge/Header/Enviroment")]
            static void CreateCustomHeadlerEnviroment(MenuCommand menuCommand) => CreateObjectOnHierarchy(Color.green, menuCommand, ENVIROMENT_OBJ_NAME); 
    
            [MenuItem("GameObject/Editor Forge/Header/UI")]
            static void CreateCustomHeadlerUI(MenuCommand menuCommand) => CreateObjectOnHierarchy(Color.blue, menuCommand, UI_OBJ_NAME); 
    
            [MenuItem("GameObject/Editor Forge/Header/Tests")]
            static void CreateCustomHeadlerTests(MenuCommand menuCommand) => CreateObjectOnHierarchy(Color.gray, menuCommand, TESTS_OBJ_NAME); 
    
            [MenuItem("GameObject/Editor Forge/Header/Create All")]
            static void CreateCustomHeadlers(MenuCommand menuCommand) 
            {
                CreateObjectOnHierarchy(Color.yellow, menuCommand, SETUP_OBJ_NAME);
                CreateObjectOnHierarchy(Color.red, menuCommand, MANAGER_OBJ_NAME); 
                CreateObjectOnHierarchy(Color.green, menuCommand, ENVIROMENT_OBJ_NAME); 
                CreateObjectOnHierarchy(Color.blue, menuCommand, UI_OBJ_NAME); 
            } 
    
            private static void CreateObjectOnHierarchy(Color objectColor, MenuCommand menuCommand, string objectName)
            {
                var obj = new GameObject(objectName); 
    
                Undo.RegisterCreatedObjectUndo(obj, "Create Header"); 
    
                GameObjectUtility.SetParentAndAlign(obj, menuCommand.context as GameObject); 
                obj.AddComponent<Header>().BackgroundColor = objectColor; 
    
                Selection.activeObject = obj; 
            }
        }
    #endif
}
