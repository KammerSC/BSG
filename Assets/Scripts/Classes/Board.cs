using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
    public Settings settings;

    public Board(Settings settings)
    {
        this.settings = settings;
    }

    public int maxVipers = 8;
    public int nextId = 0;

    public List<Civil> civils = new List<Civil>();
    public List<Viper> vipers = new List<Viper>();
    public List<Raider> raiders = new List<Raider>();
    public List<HeavyRaider> heavyRaiders = new List<HeavyRaider>();
    public List<BaseStar> baseStars = new List<BaseStar>();
    public int[] hajok = { civils, vipers, raiders, heavyRaiders, baseStars };

    public void aktivalCylon(int hajo)
    {
        switch (hajo)
        {
            case 2:
                foreach (Raider raider in raiders)
                {
                    raider.activate();
                }
            case 3:
                foreach (HeavyRaider heavyRaider in heavyRaiders)
                {
                    heavyRaider.activate();
                }
            case 4:
                foreach (BaseStar baseStar in baseStars)
                {
                    baseStar.activate();
                }
            default:
                Console.WriteLine("Nincs ilyen cylon hajó típus");
        }

    }

    civilHajokSzama()
    {
        int counter = 0;
        for (int i = 0; i < 6; i++)
        {
            counter += civilSzamSzektor(i);
        }
        return counter;
    }

    public bool raiderSzamSzektor(int szektor)
    {
        return szektorok[szektor][2];
    }
    public bool viperaSzamSzektor(int szektor)
    {
        return szektorok[szektor][1];
    }
    public bool heavyRaiderSzamSzektor(int szektor)
    {
        return szektorok[szektor][3];
    }
    public bool civilSzamSzektor(int szektor)
    {
        return szektorok[szektor][0];
    }
    public bool baseStarSzamSzektor(int szektor)
    {
        return szektorok[szektor][4];
    }

    //0 civil, 1 vipera, 2 raider, 3 heavyRaider, 4 baseStar, (5 galactica, 6 nuke)
    int[,] szektorok = new int[6, 4];

    public void tamad(int tamadoTipus, int tamadottTipus)
    {
        //unity felületről jön
        int tamadottAzonosito;
        //teszt dobas
        int dobas = 3;

        hajok[tamadottTipus].Find(x => x.id == tamadottAzonosito).tamadjak(tamadoTipus);
    }

    public class Raider
    {
        public byte position;
        public byte id;

        public Raider(int szektor)
        {
            position = szektor;
            this.id = nextId++;
        }

        public bool activated;

        public int tamadjak(int hajo, int dobas)
        {
            if (dobas > 2)
                pusztul();
        }

        public void activate()
        {
            int raiderSzam = raiderSzamSzektor(szektor);
            for (int i = 0; i < raiderSzam; i++)
            {
                if (viperaSzamSzektor(position))
                {
                    tamad(2, 1);
                }
                else if (civilSzamSzektor(position))
                {
                    raider.tamad(2, 0);
                }
                else if (civilHajokSzama())
                {
                    //mozog
                }
                else
                    tamad(2, 5);
            }
        }

        public pusztul()
        {
            raiders.Remove(raiders.Find(x => x.id == id));
        }
    }

    public class HeavyRaider
    {
        public byte position;
        public byte id;

        public HeavyRaider(int szektor)
        {
            position = szektor;
            this.id = nextId++;
        }

        public bool activated;

        public int tamadjak(int hajo, int dobas)
        {
            if (dobas > 6)
                pusztul();
        }

        public pusztul()
        {
            heavyRaiders.Remove(raiders.Find(x => x.id == id));
        }
    }

    public class Viper
    {
        public byte position;
        public byte id;

        public Viper(int szektor)
        {
            position = szektor;
            this.id = nextId++;
        }

        public byte player;
        public bool activated;

        public int tamadjak(int hajo, int dobas)
        {
            if (dobas == 8)
                pusztul();
            else if (dobas > 4)
                return 1;
            else
                return 0;
        }

        public pusztul()
        {
            vipers.Remove(raiders.Find(x => x.id == id));
            maxVipers--;
        }
        public serul()
        {
            //
        }
    }

    public class Civil
    {
        public byte position;
        public byte id;

        public Civil(int szektor)
        {
            position = szektor;
            this.id = nextId++;
        }

        public bool activated;

        public int tamadjak(int hajo, int dobas)
        {

            civils.Remove(raiders.Find(x => x.id == id));
            //TODO hatás
        }
    }

    public class BaseStar
    {
        public byte position;
        public byte id;

        public BaseStar(int szektor)
        {
            position = szektor;
            this.id = nextId++;
        }

        public bool activated;

        public int tamadjak(int hajo, int dobas)
        {
            switch (hajo)
            {
                case 1:
                    if (dobas == 8)
                        return 1;
                    else
                        return 0;
                case 5:
                    if (dobas > 4)
                        return 1;
                    else
                        return 0;
                case 6:
                    if (dobas > 6)
                    {
                        pusztul(true);
                        return 2;
                    }
                    else if (dobas > 2)
                        pusztul(false);
                    else
                        //TODO kétszer sérül
                        return 1;
            }
        }

        public pusztul(bool areaDamage)
        {
            baseStars.Remove(raiders.Find(x => x.id == id));
        }
        public serul()
        {
            //
        }
    }
}

