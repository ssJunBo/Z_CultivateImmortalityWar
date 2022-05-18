﻿using System.Collections.Generic;
using bFrameWork.Game.Base;
using bFrameWork.Game.DataTool;
using UnityEngine;

/// <summary>
/// 配置表路径
/// </summary>
public class CFG
{
    public const string TABLE_MONSTER = "Assets/GameData/Data/Binary/MonsterData.bytes";
    public const string TABLE_BUFF = "Assets/GameData/Data/Binary/BuffData.bytes";
}

public class ConfigerManager : Singleton<ConfigerManager>
{
    //存储所有已经加载的配置表 游戏刚开始进行配置表加载 一般不进行释放
    protected Dictionary<string, ExcelBase> m_AllExcelData = new Dictionary<string, ExcelBase>();

    /// <summary>
    /// 加载数据表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path">二进制路径</param>
    /// <returns></returns>
    public T LoadData<T>(string path) where T : ExcelBase
    {
        if (string.IsNullOrEmpty(path))
        {
            return null;
        }

        if (m_AllExcelData.ContainsKey(path))
        {
            Debug.LogError("重复加载相同配置文件：" + path);
            return m_AllExcelData[path] as T;
        }

        T data = BinarySerializeOpt.BinaryDeserialize<T>(path);

#if UNITY_EDITOR
        if (data == null)
        {
            Debug.Log(path + "不存在，从xml加载数据了！");
            string xmlPath = path.Replace("Binary", "Xml").Replace(".bytes", ".xml");
            data = BinarySerializeOpt.XmlDeserialize<T>(xmlPath);
        }
#endif

        if (data != null)
        {
            data.Init();
        }

        m_AllExcelData.Add(path, data);
        return data;
    }

    /// <summary>
    /// 根据路径查找数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public T FindData<T>(string path) where T : ExcelBase
    {
        if (string.IsNullOrEmpty(path))
            return null;

        ExcelBase excelBase = null;
        if (m_AllExcelData.TryGetValue(path, out excelBase))
        {
            return excelBase as T;
        }
        else
        {
            excelBase = LoadData<T>(path);
        }
        return null;
    }
}


