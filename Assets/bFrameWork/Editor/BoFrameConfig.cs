using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class BoFrameConfig : ScriptableObject
{
    //打包时生成AB包配置表的二进制路径
    public string m_ABBytePath;
    //xml文件夹路径
    public string m_XmlPath;
    //二进制文件夹路径
    public string m_BinaryPath;
    //脚本文件夹路径
    public string m_ScriptsPath;
}

[CustomEditor(typeof(BoFrameConfig))]
public class BoFrameConfigInspector : UnityEditor.Editor {
    [FormerlySerializedAs("m_ABBytePath")] public SerializedProperty mAbBytePath;
    [FormerlySerializedAs("m_XmlPath")] public SerializedProperty mXmlPath;
    [FormerlySerializedAs("m_BinaryPath")] public SerializedProperty mBinaryPath;
    [FormerlySerializedAs("m_ScriptsPath")] public SerializedProperty mScriptsPath;

    private void OnEnable()
    {
        mAbBytePath = serializedObject.FindProperty("m_ABBytePath");
        mXmlPath = serializedObject.FindProperty("m_XmlPath");
        mBinaryPath = serializedObject.FindProperty("m_BinaryPath");
        mScriptsPath = serializedObject.FindProperty("m_ScriptsPath");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(mAbBytePath, new GUIContent("ab包二进制配置路径"));
        GUILayout.Space(5);
        EditorGUILayout.PropertyField(mXmlPath, new GUIContent("Xml路径"));
        GUILayout.Space(5);
        EditorGUILayout.PropertyField(mBinaryPath, new GUIContent("二进制路径"));
        GUILayout.Space(5);
        EditorGUILayout.PropertyField(mScriptsPath, new GUIContent("配置表脚本路径"));
        GUILayout.Space(5);
        serializedObject.ApplyModifiedProperties();
    }
}

public static class BoConfig
{
    private const string BoFramePath = "Assets/sFrame/sFrame_Editor/Editor/BoFrameConfig.asset";
    public static BoFrameConfig GetBoFrame()
    {
        return AssetDatabase.LoadAssetAtPath<BoFrameConfig>(BoFramePath);
    }
}
