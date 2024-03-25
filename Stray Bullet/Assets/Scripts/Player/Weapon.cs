using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


namespace Com.Elrecoal.Stray_Bullet
{

    public class Weapon : MonoBehaviourPunCallbacks
    {

        #region Variables

        public Gun[] loadout;

        public Transform weaponParent;

        private GameObject currentEquipment;

        public GameObject bulletHolePrefab;

        public LayerMask canBeShot;

        private float currentCooldown = 0;

        private int currentIndex;

        #endregion

        #region Unity Methods

        void Update()
        {

            if (photonView.IsMine && Input.GetKeyDown(KeyCode.Alpha1)) photonView.RPC("Equip", RpcTarget.All, 0);

            if (currentEquipment != null)
            {

                if (photonView.IsMine)
                {

                    Aim(Input.GetMouseButton(1));

                    if (Input.GetMouseButton(0) && currentCooldown <= 0) photonView.RPC("Shoot", RpcTarget.All);

                    //Cooldown
                    if (currentCooldown > 0) currentCooldown -= Time.deltaTime;

                }

                //Weapon position elasticity
                currentEquipment.transform.localPosition = Vector3.Lerp(currentEquipment.transform.localPosition, Vector3.zero, Time.deltaTime * 4f);

            }

        }

        #endregion

        #region Personal Methods

        [PunRPC]
        void Equip(int p_ind)
        {

            if (currentEquipment != null) Destroy(currentEquipment);

            currentIndex = p_ind;

            GameObject t_newEquipment = Instantiate(loadout[p_ind].prefab, weaponParent.position, weaponParent.rotation, weaponParent) as GameObject;

            t_newEquipment.transform.localPosition = Vector3.zero;

            t_newEquipment.transform.localEulerAngles = Vector3.zero;

            t_newEquipment.GetComponent<Sway>().isMine = photonView.IsMine;

            currentEquipment = t_newEquipment;

        }

        void Aim(bool p_isAiming)
        {

            Transform t_anchor = currentEquipment.transform.Find("Anchor");

            Transform t_state_ads = currentEquipment.transform.Find("States/ADS");

            Transform t_state_hip = currentEquipment.transform.Find("States/Hip");

            if (p_isAiming) t_anchor.position = Vector3.Lerp(t_anchor.position, t_state_ads.position, Time.deltaTime * loadout[currentIndex].aimSpeed);

            else t_anchor.position = Vector3.Lerp(t_anchor.position, t_state_hip.position, Time.deltaTime * loadout[currentIndex].aimSpeed);

        }

        [PunRPC]
        void Shoot()
        {

            Transform t_spawn = transform.Find("Cameras/Normal Camera");

            //Bloom
            Vector3 t_bloom = t_spawn.position + t_spawn.forward * 1000f;

            t_bloom += Random.Range(-loadout[currentIndex].bloom, loadout[currentIndex].bloom) * t_spawn.up;

            t_bloom += Random.Range(-loadout[currentIndex].bloom, loadout[currentIndex].bloom) * t_spawn.right;

            t_bloom -= t_spawn.position;

            t_bloom.Normalize();

            //Cooldown (segundos que tarda en poder volver a disparar)
            currentCooldown = loadout[currentIndex].rateOfFire;

            //Raycast
            RaycastHit t_hit = new RaycastHit();

            if (Physics.Raycast(t_spawn.position, t_bloom, out t_hit, 1000f, canBeShot))
            {

                GameObject t_newBulletHole = Instantiate(bulletHolePrefab, t_hit.point + t_hit.normal * 0.001f, Quaternion.identity) as GameObject;

                t_newBulletHole.transform.LookAt(t_hit.point + t_hit.normal);

                Destroy(t_newBulletHole, 5);

                if (photonView.IsMine)
                {

                    //Shooting a player
                    if (t_hit.collider.gameObject.layer == 11)
                    {

                        //RpcTarget call to damage player
                        t_hit.collider.gameObject.GetPhotonView().RPC("TakeDamage", RpcTarget.All, loadout[currentIndex].damage);

                    }

                }

            }

            //Gun effect
            currentEquipment.transform.Rotate(-loadout[currentIndex].recoil, 0, 0);

            currentEquipment.transform.position -= currentEquipment.transform.forward * loadout[currentIndex].kickback;

        }

        [PunRPC]
        void TakeDamage(int p_damage)
        {

            GetComponent<Motion>().TakeDamage(p_damage);

        }

        #endregion

    }

}