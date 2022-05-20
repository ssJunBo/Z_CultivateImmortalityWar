using UnityEngine;
using UnityEngine.UI;

namespace Helpers.Extension
{
    /// <summary>
    /// 老版本 需要更新
    /// </summary>
    public class PinchScrollRect : ScrollRect
    {
        //是否缩放
        private bool _isZoom = false;

        //当前双指触控间距
        private float _doubleTouchCurrDis;

        //过去双指触控间距
        private float _doubleTouchLastDis;

        private const float MaxScale = 1.5f;
        private const float MinScale = 0.5f;

        private Vector2 _tmpTouchCenterPos;

        /// <summary>
        /// 双指缩放逻辑
        /// </summary>
        private void HandleTouchPinch()
        {
            if (Input.touchCount > 1)
            {
                horizontal = false;
                vertical = false;

                if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
                {
                    Touch touch1 = Input.GetTouch(0);
                    Touch touch2 = Input.GetTouch(1);


                    _doubleTouchCurrDis = Vector2.Distance(touch1.position, touch2.position);

                    if (!_isZoom)
                    {
                        _doubleTouchLastDis = _doubleTouchCurrDis;
                        _isZoom = true;
                    }

                    float distance = _doubleTouchCurrDis - _doubleTouchLastDis;

                    var tmpVal = distance > 0 ? 0.01f : -0.01f;

                    if (content.localScale.x + tmpVal > MaxScale)
                    {
                        return;
                    }

                    if (content.localScale.x + tmpVal < MinScale)
                    {
                        return;
                    }

                    content.localScale += Vector3.one * tmpVal;

                    _doubleTouchLastDis = _doubleTouchCurrDis;
                }
            }
            else
            {
                horizontal = true;
                vertical = true;
            }
        }

        private void Update()
        {
            HandleTouchPinch();
        }
    }
}