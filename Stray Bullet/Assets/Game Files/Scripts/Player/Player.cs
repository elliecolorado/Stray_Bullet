using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.Elrecoal.Stray_Bullet
{

    public class Player : MonoBehaviourPunCallbacks
    {

        #region Variables

        public float speed;

        public float sprintModifier;

        public float jumpForce;

        public int max_health;

        public Camera normalCam;

        public GameObject cameraParent;

        public Transform weaponParent;

        public Transform groundDetector;

        public LayerMask ground;

        private Rigidbody rig;

        private Vector3 targetWeaponBobPosition;

        private Vector3 weaponParentOrigin;

        public float movementCounter;

        public float idleCounter;

        private float baseFOV;

        private float sprintFOVModifier = 1.25f;

        private int current_health;

        private Manager manager;

        private Transform ui_healthBar;

        #endregion

        #region Unity Methods

        public void Start()
        {

            manager = GameObject.Find("Manager").GetComponent<Manager>();

            current_health = max_health;

            cameraParent.SetActive(photonView.IsMine);

            if (!photonView.IsMine) gameObject.layer = 11;

            baseFOV = normalCam.fieldOfView;

            if (Camera.main) Camera.main.enabled = false;

            rig = GetComponent<Rigidbody>();

            weaponParentOrigin = weaponParent.localPosition;

            if (photonView.IsMine)
            {

                ui_healthBar = GameObject.Find("HUD/Health/Bar").transform;

                RefreshHealthBar();

            }


        }

        public void Update()
        {

            if (!photonView.IsMine) return;

            // Ejes
            float t_hmove = Input.GetAxisRaw("Horizontal");

            float t_vmove = Input.GetAxisRaw("Vertical");

            // Controles
            bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

            bool jump = Input.GetKey(KeyCode.Space);

            // Estados
            bool isGrounded = Physics.Raycast(groundDetector.position, Vector3.down, 0.1f, ground);

            bool isJumping = jump && isGrounded;

            bool isSprinting = sprint && t_vmove > 0;

            // Salto
            if (isJumping) rig.AddForce(Vector3.up * jumpForce);

            //Cabeceo y "respiracion"
            if (t_hmove == 0 && t_vmove == 0)
            {

                HeadBob(idleCounter, 0.02f, 0.02f);

                idleCounter += Time.deltaTime;

                weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 2f);

            }
            else if (!isSprinting)
            {

                HeadBob(movementCounter, 0.035f, 0.035f);

                movementCounter += Time.deltaTime * 3f;

                weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 6f);

            }
            else
            {

                HeadBob(movementCounter, 0.15f, 0.075f);

                movementCounter += Time.deltaTime * 6f;

                weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 10f);

            }

            RefreshHealthBar();

        }

        private void FixedUpdate()
        {

            if (!photonView.IsMine) return;

            if (Input.GetKeyDown(KeyCode.U)) TakeDamage(1001);

            // Ejes
            float t_hmove = Input.GetAxisRaw("Horizontal");

            float t_vmove = Input.GetAxisRaw("Vertical");


            // Controles
            bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

            bool jump = Input.GetKey(KeyCode.Space);

            // Estados
            bool isGrounded = Physics.Raycast(groundDetector.position, Vector3.down, 0.1f, ground);

            bool isJumping = jump && isGrounded;

            bool isSprinting = sprint && t_vmove > 0;

            // Movimiento
            Vector3 t_direction = new Vector3(t_hmove, 0, t_vmove);

            t_direction.Normalize();

            float t_adjustedpeed = speed;

            if (isSprinting) t_adjustedpeed *= sprintModifier;

            Vector3 t_targetVelocity = transform.TransformDirection(t_direction * t_adjustedpeed * Time.deltaTime);

            t_targetVelocity.y = rig.velocity.y;

            rig.velocity = t_targetVelocity;

            // Campo de vista
            if (isSprinting) normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV * sprintFOVModifier, Time.deltaTime * 8f);

            else normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV, Time.deltaTime * 8f); ;

        }

        #endregion

        #region Personal Methods

        void RefreshHealthBar()
        {

            float t_health_ratio = (float)current_health / (float)max_health;

            ui_healthBar.localScale = Vector3.Lerp(ui_healthBar.localScale, new Vector3(t_health_ratio, 1, 1), Time.deltaTime * 8f);

        }

        void HeadBob(float p_z, float p_x_intensity, float p_y_intensity)
        {

            targetWeaponBobPosition = weaponParentOrigin + new Vector3(Mathf.Cos(p_z) * p_x_intensity, Mathf.Sin(p_z * 2) * p_y_intensity, 0);

        }

        public void TakeDamage(int p_damage)
        {

            if (photonView.IsMine)
            {

                current_health -= p_damage;

                Debug.Log(current_health);

                RefreshHealthBar();

                if (current_health <= 0)
                {

                    manager.Spawn();

                    PhotonNetwork.Destroy(gameObject);

                }

            }

        }

        #endregion

    }

}