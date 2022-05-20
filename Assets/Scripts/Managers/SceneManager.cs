using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    /// <summary>
    /// 场景异步加载
    /// </summary>
    public class SceneManager
    {
        //加载场景完成回调
        public Action LoadSceneOverCallBack;

        //加载场景开始回调
        public Action LoadSceneEnterCallBack;

        //当前场景名
        public string CurrentMapName { get; set; }

        //场景是否加载完成
        public bool AlreadyLoadScene { get; set; }

        //切换场景进度条
        public static int LoadingProgress = 0;

        private MonoBehaviour m_Mono;

        public SceneManager(MonoBehaviour mono)
        {
            m_Mono = mono;
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="name">场景名</param>
        public void LoadScene(string name)
        {
            LoadingProgress = 0;
            m_Mono.StartCoroutine(LoadSceneAsync(name));
        
            //TODO 
            //GameManager.Instance.UiManager.PopUpWnd(ConStr._LoadingPanel);
        }

        /// <summary>
        /// 设置场景环境
        /// </summary>
        /// <param name="name"></param>
        void SetSceneSetting(string name)
        {
            //设置各种场景环境，可以根据配表来TODO:
        }

        IEnumerator LoadSceneAsync(string name)
        {
            LoadSceneEnterCallBack?.Invoke();
            ClearCache();
            AlreadyLoadScene = false;
            AsyncOperation unLoadScene =
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(ConStr.EMPTYSCENE, LoadSceneMode.Single);
            while (unLoadScene != null && !unLoadScene.isDone)
            {
                yield return new WaitForEndOfFrame();
            }

            LoadingProgress = 0;
            int targetProgress = 0;
            AsyncOperation asyncScene = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(name);
            if (asyncScene != null && !asyncScene.isDone)
            {
                asyncScene.allowSceneActivation = false;
                while (asyncScene.progress < 0.9f)
                {
                    targetProgress = (int) asyncScene.progress * 100;
                    yield return new WaitForEndOfFrame();
                    //平滑过渡
                    while (LoadingProgress < targetProgress)
                    {
                        ++LoadingProgress;
                        yield return new WaitForEndOfFrame();
                    }
                }

                CurrentMapName = name;
                SetSceneSetting(name);
                //自行加载剩余的10%
                targetProgress = 100;
                while (LoadingProgress < targetProgress - 1)
                {
                    ++LoadingProgress;
                    yield return new WaitForEndOfFrame();
                }

                LoadingProgress = 0;
                asyncScene.allowSceneActivation = true;
                AlreadyLoadScene = true;
                if (LoadSceneOverCallBack != null)
                {
                    LoadSceneOverCallBack();
                }
            }
        }

        /// <summary>
        /// 跳场景需要清除的东西
        /// </summary>
        private void ClearCache()
        {
          
        }

    }
}
