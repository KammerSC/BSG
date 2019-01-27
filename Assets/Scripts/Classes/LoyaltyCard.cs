using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoyaltyCard
{
    public byte type;
    public LoyaltyCard(byte type)
    {
        this.type = type;
    }
    /*
     type = 0 -- Not Cylon
     type = 1 -- Cylon1 - 
     type = 2 -- Cylon2 - 
     type = 3 -- Cylon3 - 
     type = 4 -- Cylon4 - 
     type = 5 -- Sympatizer
     */

    public override string ToString()
    {
        string str;
        if (type == 0)
            str = "Not Cylon";
        else if(type <5)
            str = "Cylon";
        else
            str = "Sympatizer";
        return str;
    }

}
