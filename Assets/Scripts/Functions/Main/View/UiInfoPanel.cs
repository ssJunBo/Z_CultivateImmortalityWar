using AllManager.Model;
using Managers.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Functions.Main.View
{
    public class UiInfoPanel : MonoBehaviour
    {
        [SerializeField] private Image personHeadImg;
        [SerializeField] private TextMeshProUGUI personNameTxt;

        private CModelPlay modelPlay;
        public void SetData(CModelPlay modelPlay)
        {
            this.modelPlay = modelPlay;
        }

        public void OnClickHeadIcon()
        {
            modelPlay.UiPersonDetailInfoLogic.Open();
        }
    }
}