using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Server : MonoBehaviour
{
    int channel, maxuser = 12;
    int port = 14789;

    byte error;
    int hostid;
    bool isstarted;
    List<Client_data> clients;

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
    }

    void UpdateMassage(){
        if (!isstarted)
            return;

        int rechostid, connectionid, channelid;

        byte[] recbuffer = new byte[1024];
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
                Debug.Log("<SERVER> Data sent to User[" + connectionid + "]");
                break;

            //--------------------
            case NetworkEventType.ConnectEvent:
                Debug.Log("<SERVER> User[" + connectionid + "] connected.");
                clients.Add(new Client_data(connectionid));

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

    void SendToClient(int clientid, byte[] data)
    {
        NetworkTransport.Send(hostid, clientid, channel, data, 1024, out error);
    }

}
