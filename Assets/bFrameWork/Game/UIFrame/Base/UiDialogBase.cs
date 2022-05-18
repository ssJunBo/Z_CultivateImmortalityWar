using System.Collections.Generic;
using bFrame.Game.ResourceFrame;
using bFrameWork.Game.ResourceFrame;
using Common;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace bFrameWork.Game.UIFrame.Base
{
    public abstract class UiDialogBase : MonoBehaviour
    {
        //引用GameObject
        // 界面对应logic
        protected UiLogicBase UiLogic;

        //所有的Button 
        private readonly List<Button> _mAllButton = new List<Button>();

        //所有的Toggle
        private readonly List<Toggle> _mAllToggle = new List<Toggle>();

        public void SetLogic(UiLogicBase uiLogic)
        {
            UiLogic = uiLogic;
        }

        public abstract void Init();

        public abstract void ShowFinished();

        public virtual void Release()
        {
            RemoveAllButtonListener();
            RemoveAllToggleListener();
            _mAllButton.Clear();
            _mAllToggle.Clear();
        }

        // TODO 应该从图集里面取图
        public bool ChangeImageSprite(string path, Image image, bool setNativeSize = false)
        {
            if (image == null) return false;
            Sprite sp =ResourcesManager.LoadResource<Sprite>(path);
            if (sp != null)
            {
                if (image.sprite != null)
                    image.sprite = null;

                image.sprite = sp;
                if (setNativeSize)
                {
                    image.SetNativeSize();
                }

                return true;
            }

            return false;
        }


        /// <summary>
        /// 图片加载完成
        /// </summary>
        /// <param name="path"></param>
        /// <param name="obj"></param>
        /// <param name="image"></param>
        /// <param name="setNativeSize"></param>
        protected void OnLoadSpriteFinish(string path, Object obj, Image image, bool setNativeSize)
        {
            if (obj == null) return;

            Sprite sp = obj as Sprite;

            if (image == null) return;

            if (image.sprite != null)
                image.sprite = null;

            image.sprite = sp;

            if (setNativeSize)
            {
                image.SetNativeSize();
            }
        }

        /// <summary>
        /// 移除所有的Button事件
        /// </summary>
        private void RemoveAllButtonListener()
        {
            _mAllButton.ForEach(btn => btn.onClick.RemoveAllListeners());
        }

        /// <summary>
        /// 移除所有的toggle事件
        /// </summary>
        private void RemoveAllToggleListener()
        {
            _mAllToggle.ForEach(toggle => toggle.onValueChanged.RemoveAllListeners());
        }

        /// <summary>
        /// 添加Button事件监听 
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="action"></param>
        protected void AddButtonClickListener(Button btn, UnityEngine.Events.UnityAction action)
        {
            if (btn == null) return;
            
            if (!_mAllButton.Contains(btn))
            {
                _mAllButton.Add(btn);
            }

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(action);
            btn.onClick.AddListener(() =>
            {
                //TODO 放在声音系统中去处理
            });
        }

        /// <summary>
        /// Toggle事件监听
        /// </summary>
        /// <param name="toggle"></param>
        /// <param name="action"></param>
        public void AddToggleClickListener(Toggle toggle, UnityEngine.Events.UnityAction<bool> action)
        {
            if (toggle != null)
            {
                if (!_mAllToggle.Contains(toggle))
                {
                    _mAllToggle.Add(toggle);
                }

                toggle.onValueChanged.RemoveAllListeners();
                toggle.onValueChanged.AddListener(action);
                toggle.onValueChanged.AddListener((call) =>
                {
                    //TODO 放在声音系统中去处理
                });
            }
        }
    }
}