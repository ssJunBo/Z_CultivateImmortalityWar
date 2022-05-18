using System.Collections.Generic;
using Helpers;
using UnityEngine;

namespace bFrameWork.Game.ResourceFrame.Pool
{
    /// <summary>
    /// prefab 对象池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPool<T> where T : MonoBehaviour, IObject, new()
    {
        private readonly Queue<T> poolQueue;

        private string poolName => typeof(T).Name;

        private readonly Transform parentTrs;

        private readonly T prefab;
        private int maxCount;

        // 初始化对象池
        private ObjectPool(Transform parentTrs, T prefab)
        {
            this.parentTrs = parentTrs;
            this.prefab = prefab;
            poolQueue = new Queue<T>();
        }

        // 从池子中取出对象
        public T SpawnObj()
        {
            T result;
            if (poolQueue.Count>0)
            {
                result = poolQueue.Dequeue();
                result.gameObject.SetRealActive(true);
            }
            else
            {
                // 池子里没了 新创建一个
                result = Object.Instantiate(prefab, parentTrs, true);
            }
            
            return result;
        }

        // 回收对象
        public void Recycle(T obj)
        {
            if (poolQueue.Contains(obj))
            {
                return;
            }

            if (poolQueue.Count>maxCount)
            {
                GameObject.Destroy(obj);
            }
            else
            {
                poolQueue.Enqueue(obj);
                obj.Reset();
                obj.gameObject.SetRealActive(false);
            }
        }

        // 释放对象池中物体 最后可以手动将池子置null
        public void Destroy()
        {
            while (poolQueue.Count>0)
            {
                T obj= poolQueue.Dequeue();
                GameObject.Destroy(obj);
            }
        }
    }
}