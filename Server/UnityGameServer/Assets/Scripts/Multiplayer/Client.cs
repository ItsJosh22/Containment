using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Client 
{
    public static int dataBufferSize = 4096;
    public int id;
    public Player player;
    public TCP tcp;
    public UDP udp;
    public Client(int _clientID)
    {
        id = _clientID;
        tcp = new TCP(id);
        udp = new UDP(id);
    }



    public class TCP
    {
        public TcpClient socket;
        private readonly int id;
        private NetworkStream stream;
        private Packet recieveData;
        private byte[] recieveBuffer;

        public TCP(int _id)
        {
            id = _id;
        }


        public void Connect(TcpClient _socket)
        {
            socket = _socket;
            socket.ReceiveBufferSize = dataBufferSize;
            socket.SendBufferSize = dataBufferSize;

            stream = socket.GetStream();
            recieveData = new Packet();
            recieveBuffer = new byte[dataBufferSize];

            stream.BeginRead(recieveBuffer, 0, dataBufferSize, ReceiveCallback, null);

            ServerSend.Welcome(id, "Shall we Begin");

        }


        public void sendData(Packet _packet)
        {
            try
            {
                if (socket != null)
                {
                    stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                }
            }
            catch (Exception _ex)
            {

                Debug.Log($"Error sending data to player {id} via TCP: {_ex}");
            }
        }



        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                int _byteLength = stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    Server.clients[id].Disconnect();
                    return;
                }

                byte[] _data = new byte[_byteLength];
                Array.Copy(recieveBuffer, _data, _byteLength);
                recieveData.Reset(HandleData(_data));

                stream.BeginRead(recieveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }
            catch (Exception _ex)
            {
                Debug.Log($"Error receiving TCP data: {_ex}");
                Server.clients[id].Disconnect();

            }
        }

        bool HandleData(byte[] _data)
        {
            int _packetLenght = 0;

            recieveData.SetBytes(_data);

            if (recieveData.UnreadLength() >= 4)
            {
                _packetLenght = recieveData.ReadInt();
                if (_packetLenght <= 0)
                {
                    return true;
                }
            }

            while (_packetLenght > 0 && _packetLenght <= recieveData.UnreadLength())
            {
                byte[] _packetBytes = recieveData.ReadBytes(_packetLenght);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet _packet = new Packet(_packetBytes))
                    {
                        int _packetId = _packet.ReadInt();
                        Server.packetHandlers[_packetId](id, _packet);
                    }
                });

                _packetLenght = 0;
                if (recieveData.UnreadLength() >= 4)
                {
                    _packetLenght = recieveData.ReadInt();
                    if (_packetLenght <= 0)
                    {
                        return true;
                    }
                }
            }

            if (_packetLenght <= 0)
            {
                return true;
            }

            return false;

        }


        public void Disconnect()
        {
            socket.Close();
            stream = null;
            recieveData = null;
            recieveBuffer = null;
            socket = null;
        }




    }


    public class UDP
    {
        public IPEndPoint endPoint;

        private int id;

        public UDP(int _id)
        {
            id = _id;
        }

        public void Connect(IPEndPoint _endpoint)
        {
            endPoint = _endpoint;

        }

        public void SendData(Packet _packet)
        {
            Server.SendUDPData(endPoint, _packet);
        }

        public void HandleData(Packet _packetData)
        {
            int _packetLenght = _packetData.ReadInt();
            byte[] _packetBytes = _packetData.ReadBytes(_packetLenght);

            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet _packet = new Packet(_packetBytes))
                {
                    int _packetId = _packet.ReadInt();
                    Server.packetHandlers[_packetId](id, _packet);
                }
            });
        }

        public void Disconnect()
        {
            endPoint = null;
        }



    }


    public void SendIntoGame(string _playername)
    {
        player = NetworkManager.instance.InstantiatePlayer();
        player.Initialize(id,_playername);
        foreach (Client _client in Server.clients.Values)
        {
            if (_client.player != null)
            {
                if (_client.id != id)
                {
                    ServerSend.SpawnPlayer(id, _client.player);
                }
            }
        }


        foreach (Client _client in Server.clients.Values)
        {
            if (_client.player != null)
            {

                ServerSend.SpawnPlayer(_client.id, player);

            }
        }

        foreach (ItemSpawner _itemSpawner in ItemSpawner.spawner.Values)
        {
            ServerSend.CreateItemSpawner(id, _itemSpawner.spawnerId, _itemSpawner.transform.position, _itemSpawner.hasItem);
        }

        foreach (Enemy _enemy in Enemy.enemies.Values)
        {
            ServerSend.SpawnEnemy(id, _enemy);
        }
    }

    void Disconnect()
    {
        Debug.Log($"{tcp.socket.Client.RemoteEndPoint} has disconnected");

        ThreadManager.ExecuteOnMainThread(() =>
        {
            UnityEngine.Object.Destroy(player.gameObject);
            player = null;
        });



        tcp.Disconnect();
        udp.Disconnect();
        ServerSend.PlayerDisconnected(id);
    }

}
