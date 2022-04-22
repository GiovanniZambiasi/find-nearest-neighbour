using System.Collections.Generic;
using UnityEngine;

namespace NearestNeighbour
{
    /// <summary>
    /// Utility class for cached GetComponent calls
    /// </summary>
    public class GameObjectComponentCache<TComponent>
    {
        private readonly Dictionary<int, TComponent> _objectComponentMap = new Dictionary<int, TComponent>();
        private readonly HashSet<int> _invalids = new HashSet<int>();

        public bool TryGetComponent(GameObject gameObject, out TComponent component)
        {
            int instanceId = gameObject.GetInstanceID();

            if (_invalids.Contains(instanceId))
            {
                component = default;
                return false;
            }

            if (_objectComponentMap.TryGetValue(instanceId, out component))
            {
                return true;
            }

            if (gameObject.TryGetComponent(out component))
            {
                _objectComponentMap.Add(instanceId, component);
                return true;
            }

            _invalids.Add(instanceId);
            return false;
        }
    }
}
