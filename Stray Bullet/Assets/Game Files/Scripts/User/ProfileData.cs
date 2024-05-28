namespace Com.Elrecoal.Stray_Bullet
{
    [System.Serializable]
    public class ProfileData
    {
        public string username;
        public int played_matches;
        public int deaths;
        public string signup_date;

        public ProfileData(string username)
        {
            this.username = username;
        }

        public ProfileData()
        {
        }

    }
}