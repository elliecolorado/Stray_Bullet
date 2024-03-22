using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Elrecoal.Stray_Bullet
{

    public class Weapon : MonoBehaviour
    {

        #region Variables

        public Gun[] loadout;

        public Transform weaponParent;

        private GameObject currentEquipment;

        private int currentIndex;

        #endregion

        #region Unity Methods

        void Update()
        {

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {

                Equip(0);

            }

            if (currentEquipment != null)
            {

                Aim(Input.GetMouseButton(1));

            }

        }

        #endregion

        #region Personal Methods

        void Equip(int p_ind)
        {

            if (currentEquipment != null)
            {

                Destroy(currentEquipment);

            }

            currentIndex = p_ind;

            GameObject t_newEquipment = Instantiate(loadout[p_ind].prefab, weaponParent.position, weaponParent.rotation, weaponParent) as GameObject;

            t_newEquipment.transform.localPosition = Vector3.zero;

            t_newEquipment.transform.localEulerAngles = Vector3.zero;

            currentEquipment = t_newEquipment;

        }

        void Aim(bool p_isAiming)
        {

            Transform t_anchor = currentEquipment.transform.Find("Anchor");
            Transform t_state_ads = currentEquipment.transform.Find("States/ADS");
            Transform t_state_hip = currentEquipment.transform.Find("States/Hip");

            if (p_isAiming)
            {

                t_anchor.position = Vector3.Lerp(t_anchor.position, t_state_ads.position, Time.deltaTime * loadout[currentIndex].aimSpeed);

            }
            else
            {

                t_anchor.position = Vector3.Lerp(t_anchor.position, t_state_hip.position, Time.deltaTime * loadout[currentIndex].aimSpeed);

            }

        }

        #endregion

    }

}