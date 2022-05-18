using AllManager.Model;
using bFrameWork.Game.UIFrame.Base;
using Common;

namespace Functions.PersonInfoDialog.UILogic
{
    public class StartDialogLogic : UiLogicBase
    {
        protected override string Path { get; set; } = "Prefabs/Dialogs/StartDialog";
        public override EUiID Id { get; set; } = EUiID.Start;
       
        private readonly CModelPlay _model;

        public StartDialogLogic(CModelPlay model)
        {
            _model = model;
        }


        public override void Open()
        {
            base.Open();
        }
    }
}
