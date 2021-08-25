using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    class ServerSend
    {

        private static void SendTCPData(int _toclient,Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toclient].tcp.sendData(_packet);
        }

        static void SendUDPData(int _toClient,Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].udp.SendData(_packet);
        }


        //Send data to everyone
        static void SendTCPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i < Server.MaxPlayers; i++)
            {
                Server.clients[i].tcp.sendData(_packet);
            }
        }

        //Send Data to everyone BUT _exceptClient
        static void SendTCPDataToAll(int _exceptClient,Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i < Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {

                Server.clients[i].tcp.sendData(_packet);
                }
            }
        }



        //Send data to everyone
        static void SendUDPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i < Server.MaxPlayers; i++)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }

        //Send Data to everyone BUT _exceptClient
        static void SendUDPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i < Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {

                    Server.clients[i].udp.SendData(_packet);
                }
            }
        }





        public static void Welcome(int _toClient, string _msg)
        {
            using (Packet _packet = new Packet((int)ServerPackets.welcome))
            {
                _packet.Write(_msg);
                _packet.Write(_toClient);

                SendTCPData(_toClient, _packet);
            }
        }

        public static void UDPTest(int _toClient)
        {
            using (Packet _packet = new Packet((int)ServerPackets.udpTest))
            {
                _packet.Write("UDP Test");
               

                SendUDPData(_toClient, _packet);
            }
        }


    }
}
