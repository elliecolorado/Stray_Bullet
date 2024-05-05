using System;

public class User
{
    public int id;

    public string email; //User username

    public string username; //User's username (let it be changed or not? If yes, update on database)

    public string password; //User's password

    public DateTime signup_date;//Unchangeable data, registers when did the player create the user

    public Stats statistics; //File where all stats (kd, wins, loses...) is stored


}
