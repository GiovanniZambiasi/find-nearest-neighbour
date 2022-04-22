using System.Collections.Generic;
using UnityEngine;

namespace NearestNeighbour.Pooling
{
    [System.Serializable]
    public class Pool
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _defaultObjectCount = 10;

        private Queue<GameObject> _objects = new Queue<GameObject>();
        private Transform _parent;

        public GameObject Prefab => _prefab;

        public void Setup(Transform parent)
        {
            _parent = parent;

            for (int i = 0; i < _defaultObjectCount; i++)
            {
                Instantiate();
            }
        }

        public GameObject Get(Vector3 position, Quaternion rotation)
        {
            if (_objects.Count == 0)
            {
                Instantiate();
            }

            GameObject instance = _objects.Dequeue();

            Transform transform = instance.transform;
            transform.position = position;
            transform.rotation = rotation;
            transform.SetParent(null);

            instance.gameObject.SetActive(true);

            return instance;
        }

        public void Release(GameObject instance)
        {
            instance.gameObject.SetActive(false);
            instance.transform.SetParent(_parent);
            _objects.Enqueue(instance);
        }

        private void Instantiate()
        {
            _prefab.gameObject.SetActive(false);

            GameObject instance = Object.Instantiate(_prefab, _parent);
            _objects.Enqueue(instance);

            if(instance.TryGetComponent(out IPoolFeedbackReceiver poolFeedbackReceiver))
            {
                poolFeedbackReceiver.HandleInstantiated();
            }

            _prefab.gameObject.SetActive(true);
        }
    }
}
