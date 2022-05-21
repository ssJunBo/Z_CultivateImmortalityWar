using System;
using System.Collections;
using System.Data;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// 场景异步加载
    /// </summary>
    public class SceneManager
    {
        //加载场景完成回调
        public Action LoadSceneFinishAct;

        // 刷新进度信息
        public Action<float> RefreshProgressAct;

        private readonly MonoBehaviour mMono;

        public SceneManager(MonoBehaviour mono)
        {
            mMono = mono;
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="name">场景名</param>
        public void AsyncLoadScene(string name)
        {
            mMono.StartCoroutine(AsyncLoadSceneCoroutine(name));
        }

        IEnumerator AsyncLoadSceneCoroutine(string name)
        {
            AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(name);

            operation.allowSceneActivation = false;

            int LoadingProgress = 0;

            if (!operation.isDone)
            {
                int targetProgress;
                while (operation.progress < 0.9f)
                {
                    targetProgress = (int)operation.progress * 100;
                    yield return new WaitForEndOfFrame();
                    //平滑过渡
                    while (LoadingProgress < targetProgress)
                    {
                        ++LoadingProgress;
                        RefreshProgressAct?.Invoke(LoadingProgress / 100f);
                        yield return new WaitForEndOfFrame();
                    }
                }

                targetProgress = 100;
                while (LoadingProgress < targetProgress)
                {
                    ++LoadingProgress;
                    RefreshProgressAct?.Invoke(LoadingProgress / 100f);
                    yield return new WaitForEndOfFrame();
                }

                // yield return new WaitForEndOfFrame();
                // TODO 
                yield return new WaitForSeconds(0.5f);

                operation.allowSceneActivation = true;

                // 切换完场景等0.3s 展示Ui
                yield return new WaitForSeconds(0.3f);
                LoadSceneFinishAct?.Invoke();
            }
        }
    }
}
