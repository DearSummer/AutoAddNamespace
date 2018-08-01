using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;



public class AutoAddNamespace : UnityEditor.AssetModificationProcessor
{

    private static readonly string authorCode =
        "//-------------------------------------------\r\n" +
        "//  author: \r\n" +
        "//  description:  \r\n" +
        "//-------------------------------------------\r\n";

    private static readonly string headCode =
        "using System;\r\n" +
        "using System.Collections;\r\n" +
        "using System.Collections.Generic;\r\n" +
        "using UnityEngine;\r\n \r\n \r\n";


    public static void OnWillCreateAsset(string path)
    {
        path = path.Replace(".meta", "");
        if (path.EndsWith(".cs"))
        {
            string allText = "";
            allText += File.ReadAllText(path);
            var className = GetClassName(allText);
            if (className != "")
            {
                CreateClass(path,className);
            }
        }
    }

    private static void CreateClass(string path, string className)
    {
        ScriptBuilder scriptBuilder = new ScriptBuilder();

        scriptBuilder.WriteLine("namespace " + ProjectSetting.ProjectRootNamespace + GetNamespace(path));
        scriptBuilder.WriteCurlyBrackets();


        scriptBuilder.WriteLine("public class #SCRIPTNAME# : MonoBehaviour");
        scriptBuilder.WriteCurlyBrackets();

        scriptBuilder.WriteLine("// Use this for initialization");
        scriptBuilder.WriteMethod("void Start ()","");
    
        scriptBuilder.WriteLine("");
        scriptBuilder.WriteLine("// Update is called once per frame");
        scriptBuilder.WriteMethod("void Update ()","");


        string scriptText = headCode + authorCode + scriptBuilder.ToString();
        scriptText = scriptText.Replace("#SCRIPTNAME#", className);
        File.WriteAllText(path,scriptText);
    }

    private static string GetNamespace(string path)
    {
        string[] pathArr = path.Split('/');
        string namespaceStr = "";
        int scriptRootIndex = 0;
        int csFileNameIndex = 0;
        for (int i = 0; i < pathArr.Length; i++)
        {
            if (pathArr[i] == ProjectSetting.ProjectRootSprictsDir)
            {
                scriptRootIndex = i;
            }

            if (!pathArr[i].Contains(".cs"))
                continue;

            csFileNameIndex = i;
            break;
        }

        for (int i = scriptRootIndex + 1; i < csFileNameIndex; i++)
        {
            namespaceStr += ".";
            namespaceStr += FristLetterUppercase(pathArr[i]);
        }

        return namespaceStr;

    }

    private static string FristLetterUppercase(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return "Null";
        }

        if (str.Length == 1)
        {
            return str.ToUpper();
        }

        char fristStr = str[0];
        string otherStr = str.Substring(1);

        return fristStr.ToString().ToUpper() + otherStr;

    }

    private static string GetClassName(string allText)
    {
        string patterm = "public class ([A-Za-z0-9_]+)\\s*:\\s*MonoBehaviour {\\s*\\/\\/ Use this for initialization\\s*void Start \\(\\) {\\s*}\\s*\\/\\/ Update is called once per frame\\s*void Update \\(\\) {\\s*}\\s*}";
        Match match = Regex.Match(allText, patterm);
        return match.Success ? match.Groups[1].Value : "";
    }
}
