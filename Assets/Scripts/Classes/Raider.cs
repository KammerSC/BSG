using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raider
{
    public byte position;

    public int tamad(int hajo, int dobas)
    {
        switch (hajo)
        {
            //civil
            case 0:
                {
                    return 2;
                }
            //viper
            case 1:
                {
                    if (dobas == 8)
                        return 2;
                    else if (dobas > 4)
                        return 1;
                    else
                        return 0;
                }
            //galactica
            default:
                {

                }
        }
    }


    void Update()
    {

    }
}