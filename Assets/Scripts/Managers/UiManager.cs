using System.Collections.Generic;
using bFrameWork.Game.Base;
using bFrameWork.Game.UIFrame.Base;
using Common;
using UnityEngine;

namespace Managers
{
    public class UiManager : Singleton<UiManager>
    {
        /// <summary>
        /// 打开的UI列表
        /// </summary>
        private readonly Stack<UiLogicBase> uiLogicBaseStack = new Stack<UiLogicBase>();

        /// <summary>
        /// 打开过的dialog预制体缓存池 缓存所有dialog预制体
        /// </summary>
        private readonly Dictionary<EUiID, UiDialogBase> dialogDict = new Dictionary<EUiID, UiDialogBase>();

        public void PushUi(UiLogicBase ui)
        {
            uiLogicBaseStack.Push(ui);
        }

        public void Back()
        {
            if (uiLogicBaseStack.Count > 1)
            {
                UiLogicBase closeUiLogicBase = uiLogicBaseStack.Pop();
                closeUiLogicBase.Close();

                UiLogicBase openUiLogicBase = uiLogicBaseStack.Peek();
                openUiLogicBase.Open();
            }
            else
            {
                Debug.Log("Ui 堆栈里无多余界面...");
            }
        }

        public void ChangeScene()
        {
            while (uiLogicBaseStack.Count > 0)
            {
                UiLogicBase uiLogicBase = uiLogicBaseStack.Pop();
                uiLogicBase.Close();
            }
        }

        public UiDialogBase GetUiDialog(EUiID uiID)
        {
            return dialogDict.ContainsKey(uiID) ? dialogDict[uiID] : null;
        }

        /// <summary>
        /// 添加dialog
        /// </summary>
        /// <param name="uiID"></param>
        /// <param name="uiDialogBase"></param>
        public void AddUiDialog(EUiID uiID, UiDialogBase uiDialogBase)
        {
            if (!dialogDict.ContainsKey(uiID))
            {
                dialogDict[uiID] = uiDialogBase;
            }
        }

        /// <summary>
        /// 移除dialog
        /// </summary>
        public void RemoveUiDialog(EUiID uiID)
        {
            if (dialogDict.ContainsKey(uiID))
            {
                dialogDict.Remove(uiID);
            }
        }
    }
}
