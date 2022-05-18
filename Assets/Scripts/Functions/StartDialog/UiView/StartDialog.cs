using bFrameWork.Game.UIFrame.Base;
using Functions.PersonInfoDialog.UILogic;

namespace Functions.StartDialog.UiView
{
    public class StartDialog : UiDialogBase
    {
        private StartDialogLogic _startDialogLogic;

        public void OnClickStartBtn()
        {
            
        }

        public override void Init()
        {
            _startDialogLogic = (StartDialogLogic) UiLogic;
        }

        public override void ShowFinished()
        {
            // 刷新UI
        }

        public void Close()
        {
            _startDialogLogic.Close(); 
        }
    }
}
