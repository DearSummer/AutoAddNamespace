using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ScriptBuilder
{
    private StringBuilder builder;

    public ScriptBuilder()
    {
        builder = new StringBuilder();
    }

    private int indent = 0;
    private int currentCharIndex = 0;

    public void Write(string val, bool autoIndent = true)
    {
        if (autoIndent)
        {
            val = GetIndents() + val;
        }

        if (currentCharIndex == builder.Length)
        {
            builder.Append(val);
        }
        else
        {
            builder.Insert(currentCharIndex, val);
        }

        currentCharIndex += val.Length;
    
    }

    public void WriteLine(string val, bool autoIndent = true)
    {
        Write(val + "\r\n",autoIndent);
    }

    public void WriteCurlyBrackets()
    {
        string openBracket = GetIndents() + "{\r\n";
        string closeBracket = GetIndents() + "}\r\n";

        Write(openBracket + closeBracket,false);
        currentCharIndex -= closeBracket.Length;

        indent++;

    }

    public void WriteMethod(string methodName,string content)
    {
        string openBracket = GetIndents() + "{\r\n";
        string closeBracket = GetIndents() + "}\r\n";

        Write(methodName + "\r\n" + openBracket + content + "\r\n" + closeBracket);

    }
    
   

    private string GetIndents()
    {
        string str = "";
        for (var i = 0; i < indent; i++)
        {
            str += "    ";
        }

        return str;
    }

    public override string ToString()
    {
        return builder.ToString();
    }
}
