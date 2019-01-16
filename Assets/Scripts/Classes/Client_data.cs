using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

public class Client_data 
{
    public byte id, serialnum, prefchar = 0;
    public string name;
    public bool ready = false;


    public Client_data()
    {

    }


    public Client_data(int id)
    {
        this.id = (byte)id;
    }


    public byte[] ClientDataToSend()
    {
        byte[] data = new byte[1024];
        data[0] = 1; data[1] = 3; data[2] = id; data[3] = serialnum; data[4] = prefchar;
        if (ready)
            data[5] = 1;
        else
            data[5] = 0;

        byte[] nb = Encoding.ASCII.GetBytes(name);
        for (int i = 10, j=0; i < 205 && j<nb.Length; i++, j++)
            data[i] = nb[j];


        return data;
    }
}
