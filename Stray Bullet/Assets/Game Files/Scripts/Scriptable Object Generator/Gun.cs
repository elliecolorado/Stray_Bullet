using UnityEngine;

namespace Com.Elrecoal.Stray_Bullet
{

    [CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]

    public class Gun : ScriptableObject
    {

        public string gunName;

        public int damage;

        public float bloom;

        public float reload;

        public int ammo;

        public int clipSize;

        public float recoil;

        public float kickback;

        public float rateOfFire;

        public float aimSpeed;

        public GameObject prefab;

        public AudioClip gunShotSound;

        public float gunShotVolume;

        public float pitchRandomization;

        public int burst; // 0 semi | 1 auto | 2+ ráfaga?

        private int stash; //Current ammo

        private int clip; //Current clip

        public int pellets;

        public bool recovery; //Variable para animaciones

        public float aimBloomDivider;

        public void Init()
        {
            stash = ammo;
            clip = clipSize;
        }

        public bool FireBullet()
        {
            if (clip > 0)
            {
                clip -= 1;
                return true;
            }
            else return false;

        }

        public void Reload()
        {
            stash += clip;
            clip = Mathf.Min(clipSize, stash);
            stash -= clip;
        }

        public int GetStash() { return stash; }

        public int GetClip() { return clip; }

    }

}
