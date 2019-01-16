using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public bool isserver = false;
    string address;

    [SerializeField]
    GameObject mainmenu, host, join, lobby, game;

    InputField addressfield, myname;

    [SerializeField]
    GameObject serverpf, clientpf, ipaddressfield;

    GameObject serverobj, clientobj; 
    Client client; Server server;


    #region Main Menu
    
    void RandomNameAtStart()
    {
        string[] randomname = {"Macauly", "Caldwell", "Nana",  "Sheldon", "Louise",  "Browning", "Katya",  "Ireland", "Conor", "Dotson", "Carol", "John", "Polly", "Duran",
        "Corrina",  "Ford", "Abbas", "Yoder", "Brendon",  "Warren" };
        myname = transform.GetChild(0).GetChild(3).GetComponent<InputField>();
        myname.text = randomname[new System.Random().Next()%randomname.Length];
    }


    #endregion
    #region Lobby 
    public Settings settings = new Settings();

    [SerializeField]
    Text[] counters;

    public int msgsize = 1024;

    void InitLobbySettings()
    {
        int x = lobby.transform.GetChild(0).transform.childCount;
        counters = new Text[x - 4];
        for(int i=4; i < x; i++)
            counters[i-4] = lobby.transform.GetChild(0).transform.GetChild(i).transform.GetChild(0).GetComponent<Text>();

        if (isserver == false){
            Debug.Log("isserver: " + isserver);
            for(int i=0; i<lobby.transform.childCount; i++){
                if (i < 4)
                    lobby.transform.GetChild(0).transform.GetChild(i).gameObject.SetActive(false);
                else{
                    lobby.transform.GetChild(0).transform.GetChild(i).gameObject.transform.GetChild(1).gameObject.SetActive(false);
                    lobby.transform.GetChild(0).transform.GetChild(i).gameObject.transform.GetChild(2).gameObject.SetActive(false);
                }
            }
            
        }
        SetActualSettings();
    }
    void SetActualSettings()
    {
        for (int i = 0; i < counters.Length - 2; i++)
            counters[i].text = settings.array[i].ToString();
    }

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

    public void SetSettings(byte[] tmp)
    {
        settings.ReciveSetting(tmp);
        SetActualSettings();
    }



    #region Clientlist 
    GameObject[] userrow = new GameObject[10];
    Dropdown[] dropdowns = new Dropdown[10];
    Toggle[] readycheck = new Toggle[10];

    public Client_data myclient = new Client_data();
    public List<Client_data> alluser = new List<Client_data>();

    void InitUserPart()
    {
        myclient.name = myname.text;
        for (int i = 0; i < 10; i++)
        {
            userrow[i] = lobby.transform.GetChild(1).transform.GetChild(i).gameObject;
            dropdowns[i] = userrow[i].GetComponentInChildren<Dropdown>();
            readycheck[i] = userrow[i].GetComponentInChildren<Toggle>();
            readycheck[i].isOn = false;

        }
        
    }
    public void UpdateUserRow()
    {
        for(int i=0; i<10; i++)
        {

        }
    }


    public void SetUserrow(Client_data cd)
    {


    }
    public void UpdateUser(byte i)
    {

    }
    public void AddUser(byte[] tmp)
    {
        byte[] bytes = new byte[200];
        for (int i = 0; i < 200; i++)
            bytes[i] = tmp[i + 3];
        Client_data cdtmp = new Client_data(tmp[2]);
        cdtmp.name = Encoding.ASCII.GetString(bytes);
        alluser.Add(cdtmp);
        UpdateUser(tmp[2]);
    }
    public void AddNameToUser(int id, string name)
    {
        for (int i = 0; i < alluser.Count; i++)
            if (alluser[i].id == id)
                alluser[i].name = name;

    }

    public void SelectOnchange()
    {

    }
    #endregion Clientlist
    #endregion Lobby















    void Start()
    {
        RandomNameAtStart();
        mainmenu.SetActive(true);
        join.SetActive(false);
        lobby.SetActive(false);
        game.SetActive(false);

        addressfield = ipaddressfield.GetComponent<InputField>();
        addressfield.text = "192.168.0.100";
        
    }




    public void Host(){
        serverobj = Instantiate(serverpf);
        server = serverobj.GetComponent<Server>();
        isserver = true;
        mainmenu.SetActive(false);
        InitLobbySettings();
        InitUserPart();
        lobby.SetActive(true);
    }
    public void Join(){
        clientobj = Instantiate(clientpf);
        client = clientobj.GetComponent<Client>();
        mainmenu.SetActive(false);
        join.SetActive(true);
    }
    public void Connect()
    {
        Debug.Log("Addressfield: " + addressfield.text);
        client.Connect(addressfield.text);
        isserver = false;
        join.SetActive(false);
        InitLobbySettings();
        lobby.SetActive(true);
    }
}
//