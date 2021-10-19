using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
   
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }


    public static void WelcomeReceived()
    {
        using(Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myid);
            _packet.Write(UIManager.instance.usernameField.text);

            SendTCPData(_packet);
        }

    }

   public static void PlayerMovement(bool[] _inputs)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerMovement))
        {
            _packet.Write(_inputs.Length);
            foreach (bool _input in _inputs)
            {
                _packet.Write(_input);

            }
            _packet.Write(GameManager.players[Client.instance.myid].transform.rotation);

            SendUDPData(_packet);
        }
    }

    public static void PlayerShoot(Vector3 _facing,int bulletsPerPress)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerShoot))
        {
            _packet.Write(_facing);
            _packet.Write(bulletsPerPress);

            SendTCPData(_packet);
        }
    }


    public static void PlayerThrowItem(Vector3 _facing)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerThrowItem))
        {
            _packet.Write(_facing);


            SendTCPData(_packet);
        }
    }

    public static void PlayerSwapWeapon(bool _direction)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerSwapWeapon))
        {
            _packet.Write(_direction);


            SendTCPData(_packet);
        }
    }

    public static void PlayerReloaded()
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerReloaded))
        {
            

            SendTCPData(_packet);
        }
    }

   

}
