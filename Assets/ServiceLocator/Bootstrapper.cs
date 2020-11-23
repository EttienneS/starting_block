using Assets.StrategyCamera;
using UnityEngine;

namespace Assets.ServiceLocator
{
    public class Bootstrapper : MonoBehaviour
    {
        public void Awake()
        {
            Loc.Initiailze();

            Loc.Current.Register(FindObjectOfType<CameraController>());
            Loc.Current.Register(FindObjectOfType<SimpleMapGen>());

            Loc.Current.ProcessInitializationQueue();
            Loc.Current.LogServices();
        }
    }
}