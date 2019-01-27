using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Game
{
    //Board board

    Manager manager;
    Settings settings;
    Player myplayer;
    List<Player> playerlist = new List<Player>();

    Server server; Client client;
    bool isserver = false;
    System.Random rnd = new System.Random();

    #region Cards
    List<LoyaltyCard> loys = new List<LoyaltyCard>();
    List<SkillCard> polsc = new List<SkillCard>(), leadsc = new List<SkillCard>(), 
        tecsc = new List<SkillCard>(), pilsc = new List<SkillCard>(), engsc = new List<SkillCard>();
    #endregion Cards

    #region Data
    byte admiralid = 0, presidentid = 0;


    #endregion Data


    public Game(Manager manager, Settings settings)
    {
        this.manager = manager;
        isserver = manager.isserver;
        if (isserver)
            server = manager.server;
        else
            client = manager.client;
        this.settings = settings;
        SetupPlayerList();
    }
    void SetupPlayerList() {
        List<SCOne> prefc = new List<SCOne>(), filled = new List<SCOne>();
        
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
        InitPlayerList(filled);
        /*manager.Log("----------FILLED3----------");
        for (int i = 0; i < filled.Count; i++)
            manager.Log(filled[i].ToString());
        manager.Log("--------PREFLEFT3------------");
        for (int i = 0; i < prefc.Count; i++)
            manager.Log(prefc[i].ToString());
        manager.Log("--------------------");*/
    }
    void InitPlayerList(List<SCOne> sclist)
    {
        for(int i=0; i<sclist.Count; i++)
        {
            Player tmp = new Player(sclist[i].id, sclist[i].charnum);
            playerlist.Add(tmp);
            if (tmp.id == manager.myid)
                myplayer = tmp;
        }
        InitLoyals(playerlist);
    }
    void InitLoyals(List<Player> pl)
    {
        int nc = 1, c = 1;
        bool sym = false;
        switch (pl.Count)
        {
            case 1:  nc = 1; c = 1; sym = false;  break;
            case 2:  nc = 3; c = 1; sym = false; break;
            case 3:  nc = 5; c = 1; sym = false; break;
            case 4:  nc = 6; c = 1; sym = true; break;
            case 5:  nc = 8; c = 2; sym = false; break;
            case 6:  nc = 9; c = 2; sym = true; break;
            case 7:  nc = 11; c = 3; sym = false; break;
            case 8:  nc = 12; c = 3; sym = true; break;
            case 9:  nc = 14; c = 4; sym = false; break;
            case 10: nc = 15; c = 4; sym = true; break;
        }
        List<LoyaltyCard> cylon = new List<LoyaltyCard>();
        for(int i=1; i<5; i++)
            cylon.Add(new LoyaltyCard((byte)i));
        ShuffleLoyalties(cylon);
        for (int i = 0; i < nc; i++)
            loys.Add(new LoyaltyCard(0));

        for (int i = 0; i < c; i++){
            LoyaltyCard tmp = cylon[0];
            cylon.Remove(tmp);
            loys.Add(tmp);
        }
        for (int i = 0; i < pl.Count; i++)
            if(pl[i].chartype == 4 || pl[i].chartype ==8)
                loys.Add(new LoyaltyCard(0));
        ShuffleLoyalties(loys);

        for (int i = 0; i < pl.Count; i++){
            pl[i].loyalties.Add(loys[0]);
            loys.Remove(loys[0]);
            if(pl[i].chartype == 4){
                pl[i].loyalties.Add(loys[0]);
                loys.Remove(loys[0]);
            }
        }

        if (sym)
            loys.Add(new LoyaltyCard(5));
        ShuffleLoyalties(loys);
        HeriteAdmiralRank(playerlist);
        HeritePresidentRank(playerlist);
        KiirPlayerList();
    }
    void InitSkillcards()
    {

    }

    public void Translate(byte[] data)
    {




    }
    void HeriteAdmiralRank(List<Player> pl)
    {
        byte[] admiral = {0,1,2,6,7,8,9,5,4,3};
        bool[] candidates = new bool[10];
        for(int i=0; i<pl.Count; i++)
            if (pl[i].brig == false && pl[i].revealed == false && pl[i].chartype < 10)
                candidates[pl[i].chartype] = true;
        for(int i=0; i<10; i++)
            if(candidates[admiral[i]] == true)
            {
                for (int j = 0; j < pl.Count; j++)
                    if(admiral[i] == pl[j].chartype)
                    {
                        manager.Log("Found admiral id: " + pl[j].id);
                        admiralid = pl[j].id;
                        break;
                    }
                break;
            }
        for (int j = 0; j < pl.Count; j++){
            if (admiralid == pl[j].id)
                pl[j].admiral = true;
            else
                pl[j].admiral = false;
        }
    }
    void HeritePresidentRank(List<Player> pl)
    {
        byte[] president = { 3, 4, 5, 6, 0, 2, 9, 8, 1, 7 };
        bool[] candidates = new bool[10];
        for (int i = 0; i < pl.Count; i++)
            if (pl[i].revealed == false && pl[i].chartype < 10)
                candidates[pl[i].chartype] = true;
        for (int i = 0; i < 10; i++)
            if (candidates[president[i]] == true)
            {
                for (int j = 0; j < pl.Count; j++)
                    if (president[i] == pl[j].chartype)
                    {
                        manager.Log("Found president id: " + pl[j].id);
                        presidentid = pl[j].id;
                        break;
                    }
                break;
            }
        for (int j = 0; j < pl.Count; j++)
        {
            if (presidentid == pl[j].id)
                pl[j].president = true;
            else
                pl[j].president = false;
        }
    }
    void ShuffleLoyalties(List<LoyaltyCard> l)
    {
        for(int i=0; i<l.Count; i++)
        {
            int rand = rnd.Next() % l.Count;
            LoyaltyCard tmp = l[i];
            l[i] = l[rand];
            l[rand] = tmp;
        }
    }


    void KiirPlayerList()
    {
        for (int i = 0; i < playerlist.Count; i++)
            manager.Log(playerlist[i].ToString());
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
