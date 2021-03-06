﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings 
{
    public sbyte[] array = new sbyte[15];

    public Settings()
    {
        array[0] = 0;   //Extra resource at start
        array[1] = 8;   //Distance required for the last jump
        array[2] = 0;   //Extra strength to skillcard's power
        array[3] = 0;   //Extra strength to crisiscard's difficulty
        array[4] = 16;  //Maximum number of Raiders on the board
        array[5] = 4;   //Maximum number of Heavyraiders on the board
        array[6] = 2;   //Maximum number of Battlestars on the board
        array[7] = 8;   //Viper reserve at the start
        array[8] = 4;   //Raptor reserve at the start
        array[9] = 6;   //Number of damaged locations required to destroy the Galactica
        array[10] = 5;  //Number of Centurions required to capture the Galactica
        array[11] = 2;  //Number of red space in the Jump Preparation Track
        array[12] = 0;  //Extra Population lost on premature 
        array[13] = 0;  //Placeholder
        array[14] = 0;  //Placeholder
    }

    //   array[]

    public void SetBonusRes(int x){
        if (x > 0 && array[0] < 4)
            array[0]++;
        else if (x < 0 && array[0] > -3)
            array[0]--;
    }
    public void SetDistance(int x){
        if (x > 0 && array[1] < 12)
            array[1]++;
        else if (x < 0 && array[1] > 6)
            array[1]--;
    }
    public void SetSkillBonus(int x){
        if (x > 0 && array[2] < 2)
            array[2]++;
        else if (x < 0 && array[2] > -1)
            array[2]--;
    }
    public void SetCrisisStr(int x){
        if (x > 0 && array[3] < 4)
            array[3]++;
        else if (x < 0 && array[3] > -4)
            array[3]--;
    }
    public void SetMaxRaider(int x){
        if (x > 0 && array[4] < 32)
            array[4]++;
        else if (x < 0 && array[4] > 8)
            array[4]--;
    }
    public void SetMaxHeavyRaider(int x){
        if (x > 0 && array[5] < 8)
            array[5]++;
        else if (x < 0 && array[5] > 2)
            array[5]--;
    }
    public void SetMaxBattleStar(int x){
        if (x > 0 && array[6] < 6)
            array[6]++;
        else if (x < 0 && array[6] > 1)
            array[6]--;
    }
    public void SetViper(int x){
        if (x > 0 && array[7] < 16)
            array[7]++;
        else if (x < 0 && array[7] > 4)
            array[7]--;
    }
    public void SetRaptors(int x){
        if (x > 0 && array[8] < 8)
            array[8]++;
        else if (x < 0 && array[8] > 2)
            array[8]--;
    }
    public void SetDmgGalactica(int x)
    {
        if (x > 0 && array[9] < 8)
            array[9]++;
        else if (x < 0 && array[9] > 4)
            array[9]--;
    }
    public void SetBoardingParty(int x)
    {
        if (x > 0 && array[10] < 7)
            array[10]++;
        else if (x < 0 && array[10] > 3)
            array[10]--;
    }
    public void SetJumPrepRed(int x)
    {
        if (x > 0 && array[11] < 3)
            array[11]++;
        else if (x < 0 && array[11] > 1)
            array[11]--;
    }
    public void SetJumpPopLoss(int x)
    {
        if (x > 0 && array[12] < 2)
            array[12]++;
        else if (x < 0 && array[12] > -1)
            array[12]--;
    }

    public void SettingsToStandard()
    {
        array[0] = 0;   array[1] = 8;   array[2] = 0;   array[3] = 0;   array[4] = 16;
        array[5] = 4;   array[6] = 2;   array[7] = 8;   array[8] = 4;   array[9] = 6;
        array[10] = 5;  array[11] = 2;  array[12] = 0;  array[13] = 0;  array[14] = 0;
    }





    public byte[] SettingToSend(){
        byte[] tmp = new byte[array.Length+2];
        tmp[0] = 1; tmp[1] = 2;
        for (int i = 0; i < array.Length; i++)
            tmp[i + 2] = (byte) array[i];
        return tmp;
    }
    public void ReciveSetting(byte[] tmp){
        for (int i = 0; i < array.Length; i++)
            array[i] = (sbyte) tmp[i+2];

    }

}
