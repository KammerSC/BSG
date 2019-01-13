using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings 
{
    sbyte bonusres = 0, maxdist = 8, skillbonus = 0, crisisextrastr = 0;
    sbyte maxraider = 16, maxheavyraider = 4, maxbattlestar = 2, viper = 8, raptor = 4;
    sbyte dmggalactica = 6 /*4-8*/, boardingparty = 5 /*4-6*/ /*jumpprep*/;


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
    public void SetSkillBonus(int x){
        if (x > 0 && skillbonus < 3)
            skillbonus++;
        else if (x < 0 && maxdist > -2)
            skillbonus--;
    }
    public void SetCrisisStr(int x){
        if (x > 0 && crisisextrastr < 4)
            crisisextrastr++;
        else if (x < 0 && crisisextrastr > -4)
            crisisextrastr--;
    }
    public void SetMaxRaider(int x){
        if (x > 0 && maxraider < 33)
            maxraider++;
        else if (x < 0 && maxraider > 7)
            maxraider--;
    }
    public void SetMaxHeavyRaider(int x){
        if (x > 0 && maxheavyraider < 9)
            maxheavyraider++;
        else if (x < 0 && maxheavyraider > 1)
            maxheavyraider--;
    }
    public void SetMaxBattleStar(int x){
        if (x > 0 && maxbattlestar < 33)
            maxbattlestar++;
        else if (x < 0 && maxbattlestar > 7)
            maxbattlestar--;
    }
    public void SetViper(int x){
        if (x > 0 && viper < 17)
            viper++;
        else if (x < 0 && viper > 3)
            viper--;
    }
    public void SetRaptors(int x)
    {
        if (x > 0 && maxraider < 33)
            maxraider++;
        else if (x < 0 && maxraider > 7)
            maxraider--;
    }




    public byte[] SettingToSend(){
        byte[] array = new byte[11];
        array[0] = 2; array[1] = 2;

        array[2] = (byte) bonusres;
        array[3] = (byte) maxdist;
        array[4] = (byte) skillbonus;
        array[5] = (byte) crisisextrastr;
        array[6] = (byte) maxraider;
        array[7] = (byte) maxheavyraider;
        array[8] = (byte) maxbattlestar;
        array[9] = (byte) viper;
        array[10] = (byte) raptor;

        return array;
    }
    public void ReciveSetting(byte[] array){


    }

}
