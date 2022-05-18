using System.Collections.Generic;
using bFrameWork.Game.Base;
using bFrameWork.Game.UIFrame.Base;
using Common;

namespace Managers
{
    public class UiLogicManager : Singleton<UiLogicManager>
    {
        /// <summary>
        /// 打开的UI列表
        /// </summary>
        private readonly Stack<UiLogicBase> _ltUiLogicBase = new Stack<UiLogicBase>();

        public void PushUi(UiLogicBase ui)
        {
            _ltUiLogicBase.Push(ui);
        }

        public void RemoveUi()
        {
            if (_ltUiLogicBase.Count>0)
            {
                UiLogicBase uiLogicBase = _ltUiLogicBase.Pop();
            }
        }
    }
}
