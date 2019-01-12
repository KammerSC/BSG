using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings 
{
    byte bonusres = 0, maxdist = 8, skillbonus = 0, crisisextrastr = 0;
    byte maxraider = 16, maxheavyraider = 4, maxbattlestar = 2, viper = 8, raptor = 4;


    public void SetBonusRes(int x){
        if (x > 0 && bonusres < 4)
            bonusres++;
        else if (x < 0 && bonusres > -3)
            bonusres--;
    }
    public void SetMaxDist(int x){
        if (x > 0 && maxdist < 13)
            maxdist++;
        else if (x < 0 && maxdist > 5)
            maxdist--;
    }
}
