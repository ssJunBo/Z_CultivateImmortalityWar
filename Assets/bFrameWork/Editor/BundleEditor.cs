using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using bFrameWork.Editor;
using UnityEditor;
using UnityEngine;

namespace bFrame.Editor
{
    public static class BundleEditor
    {
        private static string m_BundleTargetPath = Application.dataPath + "/../AssetBundle/" + EditorUserBuildSettings.activeBuildTarget.ToString();

        private static string m_BundleTargetLoaclPath = Application.streamingAssetsPath;
        private static string ABCONFIGPATH = "Assets/sFrame/sFrame_Editor/Editor/Resource/ABConfig.asset";
        private static string ABBYTEPATH = BoConfig.GetBoFrame().m_ABBytePath;
        //"Assets/GameData/Data/ABData/AssetBundleConfig.bytes";

        //key 是ab包名  value 是路径  所有文件夹ab包dic
        private static Dictionary<string, string> m_AllFileDir = new Dictionary<string, string>();
        //过滤list
        private static List<string> m_AllFileAB = new List<string>();
        //单个prefab的ab包
        private static Dictionary<string, List<string>> m_AllPrefabDir = new Dictionary<string, List<string>>();
        //储存所有有效路径
        private static List<string> m_ConfigFil = new List<string>();

        //给外部调用 打包使用
        public static void Build()
        {
            DataEditor.AllXmlToBinary();
            m_AllFileDir.Clear();
            m_AllFileAB.Clear();
            m_AllPrefabDir.Clear();
            m_ConfigFil.Clear();
            ABConfig abConfig = AssetDatabase.LoadAssetAtPath<ABConfig>(ABCONFIGPATH);
            foreach (ABConfig.FileDirABName fileDir in abConfig.m_AllFileDirAB)
            {
                if (m_AllFileDir.ContainsKey(fileDir.ABName))
                {
                    Debug.LogError("AB包配置名字重复，请检查！");
                }
                else
                {
                    m_AllFileDir.Add(fileDir.ABName, fileDir.Path);
                    m_AllFileAB.Add(fileDir.Path);
                    m_ConfigFil.Add(fileDir.Path);
                }
            }

            //返回GUID 
            string[] allStr = AssetDatabase.FindAssets("t:Prefab", abConfig.m_AllPrefabPath.ToArray());
            for (int i = 0; i < allStr.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(allStr[i]);
                //Debug.Log("prefabs文件夹下的所有prefab文件路径：" + path);
                EditorUtility.DisplayProgressBar("查找Prefab", "Prefab:" + path, i * 1.0f / allStr.Length);
                m_ConfigFil.Add(path);

                //检查过滤列表是否包含该路径
                if (!ContainAllFileAb(path))
                {
                    //找到pre
                    GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                    string[] allDepend = AssetDatabase.GetDependencies(path);
                    List<string> allDependPath = new List<string>();
                    for (int j = 0; j < allDepend.Length; j++)
                    {
                        //Debug.Log(allDepend[j]);
                        if (!ContainAllFileAb(allDepend[j]) && !allDepend[j].EndsWith(".cs"))
                        {
                            m_AllFileAB.Add(allDepend[j]);
                            allDependPath.Add(allDepend[j]);
                        }
                    }
                    if (m_AllPrefabDir.ContainsKey(obj.name))
                    {
                        Debug.LogError("存在相同名字Prefab！名字 " + obj.name);
                    }
                    else
                    {
                        m_AllPrefabDir.Add(obj.name, allDependPath);
                    }
                }
            }

            foreach (string name in m_AllFileDir.Keys)
            {
                SetAbName(name, m_AllFileDir[name]);
            }

            foreach (string name in m_AllPrefabDir.Keys)
            {
                SetAbName(name, m_AllPrefabDir[name]);
            }

            #region 加上下列代码,耗时会非常长长长！！！！
            //AssetDatabase.SaveAssets();
            //AssetDatabase.Refresh();
            #endregion

            BuildAssetBundle(m_BundleTargetPath);

            string[] oldABName = AssetDatabase.GetAllAssetBundleNames();
            for (int i = 0; i < oldABName.Length; i++)
            {
                AssetDatabase.RemoveAssetBundleName(oldABName[i], true);
                EditorUtility.DisplayProgressBar("清除AB包名", "名字" + oldABName[i], i / 1.0f / oldABName.Length);
            }

            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();
        }

        [MenuItem("Tools/AB包/打ab包到StreamingAssets下")]//编辑器下使用
        public static void BuildAb()
        {
            m_AllFileDir.Clear();
            m_AllFileAB.Clear();
            m_AllPrefabDir.Clear();
            m_ConfigFil.Clear();
            ABConfig abConfig = AssetDatabase.LoadAssetAtPath<ABConfig>(ABCONFIGPATH);
            foreach (ABConfig.FileDirABName fileDir in abConfig.m_AllFileDirAB)
            {
                if (m_AllFileDir.ContainsKey(fileDir.ABName))
                {
                    Debug.LogError("AB包配置名字重复，请检查！");
                }
                else
                {
                    m_AllFileDir.Add(fileDir.ABName, fileDir.Path);
                    m_AllFileAB.Add(fileDir.Path);
                    m_ConfigFil.Add(fileDir.Path);
                }
            }

            //返回GUID 
            string[] allStr = AssetDatabase.FindAssets("t:Prefab", abConfig.m_AllPrefabPath.ToArray());
            for (int i = 0; i < allStr.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(allStr[i]);
                //Debug.Log("prefabs文件夹下的所有prefab文件路径：" + path);
                EditorUtility.DisplayProgressBar("查找Prefab", "Prefab:" + path, i * 1.0f / allStr.Length);
                m_ConfigFil.Add(path);

                //检查过滤列表是否包含该路径
                if (!ContainAllFileAb(path))
                {
                    //找到pre
                    GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                    string[] allDepend = AssetDatabase.GetDependencies(path);
                    List<string> allDependPath = new List<string>();
                    foreach (var t in allDepend)
                    {
                        //Debug.Log(allDepend[j]);
                        if (!ContainAllFileAb(t) && !t.EndsWith(".cs"))
                        {
                            m_AllFileAB.Add(t);
                            allDependPath.Add(t);
                        }
                    }
                    if (m_AllPrefabDir.ContainsKey(obj.name))
                    {
                        Debug.LogError("存在相同名字Prefab！名字 " + obj.name);
                    }
                    else
                    {
                        m_AllPrefabDir.Add(obj.name, allDependPath);
                    }
                }
            }

            foreach (string name in m_AllFileDir.Keys)
            {
                SetAbName(name, m_AllFileDir[name]);
            }

            foreach (string name in m_AllPrefabDir.Keys)
            {
                SetAbName(name, m_AllPrefabDir[name]);
            }

            #region 加上下列代码,耗时会非常长长长！！！！
            //AssetDatabase.SaveAssets();
            //AssetDatabase.Refresh();
            #endregion

            BuildAssetBundle(m_BundleTargetLoaclPath);

            string[] oldAbName = AssetDatabase.GetAllAssetBundleNames();
            for (int i = 0; i < oldAbName.Length; i++)
            {
                AssetDatabase.RemoveAssetBundleName(oldAbName[i], true);
                EditorUtility.DisplayProgressBar("清除AB包名", "名字" + oldAbName[i], i / 1.0f / oldAbName.Length);
            }

            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();
        }

        [MenuItem("Tools/AB包/删除StreamingAssets下ab包")]//编辑器下使用
        public static void DeleteLocalAb()
        {
            string[] allBundlesName = AssetDatabase.GetAllAssetBundleNames();
            DirectoryInfo direction = new DirectoryInfo(m_BundleTargetLoaclPath);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
            foreach (var t in files)
            {
                if (File.Exists(t.FullName))
                {
                    File.Delete(t.FullName);
                }
            }
            AssetDatabase.Refresh();
        }

        private static void SetAbName(string name, string path)
        {
            AssetImporter assetImporter = AssetImporter.GetAtPath(path);
            if (assetImporter == null)
            {
                Debug.LogError("不存在此路径文件：" + path);
            }
            else
            {
                assetImporter.assetBundleName = name;
            }
        }

        private static void SetAbName(string name, List<string> paths)
        {
            foreach (var t in paths)
            {
                SetAbName(name, t);
            }
        }

        static void BuildAssetBundle(string path)
        {
            string[] allBundles = AssetDatabase.GetAllAssetBundleNames();
            //key 为全路径 value 为包名
            Dictionary<string, string> resPathDic = new Dictionary<string, string>();
            foreach (var t in allBundles)
            {
                string[] allBundlePath = AssetDatabase.GetAssetPathsFromAssetBundle(t);
                foreach (var t1 in allBundlePath)
                {
                    if (t1.EndsWith(".cs") || !ValidPath(t1))
                        continue;

                    Debug.Log("此ab包" + t + " 下面包含的资源文件路径：" + t1);
                    resPathDic.Add(t1, t);
                }
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            DeleteAB(path);
            //生成自己的配置表
            WriteData(resPathDic);

            AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.ChunkBasedCompression,
                EditorUserBuildSettings.activeBuildTarget);
            if (manifest == null)
            {
                Debug.LogError("AssetBundle 打包失败！");
            }
            else
            {
                Debug.Log("AssetBundle 打包完毕！");

            }
        }

        static void WriteData(Dictionary<string, string> resPathDic)
        {
            AssetBundleConfig config = new AssetBundleConfig {ABList = new List<ABBase>()};
            foreach (string path in resPathDic.Keys)
            {
                if (!ValidPath(path)) continue;

                ABBase abBase = new ABBase
                {
                    Path = path,
                    ABName = resPathDic[path],
                    AssetName = path.Remove(0, path.LastIndexOf("/", StringComparison.Ordinal) + 1),
                    ABDependce = new List<string>()
                };
                string[] resDefence = AssetDatabase.GetDependencies(path);
                foreach (var tempPath in resDefence)
                {
                    if (tempPath == path || path.EndsWith(".cs"))
                        continue;

                    if (resPathDic.TryGetValue(tempPath, out var abName))
                    {
                        if (abName == resPathDic[path])
                            continue;

                        if (!abBase.ABDependce.Contains(abName))
                        {
                            abBase.ABDependce.Add(abName);
                        }

                    }
                }
                config.ABList.Add(abBase);
            }

            //写入XML
            string xmlPath = Application.dataPath + "/AssetbundleConfig.xml";
            if (File.Exists(xmlPath))
                File.Delete(xmlPath);
            FileStream fileStream = new FileStream(xmlPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            StreamWriter sw = new StreamWriter(fileStream, System.Text.Encoding.UTF8);
            XmlSerializer xs = new XmlSerializer(config.GetType());
            xs.Serialize(sw, config);
            sw.Close();
            fileStream.Close();

            //写入二进制
            foreach (ABBase abBase in config.ABList)
            {
                abBase.Path = "";
            }

            FileStream fs = new FileStream(ABBYTEPATH, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            fs.Seek(0, SeekOrigin.Begin);
            fs.SetLength(0);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, config);
            fs.Close();
            AssetDatabase.Refresh();

            SetAbName("assetbundleconfig", ABBYTEPATH);
        }

        /// <summary>
        /// 删除无用的AB包
        /// </summary>
        static void DeleteAB(string tarPath)
        {
            string[] allBundlesName = AssetDatabase.GetAllAssetBundleNames();
            DirectoryInfo direction = new DirectoryInfo(tarPath);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                if (ContainAbName(files[i].Name, allBundlesName) || files[i].Name.EndsWith(".meta")
                                                                 || files[i].Name.EndsWith(".manifest") || files[i].Name.EndsWith("assetbundleconfig"))
                {
                    continue;
                }
                else
                {
                    Debug.Log("此AB包已经被删除或改名：" + files[i].Name);
                    if (File.Exists(files[i].FullName))
                    {
                        File.Delete(files[i].FullName);
                    }
                    if (File.Exists(files[i].FullName + ".manifest"))
                    {
                        File.Delete(files[i].FullName + ".manifest");
                    }
                }
            }
        }

        /// <summary>
        /// 遍历文件夹里的文件名与设置的所有AB包进行检查判断
        /// </summary>
        /// <param name="name"></param>
        /// <param name="strs"></param>
        /// <returns></returns>
        private static bool ContainAbName(string name, string[] strs)
        {
            return strs.Any(t => name == t);
        }

        /// <summary>
        /// 是否包含在已经有的AB包里 用来做ab冗余剔除
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool ContainAllFileAb(string path)
        {
            return m_AllFileAB.Any(t => path == t || path.Contains(t) && (path.Replace(t, "")[0] == '/'));
        }

        /// <summary>
        /// 是否有效路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static bool ValidPath(string path)
        {
            return m_ConfigFil.Any(path.Contains);
        }
    }
}
