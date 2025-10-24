using UnityEngine;
using Mirror;
using System.Collections;

[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour
{
    [SyncVar]
    private bool _isDead = false;

    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    [SerializeField]
    private float maxHealth = 100f;

    [SyncVar]
    private float currentHealth;

    public int kills;
    public int deaths;

    [SerializeField]
    private Behaviour[] disableOnDeath;

    [SerializeField]
    private GameObject[] disableGameObjectOnDeath;

    private bool[] wasEnabledOnStart;

    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    private GameObject spawnEffect;

    private bool firstSetup = true;

    public void Setup()
    {
        // changement de camera
        if (isLocalPlayer)
        {
            GameManager.instance.SetSCeneCameraActive(false);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
        }

        CmdBroadCastNewPlayerSetup();
    }

    [Command]
    private void CmdBroadCastNewPlayerSetup()
    {
        RpcSetupPlayerOnAllClients();
    }

    [ClientRpc]
    private void RpcSetupPlayerOnAllClients()
    {
        if (firstSetup)
        {
            wasEnabledOnStart = new bool[disableOnDeath.Length];
            for (int i = 0; i < disableOnDeath.Length; i++)
            {
                wasEnabledOnStart[i] = disableOnDeath[i].enabled;
            }
            firstSetup = false;
        }
        SetDefaults();
    }

    // Remettre à zéro les paramètres
    public void SetDefaults()
    {
        isDead = false;
        currentHealth = maxHealth;

        // réactive les scripts
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabledOnStart[i];
        }

        // réactive les gameobjects
        for (int i = 0; i < disableGameObjectOnDeath.Length; i++)
        {
            disableGameObjectOnDeath[i].SetActive(true);
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }

        // effet de spawn
        GameObject _gfxIns = Instantiate(spawnEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f);
    }

    // ✅ Command côté serveur pour appliquer les dégâts
    [Command]
    public void CmdDealDamage(float amount, string sourceID)
    {
        RpcTakeDamage(amount, sourceID);
    }

    // ✅ RPC appelée depuis le serveur pour tous les clients
    [ClientRpc]
    public void RpcTakeDamage(float amount, string sourceID)
    {
        if (isDead)
            return;

        currentHealth -= amount;

        // ✅ Empêche la vie d’être négative
        if (currentHealth < 0)
            currentHealth = 0;

        Debug.Log(transform.name + " a maintenant : " + currentHealth + " points de vies.");

        if (currentHealth <= 0)
        {
            Die(sourceID);
        }
    }

    private void Die(string sourceID)
    {
        if (isDead) return; // ✅ Empêche plusieurs morts

        isDead = true;

        Player sourcePlayer = GameManager.GetPlayer(sourceID);
        if (sourcePlayer != null)
        {
            sourcePlayer.kills++;
        }

        deaths++;

        // désactive les composants
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        for (int i = 0; i < disableGameObjectOnDeath.Length; i++)
        {
            disableGameObjectOnDeath[i].SetActive(false);
        }

        // désactive la collision
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        // effet de mort
        GameObject _gfxIns = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f);

        // changement de caméra
        if (isLocalPlayer)
        {
            GameManager.instance.SetSCeneCameraActive(true);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
        }

        Debug.Log(transform.name + " a été éliminé.");
        StartCoroutine(Respawn());
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        // ✅ Appel correct via le serveur
        if (Input.GetKeyDown(KeyCode.K))
        {
            CmdDealDamage(999, "Joueur");
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTimer);

        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        yield return new WaitForSeconds(0.1f);
        Setup();
    }
}
