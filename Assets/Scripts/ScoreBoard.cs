using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField]
    GameObject playerScoreBoard;

    [SerializeField]
    Transform playerScoreBoardList;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
     private void OnEnable()
    {
        Player[] players = GameManager.GetAllPlayers();
        foreach (Player player in players)
        {
           GameObject itemGO = Instantiate(playerScoreBoard, playerScoreBoardList);
           PlayerScoreBoard item = itemGO.GetComponent<PlayerScoreBoard>();
           if(item != null)
           {
            item.Setup(player);
           }
        }

        //Récupéerer une tableau de tous les joueurs
        //Boucle du tableau msie en place des UI
    }

    // Update is called once per frame
    private void OnDisable()
    {
        //Vider la liste des joueurs
        foreach(Transform child in playerScoreBoardList)
        {
            Destroy(child.gameObject);
        }
    }
}
