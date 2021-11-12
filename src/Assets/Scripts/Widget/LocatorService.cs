using System;
using System.Collections.Generic;
using UnityEngine;

namespace Widget
{
    public interface IGameService
    {
    }

    public class LocatorService
    {
        private LocatorService()
        {
        }

        private readonly Dictionary<string, IGameService> _services = new Dictionary<string, IGameService>();

        public static LocatorService Instance { get; private set; }

        public static void Init()
        {
            Instance = new LocatorService();
        }

        public T Get<T>() where T : IGameService
        {
            string key = typeof(T).Name;

            if (_services.ContainsKey(key))
            {
                return (T)_services[key];
            }
        
            Debug.LogError($"{key} not registered with {GetType().Name}");
            throw new InvalidOperationException();
        }

        public void Register<T>(T service) where T : IGameService
        {
            string key = typeof(T).Name;
        
            if (_services.ContainsKey(key))
            {
                Debug.LogError(
                    $"Attempted to register service of type {key} which is already registered with the {GetType().Name}.");
                return;
            }

            _services.Add(key, service);
        }

        public void Unregister<T>() where T : IGameService
        {
            string key = typeof(T).Name;
        
            if (!_services.ContainsKey(key))
            {
                Debug.LogError(
                    $"Attempted to unregister service of type {key} which is not registered with the {GetType().Name}.");
                return;
            }

            _services.Remove(key);
        }
    }
}