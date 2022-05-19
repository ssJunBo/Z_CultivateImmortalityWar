using bFrameWork.Game.UIFrame.Base;
using Functions.Main.Logic;
using UnityEngine;

namespace Functions.Main.View
{
    public class MainDialog : UiDialogBase
    {
        [SerializeField] private UiInfoPanel uiInfoPanel;

        private UiMainLogic logic;

        public override void Init()
        {
            logic = (UiMainLogic)UiLogic;
        }

        public override void ShowFinished()
        {
            // TODO 个人信息
            uiInfoPanel.SetData(logic.model);
        }
    }
}
