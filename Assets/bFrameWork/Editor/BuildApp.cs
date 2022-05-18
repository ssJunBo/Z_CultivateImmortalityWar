using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using bFrame.Editor;

public class BuildApp
{
    private static string m_AppName = PlayerSettings.productName;
    public static string m_AndroidPath = Application.dataPath + "/../BuildTarget/Android/";
    public static string m_IOSPath = Application.dataPath + "/../BuildTarget/IOS/";
    public static string m_WindowsPath = Application.dataPath + "/../BuildTarget/Windows/";

    [MenuItem("Build/标准包")]
    public static void Build()
    {
        //打ab包
        BundleEditor.Build();

        //生成可执行程序
        //不同平台ab包对应的路径
        string abPath = Application.dataPath + "/../AssetBundle/" + EditorUserBuildSettings.activeBuildTarget.ToString() + "/";

        Copy(abPath, Application.streamingAssetsPath);

        string savePath = "";

        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
        {
            savePath = m_AndroidPath + m_AppName + "_" + EditorUserBuildSettings.activeBuildTarget + string.Format("{0:yyyy_MM_dd_HH_mm}", DateTime.Now) + ".apk";
        }
        else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
        {
            savePath = m_IOSPath + m_AppName + "_" + EditorUserBuildSettings.activeBuildTarget + string.Format("{0:yyyy_MM_dd_HH_mm}", DateTime.Now);
        }
        else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows || EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64)
        {
            savePath = m_WindowsPath + m_AppName + "_" + EditorUserBuildSettings.activeBuildTarget + string.Format("{0:yyyy_MM_dd_HH_mm}/{1}.exe", DateTime.Now, m_AppName);
        }
        BuildPipeline.BuildPlayer(FindEnableEditorScenes(), savePath, EditorUserBuildSettings.activeBuildTarget, BuildOptions.None);

        DeleteDir(Application.streamingAssetsPath);

        switch (EditorUserBuildSettings.activeBuildTarget)
        {
            case BuildTarget.Android:
                OpenDirectory(m_AndroidPath);
                break;
            case BuildTarget.iOS:
                OpenDirectory(m_IOSPath);
                break;
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                OpenDirectory(m_WindowsPath);
                break;
        }
    }

    public static string[] FindEnableEditorScenes()
    {
        List<string> editorScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            editorScenes.Add(scene.path);
        }
        return editorScenes.ToArray();
    }

    private static void Copy(string sourceDirPath, string saveDirPath)
    {
        try
        {
            if (string.IsNullOrEmpty(sourceDirPath) || string.IsNullOrEmpty(saveDirPath))
            {
                Debug.Log("输入有为空的文件夹路径！");
                return;
            }
            sourceDirPath = sourceDirPath.Replace('\\', '/');
            saveDirPath = saveDirPath.Replace('\\', '/');

            if (!Directory.Exists(saveDirPath))
            {
                Directory.CreateDirectory(saveDirPath);
            }

            string[] files = Directory.GetFileSystemEntries(sourceDirPath);

            foreach (string file in files)
            {
                string pFilePath = saveDirPath + "\\" + Path.GetFileName(file);

                pFilePath = pFilePath.Replace('\\', '/');
                string unityFile = file.Replace('\\', '/');

                if (File.Exists(pFilePath))
                {
                    File.Delete(pFilePath);
                }
                if (!File.Exists(unityFile))
                {
                    Debug.LogError("+++++"+unityFile+" :not find");
                    continue;
                }
                File.Copy(unityFile,pFilePath,true);
            }
            string[] dirs = Directory.GetDirectories(sourceDirPath);

            foreach (var dir in dirs)
            {
                Copy(dir,saveDirPath+"\\"+Path.GetFileName(dir));
            }

        }
        catch (Exception)
        {
            Debug.LogError("无法复制：" + sourceDirPath + "  到" + saveDirPath);
        }
    }

    public static void DeleteDir(string srcPath)
    {
        try
        {
            DirectoryInfo dir = new DirectoryInfo(srcPath);
            FileSystemInfo[] fileInfo = dir.GetFileSystemInfos();
            foreach (FileSystemInfo info in fileInfo)
            {
                if (info is DirectoryInfo)
                {
                    DirectoryInfo subdir = new DirectoryInfo(info.FullName);
                    subdir.Delete(true);
                }
                else
                {
                    File.Delete(info.FullName);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    //打开指定文件夹
    public static void OpenDirectory(string path)
    {
        if (string.IsNullOrEmpty(path)) return;
        path = path.Replace("/", "\\");
        if (!Directory.Exists(path))
        {
            Debug.LogError("No Directory: " + path);
            return;
        }
        System.Diagnostics.Process.Start("explorer.exe", path);
    }

}
