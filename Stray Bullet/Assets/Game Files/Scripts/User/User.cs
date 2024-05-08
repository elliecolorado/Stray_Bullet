using System;

namespace Com.Elrecoal.Stray_Bullet
{
    public class User
    {
        public int id;

        public string email; //User username

        public string username; //User's username (let it be changed or not? If yes, update on database)
        
        public int level;

        public int exp;

        public string password; //User's password

        public DateTime signup_date;//Unchangeable data, registers when did the player create the user

        public int wins;//Every time a match ends (winning) it increases

        public int loses; ////Every time a match ends (losing) it increases

        public int total_matches; //Every time a match ends it increases

        public float total_average_kd; //It's the average from all the numbers in average_kds

        public float[] average_kds; //Every time a match ends, kills is divided by deaths and the result is added here

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
    }
}