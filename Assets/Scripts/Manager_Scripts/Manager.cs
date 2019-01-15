using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public bool isserver = false;
    string address;

    [SerializeField]
    GameObject mainmenu, host, join, lobby, game;

    InputField addressfield;

    [SerializeField]
    GameObject serverpf, clientpf, ipaddressfield;

    GameObject serverobj, clientobj; 
    Client client; Server server;

    //Lobby settings
    #region Lobby 
    Settings settings = new Settings();

    [SerializeField]
    Text[] counters;

    void InitLobbySettings()
    {
        counters = new Text[lobby.transform.childCount - 4];
        for(int i=4; i< lobby.transform.childCount; i++)
            counters[i-4] = lobby.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>();

        if (isserver == false){
            Debug.Log("isserver: " + isserver);
            for(int i=0; i<lobby.transform.childCount; i++){
                if (i < 4)
                    lobby.transform.GetChild(i).gameObject.SetActive(false);
                else{
                    lobby.transform.GetChild(i).gameObject.transform.GetChild(1).gameObject.SetActive(false);
                    lobby.transform.GetChild(i).gameObject.transform.GetChild(2).gameObject.SetActive(false);
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

    #endregion Lobby















    void Start()
    {
        mainmenu.SetActive(true);
        join.SetActive(false);
        lobby.SetActive(false);
        game.SetActive(false);


        addressfield = ipaddressfield.GetComponent<InputField>();
        addressfield.text = "127.0.0.1";
    }




    public void Host(){
        serverobj = Instantiate(serverpf);
        server = serverobj.GetComponent<Server>();
        isserver = true;
        mainmenu.SetActive(false);
        InitLobbySettings();
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