using System;

namespace Com.Elrecoal.Stray_Bullet
{
    [System.Serializable]
    public class User
    {
        public int id;
        public string username;
        public string password;

        public int level;
        public int exp;

        public int wins;
        public int loses;
        public int total_matches;

        public float[] last_10_kds = new float[10]; //Every time a match ends, kills is divided by deaths and the result is added here to calculate the average of last 10 matches
        public float total_average_kd; //It's the average from all the numbers in average_kds

        public User(string username, int level, int xp)
        {
            this.username = username;
            this.level = level;
            this.exp = xp;
        }
        public User()
        {
            this.username = "";
            this.level = 0;
            this.exp = 0;
        }

        public void UpdateAverageKD(float kdNuevo)
        {

            for (int i = last_10_kds.Length - 1; i > 0; i--) last_10_kds[i] = last_10_kds[i - 1];
            last_10_kds[0] = kdNuevo;

            float sum = 0;
            for (int i = 0; i < last_10_kds.Length; i++) sum += last_10_kds[i];
            total_average_kd = sum / last_10_kds.Length;

        }

    }
}