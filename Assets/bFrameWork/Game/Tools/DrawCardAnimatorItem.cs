using System;
using DG.Tweening;
using Helpers.Extension;
using UnityEngine;
using UnityEngine.UI;

namespace bFrameWork.Game.Tools
{
    public enum FlipState
    {
        Idle,
        Flipping,
        Finish
    }

    /// <summary>
    /// 翻卡类 待完善 简单版本
    /// </summary>
    public sealed class DrawCardAnimatorItem : MonoBehaviour
    {
        private static readonly int LowFlip = Animator.StringToHash("LowFlip");
        private static readonly int AdvFlip = Animator.StringToHash("AdvFlip");

        private const string KeyPlayChargeEffect = "PlayChargeEffect";
        private const string KeyPlayChargeFinish = "PlayChargeFinish";
        private const string KeyPlayFinishEffect = "PlayFinishEffect";
        private const string KeyOnFlipAction = "OnFlipAction";
        private const string KeyOnFinishAction = "OnFinishAction";

        [SerializeField] private AnimatedButton clickBtn;
        [SerializeField] public RectTransform itemRect;

        [Space, SerializeField] private Animator drawAnimator;
        [SerializeField] private AnimationEventHelp animEventHelper;

        [Space, SerializeField] private Image cardBack;
        [SerializeField] private GameObject cardObj;

        [SerializeField] private GameObject[] effectObjs;

        //TODO UIParticle
        [Space, SerializeField] private Transform chargeEffectTran;
        [SerializeField] private Transform finishEffectTran;
        [SerializeField] private Transform boomEffectTran;

        [Space, SerializeField] private float particleScale = 1;

        private FlipState _flipState;
        private Vector2 _oriAnchoredPosition;

        public Action onFlipComplete;
        public Action onFinishClickEvent;
        public Action finishFlipAction;

        private void Awake()
        {
            _oriAnchoredPosition = itemRect.anchoredPosition;

            cardBack.gameObject.SetActive(true);
            cardObj.gameObject.SetActive(false);
        }

        public void Setup()
        {
        }

        public void FlipCard(bool isClickFlip)
        {
            _flipState = FlipState.Flipping;
            SetClickable(false);

            animEventHelper.actDict[KeyPlayChargeEffect] = PlayChargeEffect;
            animEventHelper.actDict[KeyPlayChargeFinish] = StopShake;
            animEventHelper.actDict[KeyOnFlipAction] = OnCardFlip;
            animEventHelper.actDict[KeyPlayFinishEffect] = OnCardFlipFinished;
            if (isClickFlip)
            {
                animEventHelper.actDict[KeyOnFinishAction] = OnFinishFlipAction;
            }
            else
            {
                animEventHelper.actDict[KeyOnFinishAction] = OnFinishAutoAction;
            }

            int triggerHash = AdvFlip; //TODO 

            drawAnimator.SetTrigger(triggerHash);

            //TODO 播放声音
        }

        private void OnCardFlip()
        {
            cardObj.SetActive(true);
            cardBack.gameObject.SetActive(false);
        }

        private void OnCardFlipFinished()
        {
            cardObj.SetActive(true);
            cardBack.gameObject.SetActive(false);

            transform.localRotation = Quaternion.Euler(Vector3.zero);
            transform.localScale = Vector3.one;

            PlayFinishEffect();
            PlayBoomEffect();

            onFlipComplete?.Invoke();
        }

        private void OnFinishFlipAction()
        {
            SetClickable(true);
            _flipState = FlipState.Finish;
            finishFlipAction?.Invoke();
        }

        private void OnFinishAutoAction()
        {
            SetClickable(true);
            _flipState = FlipState.Finish;
            finishFlipAction?.Invoke();
        }

        private void SetClickable(bool isClickable)
        {
            clickBtn.interactable = isClickable;
        }

        private void PlayChargeEffect()
        {
            Transform chargeTransform = chargeEffectTran.transform;
            for (int i = 0; i < chargeTransform.childCount; i++)
            {
                Transform trs = chargeTransform.GetChild(i);
                Destroy(trs.gameObject);
            }

            Transform finishTransform = finishEffectTran.transform;
            for (int i = 0; i < finishTransform.childCount; i++)
            {
                Transform trs = finishTransform.GetChild(i);
                Destroy(trs.gameObject);
            }

            GameObject effect = null; //TODO 设置特效

            if (effect)
            {
                Instantiate(effect, chargeEffectTran.transform).transform.localScale = Vector3.one * particleScale;
            }

            StartShake();
        }

        private void PlayFinishEffect()
        {
            Transform chargeTransform = chargeEffectTran.transform;
            for (int i = 0; i < chargeTransform.childCount; i++)
            {
                Transform trs = chargeTransform.GetChild(i);
                Destroy(trs.gameObject);
            }

            Transform finishTransform = finishEffectTran.transform;
            for (int i = 0; i < finishTransform.childCount; i++)
            {
                Transform trs = finishTransform.GetChild(i);
                Destroy(trs.gameObject);
            }

            GameObject effect = null; //设置特效
            if (effect)
            {
                Instantiate(effect, finishEffectTran.transform).transform.localScale = Vector3.one * particleScale;
            }

            //TODO 特效生成后记得重置特效状态
        }

        private void PlayBoomEffect()
        {
            GameObject effect = null; //设置特效
            if (effect)
            {
                Instantiate(effect, boomEffectTran.transform).transform.localScale = Vector3.one * particleScale;
                //TODO 特效生成后记得重置特效状态
            }
        }

        private Tweener _shakeTween;

        private void StartShake()
        {
            _shakeTween?.Kill();
            itemRect.anchoredPosition = _oriAnchoredPosition;
            _shakeTween = itemRect.DOShakePosition(10f, new Vector3(15, 15), 8);
        }

        private void StopShake()
        {
            _shakeTween?.Kill();
            itemRect.anchoredPosition = _oriAnchoredPosition;
        }

        public void OnClickEvent()
        {
            switch (_flipState)
            {
                case FlipState.Idle:
                    FlipCard(true);
                    break;
                case FlipState.Flipping:
                    break;
                case FlipState.Finish:
                    onFinishClickEvent?.Invoke();
                    break;
            }
        }
    }
}