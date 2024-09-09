using UnityEngine;

public class PlayerCam : MonoBehaviour
{
        public float sensX, sensY, limitAngleY, lerpTime;
        [SerializeField] Transform cameraPos;
        private float _mouseX, _mouseY;
        float _xRotation, _yRotation, newPosX, newPosY;
        private Quaternion _newRotation;
        private bool _cameraLock;

        public bool CameraLock
        {
                get => _cameraLock;
                set => _cameraLock = value;
        }
    
        void Update()
        {
                if (_cameraLock)
                {
                        _mouseX = 0;
                        _mouseY = 0;
                        return;
                }
                _mouseX = Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
                _mouseY = Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;

                _xRotation += _mouseX;
                _yRotation += _mouseY;
                _yRotation = Mathf.Clamp(_yRotation, -limitAngleY, limitAngleY);
        }

        private void LateUpdate()
        {
                cameraPos.localRotation = Quaternion.Euler(-_yRotation, 0, 0);
                transform.Rotate(Vector3.up * _mouseX);
        }
}
