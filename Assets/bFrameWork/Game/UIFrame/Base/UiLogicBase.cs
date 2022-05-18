using bFrameWork.Game.ResourceFrame;
using bFrameWork.Game.Tools;
using Common;
using Managers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace bFrameWork.Game.UIFrame.Base
{
    public abstract class UiLogicBase
    {
        protected abstract string Path { get; set; }
        public abstract EUiID Id { get; set; }

        private bool isShowing;
        private GameObject mObj;
        private UiDialogBase mDialog;

        public virtual void Open()
        {
            if (isShowing) return;
            DoOpen();
        }

        public virtual void Close()
        {
            UiLogicManager.Instance.RemoveUi();

            if (mDialog != null)
            {
                isShowing = false;
                mDialog.Release();
                mDialog = null;
            }
            
            if (mObj != null)
            {
                Object.Destroy(mObj);
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
                mObj = GameObject.Instantiate(obj, GameManager.Instance.ui2DTransform) as GameObject;

                if (mObj != null)
                {
                    mDialog = mObj.GetComponent<UiDialogBase>();

                    if (mDialog == null)
                    {
                        Debug.LogError("cant find designer component : " + obj.name);
                    }
                    else
                    {
                        InitLogic();

                        mDialog.SetLogic(this);

                        mDialog.Init();
                        //延迟一帧，当ui真正绘制出来以后，在调用ShowFinished 这样一些坐标转换，和一些UI操作才不会出错
                        //UI的显示操作都应该放在ShowFinished中去做，而不应该在Init中去做 
                        TimeHelper.Instance.DelayHowManyFramesAfterCallBack(1, () => { mDialog.ShowFinished(); });
                    }

                    UiLogicManager.Instance.PushUi(this);
                }
                else
                {
                    //加载窗口失败，返回初始化失败
                    Close();
                    Debug.LogError("加载窗口失败！path = " + path);
                }
            }
        }

        //注册游戏逻辑的委托事件
        protected virtual void InitLogic()
        {

        }
    }
}
