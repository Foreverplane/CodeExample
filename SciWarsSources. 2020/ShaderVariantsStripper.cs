#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Callbacks;
using UnityEditor.Rendering;
using UnityEngine;
using Debug = UnityEngine.Debug;

internal class ShaderVariantsStripper : IPreprocessShaders {
    private readonly ShaderVariantCollection _collection;
#if STRIPPING_LOG_ENABLED
    private static StreamWriter _shaderStripLogFile;
#endif
    public ShaderVariantsStripper() {
        _collection = Resources.Load<ShaderVariantCollection>("ShaderVariants/ShaderVariants");
#if STRIPPING_LOG_ENABLED
        if (_shaderStripLogFile == null) {
            var timeStamp = $"{DateTime.Now.Day}d_{DateTime.Now.Month}m__{DateTime.Now.Hour}h_{DateTime.Now.Minute}m";
            _shaderStripLogFile = new StreamWriter("Stripped_ShaderLog_" + timeStamp + ".txt");
        }
#endif
    }
    public int callbackOrder { get { return 0; } }

    public bool KeepVariant(Shader shader, ShaderSnippetData snippet, ShaderCompilerData shaderVariant) {
        var resultKeepVariant = true;

        var shaderKeywords = shaderVariant.shaderKeywordSet.GetShaderKeywords();
        var keywords = new string[shaderKeywords.Length];
        var i = 0;
        foreach (var shaderKeyword in shaderVariant.shaderKeywordSet.GetShaderKeywords()) {
            keywords[i] = shaderKeyword.GetKeywordName();
            i++;
        }

        try {
            var variant = new ShaderVariantCollection.ShaderVariant(shader, snippet.passType, keywords); // VideoDecodeAndroid is fucked up on this line
            resultKeepVariant = _collection != null && _collection.Contains(variant);

            WriteVariantLog(shader, snippet, resultKeepVariant, keywords);
        }
        catch (Exception e) {
            Debug.LogWarning(e.Message);
        }



        return resultKeepVariant;
    }
    [Conditional("STRIPPING_LOG_ENABLED")]
    private static void WriteVariantLog(Shader shader, ShaderSnippetData snippet, bool resultKeepVariant, string[] keywords) {
#if STRIPPING_LOG_ENABLED
        if (!resultKeepVariant) {
            var prefix = "not keepeing VARIANT: " + shader.name + " (";
            if (snippet.passName.Length > 0)
                prefix += snippet.passName + ", ";
            prefix += snippet.shaderType.ToString() + ") ";
            var log = prefix;
            var builder = new System.Text.StringBuilder();
            builder.Append(log);
            foreach (var t in keywords)
                builder.Append($"{t} ");
            log = builder.ToString();

            _shaderStripLogFile.Write(log + "\n");
        }
#endif
    }

    public void OnProcessShader(
        Shader shader, ShaderSnippetData snippet, IList<ShaderCompilerData> shaderVariants) {
#if STRIPPING_LOG_ENABLED
        var inputShaderVariantCount = shaderVariants.Count;
#endif
        for (var i = 0; i < shaderVariants.Count; ++i) {
            var keepVariant = KeepVariant(shader, snippet, shaderVariants[i]);
            if (!keepVariant) {
                shaderVariants.RemoveAt(i);
                --i;
            }
        }
#if STRIPPING_LOG_ENABLED
        var percentage = (float)shaderVariants.Count / inputShaderVariantCount * 100f;
        _shaderStripLogFile.Write("STRIPPING(" + snippet.shaderType.ToString() + ") = Kept / Total = " + shaderVariants.Count + " / " + inputShaderVariantCount + " = " + percentage + "% of the generated shader variants remain in the player data\n");
#endif
    }
    [PostProcessBuild(1)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {
#if STRIPPING_LOG_ENABLED
        _shaderStripLogFile.Close();
        _shaderStripLogFile = null;
#endif
    }
}

#endif