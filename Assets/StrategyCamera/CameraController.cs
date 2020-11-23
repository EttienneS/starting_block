using Assets.ServiceLocator;
using UnityEngine;

namespace Assets.StrategyCamera
{
    public class CameraController : LocatableMonoBehavior
    {
        public Camera Camera;
        public float fastSpeed;
        public float maxZoom;
        public float minZoom;
        public float movementSpeed;
        public float movementTime;
        public float normalSpeed;
        public float rotationAmount;
        public Vector3 zoomAmount;

        internal Vector3 dragCurrentPosition;
        internal Vector3 dragStartPosition;
        internal Vector3 newPosition;
        internal Quaternion newRotation;
        internal Vector3 newZoom;
        internal Vector3 rotateCurrentPosition;
        internal Vector3 rotateStartPosition;

        private Transform _followTransform;

        private int _maxX;
        private int _maxZ;
        private int _minX;
        private int _minZ;

        public void ConfigureBounds(int minx, int maxx, int minz, int maxz)
        {
            _minX = minx;
            _maxX = maxx;
            _minZ = minz;
            _maxZ = maxz;
        }

        public void FollowTransform(Transform transform)
        {
            if (_followTransform == transform)
            {
                StopFollowing();
            }
            else
            {
                _followTransform = transform;
            }
        }

        public override void Initialize()
        {
            newPosition = transform.position;
            newRotation = transform.rotation;

            newZoom = Camera.transform.localPosition;

            MoveToWorldCenter();
        }

        public void StopFollowing()
        {
            _followTransform = null;
        }

        public void Update()
        {
            if (_followTransform != null)
            {
                transform.position = _followTransform.position;
            }
            else
            {
                HandleMouseInput();
                HandleMovementInput();
            }
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                StopFollowing();
            }
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

        private void HandleMouseInput()
        {
            // https://www.youtube.com/watch?v=rnqF6S7PfFA&t=212s (from 12:00)
            if (Input.mouseScrollDelta.y != 0)
            {
                newZoom += Input.mouseScrollDelta.y * zoomAmount;
                newZoom = new Vector3(newZoom.x,
                                      Mathf.Clamp(newZoom.y, minZoom, maxZoom),
                                      Mathf.Clamp(newZoom.z, -maxZoom, -minZoom));
            }

            if (Input.GetMouseButtonDown(1))
            {
                var plane = new Plane(Vector3.up, Vector3.zero);
                var ray = Camera.ScreenPointToRay(Input.mousePosition);

                if (plane.Raycast(ray, out float entry))
                {
                    dragStartPosition = ray.GetPoint(entry);
                }
            }

            if (Input.GetMouseButton(1))
            {
                var plane = new Plane(Vector3.up, Vector3.zero);
                var ray = Camera.ScreenPointToRay(Input.mousePosition);

                if (plane.Raycast(ray, out float entry))
                {
                    dragCurrentPosition = ray.GetPoint(entry);
                    newPosition = transform.position + dragStartPosition - dragCurrentPosition;
                }
            }

            if (Input.GetMouseButtonDown(2))
            {
                rotateStartPosition = Input.mousePosition;
            }

            if (Input.GetMouseButton(2))
            {
                rotateCurrentPosition = Input.mousePosition;

                var diff = rotateStartPosition - rotateCurrentPosition;
                rotateStartPosition = rotateCurrentPosition;

                newRotation *= Quaternion.Euler(Vector3.up * (-diff.x / 5));
            }
        }

        private void HandleMovementInput()
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                movementSpeed = fastSpeed;
            }
            else
            {
                movementSpeed = normalSpeed;
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                newPosition += transform.forward * movementSpeed;
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                newPosition += transform.forward * -movementSpeed;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                newPosition += transform.right * movementSpeed;
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                newPosition += transform.right * -movementSpeed;
            }

            //if (Input.GetKey(KeyCode.Q))
            //{
            //    newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
            //}
            //if (Input.GetKey(KeyCode.E))
            //{
            //    newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
            //}

            if (Input.GetKey(KeyCode.R))
            {
                newZoom += zoomAmount;
            }
            if (Input.GetKey(KeyCode.F))
            {
                newZoom -= zoomAmount;
            }

            transform.position = ClampPosition(Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime));
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
            Camera.transform.localPosition = ClampZoom(Vector3.Lerp(Camera.transform.localPosition, newZoom, Time.deltaTime * movementTime));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 0, 0, 0.75f);
            Gizmos.DrawCube(transform.position, new Vector3(0.25f, 0.25f, 0.25f));
        }
    }
}