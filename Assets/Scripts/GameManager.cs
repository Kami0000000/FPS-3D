using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const string playerIdPrefix = "Player";
    private static Dictionary<string, Player> players = new Dictionary<string, Player>();
   public static void RegisterPlayer(string netID,Player player)
   {
        string playerId = playerIdPrefix + netID ;
        players.Add(playerId, player);
        player.transform.name = playerId;
   }
   public static void UnregisterPlayer(string playerId)
   {
        players.Remove(playerId);
   } 

   public static Player GetPlayer(string playerId)
   {
        return players[playerId];
   }
//    private void OnGUI()
//    {
//     GUILayout.BeginArea(new Rect(200, 200, 200, 500));//MOdele d'afichage
//     GUILayout.BeginVertical();//Affichage en vertical
//     foreach(string playerId in players.Keys)
//     {
//         GUILayout.Label(playerId+ " - " +players[playerId].transform.name);
//     }
//     GUILayout.EndVertical();
//     GUILayout.EndArea();
//    }
}
