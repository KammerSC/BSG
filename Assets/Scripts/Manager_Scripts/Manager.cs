using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public bool isserver = false;
    string address;
    public int msgsize = 1024;

    

    [SerializeField]
    GameObject serverpf, clientpf;

    GameObject serverobj, clientobj; 
    Client client; Server server;


    #region Main Menu
    
    void RandomNameAtStart()
    {
        string[] randomname = {"Macauly", "Caldwell", "Nana",  "Sheldon", "Louise",  "Browning", "Katya",  "Ireland", "Conor", "Dotson", "Carol", "John", "Polly", "Duran",
        "Corrina",  "Ford", "Abbas", "Yoder", "Brendon",  "Warren" };
        myclient.name = myname.text;
    }
    public void ChangeName()
    {
        myclient.name = myname.text;
    }


    #endregion

    #region Lobby 
    public Settings settings = new Settings();
    public Client_data myclient = new Client_data();
    public List<Client_data> alluser = new List<Client_data>();

 
    public void SetButtonPlus(int number){
        switch (number){
            case 0: settings.SetBonusRes(1); break;
            case 1: settings.SetDistance(1); break;
            case 2: settings.SetSkillBonus(1); break;
            case 3: settings.SetCrisisStr(1); break;
            case 4: settings.SetMaxRaider(1); break;
            case 5: settings.SetMaxHeavyRaider(1); break;
            case 6: settings.SetMaxBattleStar(1); break;
            case 7: settings.SetViper(1); break;
            case 8: settings.SetRaptors(1); break;
            case 9: settings.SetDmgGalactica(1); break;
            case 10: settings.SetBoardingParty(1); break;
            case 11: settings.SetJumPrepRed(1); break;
            case 12: settings.SetJumpPopLoss(1); break;
            //case 14: settings. (1); break;
            //case 15: settings. (1); break;
        }
        server.SentToAllClient(settings.SettingToSend());
        SetActualSettings();
    }
    public void SetButtonMinus(int number){
        switch (number){
            case 0: settings.SetBonusRes(-1); break;
            case 1: settings.SetDistance(-1); break;
            case 2: settings.SetSkillBonus(-1); break;
            case 3: settings.SetCrisisStr(-1); break;
            case 4: settings.SetMaxRaider(-1); break;
            case 5: settings.SetMaxHeavyRaider(-1); break;
            case 6: settings.SetMaxBattleStar(-1); break;
            case 7: settings.SetViper(-1); break;
            case 8: settings.SetRaptors(-1); break;
            case 9: settings.SetDmgGalactica(-1); break;
            case 10: settings.SetBoardingParty(-1); break;
            case 11: settings.SetJumPrepRed(-1); break;
            case 12: settings.SetJumpPopLoss(-1); break;
                //case 14: settings. (1); break;
                //case 15: settings. (1); break;
        }
        server.SentToAllClient(settings.SettingToSend());
        SetActualSettings();
    }
    void SetActualSettings()
    {
        for (int i = 0; i < counters.Length - 2; i++)
            counters[i].text = settings.array[i].ToString();
    }
    public void SetSettings(byte[] tmp)
    {
        settings.ReciveSetting(tmp);
        SetActualSettings();
    }


    #region Clientlist 

    bool[] occupied = new bool[10];


    public void AcceptClientData(byte[] data)
    {
        for(int i=0; i<alluser.Count; i++)
            if(alluser[i].id == data[2])
            {
                alluser[i].SetClientByData(data);
                return;
            }
        Client_data tmp = new Client_data();
        tmp.SetClientByData(data);
        alluser.Add(tmp);
    }

    #endregion Clientlist
    #endregion Lobby

    #region UI

    /* UI's child objects */
    GameObject mainmenu, join, lobby, game;

    GameObject[] presets = new GameObject[4];
    GameObject[] plusbuttons = new GameObject[15], minusbuttons = new GameObject[15];
    GameObject[] userrow = new GameObject[10];


    void FindUIElements(){
        /* UI's child objects */
        mainmenu = transform.GetChild(0).gameObject;
        join = transform.GetChild(1).gameObject;
        lobby = transform.GetChild(2).gameObject;
        game = transform.GetChild(3).gameObject;


        /* Lobby's child objects */
        for (int i=0; i<15; i++){
            plusbuttons[i] = lobby.transform.GetChild(0).GetChild(i + 4).GetChild(1).gameObject;
            minusbuttons[i] = lobby.transform.GetChild(0).GetChild(i + 4).GetChild(2).gameObject;
        }
        for (int i = 0; i < 4; i++)
            presets[i] = lobby.transform.GetChild(0).GetChild(i).gameObject;
        for (int i = 0; i < 10; i++)
            userrow[i] = lobby.transform.GetChild(1).GetChild(i).gameObject;



            
    }

    InputField addressfield, myname;
    Text[] counters = new Text[15];
    Text[] names = new Text[10];
    Dropdown[] dropdowns = new Dropdown[10];
    Toggle[] readycheck = new Toggle[10];



    void FindUIComponents()
    {
        myname = mainmenu.GetComponentInChildren<InputField>();
        RandomNameAtStart();

        addressfield = join.transform.GetChild(1).GetComponent<InputField>();
        addressfield.text = "127.0.0.1";

        for (int i = 4; i < 19; i++)
            counters[i - 4] = lobby.transform.GetChild(0).transform.GetChild(i).transform.GetChild(0).GetComponent<Text>();
        for (int i = 4; i < 10; i++){
            names[i] = userrow[i].GetComponentInChildren<Text>();
            dropdowns[i] = userrow[i].GetComponentInChildren<Dropdown>();
            readycheck[i] = userrow[i].GetComponentInChildren<Toggle>();
        }



    }
    void SetUpForServer()
    {
        serverobj = Instantiate(serverpf);
        server = serverobj.GetComponent<Server>();
        myclient.id = 0; myclient.name = myname.text;
        alluser.Add(myclient);

    }

    void SetUpForClient()
    {
        for (int i = 0; i < 4; i++)
            presets[i].SetActive(false);
        for (int i = 0; i < 15; i++)
        {
            plusbuttons[i].SetActive(false);
            minusbuttons[i].SetActive(false);
        }
    }

    void Panelchange(int x)
    {
        switch (x)
        {
            case 1: //Main Menu
                mainmenu.SetActive(true);
                join.SetActive(false);
                lobby.SetActive(false);
                game.SetActive(false);
                break;

            case 2: //Join
                mainmenu.SetActive(false);
                join.SetActive(true);
                lobby.SetActive(false);
                game.SetActive(false);
                break;
            case 3: //Lobby
                mainmenu.SetActive(false);
                join.SetActive(false);
                lobby.SetActive(true);
                game.SetActive(false);
                break;
            case 4: //Lobby
                mainmenu.SetActive(false);
                join.SetActive(false);
                lobby.SetActive(false);
                game.SetActive(true);
                break;

        }
    }


    #endregion UI













    void Start()
    {
        FindUIElements();
        FindUIComponents();
        Panelchange(1);
    }




    public void Host(){
        
        isserver = true;
        SetUpForServer();
        Panelchange(3);
        SetUpForServer();
    }
    public void Join(){
        clientobj = Instantiate(clientpf);
        client = clientobj.GetComponent<Client>();
        Panelchange(2);
    }
    public void Connect()
    {
        Debug.Log("Addressfield: " + addressfield.text);
        client.Connect(addressfield.text);
        isserver = false;
        Panelchange(3);
        SetUpForClient();
    }
}
//