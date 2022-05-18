using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Helpers
{
    public static class GameObjectExt
    {
        #region 隐藏与事件添加

        /// <summary>
        /// 设置物体显隐
        /// </summary>
        /// <param name="go"></param>
        /// <param name="bActive"></param>
        public static void SetRealActive(this GameObject go, bool bActive)
        {
            if (go == null)
                return;
            
            if (go.activeSelf != bActive)
                go.SetActive(bActive);
        }

        /// <summary>
        /// 给GameObject添加监听
        /// </summary>
        /// <param name="go"></param>
        /// <param name="action"></param>
        public static void AddListenObj(this GameObject go, UnityAction action)
        {
            Button btn = go.GetComponent<Button>();
            if (btn != null)
                btn.onClick.AddListener(action);
        }

        #endregion

        #region DoTween一些常用动画

        /// <summary>
        /// 渐隐渐现
        /// </summary>
        public static void PingPongAnim(CanvasGroup go, float fromVal = 1f, float toVal = 0f, float duration = 1f)
        {
            go.DOFade(toVal, duration).onComplete =
                () => { PingPongAnim(go, toVal, fromVal, duration); };
        }

        #endregion
    }
}