using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats
{

    public int wins;//Every time a match ends (winning) it increases

    public int loses; ////Every time a match ends (losing) it increases

    public int total_matches; //Every time a match ends it increases

    public float total_average_kd; //It's the average from all the numbers in average_kds

    public float[] average_kds; //Every time a match ends, kills is divided by deaths and the result is added here

}
