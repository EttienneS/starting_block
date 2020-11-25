using Assets.ServiceLocator;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.StrategyCamera
{
    public abstract class CameraCommand
    {
        public abstract void Execute(CameraController camera);
    }
}