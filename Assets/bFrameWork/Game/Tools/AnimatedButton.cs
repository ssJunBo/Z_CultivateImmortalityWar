using System;
using System.Text;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace bFrame.Game.Tools
{
    public sealed class AnimatedButton : UIBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler,
        IBeginDragHandler, IEndDragHandler, IInitializePotentialDragHandler, IScrollHandler, IPointerExitHandler
    {
        private static bool _blockInput;

        //可点击
        [SerializeField] private bool interactable = true;

        //点击变换颜色
        [Space(10), SerializeField] public bool colorTint = false;
        [SerializeField] public Color normalColor = new Color(1, 1, 1, 1);
        [SerializeField] public Color pressColor = new Color(0.78f, 0.78f, 0.78f, 1);
        [SerializeField] public Color disableColor = new Color(0.66f, 0.66f, 0.66f, 1);

        //点击变换图片
        [Space(10), SerializeField] public bool spriteSwap = false;
        [SerializeField] private Image btnImage;
        [SerializeField] public Sprite clickSprite;
        [SerializeField] public Sprite disableSprite;

        //点击动画
        [Space(10), SerializeField] public bool clickAnim = true;
        [Range(0, 1)] [SerializeField] private float overshoot = 1;

        //点击Loading
        [Space(10), SerializeField] public bool clickLoading = false;
        [SerializeField] private GameObject loadingMask;

        //点击事件
        [Space(10), SerializeField] private ButtonClickedEvent mOnClick = new ButtonClickedEvent();

        //点击声音
        [SerializeField] public bool clickSound = true;

        //是否可快速点击（正常: 1秒点击间隔 | 快速点击: 0.1f点击间隔）
        [SerializeField] private bool quickClick = false;
        [NonSerialized] public Sprite NormalSprite;

        
        private Vector2 _beginDragPosition;

        private Sequence _blockSeq;
        private Sequence _btnSequence;
        private Vector2 _currentDragPosition;

        private Transform _imgTransform;

        private Vector3 _originScale = Vector3.one;

        public bool IsShowLoadingMask => loadingMask && loadingMask.gameObject.activeSelf;

        public bool Interactable
        {
            private get => interactable;
            set
            {
                interactable = value;
                ResetBtnView();
            }
        }

        public ButtonClickedEvent onClick => mOnClick;

        public Image BtnImage
        {
            get
            {
                InitBtnImage();

                return btnImage;
            }
        }

        public Transform Transform => _imgTransform ? _imgTransform : transform;

        protected override void Awake()
        {
            InitBtnView();
        }

        protected override void OnEnable()
        {
            ResetBtnView();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _btnSequence?.Kill();
            _btnSequence = null;

            IsButtonDown = false;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            ExecuteParentEventHandler<IBeginDragHandler>(x => x.OnBeginDrag(eventData));

            _beginDragPosition = eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            ExecuteParentEventHandler<IDragHandler>(x => x.OnDrag(eventData));

            _currentDragPosition = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ExecuteParentEventHandler<IEndDragHandler>(x => x.OnEndDrag(eventData));

            // zero
            _currentDragPosition = _beginDragPosition = Vector2.zero;
        }

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            ExecuteParentEventHandler<IInitializePotentialDragHandler>(x => x.OnInitializePotentialDrag(eventData));
        }

        /// <summary>
        /// Called when there is a click/touch over the button.
        /// </summary>
        /// <param name="eventData">The data associated to the pointer event.</param>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!IsActiveAndEnabled(eventData) || _blockInput || IsShowLoadingMask)
            {
                return;
            }

            if (LargeDrag())
            {
                return;
            }

            _blockInput = true;
            _blockSeq?.Kill();
            _blockSeq = DOTween.Sequence().SetUpdate(true).AppendInterval(quickClick ? 0.1f : 0.3f)
                .AppendCallback(() => _blockInput = false);

            Press();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // zero
            _currentDragPosition = _beginDragPosition = Vector2.zero;

            if (!IsActiveAndEnabled(eventData))
            {
                return;
            }

            if (EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(gameObject, eventData);
            }

            ChangeBtnSprite(true);
            ChangeBtnColor(true);
            ChangeBtnAnim(true);

            IsButtonDown = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!IsActiveAndEnabled(eventData))
            {
                return;
            }

            if (clickSound && !_blockInput)
            {
                //TODO 播放声音
//                AudioHelper.Click1();
            }

            ChangeBtnColor(false);
            ChangeBtnSprite(false);
            ChangeBtnAnim(false);

            IsButtonDown = false;
        }

        public void OnScroll(PointerEventData eventData)
        {
            ExecuteParentEventHandler<IScrollHandler>(x => x.OnScroll(eventData));
        }

        private void InitBtnImage()
        {
            if (!btnImage)
            {
                btnImage = GetComponent<Image>();
            }

            if (btnImage)
            {
                _imgTransform = btnImage.transform;
            }
        }

        private void InitBtnView()
        {
            InitBtnImage();

            if (btnImage)
            {
                NormalSprite = btnImage.sprite;
                var originAlpha = btnImage.color.a;
                normalColor.a = originAlpha;
                pressColor.a = originAlpha;
                disableColor.a = originAlpha;
            }

            _originScale = Transform.localScale;

#if UNITY_EDITOR
            if (clickAnim && _originScale != Vector3.one)
            {
                Transform curTrans = Transform;
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Insert(0, $" => {curTrans.name}");
                for (int i = 0; i < 10; i++)
                {
                    var parentTrans = curTrans.parent;
                    if (!parentTrans)
                    {
                        break;
                    }

                    curTrans = parentTrans;
                    stringBuilder.Insert(0, $" => {curTrans.name}");
                }

                Debug.LogWarning($"{stringBuilder} : AnimatedButton按钮初始 localScale 不为 1，请检查是否和按钮缩放动画冲突");
            }
#endif
        }

        private void ResetBtnView()
        {
            if (clickAnim)
            {
                Transform.localScale = _originScale;
            }

            if (!btnImage)
            {
                return;
            }

            if (spriteSwap && NormalSprite && disableSprite)
            {
                btnImage.sprite = Interactable ? NormalSprite : disableSprite;
            }

            if (colorTint)
            {
                btnImage.color = Interactable ? normalColor : disableColor;
            }
        }

        private bool IsActiveAndEnabled(PointerEventData eventData)
        {
            return Interactable && (eventData == null || eventData.button == PointerEventData.InputButton.Left);
        }

        /// <summary>
        /// Sets this button as pressed.
        /// </summary>
        private void Press()
        {
            if (!IsActive())
            {
                return;
            }

            onClick.Invoke();
        }

        private void ChangeBtnSprite(bool isPressed)
        {
            if (spriteSwap && Interactable && btnImage && clickSprite)
            {
                btnImage.sprite = isPressed ? clickSprite : NormalSprite;
            }
        }

        private void ChangeBtnColor(bool isPressed)
        {
            if (colorTint && Interactable && btnImage)
            {
                btnImage.color = isPressed ? pressColor : normalColor;
            }
        }

        public void ChangeBtnAnim(bool isPressed)
        {
            if (clickAnim && Interactable)
            {
                _btnSequence?.Kill();
                _btnSequence = DOTween.Sequence();
                if (isPressed)
                {
                    _btnSequence.Append(Transform.DOScale((1 + (-0.1f * overshoot)) * _originScale, 2 / 30f)
                        .SetEase(Ease.InOutSine));
                }
                else
                {
                    _btnSequence.Append(Transform.DOScale((1 + (0.12f * overshoot)) * _originScale, 2 / 30f)
                        .SetEase(Ease.InSine));
                    _btnSequence.Append(Transform.DOScale(1f * _originScale, 4 / 30f).SetEase(Ease.OutSine));
                }

                _btnSequence.SetUpdate(true);
                _btnSequence.OnComplete(() => _btnSequence = null);
            }
        }

        public void StopBtnAnim()
        {
            _btnSequence?.Kill();
            Transform.localScale = _originScale;
        }

        private void ChangeBtnLoading(bool showLoading)
        {
            if (clickLoading && loadingMask)
            {
                loadingMask.gameObject.SetActive(showLoading);
            }
        }

        public void ShowLoadingMask()
        {
            ChangeBtnLoading(true);
        }

        public void CloseLoadingMask()
        {
            ChangeBtnLoading(false);
        }

        private bool LargeDrag()
        {
            return Vector2.SqrMagnitude(_beginDragPosition - _currentDragPosition) > 4000;
        }

        /// <summary>
        /// 调用父级的拖拽事件
        /// </summary>
        /// <param name="functor"></param>
        /// <typeparam name="T"></typeparam>
        private void ExecuteParentEventHandler<T>(Action<T> functor) where T : IEventSystemHandler
        {
            if (Transform.parent)
            {
                T[] results = Transform.parent.GetComponentsInParent<T>();

                foreach (T result in results)
                {
                    if (result.GetHashCode() != GetHashCode())
                    {
                        functor.Invoke(result);
                    }
                }
            }
        }

        [Serializable]
        public class ButtonClickedEvent : UnityEvent
        {}

        #region LongClick

        [Header("长按时忽略网络检测"), SerializeField] private bool longClickable;

        private const float LongClickTriggerStart = 0.8f;
        private const float LongClickTriggerInterval = 0.08f;
        private float _longClickTime;
        private bool _isLongClick;
        private bool _isButtonDown;

        private bool IsButtonDown
        {
            set
            {
                if (!longClickable)
                {
                    return;
                }

                _longClickTime = 0;
                _isLongClick = false;
                _isButtonDown = value;
            }
            get => _isButtonDown;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            IsButtonDown = false;
        }

        private void Update()
        {
            if (!longClickable)
            {
                return;
            }

            if (!IsActive())
            {
                return;
            }

            if (IsButtonDown || _isLongClick)
            {
                if (!_isLongClick)
                {
                    if (_longClickTime >= LongClickTriggerStart)
                    {
                        _isLongClick = true;
                        onClick.Invoke();
                        _longClickTime = 0;
                    }
                }
                else
                {
                    if (_longClickTime >= LongClickTriggerInterval)
                    {
                        onClick.Invoke();
                        _longClickTime = 0;
                    }
                }

                _longClickTime += Time.deltaTime;
            }
        }

        #endregion
    }
}