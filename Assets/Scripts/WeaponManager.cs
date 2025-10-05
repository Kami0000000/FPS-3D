using UnityEngine;
using Mirror;

public class WeaponManager : NetworkBehaviour
{
    [SerializeField]
    private PlayerWeapon primaryWeapon;

    private PlayerWeapon currentWeapon;

    [SerializeField]
    private Transform weaponHolder;

[SerializeField]
    private string weaponLayerName="Weapon";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EquipWeapon(primaryWeapon);
    }

public PlayerWeapon GetCurrentWeapon()
{
    return currentWeapon;
}
    void EquipWeapon(PlayerWeapon _weapon)
    {
        currentWeapon = _weapon;
        GameObject weaponIns = Instantiate(_weapon.graphics, weaponHolder.position,weaponHolder.rotation);
        weaponIns.transform.SetParent(weaponHolder);

        if(isLocalPlayer)
        {
            weaponIns.layer = LayerMask.NameToLayer(weaponLayerName);
            SetLayerRecursively(weaponIns, LayerMask.NameToLayer(weaponLayerName));
        }
    }
  private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);           
        }
    }
}
