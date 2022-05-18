using System;
using Functions.PersonInfoDialog.UILogic;
using Helpers;
using Other;

namespace AllManager.Model
{
    public class CModelPlay : IModel
    {
        #region UiLogic 数据类

        private StartDialogLogic _startDialogLogic;

        public StartDialogLogic StartDialogLogic
        {
            get
            {
                if (_startDialogLogic == null)
                {
                    _startDialogLogic = new StartDialogLogic(this);
                }

                return _startDialogLogic;
            }
        }

        #endregion

        public void Create()
        {

        }

        public void Release()
        {
            if (_startDialogLogic != null)
            {
                _startDialogLogic.Close();
                _startDialogLogic = null;
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
                case EUiType.UiStart:
                    StartDialogLogic.Open();
                    break;
                case EUiType.UiMainMenu:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(eUiType), eUiType, null);
            }
        }
    }
}
