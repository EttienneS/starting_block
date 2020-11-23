using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.ServiceLocator
{
    public interface IGameService
    {
        void BindServiceLocator(ServiceLocator locator);
        void Initialize();
    }

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