using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{

    Raider[] raiders = new Raider[16];


    public void aktivalCylon(int hajo)
    {
        switch (hajo)
        {
            case 2:
                {
                    for (int i = 0; i < szektorok.length; i++)
                    {
                        if (raiderSzamSzektor(i))
                        {
                            aktivalRaider(i);
                        }
                    }
                }
            default:
                Console.WriteLine("Nincs ilyen cylon hajó típus");
        }

    }

    public void aktivalRaider(int szektor)
    {
        int dobas = 3; //teszt dobas
        int raiderSzam = raiderSzamSzektor(szektor);
        for (int i = 0; i < raiderSzam; i++)
        {
            if (viperaSzamSzektor(szektor))
            {
                raider.tamad(1, dobas);
            }
            else if (civilSzamSzektor(szektor))
            {
                raider.tamad(0, dobas);
            }
            else if (emberiHajoSzamGlobal())
            {
                //mozog
            }
            else
                raider.tamad(5, dobas);
        }
    }

    public bool raiderSzamSzektor(int szektor)
    {
        return szektorok[szektor][2];
    }

    public bool viperaSzamSzektor(int szektor)
    {
        return szektorok[szektor][1];
    }

    //0 civil, 1 vipera, 2 raider, 3 heavyRaider, 4 baseStar, (5 galactica)
    int[,] szektorok = new int[6, 4];
}

