using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Game
{
    //Board board

    Manager manager;
    Player player;
    List<Player> playerlist = new List<Player>();

    Server server; Client client;
    bool isserver = false;

    public Game(Manager manager, Settings settings)
    {
        this.manager = manager;
        isserver = manager.isserver;
        if (isserver)
            server = manager.server;
        else
            client = manager.client;
        SetPlayerList();
    }
    void SetPlayerList() {

        List<SCOne> prefc = new List<SCOne>();
        List<SCOne> filled = new List<SCOne>();
        System.Random rnd = new System.Random();

        
        for(int i=0; i<manager.clientdata.count; i++)
            prefc.Add(new SCOne( manager.clientdata.ids[i], manager.clientdata.prefchar[i]));
        //lista vél.sorrend + supportok a végére



        //kötelező role-ok keresése
        bool mil = false, pol = false, pil = false;
        for(int i=0; i<prefc.Count; i++)
        {
            if(mil == false && prefc[i].role == 0)
            {
                filled.Add(prefc[i]);
                mil = true;
            }
            else if (pol == false && prefc[i].role == 1)
            {
                filled.Add(prefc[i]);
                pol = true;
            }
            else if (pil == false && prefc[i].role == 2)
            {
                filled.Add(prefc[i]);
                pil = true;
            }
        }
        for(int i=0; i<filled.Count; i++)
            for(int j=0; j<prefc.Count; j++)
                if(filled[i].id == prefc[j].id)
                {
                    prefc.Remove(prefc[j]);
                    break;
                }
        byte[] pc = new byte[10];
        for (int i = 0; i < filled.Count; i++)
            pc[filled[i].charnum]++;


        if (mil == false && prefc.Count > 0)
        {
            SCOne tmp = prefc[0];
            prefc.Remove(tmp);
            byte[] trio = new byte[3];
            int counter = 0;
            for(int i=0; i<3; i++)
                if(pc[i] == 0)
                {
                    trio[counter] = (byte)i;
                    counter++;
                }
            int choosen = rnd.Next() % counter;
            SCOne tmp2 = new SCOne(tmp.id, (byte)choosen);
            filled.Add(tmp2);
        }




        /*byte[] pc = new byte[10];
        for (int i = 0; i < filled.Count; i++)
            pc[filled[i].charnum]++;*/




        manager.Log("----------FILLED----------");
        for (int i = 0; i < filled.Count; i++)
            manager.Log(filled[i].ToString());
        manager.Log("--------PREFLEFT------------");
        for (int i = 0; i < prefc.Count; i++)
            manager.Log(prefc[i].ToString());
        manager.Log("--------------------");


    }



    public void Translate(byte[] data)
    {




    }





    class SCOne{
        public byte id, role, charnum;
        public SCOne(){}
        public SCOne(byte id, byte charnum) {
            Debug.Log(id + "  " + charnum);
            this.id = id;
            this.charnum = charnum;
            if (charnum < 3)
                role = 0;
            else if (charnum < 6)
                role = 1;
            else if (charnum < 9)
                role = 2;
            else if (charnum < 10)
                role = 3;
            else
                role = 4;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Client id: " + id + ", ");
            switch (role)
            {

                case 0: sb.Append("Role: Military Leader, "); break;
                case 1: sb.Append("Role: Political Leader, "); break;
                case 2: sb.Append("Role: Pilot, "); break;
                case 3: sb.Append("Role: Support, "); break;
                case 4: sb.Append("Role: AnyRole, "); break;
            }
            switch (charnum)
            {
                case 0: sb.Append("Name: William Adam"); break;
                case 1: sb.Append("Name: Saul Tigh"); break;
                case 2: sb.Append("Name: Karl \"Helo\" Agaton"); break;
                case 3: sb.Append("Name: Laura Rosline"); break;
                case 4: sb.Append("Name: Gaius Baltar"); break;
                case 5: sb.Append("Name: Tom Zarek"); break;
                case 6: sb.Append("Name: Lee “Apollo” Adama"); break;
                case 7: sb.Append("Name: Kara “Starbuck” Thrace"); break;
                case 8: sb.Append("Name: Sharon “Boomer” Valerii"); break;
                case 9: sb.Append("Name: Galen Tyrol"); break;
                case 10: sb.Append("Name: ANY"); break;
            }

            return sb.ToString();
        }
    }
}
