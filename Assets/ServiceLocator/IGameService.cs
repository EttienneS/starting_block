using UnityEngine;

namespace Assets.ServiceLocator
{
    public abstract class LocatableMonoBehavior : MonoBehaviour, IGameService
    {
        private ServiceLocator _locator;

        public void BindServiceLocator(ServiceLocator locator)
        {
            _locator = locator;
        }

        public ServiceLocator GetLocator()
        {
            return _locator;
        }

        public T Locate<T>() where T : IGameService
        {
            return _locator.Get<T>();
        }

        public abstract void Initialize();
    }
}