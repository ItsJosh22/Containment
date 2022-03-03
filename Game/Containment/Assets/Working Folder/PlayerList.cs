using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class PlayerList : NetworkBehaviour
{
    public static PlayerList Instance;

    
     public List<PlayerScript> players;

    private void Awake()
    {
        Instance = this;
    }

  
    public void Addplayer(PlayerScript t)
    {
        players.Add(t);
        commandAddplayer(t);
    }
    [Command]
    public void commandAddplayer(PlayerScript t)
    {
        rpcAddplayer(t);
    }
    [ClientRpc]
    public void rpcAddplayer(PlayerScript t)
    {
        players.Add(t);
    }


    public void Removeplayer(PlayerScript t)
    {
        players.Remove(t);
        commandRemoveplayer(t);
    }
    [Command]
    public void commandRemoveplayer(PlayerScript t)
    {
        rpcRemoveplayer(t);
    }
    [ClientRpc]
    public void rpcRemoveplayer(PlayerScript t)
    {
        players.Remove(t);
    }

}
