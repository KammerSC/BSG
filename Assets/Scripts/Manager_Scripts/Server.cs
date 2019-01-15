using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;

public class Server : MonoBehaviour
{
    int channel, maxuser = 12;
    int port = 14789;

    byte error;
    int hostid;
    bool isstarted;

    int msgsize = 1024;

    List<Client_data> clients;
    Manager manager;

    void Start(){
        Init();
    }
    void Update(){
        UpdateMassage();
    }
    public void Init(){
        NetworkTransport.Init();

        ConnectionConfig cc = new ConnectionConfig();
        channel = cc.AddChannel(QosType.ReliableSequenced);
        HostTopology topo = new HostTopology(cc, maxuser);
        hostid = NetworkTransport.AddHost(topo, port, null);

        Debug.Log("Opening port: " + port);
        isstarted = true;
        clients = new List<Client_data>();
        clients.Add(new Client_data(0));

        Debug.Log("<{[SERVER STARTED]}>");


        manager = GameObject.Find("UI").GetComponent<Manager>();



    }

    void UpdateMassage(){
        if (!isstarted)
            return;

        int rechostid, connectionid, channelid;

        byte[] recbuffer = new byte[msgsize];
        int datasize;

        NetworkEventType etype = NetworkTransport.Receive(out rechostid, out connectionid, out channelid, recbuffer, 1024, out datasize, out error);
        switch (etype){
            case NetworkEventType.Nothing:
                //Debug.Log("<SERVER> Nothing.");
                break;
            case NetworkEventType.DataEvent:
                Debug.Log("<SERVER> User[" + connectionid + "] sent data: buffer[0]: " + recbuffer[0]);
                recbuffer[0]++;
                SendToClient(connectionid, recbuffer);
                ServerTranslate(recbuffer);
                Debug.Log("<SERVER> Data sent to User[" + connectionid + "]");
                break;

            //--------------------
            case NetworkEventType.ConnectEvent:
                Debug.Log("<SERVER> User[" + connectionid + "] connected.");
                clients.Add(new Client_data(connectionid));
                byte[] tmp = new byte[msgsize];
                tmp[0] = 1; tmp[1] = 1; tmp[2] = (byte)connectionid;
                SendToClient(connectionid, tmp);
                SendToClient(connectionid, manager.settings.SettingToSend());

                break;
            case NetworkEventType.DisconnectEvent:
                Debug.Log("<SERVER> User[" + connectionid + "] disconnected.");
                for(int i=0; i<clients.Count; i++)
                    if(clients[i].id == connectionid)
                    {
                        clients.RemoveAt(i);
                        break;
                    }
                break;
        }
    }

    void ServerTranslate(byte[] recbuffer)
    {
        switch (recbuffer[0])
        {
            case 1:
                switch (recbuffer[1])
                {

                }
                break;



        }




    }

    void SendToClient(int clientid, byte[] data)
    {
        NetworkTransport.Send(hostid, clientid, channel, data, 1024, out error);
    }
    public void SentToAllClient(byte[] data)
    {
        for(int i=0; i<clients.Count; i++)
            if(clients[i].id != 0)
                SendToClient(clients[i].id, data);
    }


}
