using UnityEngine;

public class PlayerCam : MonoBehaviour
{
        [SerializeField] float sensX, sensY, limitAngleY, lerpTime;
        [SerializeField] Transform cameraPos;
        private float _mouseX, _mouseY;
        float _xRotation, _yRotation, newPosX, newPosY;
        private Quaternion _newRotation;
    
        void Update()
        {
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
