using Assets.ServiceLocator;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.StrategyCamera
{
    public class CameraController : LocatableMonoBehavior
    {
        public Camera Camera;
        public float maxZoom;
        public float minZoom;
        public float movementSpeed;
        public float movementTime;
        public float normalSpeed;
        public float rotationAmount;
        public Vector3 zoomAmount;

        internal Vector3 newPosition;
        internal Quaternion newRotation;
        internal Vector3 newZoom;

        private readonly Queue<CameraCommand> _commands = new Queue<CameraCommand>();
        private CameraInputHandler _cameraInputHandler;
        private int _maxX;
        private int _maxZ;
        private int _minX;
        private int _minZ;

        public void AddCameraCommand(CameraCommand command)
        {
            _commands.Enqueue(command);
        }

        public void ConfigureBounds(int minx, int maxx, int minz, int maxz)
        {
            _minX = minx;
            _maxX = maxx;
            _minZ = minz;
            _maxZ = maxz;
        }

        public override void Initialize()
        {
            ResetDeltas();

            MoveToWorldCenter();

            // https://gameprogrammingpatterns.com/command.html
            // using the command pattern we can easily change the handler to work diffirently when on a phone
#if (UNITY_IPHONE || UNITY_ANDROID)
            //_cameraInputHandler = new TouchScreenHandler(this);
            _cameraInputHandler = new MouseAndKeyboardInputHandler(this);
#else
            _cameraInputHandler = new MouseAndKeyboardInputHandler(this);
#endif

        }

        public void Update()
        {
            _cameraInputHandler.HandleInput();

            while (_commands.Count > 0)
            {
                _commands.Dequeue().Execute(this);
            }
            UpdateCameraAndEnsureBounds();
        }

        internal float GetPerpendicularRotation()
        {
            return 90 + transform.rotation.eulerAngles.y;
        }

        internal void MoveToWorldCenter()
        {
            transform.position = new Vector3((_maxX - _minX) / 2f, 1f, (_maxZ - _minZ) / 2f);

            newPosition = transform.position;
            newZoom = new Vector3(0, (minZoom + maxZoom) / 2f, -((minZoom + maxZoom) / 2f));
        }

        private Vector3 ClampPosition(Vector3 position)
        {
            return new Vector3(Mathf.Clamp(position.x, _minX, _maxX),
                               position.y,
                               Mathf.Clamp(position.z, _minZ, _maxZ));
        }

        private Vector3 ClampZoom(Vector3 zoom)
        {
            return new Vector3(zoom.x,
                               Mathf.Clamp(zoom.y, minZoom, maxZoom),
                               Mathf.Clamp(zoom.z, -maxZoom, -minZoom));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 0, 0, 0.75f);
            Gizmos.DrawCube(transform.position, new Vector3(1f, 5f, 1f));
        }

        private void ResetDeltas()
        {
            newPosition = transform.position;
            newRotation = transform.rotation;
            newZoom = Camera.transform.localPosition;
        }

        private void UpdateCameraAndEnsureBounds()
        {
            transform.position = ClampPosition(Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime));
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
            Camera.transform.localPosition = ClampZoom(Vector3.Lerp(Camera.transform.localPosition, newZoom, Time.deltaTime * movementTime));
        }
    }
}