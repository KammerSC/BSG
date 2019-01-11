using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class Client : MonoBehaviour
{
    int channel, maxuser = 12;
    int port = 14789;
    public string serverip = "127.0.0.1";

    byte error;
    int hostid, connectionid;
    bool isstarted;

    byte databyte = 0;

    [SerializeField]
    Text txt;

    void Start(){
        Init();
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
    public void Connect()
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
                txt.text = recbuffer[0].ToString();
                Debug.Log("<Client> The server sent data: " + recbuffer[0]);
                break;
            case NetworkEventType.ConnectEvent:
                Debug.Log("<Client> Connection established."); break;
            case NetworkEventType.DisconnectEvent:
                Debug.Log("<Client> Connection terminated."); break;

        }
    }
    public void SendToServer()
    {
        databyte++;
        byte[] buffer = new byte[1024];
        buffer[0] = databyte;

        NetworkTransport.Send(hostid, connectionid, channel, buffer, 1024, out error);
    }

}
