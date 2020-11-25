using Assets.ServiceLocator;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.StrategyCamera
{

    public class MoveCommand : CameraCommand
    {
        private Vector3 _amount;

        public MoveCommand(Vector3 amount)
        {
            _amount = amount;
        }

        public override void Execute(CameraController camera)
        {
            camera.newPosition += _amount;
        }
    }
}