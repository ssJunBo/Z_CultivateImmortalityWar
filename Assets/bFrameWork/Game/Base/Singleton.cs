namespace bFrameWork.Game.Base
{
    public class Singleton<T> where T : new()
    {
        private static T _mInstance;

        public static T Instance => _mInstance ?? new T();
    }
}
