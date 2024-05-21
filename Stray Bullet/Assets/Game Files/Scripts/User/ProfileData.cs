using System;

namespace Com.Elrecoal.Stray_Bullet
{
    [System.Serializable]
    public class ProfileData
    {
        public string username;
        public string signup_date;
        public int level;
        public int exp;

        public ProfileData(string username, int level, int xp)
        {
            this.username = username;
            this.level = level;
            this.exp = xp;
        }

        public ProfileData()
        {
        }



        /*
        ----------Implementar más tarde----------
        public int wins;
        public int loses;
        public int total_matches;
        public float[] last_10_kds = new float[10]; //Every time a match ends, kills is divided by deaths and the result is added here to calculate the average of last 10 matches
        public float total_average_kd; //It's the average from all the numbers in average_kds

        public void UpdateAverageKD(float kdNuevo)
        {

            for (int i = last_10_kds.Length - 1; i > 0; i--) last_10_kds[i] = last_10_kds[i - 1];
            last_10_kds[0] = kdNuevo;

            float sum = 0;
            for (int i = 0; i < last_10_kds.Length; i++) sum += last_10_kds[i];
            total_average_kd = sum / last_10_kds.Length;

        }
        */

    }
}