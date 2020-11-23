using Assets.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.ServiceLocator
{
    public sealed class Loc
    {
        private readonly Queue<IGameService> _initializationQueue;
        private readonly Dictionary<string, IGameService> _services;

        private Loc()
        {
            _services = new Dictionary<string, IGameService>();
            _initializationQueue = new Queue<IGameService>();
        }

        public static Loc Current { get; private set; }

        public static void Initiailze()
        {
            Current = new Loc();
        }

        public static void Reset()
        {
            Current = null;
        }

        public T Get<T>() where T : IGameService
        {
            ProcessInitializationQueue();

            string key = typeof(T).Name;
            if (!_services.ContainsKey(key))
            {
                Debug.LogError($"{key} not registered with {GetType().Name}");
                throw new InvalidOperationException();
            }

            return (T)_services[key];
        }

        public void ProcessInitializationQueue()
        {
            while (_initializationQueue.Count > 0)
            {
                var nextItem = _initializationQueue.Dequeue();
                var name = nextItem.GetType().Name;
                using (Instrumenter.Start(name))
                {
                    nextItem.Initialize();
                    _services.Add(name, nextItem);
                }
            }
        }

        public void Register<T>(T service) where T : IGameService
        {
            var serviceType = service.GetType();
            if (_initializationQueue.Any(i => i.GetType() == serviceType) || _services.Any(i => i.GetType() == serviceType))
            {
                Debug.LogError($"Attempted to register service of type {serviceType} which is already registered with the {GetType().Name}.");
                return;
            }

            _initializationQueue.Enqueue(service);
        }

        public void Unregister<T>() where T : IGameService
        {
            string key = typeof(T).Name;
            if (!_services.ContainsKey(key))
            {
                Debug.LogError($"Attempted to unregister service of type {key} which is not registered with the {GetType().Name}.");
                return;
            }

            _services.Remove(key);
        }

        internal void LogServices()
        {
            var msg = "Loaded services:\n";
            foreach (var service in _services)
            {
                msg += $"- {service.Key}\n";
            }
            Debug.Log(msg);
        }
    }
}