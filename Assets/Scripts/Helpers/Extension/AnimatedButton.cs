using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Helpers.Expand
{
    public class AnimatedButton : Button
    {
        private Sequence _mSequence;

        public override void OnPointerClick(PointerEventData eventData)
        {
            BtnTweenAnim();

            base.OnPointerClick(eventData);
        }

        private void BtnTweenAnim()
        {
            _mSequence = DOTween.Sequence();
            _mSequence.Append(transform.DOScale(2f, 5 / 30f));
            _mSequence.Append(transform.DOScale(4f, 5 / 30f));
            _mSequence.Append(transform.DOScale(1f, 5 / 30f));
            _mSequence.AppendCallback(() =>
            {
                _mSequence.SetAutoKill(false);
                _mSequence.Pause();
            });
        }
    }
}
