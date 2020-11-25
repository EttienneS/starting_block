using Assets.ServiceLocator;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.StrategyCamera
{

    public class ZoomCommand : CameraCommand
    {
        private Vector3 _amount;

        public ZoomCommand(Vector3 amount)
        {
            _amount = amount;
        }

        public override void Execute(CameraController camera)
        {
            camera.newZoom += _amount;
            camera.newZoom = new Vector3(camera.newZoom.x,
                                         Mathf.Clamp(camera.newZoom.y, camera.minZoom, camera.maxZoom),
                                         Mathf.Clamp(camera.newZoom.z, -camera.maxZoom, -camera.minZoom));
        }
    }
}