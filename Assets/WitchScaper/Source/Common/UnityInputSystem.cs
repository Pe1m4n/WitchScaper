using UnityEngine;

namespace WitchScaper.Common
{
    public class UnityInputSystem : IInputSystem
    {
        private readonly Camera _camera;

        public UnityInputSystem(Camera camera)
        {
            _camera = camera;
        }

        public Vector2 GetMousePosition()
        {
            return _camera.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}