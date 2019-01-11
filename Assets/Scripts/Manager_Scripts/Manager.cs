using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public bool isserver = false;
    string address;

    [SerializeField]
    GameObject clientobj, mainmenu, host, join, lobby, game;

    [SerializeField]
    Text addressfield;

    [SerializeField]
    GameObject serverpf;

    GameObject serverobj; Server server;
    Client client;

    void Start()
    {
        client = clientobj.GetComponent<Client>();
        addressfield.text = "127.0.0.1";
    }




    public void Host(){
        serverobj = Instantiate(serverpf);
        server = serverobj.GetComponent<Server>();
        mainmenu.SetActive(false);
        lobby.SetActive(true);
    }
    public void Join(){
        mainmenu.SetActive(false);
        join.SetActive(true);
    }
    public void Connect()
    {
        Debug.Log("Addressfield: " + addressfield.text);



        client.serverip = addressfield.text;
        client.Connect();
        join.SetActive(false);
        lobby.SetActive(true);
    }
}
