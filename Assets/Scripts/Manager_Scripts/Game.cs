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
        List<SCOne> prefc = new List<SCOne>(), filled = new List<SCOne>();
        System.Random rnd = new System.Random();
        // ID - preferált karakter összerendelés aka ki kit szeretne alakítani
        for(int i=0; i<manager.clientdata.count; i++)
            prefc.Add(new SCOne( manager.clientdata.ids[i], manager.clientdata.prefchar[i]));
        /* Lista randomizálása és egy support karaktert választó player a végére kerül. */
        for (int i = 0; i < prefc.Count; i++)
        {
            int x = rnd.Next() % prefc.Count;
            SCOne tmp = prefc[i];
            prefc[i] = prefc[x];
            prefc[x] = tmp;
        }
        // Az első support karakter a lista végére kerül 
        for (int i = 0; i < prefc.Count; i++){
            if(prefc[i].charnum == 9){
                SCOne tmp = prefc[i];
                prefc.Remove(tmp);
                prefc.Add(tmp);
                break;
            }
        }

        /*Military L., Political L., Pilot szerepek keresése és betöltése.*/
        bool[] roles = new bool[3];
        //ha van a listában megfelelő karakter akkor az itt kiválasztásra kerül
        for(int i=0; i<prefc.Count; i++)
        {
            if(roles[0] == false && prefc[i].role == 0)
            {
                filled.Add(prefc[i]);
                prefc.Remove(prefc[i]);
                i--;
                roles[0] = true;
            }
            else if (roles[1] == false && prefc[i].role == 1)
            {
                filled.Add(prefc[i]);
                prefc.Remove(prefc[i]);
                i--;
                roles[1] = true;
            }
            else if (roles[2] == false && prefc[i].role == 2)
            {
                filled.Add(prefc[i]);
                prefc.Remove(prefc[i]);
                i--;
                roles[2] = true;
            }
        }

        //A betöltetlen kötelező role-okra történő kiválasztás a role megfelelő karakterei közül véletlenszerűen
        for (int i=0; i<3; i++)
            if (roles[i] == false && prefc.Count > 0){
                SCOne tmp = prefc[0];
                prefc.Remove(tmp);
                SCOne tmp2 = new SCOne(tmp.id, (byte)((rnd.Next() % 3)+i*3));
                filled.Add(tmp2);
            }

        //A tömb a szabad karakterek tárolására szolgál
        bool[] or = new bool[10]; 

        /*Szabad és foglalt karakterek jegyzése.*/
        byte[] pc = new byte[10];
        for (int i = 0; i < filled.Count; i++)
            or[filled[i].charnum] = true;

        /*Azon preferált karakterek kiosztása amelyek szabadok*/
        for (int i = 0; i < prefc.Count; i++)
        {
            if(or[prefc[i].charnum] == false)
            {
                or[prefc[i].charnum] = true;
                SCOne tmp = prefc[i];
                filled.Add(tmp);
                prefc.Remove(tmp);
                i--;
            }
        }

        /*Maradék játékos - maradék karakter kiosztás.*/
        //Szabad karakterek megtalálása és a karaktert azonosító száma elrakása egy tömbbe, a "c" változó számon tartja
        //a szabad karakterek számát
        int[] leftover = new int[10]; int c = 0;
        for(int i=0; i<10; i++)
        {
            if(or[i] == false)
            {
                leftover[c] = i;
                c++;
            }
        }
        
        /*A karakter nélküli játékosok véletlenszerűen kapnak egy még szabad karaktert*/
        //A szabad karaktereket tartalmazó tömb randomizálása
        for (int i = 0; i < c; i++)
        {
            int x = rnd.Next() % c;
            byte tmp = (byte) leftover[i];
            leftover[i] = leftover[x];
            leftover[x] = tmp;
        }
        //A szabad karakterek játékoshoz való rendelése, ha több a játékos mint a karakter akkor
        //a 10 számot kapja meg ami későbbiekben az observert fogja jelenteni
        for (int i = 0; i < prefc.Count; i++)
        {
            if (i < c)
                filled.Add(new SCOne(prefc[i].id, (byte)leftover[i]));
            else
                filled.Add(new SCOne(prefc[i].id, 10));
        }
        prefc.Clear();

        /*manager.Log("----------FILLED3----------");
        for (int i = 0; i < filled.Count; i++)
            manager.Log(filled[i].ToString());
        manager.Log("--------PREFLEFT3------------");
        for (int i = 0; i < prefc.Count; i++)
            manager.Log(prefc[i].ToString());
        manager.Log("--------------------");*/
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
