using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NearestNeighbour
{
    public static class EventSystemExtensions
    {
        private static PointerEventData _eventData;
        private static List<RaycastResult> _raycastResults = new List<RaycastResult>(2);

        public static bool IsPointerOverUI(this EventSystem eventSystem)
        {
            PointerEventData eventData = GetPointerEventData(eventSystem);
            eventData.position = Input.mousePosition;

            _raycastResults.Clear();

            eventSystem.RaycastAll(eventData, _raycastResults);

            return _raycastResults.Count > 0;
        }

        private static PointerEventData GetPointerEventData(EventSystem eventSystem)
        {
            if (_eventData == null)
            {
                _eventData = new PointerEventData(eventSystem);
            }

            return _eventData;
        }
    }
}
