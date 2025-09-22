using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
 [SerializeField]
 private float speed = 3f;

[SerializeField]
 private float mouseSensitivityX = 3f;
 [SerializeField]
 private float mouseSensitivityY = 3f;

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
    Vector3 moveVertical = transform.forward * zMov; 
 
    Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;//quelmle direction quelle vitesse
   motor.Move(velocity);

   //Mouse x Mouse y Rotation du joueur
   float yRot = Input.GetAxisRaw("Mouse X");


   Vector3 rotation = new Vector3(0, yRot , 0) * mouseSensitivityX; //blocage de x et z 
   motor.Rotate(rotation);



    //Mouse x Mouse y Rotation du camera
   float xRot = Input.GetAxisRaw("Mouse X");


   Vector3 cameraRotation = new Vector3(xRot,0 , 0) * mouseSensitivityY; //blocage de x et z 
   motor.RotateCamera(cameraRotation);
 }
}
