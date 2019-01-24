using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
    Settings settings;

    Board(Settings settings)
    {
        this.settings = settings;
    }

    List<Civil> civils = new List<Civil>();
    List<Viper> vipers = new List<Viper>();
    List<Raider> raiders = new List<Raider>();
    List<HeavyRaider> heavyRaiders = new List<HeavyRaider>();
    List<BaseStar> baseStars = new List<BaseStar>();

    //0 civil, 1 vipera, 2 raider, 3 heavyRaider, 4 baseStar, (5 galactica, 6 nuke)
    byte[,] szektorok = new byte[6, 5];

    void levesz(byte szektor, object hajo, byte index)
    {
        if (hajo is Civil)
            civils.RemoveAt(index);
        else if (hajo is Viper)
            vipers.RemoveAt(index);
        else if (hajo is Raider)
            raiders.RemoveAt(index);
        else if (hajo is HeavyRaider)
            heavyRaiders.RemoveAt(index);
        else if (hajo is BaseStar)
            baseStars.RemoveAt(index);
        else
            Console.WriteLine("No such ship type!");
    }

    void lerak(byte szektor, object hajo)
    {
        if (hajo is Civil)
            civils.Add(new Civil(szektor));
        else if (hajo is Viper)
            vipers.Add(new Viper(szektor));
        else if (hajo is Raider)
            raiders.Add(new Raider(szektor));
        else if (hajo is HeavyRaider)
            heavyRaiders.Add(new HeavyRaider(szektor));
        else if (hajo is BaseStar)
            baseStars.Add(new BaseStar(szektor));
        else
            Console.WriteLine("No such ship type!");
    }

    class Raider
    {
        byte szektor;

        public Raider(byte szektor)
        {
            this.szektor = szektor;
        }
    }

    class HeavyRaider
    {
        byte szektor;

        public HeavyRaider(byte szektor)
        {
            this.szektor = szektor;
        }
    }

    class Viper
    {
        byte szektor;

        public Viper(byte szektor)
        {
            this.szektor = szektor;
        }
    }

    class Civil
    {
        byte szektor;

        public Civil(byte szektor)
        {
            this.szektor = szektor;
        }
    }

    class BaseStar
    {
        byte szektor;

        public BaseStar(byte szektor)
        {
            this.szektor = szektor;
        }
    }
}

