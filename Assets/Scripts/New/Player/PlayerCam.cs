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
        private Vector3 cameraForward;

        private float yaw;   // rotación acumulada en X (horizontal)
        private float pitch; // rotación acumulada en Y (vertical)

        private float targetYaw;
        private float targetPitch;
        public float smoothSpeed = 10;
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
                float mouseX = Input.GetAxis("Mouse X") * sens * sensX * Time.deltaTime;
                float mouseY = Input.GetAxis("Mouse Y") * sens * sensY * Time.deltaTime;
                
                targetYaw += mouseX;
                targetPitch -= mouseY;
                targetPitch = Mathf.Clamp(targetPitch, -limitAngleY, limitAngleY);

                yaw = Mathf.Lerp(yaw, targetYaw, Time.deltaTime * smoothSpeed);
                pitch = Mathf.Lerp(pitch, targetPitch, Time.deltaTime * smoothSpeed);

                transform.localRotation = Quaternion.Euler(0f, yaw, 0f);
                cameraPos.localRotation = Quaternion.Euler(pitch, 0f, 0f); 
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
                if (!_lookVoodoDoll)
                {
                        cameraForward = cameraPos.forward;
                        return;
                }
                ticks += Time.deltaTime;

                Vector3 dir = RitualManager.Instance.actualItemActive.transform.position - cameraPos.position;

                cameraPos.forward = Vector3.Lerp(cameraForward, dir, ticks);

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
