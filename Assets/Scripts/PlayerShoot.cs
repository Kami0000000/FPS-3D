using UnityEngine;
using Mirror;//Multijoueur

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{
    
    


    [SerializeField]//Remplier dans l'éditeur
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

   private PlayerWeapon currentWeapon;
    private WeaponManager weaponManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(cam == null)
        {
            Debug.LogError("Tsy mandeha ny camera satria tsy misy");
            this.enabled = false;
        }
       

        weaponManager = GetComponent<WeaponManager>();
        
    }
    private void Update()
    {
        currentWeapon = weaponManager.GetCurrentWeapon();
        if(currentWeapon.fireRate <=0f)
        {
             if(Input.GetButtonDown("Fire1"))
        {
                Shoot();
         }
        }
      else
{
    if (Input.GetButtonDown("Fire1"))
    {
        // Commence à tirer en continu
        InvokeRepeating("Shoot", 0f, 1f / currentWeapon.fireRate);
    }
    else if (Input.GetButtonUp("Fire1"))
    {
        // Arrête de tirer quand on relâche
        CancelInvoke("Shoot");
    }
}

    }
    [Command]
    void CmdOnHit(Vector3 pos, Vector3 normal)
    {
        RpcDoHitEffect(pos, normal);
    }
    [ClientRpc]
    void RpcDoHitEffect(Vector3 pos, Vector3 normal)
    {
       GameObject hitEffect = Instantiate(weaponManager.GetCurrentGraphics().hitEffectPrefab, pos, Quaternion.LookRotation(normal));
       Destroy(hitEffect, 2f);
    }
    //Fonction appelé sur le serveur lors du tir du joueur prévenir le serveur
    [Command]
    void CmdOnShot()
    {
        RpcDoShootEffect();
    }

//Apparaitre les effets de tirs
    [ClientRpc]
    void RpcDoShootEffect()
    {
        weaponManager.GetCurrentGraphics().muzzleFlash.Play();
    }

    [Client]//Seulement au client
    private void Shoot()
    {

        if(!isLocalPlayer)
        {
            return;
        }

        CmdOnShot();

        RaycastHit hit;
    //Si on a touché quelque chose
        if(Physics.Raycast(cam.transform.position,cam.transform.forward,out hit, currentWeapon.range, mask) )
        {
            if(hit.collider.tag == "Player")
            {
                //Commande
                CmdPlayerShot(hit.collider.name, currentWeapon.damage);
            }

            CmdOnHit(hit.point, hit.normal);
        }
    }

    [Command]//Client vers serveur
    private void CmdPlayerShot(string playerId, float damage)
    {
        Debug.Log(playerId+ "a été touché.");
        Player player = GameManager.GetPlayer(playerId);
        player.RpcTakeDamage(damage);
    }

}
