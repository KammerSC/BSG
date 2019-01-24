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

    public byte myid = 0;
    public string myname;
    int myindex = 0;

    

    [SerializeField]
    GameObject serverpf, clientpf;

    GameObject serverobj, clientobj; 
    public Client client; public Server server;
    Game game;


    #region Main Menu
    
    void RandomNameAtStart()
    {
        string[] randomname = {"Macauly", "Caldwell", "Nana",  "Sheldon", "Louise",  "Browning", "Katya",  "Ireland", "Conor", "Dotson", "Carol", "John", "Polly", "Duran",
        "Corrina",  "Ford", "Abbas", "Yoder", "Brendon",  "Warren" };
        namefield.text = randomname[new System.Random().Next() % randomname.Length];
    }
    public void ChangeName()
    {
        myname = namefield.text;
    }


    #endregion

    #region Lobby 
    public Settings settings = new Settings();
    public Client_data clientdata;
 
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
        server.SendToAllClient(settings.SettingToSend());
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
        server.SendToAllClient(settings.SettingToSend());
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

    public void SetUpClientrows()
    {
        for(int i=0; i<10; i++)
        {
            if (i < clientdata.count)
            {
                userrow[i].SetActive(true);
                names[i].text = clientdata.names[i];
                if (myid == clientdata.ids[i]) {
                    myindex = i;
                    dropdowns[i].interactable = true;
                    readycheck[i].interactable = true;
                }
                else {
                    dropdowns[i].interactable = false;
                    readycheck[i].interactable = false;
                }
            }
            else
                userrow[i].SetActive(false);
        }
        //clientdata.Kiirat();
    }
    public void UpdateUserPrefchar(byte id)
    {
        Debug.Log("<CLIENT> Prefered character dropdow update started.");
        for (int i = 0; i < clientdata.count; i++)
            if (clientdata.ids[i] == id)
            {
                dropdowns[i].value = clientdata.prefchar[i];
                Debug.Log("<CLIENT> Prefered character dropdow update done.");
                return;
            }
    }
    public void UpdateUserReady(byte id)
    {
        for (int i = 0; i < clientdata.count; i++)
            if (clientdata.ids[i] == id)
            {
                if(clientdata.ready[i] == 1)
                    readycheck[i].isOn = true;
                else
                    readycheck[i].isOn = false;
                return;
            }
    }
    public void OnPrefCharChange()
    {
        clientdata.SetPrefChar(myid, (byte)dropdowns[myindex].value);
        byte[] tmp = new byte [msgsize];
        tmp[0] = 1; tmp[1] = 4; tmp[2] = myid; tmp[3] = (byte)dropdowns[myindex].value;
        if (isserver)
            server.SendToAllClient(tmp);
        else
            client.SendToServer(tmp);
        SetPrefCharCard(dropdowns[myindex].value);
    }
    public void OnReadyChange()
    {
        if(readycheck[myindex].isOn)
            clientdata.SetReady(myid, 1);
        else
            clientdata.SetReady(myid, 0);
        byte[] tmp = new byte[msgsize];
        tmp[0] = 1; tmp[1] = 5; tmp[2] = myid; tmp[4] = clientdata.ready[myindex];
        if (isserver)
            server.SendToAllClient(tmp);
        else
            client.SendToServer(tmp);
    }



    #endregion Clientlist

    
    public void LaunchGame()
    {
        if (isserver == false)
            return;
        game = new Game(this, settings);
    }



    #endregion Lobby

    #region UI

    /* UI's child objects */
    GameObject mainmenu, join, lobby, gamebutton;

    GameObject[] presets = new GameObject[4];
    GameObject[] plusbuttons = new GameObject[15], minusbuttons = new GameObject[15];
    GameObject[] userrow = new GameObject[10];
    public GameObject selectedchar;

    void FindUIElements(){
        /* UI's child objects */
        mainmenu = transform.GetChild(0).gameObject;
        join = transform.GetChild(1).gameObject;
        lobby = transform.GetChild(2).gameObject;
        gamebutton = transform.GetChild(3).gameObject;

        /* Lobby's child objects */
        for (int i=0; i<15; i++){
            plusbuttons[i] = lobby.transform.GetChild(0).GetChild(i + 4).GetChild(1).gameObject;
            minusbuttons[i] = lobby.transform.GetChild(0).GetChild(i + 4).GetChild(2).gameObject;
        }
        for (int i = 0; i < 4; i++)
            presets[i] = lobby.transform.GetChild(0).GetChild(i).gameObject;
        for (int i = 0; i < 10; i++)
            userrow[i] = lobby.transform.GetChild(1).GetChild(i).gameObject;

        selectedchar = lobby.transform.GetChild(2).gameObject;

    }

    InputField addressfield, namefield;
    Text[] counters = new Text[15];
    Text[] names = new Text[10];
    Dropdown[] dropdowns = new Dropdown[10];
    Toggle[] readycheck = new Toggle[10];
    Image prefcharcard;
    [SerializeField]
    Sprite[] charcards;

    void FindUIComponentsAndSet()
    {
        namefield = mainmenu.GetComponentInChildren<InputField>();
        RandomNameAtStart();

        addressfield = join.transform.GetChild(1).GetComponent<InputField>();
        addressfield.text = "127.0.0.1";

        for (int i = 4; i < 19; i++)
            counters[i - 4] = lobby.transform.GetChild(0).transform.GetChild(i).transform.GetChild(0).GetComponent<Text>();
        for (int i = 0; i < 10; i++){
            names[i] = userrow[i].GetComponentInChildren<Text>();
            dropdowns[i] = userrow[i].GetComponentInChildren<Dropdown>();
            dropdowns[i].interactable = false;
            readycheck[i] = userrow[i].GetComponentInChildren<Toggle>();
            readycheck[i].interactable = false;
        }
        selectedchar.SetActive(false);
        prefcharcard = selectedchar.GetComponent<Image>();

        RandomNameAtStart();
    }
    void SetUpForServer()
    {
        serverobj = Instantiate(serverpf);
        server = serverobj.GetComponent<Server>();

        clientdata = new Client_data(myid, myname);

        SetUpClientrows();
        SetActualSettings();
        selectedchar.SetActive(true);
        SetPrefCharCard(dropdowns[myindex].value);
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
        SetPrefCharCard(dropdowns[myindex].value);
    }

    void Panelchange(int x)
    {
        switch (x)
        {
            case 1: //Main Menu
                mainmenu.SetActive(true);
                join.SetActive(false);
                lobby.SetActive(false);
                gamebutton.SetActive(false);
                break;

            case 2: //Join
                mainmenu.SetActive(false);
                join.SetActive(true);
                lobby.SetActive(false);
                gamebutton.SetActive(false);
                break;
            case 3: //Lobby
                mainmenu.SetActive(false);
                join.SetActive(false);
                lobby.SetActive(true);
                gamebutton.SetActive(false);
                break;
            case 4: //Lobby
                mainmenu.SetActive(false);
                join.SetActive(false);
                lobby.SetActive(false);
                gamebutton.SetActive(true);
                break;

        }
    }
    void SetPrefCharCard(int index)
    {
        prefcharcard.sprite = charcards[index];
    }

    #endregion UI

    #region Log
    [SerializeField]
    GameObject scrdwn, scrup, scrtxt;
    Text logobj;
    int logoffset = 0;
    List<string> logs = new List<string>();

    void InitLogStuff()
    {
        logobj = scrtxt.GetComponent<Text>();
    }
    public void Log(string msg)
    {
        if (logs.Count == 100)
            logs.RemoveAt(0);
        StringBuilder sb = new StringBuilder();
        System.DateTime time = System.DateTime.Now;
        sb.Append(time.ToString("HH:mm:ss")).Append(" - ").Append(msg);
        logs.Add(sb.ToString());
        Debug.Log(sb.ToString());
        if (logs.Count < 6)
            logoffset = 0;
        else
            logoffset = 4;
        ShowLastFive();
    }
    void ShowLastFive()
    {
        if (logs.Count == 0)
            return;
        StringBuilder sb = new StringBuilder();

        for (int i = logs.Count - 1 - logoffset, j = 0; i < logs.Count && j < 5; i++, j++)
            sb.AppendLine(logs[i]);

        logobj.text = sb.ToString();

    }
    public void LogPlus()
    {
        if (logoffset < logs.Count-1)
            logoffset++;
        ShowLastFive();
    }
    public void LogMinus()
    {
        if (logoffset > 4)
            logoffset--;
        ShowLastFive();
    }

    #endregion Log


    void Start()
    {
        InitLogStuff();
        FindUIElements();
        FindUIComponentsAndSet();

        Panelchange(1);
    }

    public void Host(){
        isserver = true;
        SetUpForServer();
        Panelchange(3);
    }
    public void Join(){
        clientobj = Instantiate(clientpf);
        client = clientobj.GetComponent<Client>();
        Panelchange(2);
    }
    public void Connect()
    {
        //Debug.Log("Addressfield: " + addressfield.text);
        client.Connect(addressfield.text);
        isserver = false;
        Panelchange(3);
        SetUpForClient();
    }
}