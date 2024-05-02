using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

namespace Com.Elrecoal.Stray_Bullet
{

    public class Player : MonoBehaviourPunCallbacks
    {

        #region Variables

        public float speed;

        public float sprintModifier;

        public float jumpForce;

        public float lengthOfSlide;

        public float slideModifier;

        public float movementCounter;

        public float idleCounter;

        public int max_health;

        public Camera normalCam;

        public GameObject cameraParent;

        public Transform weaponParent;

        public Transform groundDetector;

        public LayerMask ground;

        private float baseFOV;

        private float sprintFOVModifier = 1.25f;

        private float slide_time;

        private int current_health;

        private bool sliding;

        private Rigidbody rig;

        private Vector3 targetWeaponBobPosition;

        private Vector3 weaponParentOrigin;

        private Vector3 slide_direction;

        private Manager manager;

        private Weapon weapon;

        private Transform ui_healthBar;

        private TMP_Text ui_ammo;

        private Vector3 origin;

        private Vector3 weaponParentCurrentPosition;

        #endregion

        #region Unity Methods

        public void Start()
        {

            manager = GameObject.Find("Manager").GetComponent<Manager>();
            weapon = this.GetComponent<Weapon>();
            current_health = max_health;
            cameraParent.SetActive(photonView.IsMine);

            if (!photonView.IsMine) gameObject.layer = 11;

            baseFOV = normalCam.fieldOfView;
            origin = normalCam.transform.localPosition;

            if (Camera.main) Camera.main.enabled = false;

            rig = GetComponent<Rigidbody>();
            weaponParentOrigin = weaponParent.localPosition;
            weaponParentCurrentPosition = weaponParentOrigin;

            if (photonView.IsMine)
            {

                ui_healthBar = GameObject.Find("HUD/Health/Bar").transform;
                ui_ammo = GameObject.Find("HUD/Ammo/Text").GetComponent<TMP_Text>();
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
            bool jump = Input.GetKeyDown(KeyCode.Space);

            // Estados
            bool isGrounded = Physics.Raycast(groundDetector.position, Vector3.down, 0.1f, ground);
            bool isJumping = jump && isGrounded;
            bool isSprinting = sprint && t_vmove > 0 && !isJumping && isGrounded;

            // Salto
            if (isJumping) rig.AddForce(Vector3.up * jumpForce);
            if (Input.GetKeyDown(KeyCode.U)) TakeDamage(1001);

            //Cabeceo y "respiracion"
            if (!isGrounded)
            {
                //En el aire
                HeadBob(idleCounter, 0.01f, 0.01f);
                idleCounter += 0;
                weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 2f);

            }
            else if (sliding)
            {
                //Deslizandose
                HeadBob(movementCounter, 0.15f, 0.075f);
                weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 10f);
            }
            else if (t_hmove == 0 && t_vmove == 0)
            {
                //Sin moverse
                HeadBob(idleCounter, 0.025f, 0.025f);
                idleCounter += Time.deltaTime;
                weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 2f);

            }
            else if (!isSprinting)
            {
                //Andando
                HeadBob(movementCounter, 0.035f, 0.035f);
                movementCounter += Time.deltaTime * 6f;
                weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 6f);
            }
            else
            {
                //Esprintando
                HeadBob(movementCounter, 0.15f, 0.075f);
                movementCounter += Time.deltaTime * 13.5f;
                weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 10f);

            }

            //Actualizacion de interfaz
            RefreshHealthBar();

            weapon.RefreshAmmo(ui_ammo);

        }

        private void FixedUpdate()
        {

            if (!photonView.IsMine) return;

            if (Input.GetKeyDown(KeyCode.U)) TakeDamage(1001); //Botón temporal para forzar reaparición

            // Ejes
            float t_hmove = Input.GetAxisRaw("Horizontal");
            float t_vmove = Input.GetAxisRaw("Vertical");

            // Controles
            bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            bool jump = Input.GetKey(KeyCode.Space);
            bool slide = Input.GetKey(KeyCode.LeftControl);

            // Estados
            bool isGrounded = Physics.Raycast(groundDetector.position, Vector3.down, 0.1f, ground);
            bool isJumping = jump && isGrounded;
            bool isSprinting = sprint && t_vmove > 0 && !isJumping && isGrounded;
            bool isSliding = isSprinting && slide && !sliding;

            // Movimiento
            Vector3 t_direction = Vector3.zero;
            float t_adjustedpeed = speed;

            if (!sliding)
            {

                t_direction = new Vector3(t_hmove, 0, t_vmove);
                t_direction.Normalize();
                t_direction = transform.TransformDirection(t_direction);

                if (isSprinting) t_adjustedpeed *= sprintModifier;

            }
            else
            {

                t_direction = slide_direction;
                t_adjustedpeed *= slideModifier;
                slide_time -= Time.deltaTime;
                if (slide_time <= 0)
                {
                    sliding = false;
                    weaponParentCurrentPosition += Vector3.up * 0.5f;

                }

            }

            Vector3 t_targetVelocity = t_direction * t_adjustedpeed * Time.deltaTime;
            t_targetVelocity.y = rig.velocity.y;
            rig.velocity = t_targetVelocity;


            //Sliding
            if (isSliding)
            {

                sliding = true;
                slide_direction = t_direction;
                slide_time = lengthOfSlide;
                weaponParentCurrentPosition += Vector3.down * 0.5f;

            }

            // Cosas de la cámara
            if (sliding)
            {

                normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV * sprintFOVModifier * 1.15f, Time.deltaTime * 8f);
                normalCam.transform.localPosition = Vector3.Lerp(normalCam.transform.localPosition, origin + Vector3.down * 0.5f, Time.deltaTime * 8f);

            }
            else
            {

                if (isSprinting)
                {
                    normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV * sprintFOVModifier, Time.deltaTime * 8f);
                }

                else
                {
                    normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV, Time.deltaTime * 8f);
                }

                normalCam.transform.localPosition = Vector3.Lerp(normalCam.transform.localPosition, origin, Time.deltaTime * 8f);

            }

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

            float t_aim_Adjust = 1f;

            if (weapon.isAiming)
            {
                t_aim_Adjust = 0.1f;
            }

            targetWeaponBobPosition = weaponParentCurrentPosition + new Vector3(Mathf.Cos(p_z) * p_x_intensity * t_aim_Adjust, Mathf.Sin(p_z * 2) * p_y_intensity * t_aim_Adjust, 0);

        }

        public void TakeDamage(int p_damage)
        {

            if (photonView.IsMine)
            {

                current_health -= p_damage;
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