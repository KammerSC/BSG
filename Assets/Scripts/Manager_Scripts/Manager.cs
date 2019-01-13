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

    void Start()
    {
        addressfield = ipaddressfield.GetComponent<InputField>();
        addressfield.text = "127.0.0.1";
    }




    public void Host(){
        serverobj = Instantiate(serverpf);
        server = serverobj.GetComponent<Server>();
        isserver = true;
        mainmenu.SetActive(false);
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
        join.SetActive(false);
        lobby.SetActive(true);
    }
}
//