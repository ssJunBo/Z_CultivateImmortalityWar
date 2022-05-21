using bFrameWork.Game.UIFrame.Base;
using Functions.Fighting.Logic;
using Managers;
using UnityEngine;

namespace Functions.Fighting.View
{
    public class UiFightingDialog : UiDialogBase
    {
        [SerializeField] private FightDetailPanel fightDetailPanel;

        private UiFightingLogic logic;

        public override void Init()
        {
            logic = (UiFightingLogic)UiLogic;
        }

        public override void ShowFinished()
        {
            fightDetailPanel.SetData();
        }

        public void QuitFightScene()
        {
            logic.modelPlay.UiLoadingLogic.Open(ConStr.Main, () =>
            {
                GameManager.Instance.ModelPlay.UiMainLogic.Open();
            });
            logic.Close();
        }
    }
}