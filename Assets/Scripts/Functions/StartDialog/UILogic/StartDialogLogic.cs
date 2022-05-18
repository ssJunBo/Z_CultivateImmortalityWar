using AllManager.Model;
using bFrameWork.Game.UIFrame.Base;
using Common;

namespace Functions.PersonInfoDialog.UILogic
{
    public class StartDialogLogic : UiLogicBase
    {
        #region base info
        protected override string Path { get; set; } = "Prefabs/Dialogs/StartDialog";
        public override EUiID UiId { get; set; } = EUiID.Start;
        public override EUiType UiType { get; set; } = EUiType.ImmediateDestroy;
        #endregion
        
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
