using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class Client : MonoBehaviour
{

    #region Connect stuff
    int channel, maxuser = 12;
    int port = 14789;
    public string serverip = "127.0.0.1";

    byte error;
    int hostid, connectionid;
    bool isstarted;

    #endregion


    byte databyte = 0;

    GameObject ui;

    Manager manager;

    #region Lobby stuff





    #endregion 



    void Start(){
        Init();
        ui = GameObject.Find("UI");
        manager = GameObject.Find("UI").GetComponent<Manager>();
    }
    void Update(){
        UpdateMassage();
    }

    public void Init()
    {
        NetworkTransport.Init();

        ConnectionConfig cc = new ConnectionConfig();
        channel = cc.AddChannel(QosType.ReliableSequenced);

        HostTopology topo = new HostTopology(cc, maxuser);

        hostid = NetworkTransport.AddHost(topo, 0);

    }
    public void Connect(string serverip)
    {
        connectionid = NetworkTransport.Connect(hostid, serverip,  port, 0, out error);

        Debug.Log("<Client>Connected to:" + serverip);
        isstarted = true;
    }
    void UpdateMassage()
    {
        if (!isstarted)
            return;
        int rechostid, connectionid, channelid;

        byte[] recbuffer = new byte[1024];
        int datasize;

        NetworkEventType etype = NetworkTransport.Receive(out rechostid, out connectionid, out channelid, recbuffer, 1024, out datasize, out error);
        switch (etype)
        {

            case NetworkEventType.Nothing:
                //Debug.Log("<SERVER> Nothing.");
                break;
            case NetworkEventType.DataEvent:
                Debug.Log("<Client> The server sent data: " + recbuffer[0]);
                TranslateMsg(recbuffer);
                break;
            case NetworkEventType.ConnectEvent:
                Debug.Log("<Client> Connection established."); break;
            case NetworkEventType.DisconnectEvent:
                Debug.Log("<Client> Connection terminated."); break;

        }
    }





    public void SendToServer(byte[] data)
    {
        NetworkTransport.Send(hostid, connectionid, channel, data, 1024, out error);
    }
    void TranslateMsg(byte[] tmp)
    {
        switch (tmp[0])
        {
            case 1:
                switch (tmp[1])
                {
                    case 1:
                        Debug.Log("Recived my number: " + tmp[2]);


                        break;
                    case 2:
                        Debug.Log("Recived settings.");
                        manager.SetSettings(tmp);
                        break;
                }

                break;


            case 2:
                switch (tmp[1])
                {
                    case 1:

                        break;
                }
                break;
        }
    }


}
