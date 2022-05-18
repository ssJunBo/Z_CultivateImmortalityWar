using System.Collections.Generic;
using bFrameWork.Game.Base;
using Helpers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace bFrameWork.Game.ResourceFrame
{
    public class ResourcesManager : Singleton<ResourcesManager>
    {
        /// <summary>
        /// 缓存使用的资源列表  key = 路径 
        /// </summary>
        private readonly Dictionary<string, GameObject> _assetDic = new Dictionary<string, GameObject>();

        // 资源加载完成回调
        public delegate void DelegateResourceLoaded(string path, Object obj);

        /// <summary>
        /// 同步加载资源 针对给ObjectManager的接口
        /// </summary>
        /// <param name="path"></param>
        /// <param name="onLoaded"></param>
        public void LoadResource(string path, DelegateResourceLoaded onLoaded)
        {
            var obj = Resources.Load(path);

            onLoaded?.Invoke(path, obj);
        }

        /// <summary>
        /// 同步资源加载 外部直接调用 仅加载不需要实例化的资源 例如texture 音频等
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        public static T LoadResource<T>(string path) where T : Object
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            var needT = Resources.Load<T>(path);

            if (needT==null)
            {
                Debug.LogError("加载失败！");
                return null;
            }

            return needT;
        }
    }
}