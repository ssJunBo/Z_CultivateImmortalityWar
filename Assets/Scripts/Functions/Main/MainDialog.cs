using bFrameWork.Game.UIFrame.Base;
using UnityEngine;

namespace Functions.Main
{
    public class MainDialog : UiDialogBase
    {
        [SerializeField] private MainPanel mainPanel; 
        public override void Init()
        {

        }

        public override void ShowFinished()
        {
            // TODO 个人信息
            mainPanel.SetData();
        }
    }
}
