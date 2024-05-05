using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace EditorForge.CustomScripts
{
    public class ScriptWindow : EditorWindow
    {
        private const string DEFAULT_CLASS_NAME = "NewClass";
        public static bool CreateFile;

        private static readonly List<char> INVALID_CHARACTERS = new()
        {
            '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '+', '=', '[', ']',
            '{', '}', '|', '\\', ':', ';', '\'', '"', '<', '>', ',', '.', '/', '?', ' ',
            'á', 'à', 'ã', 'â', 'ä', 'é', 'è', 'ẽ', 'ê', 'ë', 'í', 'ì', 'ĩ', 'î', 'ï',
            'ó', 'ò', 'õ', 'ô', 'ö', 'ú', 'ù', 'ũ', 'û', 'ü', 'ç'
        };

        private const char CORRECTION_CHAR = '_'; 
        
        //[MenuItem("Assets/Create/CustomScript/C# Empty Class", priority = 50)]
        static void CreateCSharpEmptyClassFile(MenuCommand menuCommand)
        {
            GenerateScriptFile("Empty.txt"); 
        }

        private static void GenerateScriptFile(string templateName)
        {
            string filePath = GetSelectedFolderPath();

            if (!string.IsNullOrEmpty(filePath))
            {
                string className = ShowFileNameDialog(DEFAULT_CLASS_NAME);

                className = ValidadeChars(className);

                if (!string.IsNullOrEmpty(className) && CreateFile)
                {
                    string fileName = className + ".cs";
                    string fullPath = Path.Combine(filePath, fileName);

                    File.WriteAllText(fullPath, ScriptGenerator.GetFileContent(templateName, className));
                    ScriptGenerator.RepleceFileNamespace(fullPath); 

                    AssetDatabase.ImportAsset(fullPath, ImportAssetOptions.Default);
                    AssetDatabase.Refresh();
                }
            }
        }
        
        private static string GetSelectedFolderPath()
        {
            string folderPath = string.Empty;
            Object obj = Selection.activeObject;

            if (obj != null)
            {
                folderPath = AssetDatabase.GetAssetPath(obj.GetInstanceID());
                if (!string.IsNullOrEmpty(folderPath) && File.Exists(folderPath))
                {
                    folderPath = Path.GetDirectoryName(folderPath);
                }
            }
            return folderPath;
        }

        private static string ShowFileNameDialog(string defaultName)
        {
            CreateFile = false; 
            
            string className = defaultName;

            CreateFileDialog dialog = CreateFileDialog.CreateInstance<CreateFileDialog>();
            dialog.position = new Rect(Screen.currentResolution.width / 2, Screen.currentResolution.height / 2, 500, 200); 
            dialog.className = className;
            dialog.ShowModalUtility();

            return dialog.className;
        }

        private static string ValidadeChars(string className)
        {
            char[] classChars = className.ToCharArray(); 

            for (int i = 0; i < classChars.Length; i++)
            {
                if(INVALID_CHARACTERS.Contains(classChars[i]))
                {
                    classChars[i] = CORRECTION_CHAR; 
                }
            }

            string corretedName = new string(classChars); 
            
            return corretedName;  
        }
    }

    // Confirmation window manager
    public class CreateFileDialog : EditorWindow
    {
        public string className = "";

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Enter the file name:", EditorStyles.boldLabel);
            className = EditorGUILayout.TextField(className);

            GUILayout.Space(20);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Create", GUILayout.Width(80)))
            {
                CreateFile();
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Cancel", GUILayout.Width(80)))
            {
                Close();
            }

            EditorGUILayout.EndHorizontal();

            CheckEnterKeyPress();
        }

        private void CreateFile()
        {
            ScriptWindow.CreateFile = true;
            Close();
        }

        private void CheckEnterKeyPress()
        {
            if (Event.current.keyCode == KeyCode.Return)
            {
                CreateFile();
            }
        }
    }
}
