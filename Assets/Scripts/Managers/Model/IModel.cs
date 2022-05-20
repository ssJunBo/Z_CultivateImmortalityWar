namespace Managers.Model
{
    public interface IModel
    {
        void Create();
        void Release();
        void Update(float fDeltaTime);
        void LateUpdate();
        void OnApplicationPause(bool paused);
    }
}
