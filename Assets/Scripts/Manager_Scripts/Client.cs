﻿using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Text;

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

    GameObject ui;
    Manager manager;


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

        //Debug.Log("<Client>Connected to:" + serverip);
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
                TranslateMsg(recbuffer);
                break;
            case NetworkEventType.ConnectEvent:
                //Debug.Log("<Client> Connection established."); 
                break;
            case NetworkEventType.DisconnectEvent:
                Debug.Log("<Client> Connection terminated.");
                break;

        }
    }
    public void SendToServer(byte[] data)
    {
        manager.Log("<Client> Data Sent to server, data[0]: " + data[0] + " data[1]: "+data[1] );
        NetworkTransport.Send(hostid, connectionid, channel, data, 1024, out error);
    }
    void TranslateMsg(byte[] data)
    {
        switch (data[0])
        {
            case 1:
                switch (data[1])
                {
                    case 1:
                        #region 1-1 case
                        Debug.Log("<Client> 1-1 Recived my number: " + data[2]);
                        manager.myid = data[2];
                        manager.clientdata = new Client_data(manager.myid, manager.myname);
                        SendToServer(manager.clientdata.ClientDataToSend(manager.myid));
                        #endregion 1-1 case
                        break;
                    case 2:
                        Debug.Log("<Client> 1-2 Recived settings.");
                        manager.SetSettings(data);
                        manager.selectedchar.SetActive(true);
                        break;

                    case 3:
                        Debug.Log("<Client> 1-3 Recived Client data (ALL DATA).");
                        manager.clientdata.AcceptData(data);
                        manager.SetUpClientrows();
                        break;

                    case 4:
                        Debug.Log("<Client> 1-4 Recived Client data (PREFCHAR).");
                        manager.clientdata.AcceptData(data);
                        manager.UpdateUserPrefchar(data[2]);

                        break;

                    case 5:
                        Debug.Log("<Client> 1-5 Recived Client data (READY).");
                        manager.clientdata.AcceptData(data);
                        manager.UpdateUserReady(data[2]);
                        break;

                    case 10:
                        Debug.Log("<Client> 1-10 Recived Client data.");
                        manager.clientdata.AcceptData(data);
                        manager.SetUpClientrows();
                        break;


                }

                break;


            case 2:
                switch (data[1])
                {
                    case 1:

                        break;
                }
                break;
        }
    }

}
