using System;
using bFrameWork.Game.UIFrame.Base;
using Common;
using Managers;
using Managers.Model;

namespace Functions.Loading.Logic
{
    public class UiLoadingLogic : UiLogicBase
    {
        protected override string Path => "Prefabs/Dialogs/UiLoadingDialog";
        public override EUiID UiId => EUiID.Loading;

        #region data
        private CModelPlay modelPlay;
        public string sceneName;
        #endregion

        public Action SceneOpenFinishAct { get; private set; }

        public UiLoadingLogic(CModelPlay modelPlay)
        {
            this.modelPlay = modelPlay;
        }
        public void Open(string sceneName,Action sceneOpenFinishAct)
        {
            this.sceneName = sceneName;
            GameManager.Instance.UiManager.ChangeScene();
            SceneOpenFinishAct = sceneOpenFinishAct;
            Open();
        }
    }
}
