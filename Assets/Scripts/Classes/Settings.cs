using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings 
{
    public sbyte bonusres = 0, distance = 8, skillbonus = 0, crisisextrastr = 0;
    public sbyte maxraider = 16, maxheavyraider = 4, maxbattlestar = 2, viper = 8, raptor = 4;
    public sbyte dmggalactica = 6, boardingparty = 5, jumpprepred = 2, jumppoploss = 0;


    public void SetBonusRes(int x){
        if (x > 0 && bonusres < 4)
            bonusres++;
        else if (x < 0 && bonusres > -3)
            bonusres--;
    }
    public void SetDistance(int x){
        if (x > 0 && distance < 13)
            distance++;
        else if (x < 0 && distance > 5)
            distance--;
    }
    public void SetSkillBonus(int x){
        if (x > 0 && skillbonus < 3)
            skillbonus++;
        else if (x < 0 && distance > -2)
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
    public void SetRaptors(int x){
        if (x > 0 && maxraider < 33)
            maxraider++;
        else if (x < 0 && maxraider > 7)
            maxraider--;
    }
    public void SetDmgGalactica(int x)
    {
        if (x > 0 && dmggalactica < 9)
            dmggalactica++;
        else if (x < 0 && dmggalactica > 3)
            dmggalactica--;
    }
    public void SetBoardingParty(int x)
    {
        if (x > 0 && boardingparty < 7)
            boardingparty++;
        else if (x < 0 && boardingparty > 3)
            boardingparty--;
    }
    public void SetJumPrepRed(int x)
    {
        if (x > 0 && jumpprepred < 4)
            jumpprepred++;
        else if (x < 0 && jumpprepred > 30)
            jumpprepred--;
    }
    public void SetJumpPopLoss(int x)
    {
        if (x > 0 && jumppoploss < 2)
            jumppoploss++;
        else if (x < 0 && jumppoploss > -1)
            jumppoploss--;
    }

    public void SettingsToStandard()
    {
        bonusres = 0; distance = 8; skillbonus = 0; crisisextrastr = 0;
        maxraider = 16; maxheavyraider = 4; maxbattlestar = 2; viper = 8; raptor = 4;
        dmggalactica = 6; boardingparty = 5; jumpprepred = 2; jumppoploss = 0;
    }





    public byte[] SettingToSend(){
        byte[] array = new byte[11];
        array[0] = 2; array[1] = 2;

        array[2] = (byte) bonusres;
        array[3] = (byte) distance;
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
