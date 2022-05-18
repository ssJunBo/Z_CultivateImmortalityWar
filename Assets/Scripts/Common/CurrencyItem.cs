using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common
{
    public class CurrencyItem : MonoBehaviour
    {
        [SerializeField] private Image iconImg;
        [SerializeField] private TextMeshProUGUI countTxt;

        public void SetData(Sprite iconSpr, int count)
        {
            iconImg.sprite = iconSpr;
            countTxt.text = count.ToString();
        }
    }
}