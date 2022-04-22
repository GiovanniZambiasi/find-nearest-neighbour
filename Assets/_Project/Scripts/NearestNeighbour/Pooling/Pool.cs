using System.Collections.Generic;
using UnityEngine;

namespace NearestNeighbour.Pooling
{
    [System.Serializable]
    public class Pool
    {
        [SerializeField] private Component _prefab;
        [SerializeField] private int _defaultObjectCount = 10;

        private Queue<Component> _objects = new Queue<Component>();
        private Transform _parent;

        public Component Prefab => _prefab;

        public void Setup(Transform parent)
        {
            _parent = parent;

            for (int i = 0; i < _defaultObjectCount; i++)
            {
                Instantiate();
            }
        }

        public Component Get(Vector3 position, Quaternion rotation)
        {
            if (_objects.Count == 0)
            {
                Instantiate();
            }

            Component instance = _objects.Dequeue();

            Transform transform = instance.transform;
            transform.position = position;
            transform.rotation = rotation;
            transform.SetParent(null);

            instance.gameObject.SetActive(true);

            return instance;
        }

        public void Release(Component instance)
        {
            instance.gameObject.SetActive(false);
            instance.transform.SetParent(_parent);
            _objects.Enqueue(instance);
        }

        private void Instantiate()
        {
            _prefab.gameObject.SetActive(false);

            Component instance = Object.Instantiate(_prefab, _parent);
            _objects.Enqueue(instance);

            _prefab.gameObject.SetActive(true);
        }
    }
}
