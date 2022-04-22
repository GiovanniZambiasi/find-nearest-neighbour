using UnityEngine;
using UnityEngine.EventSystems;

namespace NearestNeighbour.Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Weapon _weapon;

        private bool ShouldFire => Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject(0);

        public void Setup(IPoolingService poolingService)
        {
            _weapon.Setup(poolingService);
        }

        public void Tick()
        {
            if (ShouldFire)
            {
                FireWeapon();
            }
        }

        private void FireWeapon()
        {
            Ray aimRay = _camera.ScreenPointToRay(Input.mousePosition);
            _weapon.Aim(aimRay.origin, aimRay.direction);
            _weapon.Fire();
        }
    }
}
