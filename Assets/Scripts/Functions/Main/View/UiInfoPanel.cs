using Data;
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
        public void SetData(CModelPlay modelPlay,PersonDetailData personDetailInfo)
        {
            this.modelPlay = modelPlay;

            InitUi(personDetailInfo);
        }

        private void InitUi(PersonDetailData personDetailInfo)
        {
            personHeadImg.sprite = null;
            personNameTxt.text = personDetailInfo.name;
        }
        
        public void OnClickHeadIcon()
        {
            modelPlay.UiPersonDetailInfoLogic.Open();
        }
    }
}