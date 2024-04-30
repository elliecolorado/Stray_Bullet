using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using TMPro;


namespace Com.Elrecoal.Stray_Bullet
{

    public class Weapon : MonoBehaviourPunCallbacks
    {
        //-----------------------------------Mover todo a player-----------------------------------
        #region Variables

        public Gun[] loadout;

        public Transform weaponParent;

        private GameObject currentEquipment;

        public GameObject bulletHolePrefab;

        public LayerMask canBeShot;

        private float currentCooldown = 0;

        private int currentIndex;

        private bool isReloading;

        #endregion

        #region Unity Methods

        private void Start()
        {
            foreach (Gun g in loadout) g.Init();

            Equip(0);

        }

        void Update()
        {

            if (photonView.IsMine)
            {

                if (Input.GetKeyDown(KeyCode.Alpha1)) photonView.RPC("Equip", RpcTarget.All, 0);
                if (Input.GetKeyDown(KeyCode.Alpha2)) photonView.RPC("Equip", RpcTarget.All, 1);
                if (Input.GetKeyDown(KeyCode.Alpha3)) photonView.RPC("Equip", RpcTarget.All, 2);
                if (Input.GetKeyDown(KeyCode.Alpha4)) photonView.RPC("Equip", RpcTarget.All, 3);
                if (Input.GetKeyDown(KeyCode.Alpha5)) photonView.RPC("Equip", RpcTarget.All, 4);

            }

            if (currentEquipment != null)
            {

                if (photonView.IsMine)
                {

                    Aim(Input.GetMouseButton(1));

                    if (Input.GetMouseButton(0) && currentCooldown <= 0 && !isReloading)
                    {
                        if (loadout[currentIndex].FireBullet()) photonView.RPC("Shoot", RpcTarget.All);

                        else if (loadout[currentIndex].GetStash() > 0) StartCoroutine(Reload(loadout[currentIndex].reload));

                    }

                    if (Input.GetKeyDown(KeyCode.R) && !isReloading && loadout[currentIndex].GetStash() > 0) StartCoroutine(Reload(loadout[currentIndex].reload));

                    //Cooldown
                    if (currentCooldown > 0) currentCooldown -= Time.deltaTime;

                }

                //Weapon position elasticity
                currentEquipment.transform.localPosition = Vector3.Lerp(currentEquipment.transform.localPosition, Vector3.zero, Time.deltaTime * 4f);

            }

        }

        #endregion

        #region Personal Methods

        public void RefreshAmmo(TMP_Text p_text)
        {

            int t_clip = loadout[currentIndex].GetClip();

            int t_stash = loadout[currentIndex].GetStash();

            p_text.text = t_clip.ToString("D2") + " / " + t_stash.ToString("D2");

        }

        IEnumerator Reload(float p_wait)
        {

            isReloading = true;

            currentEquipment.SetActive(false);

            yield return new WaitForSeconds(p_wait);

            loadout[currentIndex].Reload();

            currentEquipment.SetActive(true);

            isReloading = false;

        }

        [PunRPC]
        void Equip(int p_ind)
        {
            //-----------------------------------Usar rueda del rat�n para ciclar entre armas (tener en cuenta final de loadout y volver a empezar o poner el ultimo arma de limite?)-----------------------------------
            if (p_ind < loadout.Length)
            {

                if (currentEquipment != null)
                {
                    StopCoroutine("Reload");
                    Destroy(currentEquipment);
                }

                currentIndex = p_ind;

                GameObject t_newEquipment = Instantiate(loadout[p_ind].prefab, weaponParent.position, weaponParent.rotation, weaponParent) as GameObject;

                t_newEquipment.transform.localPosition = Vector3.zero;

                t_newEquipment.transform.localEulerAngles = Vector3.zero;

                t_newEquipment.GetComponent<Sway>().isMine = photonView.IsMine;

                currentEquipment = t_newEquipment;
            }

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
                //-----------------------------------Modificar si a�ado explosivos para que sean diferentes agujeros de bala/explosivo-----------------------------------
                //-----------------------------------Modificar para que las balas no se pongan en la cara de los jugadores y solucionar el que apunte siempre hacia delante-----------------------------------
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

            GetComponent<Player>().TakeDamage(p_damage);

        }

        #endregion

    }

}