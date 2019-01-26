using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
    Settings settings;
    Manager manager;

    Board(Manager manager, Settings settings)
    {
        this.settings = settings;
        this.manager = manager;
    }

    //Ship types should be referred in methods by numbers as follows
    //0 civil, 1 vipera, 2 raider, 3 heavyRaider, 4 baseStar
    List<Civil> civils = new List<Civil>();
    List<Viper> vipers = new List<Viper>();
    List<Raider> raiders = new List<Raider>();
    List<HeavyRaider> heavyRaiders = new List<HeavyRaider>();
    List<BaseStar> baseStars = new List<BaseStar>();

    byte[,] szektorok = new byte[6, 5];

    void leveszRaider(byte szektor, byte type)
    {
        if (type == 2)
            raiders.Remove(raiders.Find(x => x.szektor == szektor));
        else if (type == 3)
            heavyRaiders.Remove(heavyRaiders.Find(x => x.szektor == szektor));
        else
            manager.Log("leveszRaider - No such ship type!");
    }

    void levesz(object hajo)
    {
        if (hajo is Civil)
            civils.Remove((Civil)hajo);
        else if (hajo is Viper)
            vipers.Remove((Viper)hajo);
        else if (hajo is BaseStar)
            baseStars.Remove((BaseStar)hajo);
        else
            manager.Log("levesz - No such ship type!");
    }

    void lerak(byte szektor, byte type)
    {
        if (type == 0)
            civils.Add(new Civil(szektor));
        else if (type == 1)
            vipers.Add(new Viper(szektor));
        else if (type == 2)
            raiders.Add(new Raider(szektor));
        else if (type == 3)
            heavyRaiders.Add(new HeavyRaider(szektor));
        else if (type == 4)
            baseStars.Add(new BaseStar(szektor));
        else
            manager.Log("lerak - No such ship type!");
    }

    class Raider
    {
        public byte szektor;

        public Raider(byte szektor)
        {
            this.szektor = szektor;
        }
    }

    class HeavyRaider
    {
        public byte szektor;

        public HeavyRaider(byte szektor)
        {
            this.szektor = szektor;
        }
    }

    class Viper
    {
        public byte szektor;

        public Viper(byte szektor)
        {
            this.szektor = szektor;
        }
    }

    class Civil
    {
        public byte szektor;

        public Civil(byte szektor)
        {
            this.szektor = szektor;
        }
    }

    class BaseStar
    {
        public byte szektor;

        public BaseStar(byte szektor)
        {
            this.szektor = szektor;
        }
    }
}