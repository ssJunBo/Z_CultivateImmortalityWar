using System;
using Common;
using Functions.Main.Logic;
using Functions.PersonDetailInfo.Logic;
using Helpers;

namespace Managers.Model
{
    public class CModelPlay : IModel
    {
        #region UiLogic 数据类

        private UiPersonDetailInfoLogic uiPersonDetailInfoLogic;

        public UiPersonDetailInfoLogic UiPersonDetailInfoLogic =>
            uiPersonDetailInfoLogic ??= new UiPersonDetailInfoLogic(this);

        private UiMainLogic uiMainLogic;
        public UiMainLogic UiMainLogic => uiMainLogic ??= new UiMainLogic(this);

        #endregion

        public void Create()
        {

        }

        public void Release()
        {
            if (uiPersonDetailInfoLogic != null)
            {
                uiPersonDetailInfoLogic.Close();
                uiPersonDetailInfoLogic = null;
            }
        }

        public void Update(float fDeltaTime)
        {

        }

        public void LateUpdate()
        {

        }

        public void OnApplicationPause(bool paused)
        {

        }

        public void OpenUiByType(EUiType eUiType)
        {
            switch (eUiType)
            {
                case EUiType.PersonDetailInfo:
                    UiPersonDetailInfoLogic.Open();
                    break;
                case EUiType.Main:
                    UiMainLogic.Open();
                    break;
            }
        }
    }
}
