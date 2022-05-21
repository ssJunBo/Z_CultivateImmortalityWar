using System;
using System.Collections.Generic;
using bFrameWork.Game.Base;
using UnityEngine;

namespace bFrame.Game.UIFrame
{
    public enum EDispatchMsg
    {
        None = 0,
        Ui=100,
        UiOpen = 101,
        UiOpenFinish = 102,
        UiClose = 103,
    }

    public class MessageDispatcher : Singleton<MessageDispatcher>
    {
        public delegate void DelegateRecevieMsg(DelegateParam delegateParam);

        /// <summary>
        /// 上下文
        /// </summary>
        private class Context
        {
            private int _contextId;

            public Context(int id)
            {
                _contextId = id;
            }

            private readonly Dictionary<int, DelegateRecevieMsg> _mDictMsgReceiveMsg =
                new Dictionary<int, DelegateRecevieMsg>();

            /// <summary>
            /// 注册回调
            /// </summary>
            /// <param name="nNo"></param>
            /// <param name="dl"></param>
            public void RegisterMsgCallBack(int nNo, DelegateRecevieMsg dl)
            {
                if (!_mDictMsgReceiveMsg.ContainsKey(nNo))
                    _mDictMsgReceiveMsg[nNo] = null;
                _mDictMsgReceiveMsg[nNo] += dl;
            }

            /// <summary>
            /// 卸载回调
            /// </summary>
            /// <param name="nNo"></param>
            /// <param name="dl"></param>
            public void UnRegisterMsgCallBack(int nNo, DelegateRecevieMsg dl)
            {
                if (!_mDictMsgReceiveMsg.ContainsKey(nNo))
                    return;
                if (dl != null) _mDictMsgReceiveMsg[nNo] -= dl;
            }

            public void DispatchMessage(DelegateParam delegateParam)
            {
                if (_mDictMsgReceiveMsg.TryGetValue(delegateParam.ContextId, out var dl))
                {
                    dl?.Invoke(delegateParam);
                }
            }
        }

        /// <summary>
        /// 所有的上下文环境
        /// </summary>
        private Dictionary<int, Context> _contexts = new Dictionary<int, Context>();

        public void AddContext(int id)
        {
            if (_contexts.ContainsKey(id))
            {
                throw new Exception("There are some context already in this solt");
            }

            _contexts[id] = new Context(id);
        }

        public void RemoveContext(int id)
        {
            if (!_contexts.ContainsKey(id))
            {
                throw new Exception("There are some context already not in this solt");
            }

            //移除
            _contexts.Remove(id);
        }

        private Context GetContext(int id, bool create = false)
        {
            if (!_contexts.ContainsKey(id))
            {
                if (create)
                {
                    _contexts[id] = new Context(id);
                }
                else
                    throw new Exception("There are some context already in this slot");
            }

            return _contexts[id];
        }

        private const int DefaultContextId = 0;

        public void Init()
        {
            //添加默认域 当派发者或者注册时 不填context id 则添加入到默认域
            AddContext(DefaultContextId);
        }

        /// <summary>
        /// 注册回调
        /// </summary>
        /// <param name="nNo"></param>
        /// <param name="dl"></param>
        /// <param name="contextId"></param>
        public void RegisterMsgCallback(int nNo, DelegateRecevieMsg dl, int contextId = 0)
        {
            var context = GetContext(contextId, true);
            context.RegisterMsgCallBack(nNo, dl);

            /*
             * 当欲添加进的上下文环境不是默认上下文时
             * 同时将此事件监听添加进默认的上下文环境
             */
            if (contextId != DefaultContextId)
            {
                context = GetContext(DefaultContextId);
                context.RegisterMsgCallBack(nNo, dl);
            }
        }

        /// <summary>
        /// 卸载回调
        /// </summary>
        /// <param name="nNo"></param>
        /// <param name="dl"></param>
        /// <param name="contextId"></param>
        public void UnRegisterMsgCallback(int nNo, DelegateRecevieMsg dl, int contextId = 0)
        {
            var context = GetContext(contextId);
            context.UnRegisterMsgCallBack(nNo, dl);

            if (contextId != DefaultContextId)
            {
                context = GetContext(DefaultContextId);
                context.UnRegisterMsgCallBack(nNo, dl);
            }
        }

        /// <summary>
        /// 派发消息 (不存在延时的时候直接派发，所以需要注意循环调用的问题，消息回调之间不要成环)
        /// </summary>
        /// <param name="nNo"></param>
        /// <param name="objPara"></param>
        /// <param name="fDelay"></param>
        /// <param name="contextId"></param>
        public void DispatchMessage(int nNo, object objPara = null, float fDelay = 0f, int contextId = 0)
        {
            DelegateParam delegateParam = new DelegateParam(contextId, nNo, fDelay);
            DispatchMessage(delegateParam);
        }

        public void DispatchMessage(EDispatchMsg eDispatchMsg, object objPara = null, float fDealy = 0,
            int contextId = 0)
        {
        }

        List<DelegateParam> mLtDelayDelegateParams = new List<DelegateParam>();
        List<DelegateParam> mLtDelayAddDelegateParams = new List<DelegateParam>();

        private void InsertDelayMessage(DelegateParam delegram)
        {
            bool bInserted = false;
            for (int i = 0; i < mLtDelayDelegateParams.Count; i++)
            {
                if (mLtDelayDelegateParams[i].DelayTime <= delegram.DelayTime)
                {
                    mLtDelayDelegateParams.Insert(i, delegram);
                    bInserted = true;
                    break;
                }
            }

            if (!bInserted)
            {
                mLtDelayDelegateParams.Add(delegram);
            }
        }

        private void DispatchMessage(DelegateParam delegateParam)
        {
            _bDispatching = true;

            int contextId = delegateParam.ContextId;
            var context = GetContext(contextId, true);
            context.DispatchMessage(delegateParam);

            _bDispatching = false;
        }

        private void DelayDispatchMessage(DelegateParam delegateParam)
        {
            //需要延时发送
            if (delegateParam.DelayTime > Time.realtimeSinceStartup)
            {
                if (_bDispatching)
                {
                    mLtDelayAddDelegateParams.Add(delegateParam);
                }
                else
                {
                    InsertDelayMessage(delegateParam);
                }
            }
            else
            {
                DispatchMessage(delegateParam);
            }
        }

        private bool _bDispatching = false;

        //call it every frame
        public void DispatchDelayMessage()
        {
            int nRmCount = 0;
            for (int i = 0; i < mLtDelayDelegateParams.Count; i++)
            {
                if (Time.realtimeSinceStartup >= mLtDelayDelegateParams[i].DelayTime)
                {
                    var delegram = mLtDelayDelegateParams[i];
                    DispatchMessage(delegram);
                    nRmCount = i + 1;
                }
                else
                    break;
            }

            if (mLtDelayDelegateParams.Count > 0)
            {
                foreach (var t in mLtDelayAddDelegateParams)
                {
                    InsertDelayMessage(t);
                }

                mLtDelayAddDelegateParams.Clear();
            }

            if (nRmCount > 0)
            {
                mLtDelayDelegateParams.RemoveRange(0, nRmCount);
            }
        }
    }

    public class DelegateParam
    {
        public DelegateParam(int contextId, int nMsgNo, float delayTime)
        {
            ContextId = contextId;
            NMsgNo = nMsgNo;
            DelayTime = delayTime;
        }

        public int ContextId { get; }
        public int NMsgNo { get; }
        public float DelayTime { get; }
    }
}