using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;
using System.Text;

public class Server : MonoBehaviour
{
    int channel, maxuser = 12;
    int port = 14789;

    byte error;
    int hostid;
    bool isstarted;

    int msgsize = 1024;

    Manager manager;

    void Start(){
        Init();
    }
    void Update(){
        UpdateMassage();
    }
    public void Init(){
        manager = GameObject.Find("UI").GetComponent<Manager>();
        NetworkTransport.Init();

        ConnectionConfig cc = new ConnectionConfig();
        channel = cc.AddChannel(QosType.ReliableSequenced);
        HostTopology topo = new HostTopology(cc, maxuser);
        hostid = NetworkTransport.AddHost(topo, port, null);

        manager.Log("Opening port: " + port);
        isstarted = true;
        manager.Log("<{[SERVER STARTED]}>");
    }

    void UpdateMassage(){
        if (!isstarted)
            return;

        int rechostid, connectionid, channelid;

        byte[] recbuffer = new byte[msgsize];
        int datasize;

        NetworkEventType etype = NetworkTransport.Receive(out rechostid, out connectionid, out channelid, recbuffer, 1024, out datasize, out error);
        byte[] tmp;
        switch (etype){
            case NetworkEventType.Nothing:
                //Debug.Log("<SERVER> Nothing.");
                break;
            case NetworkEventType.DataEvent:
                ServerTranslate(recbuffer);
                manager.Log("Data recived from client[" + connectionid + "]: " + "data[0]: " + recbuffer[0]+ ", data[1]: " + recbuffer[1]);
                break;

            //--------------------
            case NetworkEventType.ConnectEvent:
                Debug.Log("<SERVER> User[" + connectionid + "] connected.");
                tmp = new byte[manager.msgsize];
                tmp[0] = 1; tmp[1] = 1; tmp[2] = (byte)connectionid;
                SendToClient(connectionid, tmp);
                break;
            case NetworkEventType.DisconnectEvent:
                Debug.Log("<SERVER> User[" + connectionid + "] disconnected.");
                manager.clientdata.Remove((byte)connectionid);
                tmp = new byte[manager.msgsize];
                tmp[0] = 1; tmp[1] = 10; tmp[2] = (byte)connectionid;
                manager.SetUpClientrows();
                SendToAllClient(tmp);
                break;
        }
    }

    void ServerTranslate(byte[] data)
    {
        switch (data[0])
        {
            case 1:
                switch (data[1])
                {
                    case 1:

                        break;

                    case 3:
                        Debug.Log("Recived client data <ALL DATA>.");
                        manager.clientdata.AcceptData(data);
                        manager.SetUpClientrows();

                        
                        for (int i = 0; i < manager.clientdata.count; i++)
                            SendToAllClient(manager.clientdata.ClientDataToSend(manager.clientdata.ids[i]));
                        SendToAllClient(manager.settings.SettingToSend());
                        break;
                    case 4:
                        Debug.Log("Recived client data <PREFCHAR>.");
                        SendToAllClient(data);
                        manager.clientdata.AcceptData(data);
                        manager.UpdateUserPrefchar(data[2]);
                        break;
                    case 5:
                        Debug.Log("Recived client data <READY>.");
                        SendToAllClient(data);
                        manager.clientdata.AcceptData(data);
                        manager.UpdateUserReady(data[2]);
                        break;
                }
                break;


            default:
                Debug.Log("<SERVER> Default translate: data[0] = " + data[0] + " data[1] = " + data[1]);
                break;

        }




    }
    void ServerSendByCode(int x, int y)
    {
        switch (x)
        {
            case 1:
                switch (y){
                    case 1:

                        break;

                    }

                break;

        }
    }

    void SendToClient(int clientid, byte[] data)
    {
        NetworkTransport.Send(hostid, clientid, channel, data, 1024, out error);
    }
    public void SendToAllClient(byte[] data)
    {
        for (int i = 0; i < manager.clientdata.count; i++)
            if(manager.clientdata.ids[i] != manager.myid)
                SendToClient(manager.clientdata.ids[i], data);
    }
}
