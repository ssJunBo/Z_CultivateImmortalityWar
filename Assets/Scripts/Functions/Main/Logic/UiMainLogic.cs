using AllManager.Model;
using bFrameWork.Game.UIFrame.Base;
using Common;
using Managers.Model;

namespace Functions.Main.Logic
{
    public class UiMainLogic : UiLogicBase
    {
        protected override string Path => "Prefabs/Dialogs/MainDialog";
        protected override EUiID UiId => EUiID.Main;

        public CModelPlay model { get; }

        public UiMainLogic(CModelPlay model)
        {
            this.model = model;
        }
    }
}