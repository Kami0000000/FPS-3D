using UnityEngine;
using Mirror;//Multijoueur

public class PlayerShoot : NetworkBehaviour
{
    [SerializeField]
    private PlayerWeapon weapon;

    [SerializeField]
    private GameObject weaponGFX;

    [SerializeField]
    private string weaponLayerName="Weapon";

    [SerializeField]//Remplier dans l'éditeur
    private Camera cam;

    [SerializeField]
    private LayerMask mask;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(cam == null)
        {
            Debug.LogError("Tsy mandeha ny camera satria tsy misy");
            this.enabled = false;
        }
        weaponGFX.layer = LayerMask.NameToLayer(weaponLayerName);
        
    }
    private void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
                Shoot();
         }
    }
    [Client]//Seulement au client
    private void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position,cam.transform.forward,out hit, weapon.range, mask) )
        {
            if(hit.collider.tag == "Player")
            {
                //Commande
                CmdPlayerShot(hit.collider.name, weapon.damage);
            }
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
