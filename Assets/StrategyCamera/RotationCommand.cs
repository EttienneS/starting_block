using UnityEngine;

namespace Assets.StrategyCamera
{
    public class RotationCommand : CameraCommand
    {
        private Vector3 _amount;

        public RotationCommand(Vector3 amount)
        {
            _amount = amount;
        }

        public override void Execute(CameraController camera)
        {
            camera.newRotation *= Quaternion.Euler(Vector3.up * (-_amount.x / 5));
        }
    }
}