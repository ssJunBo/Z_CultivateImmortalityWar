using System;
using bFrameWork.Game.ResourceFrame;
using Common;
using Helpers;
using Managers;
using UnityEngine;
using Object = UnityEngine.Object;
using TimeHelper = bFrameWork.Game.Tools.TimeHelper;

namespace bFrameWork.Game.UIFrame.Base
{
    public abstract class UiLogicBase
    {
        protected abstract string Path { get; }
        // 具体ui
        protected abstract EUiID UiId { get; }
        protected virtual EUiLayer UiLayer => EUiLayer.High_2D;

        private bool isShowing;
        private GameObject mObj;
        private UiDialogBase mDialog;

        private GameManager gameManager;        
        public virtual void Open()
        {
            if (isShowing) return;
            gameManager = GameManager.Instance;
            
            DoOpen();
        }

        public virtual void Close()
        {
            if (mDialog != null)
            {
                isShowing = false;
                mDialog.Release();
            }

            if (mObj != null)
            {
                mObj.SetRealActive(false);
                mObj.transform.SetParent(gameManager.recyclePoolTrs);
                mObj.transform.localPosition=Vector3.zero;
            }
        }

        //实际打开
        private void DoOpen()
        {
            isShowing = true;

            ResourcesManager.Instance.LoadResource(Path, HandleUiResourceOk);
        }

        private void HandleUiResourceOk(string path, Object obj)
        {
            if (!isShowing) return;

            if (obj != null)
            {
                var parentTrs = GetParentTrs();
                mDialog = gameManager.UiManager.GetUiDialog(UiId);

                if (mDialog != null)
                {
                    mObj = mDialog.gameObject;
                    mObj.SetRealActive(true);
                    mObj.transform.SetParent(parentTrs);
                    mObj.transform.localPosition = Vector3.zero;
                    mObj.transform.SetAsLastSibling();
                }
                else
                {
                    mObj = Object.Instantiate(obj, parentTrs) as GameObject;
                    if (mObj == null)
                    {
                        //加载窗口失败，返回初始化失败
                        Debug.LogError("加载窗口失败！path = " + path);
                        return;
                    }

                    mDialog = mObj.GetComponent<UiDialogBase>();

                    if (mDialog == null)
                    {
                        Debug.LogError("cant find designer component : " + obj.name);
                        return;
                    }

                    gameManager.UiManager.AddUiDialog(UiId, mDialog);
                }

                InitLogic();

                mDialog.SetLogic(this);

                mDialog.Init();
                
                //延迟一帧，当ui真正绘制出来以后，在调用ShowFinished 这样一些坐标转换，和一些UI操作才不会出错
                //UI的显示操作都应该放在ShowFinished中去做，而不应该在Init中去做 
                TimeHelper.Instance.DelayHowManyFramesAfterCallBack(1, () => { mDialog.ShowFinished(); });

                gameManager.UiManager.PushUi(this);
            }
        }

        private Transform GetParentTrs()
        {
            Transform parentTrs = null;
            switch (UiLayer)
            {
                case EUiLayer.Low_2D:
                    parentTrs = gameManager.ui2DTrsLow;
                    break;
                case EUiLayer.High_2D:
                    parentTrs = gameManager.ui2DTrsHigh;
                    break;
            }
            
            return parentTrs;
        }
        
        //注册游戏逻辑的委托事件
        protected virtual void InitLogic()
        {

        }
    }
}
