using bFrameWork.Game.ResourceFrame;
using Common;
using Helpers;
using Managers;
using UnityEngine;
using EUiType = Common.EUiType;
using Object = UnityEngine.Object;
using TimeHelper = bFrameWork.Game.Tools.TimeHelper;

namespace bFrameWork.Game.UIFrame.Base
{
    public abstract class UiLogicBase
    {
        protected abstract string Path { get; set; }

        // 具体ui
        public abstract EUiID UiId { get; set; }

        // ui类型 添加栈中or直接销毁
        public abstract EUiType UiType { get; set; }

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
            // UiManager.Instance.PopUi();

            if (mDialog != null)
            {
                isShowing = false;
                mDialog.Release();
                mDialog = null;
            }

            if (mObj != null)
            {
                mObj.SetRealActive(false);
                mObj.transform.SetAsFirstSibling();
                // Object.Destroy(mObj);
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
                var parentTrs = GameManager.Instance.ui2DTransform;
                mDialog = UiManager.Instance.GetUiDialog(UiId);

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

                    UiManager.Instance.AddUiDialog(UiId, mDialog);
                }

                InitLogic();

                mDialog.SetLogic(this);

                mDialog.Init();
                //延迟一帧，当ui真正绘制出来以后，在调用ShowFinished 这样一些坐标转换，和一些UI操作才不会出错
                //UI的显示操作都应该放在ShowFinished中去做，而不应该在Init中去做 
                TimeHelper.Instance.DelayHowManyFramesAfterCallBack(1, () => { mDialog.ShowFinished(); });

                if (UiType == EUiType.AddStack)
                {
                    UiManager.Instance.PushUi(this);
                }
            }
        }

        //注册游戏逻辑的委托事件
        protected virtual void InitLogic()
        {

        }
    }
}
