using bFrameWork.Game.UIFrame.Base;
using Common;
using Managers.Model;

namespace Functions.Fighting.Logic
{
    public class UiFightingLogic : UiLogicBase
    {
        protected override string Path => "Prefabs/Dialogs/UiFightingDialog";
        public override EUiID UiId => EUiID.Fighting;

        public CModelPlay modelPlay;
        public UiFightingLogic(CModelPlay modelPlay)
        {
            this.modelPlay = modelPlay;
        }
        
        public override void Open()
        {
            base.Open();
        }
    }
}