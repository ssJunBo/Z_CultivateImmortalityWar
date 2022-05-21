using bFrameWork.Game.UIFrame.Base;
using Functions.PersonDetailInfo.Logic;
using Managers;
using UnityEngine;

namespace Functions.PersonDetailInfo.View
{
    public class UiPersonDetailInfoDialog : UiDialogBase
    {
        [SerializeField] private PersonInfoPanel personInfoPanel;
        
        private UiPersonDetailInfoLogic uiPersonDetailInfoLogic;

        public void OnClickStartBtn()
        {
            
        }

        public override void Init()
        {
            uiPersonDetailInfoLogic = (UiPersonDetailInfoLogic) UiLogic;
        }

        public override void ShowFinished()
        {
            // 刷新UI
            personInfoPanel.SetData();
        }

        public void Close()
        {
            GameManager.Instance.UiManager.Back();
        }
    }
}
