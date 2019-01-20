using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

public class Client_data 
{
    public byte count = 1;
    public byte[] ids, prefchar, ready;
    public string[] names;

    public Client_data(byte myid, string myname)
    {
        ids = new byte[count];
        prefchar = new byte[count];
        ready = new byte[count];
        names = new string[count];
        ids[0] = myid;
        names[0] = myname;
        //Kiirat();
    }
    public void Add(byte id){
        count++;
        byte[] tids = new byte[count], tprefchar = new byte[count], tready = new byte[count];
        string[] tnames = new string[count]; 
        for (int i = 0; i < count - 1; i++){
            tids[i] = ids[i];   tprefchar[i] = prefchar[i];
            tready[i] = ready[i];   tnames[i] = names[i]; 
        }
        ids = tids; prefchar = tprefchar;
        ready = tready; names = tnames;
        ids[count - 1] = id;
        SortByID();
        //Kiirat();
    } 
    public void Remove(byte id)
    {
        for(int i=0; i<count; i++)
        {
            if(ids[i] == id)
            {
                count--;
                for (int j = i; j < count; j++)
                {
                    ids[j] = ids[j + 1];prefchar[j] = prefchar[j + 1];
                    ready[j] = ready[j + 1];names[j] = names[j + 1];
                }
                byte[] tids = new byte[count], tprefchar = new byte[count], tready = new byte[count];
                string[] tnames = new string[count];
                for (int j = 0; j < count; j++)
                {
                    tids[j] = ids[j]; tprefchar[j] = prefchar[j];
                    tready[j] = ready[j]; tnames[j] = names[j];
                }
                ids = tids; prefchar = tprefchar;
                ready = tready; names = tnames;
                SortByID();
                return;
            }
        }

    }

    void SetName(byte id, string name)
    {
        for (int i = 0; i < count; i++)
            if (ids[i] == id)
            {
                names[id] = name;
                return;
            }
    }
    public void SetPrefChar(byte id, byte cn)
    {
        for (int i = 0; i < count; i++)
            if (ids[i] == id)
            {
                prefchar[id] = cn;
                return;
            }
    }
    public void SetReady(byte id, byte rd)
    {
        for (int i = 0; i < count; i++)
            if (ids[i] == id)
            {
                ready[id] = rd;
                return;
            }
    }

    public void Kiirat()
    {
        Debug.Log("--<< Client Data >>-- ");
        Debug.Log("Entries: "+ count);
        for(int i=0; i<count; i++)
            Debug.Log("ID: " + ids[i] + " Pref.Char.: " + prefchar[i] + " Ready: " + ready[i] + " Name: " + names[i]);
        Debug.Log("--<< Client Data >>-- ");
    }


    public byte[] ClientDataToSend(byte id)
    {
        byte[] data = new byte[1024];
        bool noid = true;
        int index = 0;
        for (int i = 0; i < count; i++)
            if (ids[i] == id)
            {
                index = i;
                noid = false;
                break;
            }
        if (noid) {
            data[0] = data[1] = 0;
            return data;
        }
        data[0] = 1; data[1] = 3;
        data[2] = ids[index]; data[3] = prefchar[index];
        if(ready[index] == 1)
            data[4] = 1;
        else
            data[4] = 0;
        int l = Encoding.ASCII.GetByteCount(names[index]);
        if (l > 200)
            data[5] = 200;
        else
            data[5] = (byte) l;

        byte[] tmp = Encoding.ASCII.GetBytes(names[index]);
        for (int i = 10, j = 0; i < 210 && j < data[5]; i++, j++)
            data[i] = tmp[j];

        return data;
    }
    public void AcceptData(byte[] data)
    {
        if (data[0] != 1)
        {
            Debug.Log("<CLIENT> Bad Data recived with code: data[0] = " + data[0] + " data[1] = " + data[1]);
            return;
        }
        switch (data[1])
        {
            case 3: //Add to client data
                #region 1:3
                //Debug.Log("<CLIENT> Recived client data with [0]: " + data[0] + " [1]: " + data[1] + " [2]: " + data[2] + " [5]: " + data[5]);
                for (int i = 0; i < count; i++)
                    if (ids[i] == data[2]){
                        Debug.Log("<CLIENT> Existing client record recived.");
                        return;
                    }
                
                Add(data[2]);
                byte[] tmp = new byte[data[5]];
                for (int i = 10, j = 0; i < 210 && j < data[5]; i++, j++)
                    tmp[j] = data[i];
                SetName(data[2], Encoding.ASCII.GetString(tmp));
                SetPrefChar(data[2], data[3]);
                SetReady(data[2], data[4]);
                Debug.Log("<CLIENT> Non-existing client record recived, added.");
                //Kiirat();
                #endregion 1:3
                break;
            case 4://Set prefered character
                #region 1:4
                Debug.Log("<CLIENT> Prefered character update recived.");
                SetPrefChar(data[2], data[3]);
                #endregion 1:4
                break;
            case 5://Set readiness
                #region 1:5
                Debug.Log("<CLIENT> Cient readiness update recived.");
                SetReady(data[2], data[4]);
                #endregion 1:5
                break;
            case 10://Delete a client by ID
                #region 1:5
                Debug.Log("<CLIENT> Client record removed.");
                Remove(data[2]);
                #endregion 1:5
                break;

        }
    }



    void SortByID(){
        for(int i = 0; i < count-1; i++ ){
            if(ids[i] > ids[i + 1])
            {
                byte tmp = ids[i];  ids[i] = ids[i + 1];    ids[i + 1] = tmp;
                tmp = prefchar[i];  prefchar[i] = prefchar[i + 1];  prefchar[i + 1] = tmp;
                tmp = ready[i]; ready[i] = ready[i + 1]; ready[i + 1] = tmp;
                string tname = names[i];    names[i] = names[i + 1];    names[i + 1] = tname;
            }
        }
    }
}
