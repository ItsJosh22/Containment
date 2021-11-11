using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class ClientHandle : MonoBehaviour
{
    
   public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myid = _myId;

        ClientSend.WelcomeReceived();


        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);

    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);
    }
   

    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        //if (GameManager.players.TryGetValue(_id, out PlayerManager _player))
        //{
        //    _player.transform.position = _position;
        //}
        GameManager.players[_id].transform.position = _position;
     
    }

    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();
        //if (GameManager.players.TryGetValue(_id, out PlayerManager _player))
        //{
        //    _player.transform.rotation = _rotation;
        //}
        GameManager.players[_id].transform.rotation = _rotation;
    }

    public static void PlayerDisconnect(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Destroy(GameManager.players[_id].gameObject);
        GameManager.players.Remove(_id);

    }

    public static void PlayerHealth(Packet _packet)
    {
        int _id = _packet.ReadInt();
        float _health = _packet.ReadFloat();
        GameManager.players[_id].SetHealth(_health);
    }

    public static void PlayerRespawned(Packet _packet)
    {
        int _id = _packet.ReadInt();

        GameManager.players[_id].Respawn();
    }

    public static void CreateItemSpawner(Packet _packet)
    {
        int _spawnerId = _packet.ReadInt();
        Vector3 _spawnerPosition = _packet.ReadVector3();
        bool _hasItem = _packet.ReadBool();
        GameManager.instance.CreateItemSpawner(_spawnerId, _spawnerPosition, _hasItem);
    }

    public static void ItemSpawned(Packet _packet)
    {
        int _spawnerId = _packet.ReadInt();
        GameManager.itemSpawners[_spawnerId].ItemSpawned();
    }

    public static void ItemPickedUp(Packet _packet)
    {
        int _spawnerId = _packet.ReadInt();
        int _byPlayer = _packet.ReadInt();
        GameManager.itemSpawners[_spawnerId].ItemPickedUp();
        GameManager.players[_byPlayer].itemCount++;
    }

    public static void SpawnProjectile(Packet _packet)
    {
        int _projectileId = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        int _throwByPlayer = _packet.ReadInt();

        GameManager.instance.SpawnProjectile(_projectileId, _position);
        GameManager.players[_throwByPlayer].itemCount--;
    
    }

    public static void ProjectilePosition(Packet _packet)
    {
        int _projectileId = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        if (GameManager.projectiles.TryGetValue(_projectileId, out ProjectileManager _projectile))
        {

            _projectile.transform.position = _position;
        }

        GameManager.projectiles[_projectileId].transform.position = _position;

    }

    public static void ProjectileExploded(Packet _packet)
    {
        int _projectileId = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();



        GameManager.projectiles[_projectileId].Explode(_position);

    }
    public static void SpawnEnemy(Packet _packet)
    {
        int _enemyId = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        GameManager.instance.SpawnEnemy(_enemyId,_position);

    }
    public static void EnemyPosition(Packet _packet)
    {
        int _enemyId = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        if (GameManager.enemies.TryGetValue(_enemyId, out EnemyManager _enemy) && _enemy != null)
        {

             _enemy.transform.position = _position;
        }


    }
    public static void EnemyHealth(Packet _packet)
    {
        int _enemyId = _packet.ReadInt();
        float _health = _packet.ReadFloat();
        if (GameManager.enemies.TryGetValue(_enemyId,out EnemyManager e))
        {

         e.SetHealth(_health);
        }

    }

    public static void SwapWep(Packet _packet)
    {
        int _id = _packet.ReadInt();
        int currentWep = _packet.ReadInt();


        GameManager.players[_id].SwapWeapon(currentWep);

    }


    public static void Impact(Packet _packet)
    {
        Vector3 _pos = _packet.ReadVector3();
        Quaternion _rot = _packet.ReadQuaternion();


        GameManager.instance.Impact(_pos,_rot);

    }
    public static void LevelChanging(Packet _packet)
    {
        string _levelname = _packet.ReadString();
        


        SceneChanger.instance.leveltoChange(_levelname);

    }

    public static void ClientlevelChange(Packet _packet)
    {
        //   Vector3 _pos = _packet.ReadVector3();
        //Quaternion _rot = _packet.ReadQuaternion();


        SceneChanger.instance.Changing();

    }

    public static void PlayerFlashlight(Packet _packet)
    {
        int id = _packet.ReadInt();


        GameManager.players[id].EnableFlashLight();
        
    }

    public static void DoorInteracted(Packet _packet)
    {
        int id = _packet.ReadInt();
        bool activated = _packet.ReadBool();

        InteractionHolder.instance.doors[id].DoorInter(activated);

    }

    public static void PlayerPickedUpWeapon(Packet _packet)
    {
        int _wepid = _packet.ReadInt();
        int _playerid = _packet.ReadInt();

        GameManager.allGuns[_wepid].ClientTakeWeapon(GameManager.players[_playerid].gunPos);

    }

    public static void FreeWeapon(Packet _packet)
    {
        int _wepid = _packet.ReadInt();
       

        GameManager.allGuns[_wepid].ReturnWep();

    }
}