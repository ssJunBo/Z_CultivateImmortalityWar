using System.Collections.Generic;
using bFrameWork.Game.Base;
using bFrameWork.Game.UIFrame.Base;
using Common;

namespace Managers
{
    public class UiManager : Singleton<UiManager>
    {
        /// <summary>
        /// 打开的UI列表
        /// </summary>
        private readonly Stack<UiLogicBase> _ltUiLogicBase = new Stack<UiLogicBase>();

        private readonly Dictionary<EUiID, UiDialogBase> _dialogDict = new Dictionary<EUiID, UiDialogBase>();

        public void PushUi(UiLogicBase ui)
        {
            _ltUiLogicBase.Push(ui);
        }

        public void PopUi()
        {
            if (_ltUiLogicBase.Count > 0)
            {
                UiLogicBase uiLogicBase = _ltUiLogicBase.Pop();
            }
        }

        public UiDialogBase GetUiDialog(EUiID uiID)
        {
            return _dialogDict.ContainsKey(uiID) ? _dialogDict[uiID] : null;
        }

        /// <summary>
        /// 添加dialog
        /// </summary>
        /// <param name="uiID"></param>
        /// <param name="uiDialogBase"></param>
        public void AddUiDialog(EUiID uiID, UiDialogBase uiDialogBase)
        {
            if (!_dialogDict.ContainsKey(uiID))
            {
                _dialogDict[uiID] = uiDialogBase;
            }
        }

        /// <summary>
        /// 移除dialog
        /// </summary>
        /// <param name="uiDialogBase"></param>
        public void RemoveUiDialog(EUiID uiID)
        {
            if (_dialogDict.ContainsKey(uiID))
            {
                _dialogDict.Remove(uiID);
            }
        }
    }
}
