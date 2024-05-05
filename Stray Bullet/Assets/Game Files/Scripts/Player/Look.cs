using UnityEngine;
using Photon.Pun;


namespace Com.Elrecoal.Stray_Bullet
{

    public class Look : MonoBehaviourPunCallbacks
    {

        public static bool cursorLocked = true;

        public Transform player;

        public Transform cams;

        public Transform weapon;

        public float xSensitivity;

        public float ySensitivity;

        public float maxAngle;

        private Quaternion camCenter;


        public void Start()
        {

            camCenter = cams.localRotation; //Pone el origen de rotación de las cámaras a camCencer

        }

        void Update()
        {

            if (!photonView.IsMine || Pause.paused) return;

            SetX();

            SetY();

            UpdateCursorLock();

        }


        void SetY()
        {

            float t_input = Input.GetAxis("Mouse Y") * ySensitivity * Time.deltaTime;

            Quaternion t_adj = Quaternion.AngleAxis(t_input, -Vector3.right);

            Quaternion t_delta = cams.localRotation * t_adj;

            if (Quaternion.Angle(camCenter, t_delta) < maxAngle)
            {

                cams.localRotation = t_delta;

                weapon.localRotation = t_delta;

            }

        }

        void SetX()
        {

            float t_input = Input.GetAxis("Mouse X") * xSensitivity * Time.deltaTime;

            Quaternion t_adj = Quaternion.AngleAxis(t_input, Vector3.up);

            Quaternion t_delta = player.localRotation * t_adj;

            player.localRotation = t_delta;

        }

        void UpdateCursorLock()
        {

            if (cursorLocked)
            {

                Cursor.lockState = CursorLockMode.Locked;

                Cursor.visible = false;

            }
            else
            {

                Cursor.lockState = CursorLockMode.None;

                Cursor.visible = true;

            }

        }


    }

}