using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;

public class Client : MonoBehaviour
{
    public static Client instance;
    public static int dataBuffersize = 4096;

    public string ip = "127.0.0.1";
    public int port = 42050;
    public int myid = 0;
    public TCP tcp;
    public UDP udp;

    private bool isConnected = false;

    private delegate void PacketHandler(Packet _packet);
    private static Dictionary<int, PacketHandler> packetHandlers;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }

    }


    private void OnApplicationQuit()
    {
        Disconnect();
    }

    public void ConnectToServer()
    {
        tcp = new TCP();
        udp = new UDP();
        InitializeClientData();
        isConnected = true;
        tcp.Connect();

    }

    // TCP STUFF
    public class TCP
    {
        public TcpClient socket;
        private byte[] receiveBuffer;
        private NetworkStream stream;
        private Packet receivedData;
        public void Connect()
        {

            socket = new TcpClient
            {
                ReceiveBufferSize = dataBuffersize,
                SendBufferSize = dataBuffersize

            };

            receiveBuffer = new byte[dataBuffersize];

            socket.BeginConnect(instance.ip, instance.port, ConnectCallback, socket);

        }


        void ConnectCallback(IAsyncResult _result)
        {

            socket.EndConnect(_result);

            if (!socket.Connected)
            {
                return;
            }

            stream = socket.GetStream();

            receivedData = new Packet();

            stream.BeginRead(receiveBuffer, 0, dataBuffersize, ReceiveCallback, null);

        }

        public void SendData(Packet _packet)
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

                Debug.Log($"Error sending data to server via TCP: {_ex}");
            }
        }

        void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                int _byteLength = stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    instance.Disconnect();
                    return;
                }
                byte[] _data = new byte[_byteLength];
                Array.Copy(receiveBuffer, _data, _byteLength);

                receivedData.Reset(HandleData(_data));

                stream.BeginRead(receiveBuffer, 0, dataBuffersize, ReceiveCallback, null);

            }
            catch
            {
                Disconnect();

            }
        }


        bool HandleData(byte[] _data)
        {
            int _packetLenght = 0;

            receivedData.SetBytes(_data);

            if (receivedData.UnreadLength() >= 4)
            {
                _packetLenght = receivedData.ReadInt();
                if (_packetLenght <= 0)
                {
                    return true;
                }
            }

            while (_packetLenght > 0 && _packetLenght <= receivedData.UnreadLength())
            {
                byte[] _packetBytes = receivedData.ReadBytes(_packetLenght);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet _packet = new Packet(_packetBytes))
                    {
                        int _packetId = _packet.ReadInt();
                        packetHandlers[_packetId](_packet);
                    }
                });

                _packetLenght = 0;
                if (receivedData.UnreadLength() >= 4)
                {
                    _packetLenght = receivedData.ReadInt();
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

        void Disconnect()
        {
            instance.Disconnect();
            stream = null;
            receivedData = null;
            receiveBuffer = null;
            socket = null;
        }

    }

    // UDP STUFF
    public class UDP
    {
        public UdpClient socket;
        public IPEndPoint endPoint;

        public UDP()
        {
            endPoint = new IPEndPoint(IPAddress.Parse(instance.ip), instance.port);
        }

        public void Connect(int _localPort)
        {
            socket = new UdpClient(_localPort);

            socket.Connect(endPoint);
            socket.BeginReceive(ReceiveCallBack, null);

            using (Packet _packet = new Packet())
            {
                SendData(_packet);
            }

        }

        public void SendData(Packet _packet)
        {
            try
            {
                _packet.InsertInt(instance.myid);
                if (socket != null)
                {
                    socket.BeginSend(_packet.ToArray(), _packet.Length(), null, null);
                }
            }
            catch (Exception _ex)
            {
                Debug.Log($"Error sending Data to server via UDP: {_ex}");
                throw;
            }
        }


        void ReceiveCallBack(IAsyncResult _result)
        {
            try
            {
                byte[] _data = socket.EndReceive(_result, ref endPoint);
                socket.BeginReceive(ReceiveCallBack, null);
                if (_data.Length < 4)
                {
                    instance.Disconnect();
                    return;
                }
                HandleData(_data);
            }
            catch (Exception)
            {
                Disconnect();
            }
        }

        void HandleData(byte[] _data)
        {
            using (Packet _packet = new Packet(_data))
            {
                int _packetLength = _packet.ReadInt();
                _data = _packet.ReadBytes(_packetLength);
            }


            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet _packet = new Packet(_data))
                {
                    int _packetId = _packet.ReadInt();
                    packetHandlers[_packetId](_packet);
                }
            });

        }

        void Disconnect()
        {
            instance.Disconnect();
            endPoint = null;
            socket = null;
        }

    }

    private void InitializeClientData()
    {
        packetHandlers = new Dictionary<int, PacketHandler>()
        {
            {(int)ServerPackets.welcome, ClientHandle.Welcome },
            {(int)ServerPackets.spawnPlayer, ClientHandle.SpawnPlayer },
            {(int)ServerPackets.playerPosition, ClientHandle.PlayerPosition },
            {(int)ServerPackets.playerRotation, ClientHandle.PlayerRotation },
            {(int)ServerPackets.playerDisconnect, ClientHandle.PlayerDisconnect },
            {(int)ServerPackets.playerHealth, ClientHandle.PlayerHealth },
            {(int)ServerPackets.playerRespawned, ClientHandle.PlayerRespawned },
            {(int)ServerPackets.createItemSpawner, ClientHandle.CreateItemSpawner },
            {(int)ServerPackets.itemSpawned, ClientHandle.ItemSpawned },
            {(int)ServerPackets.itemPickedUp, ClientHandle.ItemPickedUp },
            {(int)ServerPackets.spawnProjectile, ClientHandle.SpawnProjectile },
            {(int)ServerPackets.projectilePosition, ClientHandle.ProjectilePosition },
            {(int)ServerPackets.projectileExploded, ClientHandle.ProjectileExploded },
                 {(int)ServerPackets.spawnEnemy, ClientHandle.SpawnEnemy },
                      {(int)ServerPackets.enemyPosition, ClientHandle.EnemyPosition },
                           {(int)ServerPackets.enemyHealth, ClientHandle.EnemyHealth },
                           {(int)ServerPackets.swapedWeapon, ClientHandle.SwapWep },


        };
        Debug.Log("Initalized Packets");
    }

    void Disconnect()
    {
        if (isConnected)
        {
            isConnected = false;
            tcp.socket.Close();
            udp.socket.Close();

            Debug.Log("Disconnected from server");
        }
    }

}
