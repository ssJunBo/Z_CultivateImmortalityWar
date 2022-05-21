using bFrameWork.Game.UIFrame.Base;
using Common;
using Managers.Model;

namespace Functions.PersonDetailInfo.Logic
{
    public class UiPersonDetailInfoLogic : UiLogicBase
    {
        #region base info
        protected override string Path => "Prefabs/Dialogs/UiPersonDetailInfoDialog";
        public override EUiID UiId => EUiID.PersonDetailInfo;
        #endregion
        
        private readonly CModelPlay _model;

        public UiPersonDetailInfoLogic(CModelPlay model)
        {
            _model = model;
        }


        public override void Open()
        {
            base.Open();
        }
    }
}
