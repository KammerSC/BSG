using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client_data 
{
    public byte id, serialnum, prefchar;
    public string name;


    public Client_data()
    {

    }


    public Client_data(int id)
    {
        this.id = (byte)id;
    }

    public void SetName(string newname)
    {
        name = newname;

    }
}
