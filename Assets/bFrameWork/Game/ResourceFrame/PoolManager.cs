using System;
using System.Collections.Generic;
using bFrame.Game.Base;
using bFrame.Game.ResourceFrame;
using bFrameWork.Game.Base;

namespace bFrameWork.Game.ResourceFrame
{
    public class PoolManager : Singleton<PoolManager>
    {
        #region 类对象池使用

        private readonly Dictionary<Type, object> _mClassPoolDic = new Dictionary<Type, object>();

        /// <summary>
        /// 创建类对象池，创建完成后外面可以保存ClassObjectPool<T>，然后调用spwan和recycle来创建和回收类对象
        /// </summary>
        public ClassObjectPool<T> GetOrCreateClassPool<T>(int maxCount) where T : class, new()
        {
            Type type = typeof(T);
            if (!_mClassPoolDic.TryGetValue(type, out var outObj) || outObj == null)
            {
                ClassObjectPool<T> newPool = new ClassObjectPool<T>(maxCount);
                _mClassPoolDic.Add(type, newPool);
                return newPool;
            }
            return outObj as ClassObjectPool<T>;
        }
        #endregion  
        
        
        
    }
}
