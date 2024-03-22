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

        #endregion

        #region Unity Methods

        void Update()
        {

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {

                Equip(0);

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

            GameObject t_newEquipment = Instantiate(loadout[p_ind].prefab, weaponParent.position, weaponParent.rotation, weaponParent) as GameObject;

            t_newEquipment.transform.localPosition = Vector3.zero;

            t_newEquipment.transform.localEulerAngles = Vector3.zero;

            currentEquipment = t_newEquipment;

        }

        #endregion

    }

}