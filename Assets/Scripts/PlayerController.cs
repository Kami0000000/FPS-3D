using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
 [SerializeField]
 private float speed;

 private PlayerMotor motor;
 
 void Start()
 {
    motor = GetComponent<PlayerMotor>();
 }
 void Update()
 {
    //Vélocité vitesse mouvement du joueur
    float xMov = Input.GetAxisRaw("Horizontal");//1 devan -1 arriere
    float zMov = Input.GetAxisRaw("Vertical");

    Vector3 moveHorizontal = transform.right * xMov;
    Vector3 moveVertical = transform.forward * xMov; 
 
    Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;//quelmle direction quelle vitesse

 }
}
