using UnityEngine;
using WitchScaper.Core.Character;
using Zenject;

namespace WitchScaper.Core
{
    public class CameraArmSpring : ILateTickable
    {
        private const float FOLLOW_SECONDS = 2.5f;
        private readonly Camera _camera;
        private readonly PlayerCharacterController _playerCharacterController;

        public CameraArmSpring(Camera camera, PlayerCharacterController playerCharacterController)
        {
            _camera = camera;
            _playerCharacterController = playerCharacterController;
        }
        
        public void LateTick()
        {
            var desiredPos = _playerCharacterController.transform.position;
            desiredPos.z = _camera.transform.position.z;

            var targetPos = Vector3.Slerp(_camera.transform.position, desiredPos, FOLLOW_SECONDS * Time.deltaTime);

            _camera.transform.position = targetPos;
        }
    }
}