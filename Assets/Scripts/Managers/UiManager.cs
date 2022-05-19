using System.Collections.Generic;
using bFrameWork.Game.UIFrame.Base;
using Common;
using UnityEngine;

namespace Managers
{
    public static class UiManager 
    {
        /// <summary>
        /// 打开的UI列表
        /// </summary>
        private static readonly Stack<UiLogicBase> UiLogicBaseStack = new Stack<UiLogicBase>();

        /// <summary>
        /// 打开过的dialog预制体缓存池 缓存所有dialog预制体
        /// </summary>
        private static readonly Dictionary<EUiID, UiDialogBase> DialogDict = new Dictionary<EUiID, UiDialogBase>();

        public static void PushUi(UiLogicBase ui)
        {
            UiLogicBaseStack.Push(ui);
        }

        public static void Back()
        {
            if (UiLogicBaseStack.Count > 1)
            {
                UiLogicBase closeUiLogicBase = UiLogicBaseStack.Pop();
                closeUiLogicBase.Close();
                
                UiLogicBase openUiLogicBase = UiLogicBaseStack.Peek();
                openUiLogicBase.Open();
            }
            else
            {
                Debug.Log("Ui 堆栈里无多余界面...");
            }
        }

        public static UiDialogBase GetUiDialog(EUiID uiID)
        {
            return DialogDict.ContainsKey(uiID) ? DialogDict[uiID] : null;
        }

        /// <summary>
        /// 添加dialog
        /// </summary>
        /// <param name="uiID"></param>
        /// <param name="uiDialogBase"></param>
        public static void AddUiDialog(EUiID uiID, UiDialogBase uiDialogBase)
        {
            if (!DialogDict.ContainsKey(uiID))
            {
                DialogDict[uiID] = uiDialogBase;
            }
        }

        /// <summary>
        /// 移除dialog
        /// </summary>
        public static void RemoveUiDialog(EUiID uiID)
        {
            if (DialogDict.ContainsKey(uiID))
            {
                DialogDict.Remove(uiID);
            }
        }
    }
}
