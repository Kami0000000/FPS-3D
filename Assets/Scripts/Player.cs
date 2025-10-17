using UnityEngine;
using Mirror;
using System.Collections;

[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour
{
    [SyncVar]
    private bool _isDead= false;

    public bool isDead//property
    {
        get{ return _isDead;} //getter and setter
        protected set {_isDead = value;}
    }

    [SerializeField]
    private float maxHealth = 100f;

    


    [SyncVar]
    private float currentHealth;

    [SerializeField]//Affichage sur l'éditeur
    private Behaviour[] disableOnDeath;

    [SerializeField]//Affichage sur l'éditeur
    private GameObject[] disableGameObjectOnDeath;


    private bool[] wasEnabledOnStart;

    [SerializeField]
    private GameObject deathEffect;

        [SerializeField]
    private GameObject spawnEffect;

    public void Setup()
    {
        wasEnabledOnStart = new bool[disableOnDeath.Length];
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            wasEnabledOnStart[i] = disableOnDeath[i].enabled;
        }

        SetDefaults();
    }


    //Remettre à zéro les paramètre
    public void SetDefaults()
    {
        isDead = false;
        currentHealth = maxHealth;
        //réactive les scripts
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabledOnStart[i];
        }
        //réactive les gameobject
          for (int i = 0; i < disableGameObjectOnDeath.Length; i++)
        {
            disableGameObjectOnDeath[i].SetActive(true);
        }
        Collider col = GetComponent<Collider>();
        if(col != null)
        {
            col.enabled = true;
        }
         //chanegement de camera
        if(isLocalPlayer)
        {
           GameManager.instance.SetSCeneCameraActive(false); 
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
        }

         //  EFFETS DE PARTICULES
       GameObject _gfxIns = Instantiate (spawnEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns ,3f);
    }

    [ClientRpc]//Méthode marqué serveur aux instances ou clints
    public void RpcTakeDamage(float amount)
    {
        if(isDead)
        {
            return;
        }
        currentHealth -=amount;
        Debug.Log(transform.name+ " a maintenant : "+ currentHealth + "points de vies.");
        if(currentHealth <=0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true ; 

        //Désactive les composants
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }
         for (int i = 0; i < disableGameObjectOnDeath.Length; i++)
        {
            disableGameObjectOnDeath[i].SetActive(false);
        }

        //Désactive la collision
         Collider col = GetComponent<Collider>();
        if(col != null)
        {
            col.enabled = false; 
        }
       
        //  EFFETS DE PARTICULES
       GameObject _gfxIns = Instantiate (deathEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns ,3f);
        //chanegement de camera
        if(isLocalPlayer)
        {
           GameManager.instance.SetSCeneCameraActive(true); 
           GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
        }
         Debug.Log(transform.name + "a été éliminé.");
        StartCoroutine(Respawn());

    }
    private void Update()
    {
        if(!isLocalPlayer)
        {
            return;
        }
        if(Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(999);
        }
    }
    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTimer);
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
        SetDefaults();
    }
}
