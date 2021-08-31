using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class ServerHandle  
{
    public static void WelcomeReceived(int _fromClient, Packet _packet)
    {
        int _clientIdCheck = _packet.ReadInt();
        string _username = _packet.ReadString();

        Debug.Log($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected and is now player {_fromClient} ");
        if (_fromClient != _clientIdCheck)
        {
            Debug.Log($"Player \"{_username}\" (ID: {_fromClient} has assumed the wrong client ID ({_clientIdCheck})");
        }

        Server.clients[_fromClient].SendIntoGame(_username);
    }

    public static void PlayerMovement(int _fromclient, Packet _packet)
    {
        bool[] _inputs = new bool[_packet.ReadInt()];
        for (int i = 0; i < _inputs.Length; i++)
        {
            _inputs[i] = _packet.ReadBool();
        }

        Quaternion _rotation = _packet.ReadQuaternion();

        Server.clients[_fromclient].player.SetInput(_inputs, _rotation);
    }

    public static void PlayerShoot(int _fromclient, Packet _packet)
    {
        Vector3 _shootDirection = _packet.ReadVector3();
        Server.clients[_fromclient].player.Shoot(_shootDirection);

    }


    public static void PlayerThrowItem(int _fromclient, Packet _packet)
    {
        Vector3 _throwDirection = _packet.ReadVector3();
        Server.clients[_fromclient].player.ThrowItem(_throwDirection);

    }

}
