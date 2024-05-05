using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    int id;

    string email; //User username

    string username; //User's username (maybe let it be changed? If so, update on database

    string password; //User's password

    DateTime signup_date;//Unchangeable data, registers when did the player create the user

    Stats statistics; //File where all stats (kd, wins, loses...) is stored


}
