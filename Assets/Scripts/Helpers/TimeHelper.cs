using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Helpers
{
    public class TimeHelper
    {
        #region 定时回调系统

        public static Coroutine TimeCallback(MonoBehaviour mono, float time, UnityAction callBack, float time2 = -1,
            UnityAction callback2 = null)
        {
            return mono.StartCoroutine(Coroutine(time, callBack, time2, callback2));
        }

        private static IEnumerator Coroutine(float time, UnityAction callback, float time2 = 0,
            UnityAction callback2 = null)
        {
            yield return new WaitForSeconds(time);
            callback?.Invoke();

            if (time2 > 0)
            {
                yield return new WaitForSeconds(time2);
                callback2?.Invoke();
            }
        }

        #endregion
    }
}