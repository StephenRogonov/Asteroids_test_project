using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Common
{
    public class Pool<T> where T : MonoBehaviour
    {
        private readonly T _prefab;
        private readonly Queue<T> _queue = new();

        private IInstantiator _instantiator;

        public Pool(T prefab, int poolSize, IInstantiator instantiator)
        {
            _prefab = prefab;
            _instantiator = instantiator;

            T item;

            for (int i = 0; i < poolSize; i++)
            {
                item = _instantiator.InstantiatePrefabForComponent<T>(_prefab);
                item.gameObject.SetActive(false);
                _queue.Enqueue(item);
            }
        }

        public T Get()
        {
            if (_queue.Count > 0)
            {
                return _queue.Dequeue();
            }

            T item = _instantiator.InstantiatePrefabForComponent<T>(_prefab);
            item.gameObject.SetActive(false);
            return item;
        }

        public void Return(T item)
        {
            _queue.Enqueue(item);
        }
    }
}