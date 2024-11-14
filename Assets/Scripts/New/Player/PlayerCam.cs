using System;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
        public float sensX, sensY, limitAngleY, sens;
        [SerializeField] Transform cameraPos;
        private float _mouseX, _mouseY;
        float _xRotation, _yRotation, newPosX, newPosY;
        private Quaternion _newRotation;
        private bool _cameraLock;
        private bool _ritualCinematic;
        private bool _lookVoodoDoll;
        private bool _inSpot;
        public float ticks;

        private void Awake()
        {
                GetValueSens();
        }

        public bool CameraLock
        {
                get => _cameraLock;
                set => _cameraLock = value;
        }

        void Update()
        {
                _ritualCinematic = PlayerHandler.Instance.movement.ritualCinematic;
                _lookVoodoDoll = PlayerHandler.Instance.movement.voodooMovement;
                RotatePlayer();
                LookRitual();
                LookVoodoo();
        }

        void RotatePlayer()
        {
                if (_ritualCinematic || _lookVoodoDoll) return;
                if (_cameraLock)
                {
                        _mouseX = 0;
                        _mouseY = 0;
                        return;
                }
                _mouseX = Input.GetAxis("Mouse X") * sensX * sens * Time.deltaTime;
                _mouseY = Input.GetAxis("Mouse Y") * sensY * sens * Time.deltaTime;
                _xRotation = Mathf.Lerp(_xRotation, _mouseX, .5f);
                _yRotation += _mouseY;
                _yRotation = Mathf.Clamp(_yRotation, -limitAngleY, limitAngleY);
                cameraPos.localRotation =
                        Quaternion.Slerp(cameraPos.localRotation, Quaternion.Euler(-_yRotation, 0, 0), .5f);
                //cameraPos.localRotation = Quaternion.Euler(-_yRotation, 0, 0);
                transform.Rotate(Vector3.up * _xRotation);
        }

        void LookRitual()
        {
                if (!_ritualCinematic) return;
                if (_lookVoodoDoll) return;
                var target = transform.localRotation;
                target.x = Quaternion.identity.x;
                cameraPos.localRotation = Quaternion.Slerp(cameraPos.localRotation, Quaternion.identity, .7f * Time.deltaTime);
                _yRotation = 0;
        }

        void LookVoodoo()
        {
                if (!_lookVoodoDoll) return;
                

        }
        
        
        private void GetValueSens()
        {
                if (PlayerPrefs.HasKey("Sens"))
                {
                        sens = PlayerPrefs.GetFloat("Sens");
                        PlayerPrefs.Save();
                        return;
                }

                sens = 1;
                PlayerPrefs.SetFloat("Sens",sens);
                PlayerPrefs.Save();
        }
}
