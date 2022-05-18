using UnityEngine;

namespace bFrame.Game.Base
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        public static T Instance;

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = (T) this;
            }
            else
            {
                Debug.LogError("Get a second instance of this class :" + this.GetType());
            }
        }
        
    }
}
