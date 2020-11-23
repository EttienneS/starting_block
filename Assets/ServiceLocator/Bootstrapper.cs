using Assets.StrategyCamera;
using UnityEngine;

namespace Assets.ServiceLocator
{
    /// <summary>
    ///   <para>Required controller monobehavior for the Service Locator.  This class initializes the ServiceLocator and keeps it alive.</para>
    ///   <para>Anything that manages the ServiceLocator lifecycle should live here.</para>
    /// </summary>
    public class Bootstrapper : MonoBehaviour
    {
        private ServiceLocator _serviceLocator;

        /// <summary>
        /// Creates the ServiceLocator and registers MonoBehaviors that already exist in the scene.
        /// Does an optional call to ProcessInitializationQueue to resovle the references in the loded monobehaviors.
        /// </summary>
        public void Awake()
        {
            var locator = new ServiceLocator();

            locator.Register(FindObjectOfType<CameraController>());
            locator.Register(FindObjectOfType<SimpleMapGen>());

            locator.ProcessInitializationQueue();
            locator.LogServices();

            _serviceLocator = locator;
        }

        /// <summary>Gets the service locator.</summary>
        /// <returns>
        ///   The current ServiceLocator
        /// </returns>
        public ServiceLocator GetServiceLocator()
        {
            return _serviceLocator;
        }
    }
}