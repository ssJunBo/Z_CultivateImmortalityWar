using bFrameWork.Game.Tools;
using bFrameWork.Game.UIFrame.Base;
using Functions.Loading.Logic;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Functions.Loading.View
{
    public class UiLoadingDialog : UiDialogBase
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI progressTxt;
        [SerializeField] private TextMeshProUGUI loadingTxt;

        private UiLoadingLogic logic;
        private SceneManager sceneManager;

        public override void Init()
        {
            logic = (UiLoadingLogic)UiLogic;
            sceneManager = GameManager.Instance.SceneManager;

            sceneManager.LoadSceneFinishAct = LoadSceneFinishAct;
            sceneManager.RefreshProgressAct = RefreshProgress;
        }

        public override void ShowFinished()
        {
            SetUiDefaultInfo();
            sceneManager.AsyncLoadScene(logic.sceneName);
        }

        private void SetUiDefaultInfo()
        {
            slider.value = 0;
            progressTxt.text = "0%";
            loadingTxt.text = "Loading";
        }

        private void RefreshProgress(float progressVal)
        {
            slider.value = progressVal;
            Debug.Log("progressVal = " + progressVal);
            progressTxt.text = $"{(int)(progressVal * 100)}%";
        }

        private void LoadSceneFinishAct()
        {
            logic.Close();
            // 等一帧 在调用 要展示界面
            TimeHelper.Instance.DelayHowManyFramesAfterCallBack(1, () =>
            {
                logic.SceneOpenFinishAct?.Invoke();
            });
        }
    }
}
