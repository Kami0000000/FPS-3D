using UnityEngine;
using Mirror;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Player))]

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;


    [SerializeField]
    private string remoteLayerName = "RemotePlayer";  

     [SerializeField]
    private string dontDrawLayerName = "DontDraw";  
     
     [SerializeField]
    private GameObject playerGraphics;

     [SerializeField]
    private GameObject playerUIPrefab;

    [HideInInspector]
    public GameObject playerUIInstance;

    //Camera sceneCamera;

    private void Start()
    {
        if(!isLocalPlayer)
        {
           DisableComponents();
           AssignRemoteLayer();
        }
        else
{
    //  sceneCamera =  Camera.main;
    // if(sceneCamera != null)
    // {
    //     sceneCamera.gameObject.SetActive(false);
    // }
    //Désactiver la partie graphique du joueur local 
    Util.SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));
    //Création du UI du joueur local
    playerUIInstance=Instantiate(playerUIPrefab);

    //Configuration du UI
    PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
    if(ui == null)
    {
        Debug.LogError("Pas de ui");

    } else
    {
        ui.SetController(GetComponent<PlayerController>());
    }
GetComponent<Player>().Setup();
  
}
    } 


    public override void OnStartClient()
    {
        base.OnStartClient();
        string netId = GetComponent<NetworkIdentity>().netId.ToString();
        Player player = GetComponent<Player>();
        GameManager.RegisterPlayer(netId, player);
    }
 
    private void AssignRemoteLayer()
{
 gameObject.layer = LayerMask.NameToLayer(remoteLayerName); 
}

private void DisableComponents()//Eviter de prendre le controle des autres joueurs
{
             for (int i = 0; i <componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;//desactivation des composants 
            }
}
//quitte le jeu
    private void OnDisable()
    {
        Destroy(playerUIInstance);
    if(!isLocalPlayer)
    {
         GameManager.instance.SetSCeneCameraActive(true);
    }
   
    //      if(sceneCamera != null)
    // {
    //     sceneCamera.gameObject.SetActive(true);
    // }
     GameManager.UnregisterPlayer(transform.name);
    }
   
}

