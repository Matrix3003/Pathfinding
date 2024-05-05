using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace EditorForge.CustomScripts
{
    public static class ScriptGenerator
    {
        private const string TEMPLATES_FOLDER_PATH = "EditorForge\\Framework\\_Scripts\\CustomScripts\\Templetes";

        private const string FILE_SUBSTITUTE = "{file}"; 
        private const string NAMESPACE_SUBSTITUTE = "{namespace}"; 

        public static string GetFileContent(string templateName, string fileName, string namespaceName = null)
        {
            namespaceName ??= Application.productName; 
            
            string templatePath = Path.Combine(Application.dataPath, TEMPLATES_FOLDER_PATH, templateName);

            if (File.Exists(templatePath))
            {
                try
                {
                    string fileContent = File.ReadAllText(templatePath);
                    return ReplaceTempleteFileName(fileContent, fileName);
                }
                catch (IOException e)
                {
                    Debug.LogError($"Erro ao ler o arquivo {templateName}: {e.Message}");
                }
            }
            else
            {
                Debug.LogWarning($"Arquivo {templateName} não encontrado no caminho {templatePath}");
            }

            return null;
        }

        private static string ReplaceTempleteFileName(string content, string fileName)
        {
            var res = content.Replace(FILE_SUBSTITUTE, fileName); 

            return res; 
        }

        public static void RepleceFileNamespace(string filePath)
        {
            string asmdefFileContent = File.ReadAllText(filePath);

            // Regex para encontrar a propriedade "rootNamespace"
            Match match = Regex.Match(asmdefFileContent, "\"rootNamespace\"\\s*:\\s*\"([^\"]*)\"");
            
            if (match.Success)
            {
                string rootNamespace = match.Groups[1].Value;
                Debug.Log("Namespace do Assembly Definition: " + rootNamespace);
            }
            else
            {
                Debug.LogWarning("Não foi possível encontrar o namespace no arquivo de definição do Assembly.");
            }
        } 
    }
}
