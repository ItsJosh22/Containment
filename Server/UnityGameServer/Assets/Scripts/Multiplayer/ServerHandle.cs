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
        int _bulletsPerPress = _packet.ReadInt();
        Server.clients[_fromclient].player.Shoot(_shootDirection,_bulletsPerPress);

    }


    public static void PlayerThrowItem(int _fromclient, Packet _packet)
    {
        Vector3 _throwDirection = _packet.ReadVector3();
        Server.clients[_fromclient].player.ThrowItem(_throwDirection);

    }

    public static void PlayerSwapWeapon(int _fromclient, Packet _packet)
    {
        bool SwapDirection = _packet.ReadBool();
        Server.clients[_fromclient].player.SwapWeapon(SwapDirection);

    }

    public static void PlayerReloaded(int _fromclient, Packet _packet)
    {
        
        Server.clients[_fromclient].player.Reload();

    }

    public static void PlayerFlashlight(int _fromclient, Packet _packet)
    {

        Server.clients[_fromclient].player.EnableFlashlight();

    }

    public static void DoorInteracted(int _fromclient, Packet _packet)
    {

        int _doorID = _packet.ReadInt();
        bool _activated = _packet.ReadBool();

        InteractionHolder.instance.doors[_doorID].doorInteracted(_activated);

    }

    public static void PlayerPickedupGun(int _fromclient, Packet _packet)
    {
        int _wepid = _packet.ReadInt();
        int _playerId = _packet.ReadInt();

        ServerSend.PlayerPickedupWeapon(_wepid, _playerId);

    }

    public static void FreeWeapon(int _fromclient, Packet _packet)
    {
        int _wepid = _packet.ReadInt();
       

        ServerSend.FreeWeapon(_wepid);

    }
}
