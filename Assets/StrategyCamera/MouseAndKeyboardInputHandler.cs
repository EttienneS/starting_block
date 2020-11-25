using UnityEngine;

namespace Assets.StrategyCamera
{
    public class MouseAndKeyboardInputHandler : CameraInputHandler
    {
        private Vector3 _dragCurrentPosition;
        private Vector3 _dragStartPosition;
        private Vector3 _rotateCurrentPosition;
        private Vector3 _rotateStartPosition;

        private CameraController _cameraController;

        public MouseAndKeyboardInputHandler(CameraController cameraController)
        {
            _cameraController = cameraController;
        }

        public override void HandleInput()
        {
            HandleMouseWheel();

            HandleRightClick();

            HandleMiddleMouse();

            HandleArrowMovement();

            HandleKeyboardZoom();
        }

        private void HandleArrowMovement()
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                _cameraController.AddCameraCommand(new MoveCommand(_cameraController.transform.forward * _cameraController.movementSpeed));
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                _cameraController.AddCameraCommand(new MoveCommand(_cameraController.transform.forward * -_cameraController.movementSpeed));
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                _cameraController.AddCameraCommand(new MoveCommand(_cameraController.transform.right * _cameraController.movementSpeed));
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                _cameraController.AddCameraCommand(new MoveCommand(_cameraController.transform.right * -_cameraController.movementSpeed));
            }
        }

        private void HandleKeyboardZoom()
        {
            if (Input.GetKey(KeyCode.R))
            {
                _cameraController.AddCameraCommand(new ZoomCommand(_cameraController.zoomAmount));
            }
            if (Input.GetKey(KeyCode.F))
            {
                _cameraController.AddCameraCommand(new ZoomCommand(_cameraController.zoomAmount * -1f));
            }
        }

        private void HandleMouseWheel()
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                _cameraController.AddCameraCommand(new ZoomCommand(Input.mouseScrollDelta.y * _cameraController.zoomAmount));
            }
        }

        private void FollowPlaneDrag()
        {
            var plane = new Plane(Vector3.up, Vector3.zero);
            var ray = _cameraController.Camera.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out float entry))
            {
                _dragCurrentPosition = ray.GetPoint(entry);
                _cameraController.newPosition = _cameraController.transform.position + _dragStartPosition - _dragCurrentPosition;
            }
        }

        private void HandleMiddleMouse()
        {
            if (Input.GetMouseButtonDown(2))
            {
                StartMiddleMouseRotate();
            }

            if (Input.GetMouseButton(2))
            {
                RotateWithMiddleMouse();
            }
        }

        private void HandleRightClick()
        {
            if (Input.GetMouseButtonDown(1))
            {
                StartPlaneDrag();
            }

            if (Input.GetMouseButton(1))
            {
                FollowPlaneDrag();
            }
        }

        private void RotateWithMiddleMouse()
        {
            _rotateCurrentPosition = Input.mousePosition;

            var diff = _rotateStartPosition - _rotateCurrentPosition;
            _rotateStartPosition = _rotateCurrentPosition;

            _cameraController.AddCameraCommand(new RotationCommand(diff));
        }

        private void StartMiddleMouseRotate()
        {
            _rotateStartPosition = Input.mousePosition;
        }

        private void StartPlaneDrag()
        {
            var plane = new Plane(Vector3.up, Vector3.zero);
            var ray = _cameraController.Camera.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out float entry))
            {
                _dragStartPosition = ray.GetPoint(entry);
            }
        }
    }
}