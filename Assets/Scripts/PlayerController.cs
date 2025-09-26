using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(ConfigurableJoint))]
public class PlayerController : MonoBehaviour
{
 [SerializeField]
 private float speed = 3f;

[SerializeField]
 private float mouseSensitivityX = 3f;

 [SerializeField]
 private float mouseSensitivityY = 3f;

 [SerializeField]
 private float jetpackForce = 1000f;

[Header("Joint Options")]
[SerializeField]
private float jointSpring = 20f;
[SerializeField]
private float jointMaxForce = 60f;

 private PlayerMotor motor;
 private ConfigurableJoint joint;
 
 void Start()
 {
    motor = GetComponent<PlayerMotor>();
    joint = GetComponent<ConfigurableJoint>();
    SetJointSettings(jointSpring);
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
   float xRot = Input.GetAxisRaw("Mouse Y");


   float cameraRotationX = xRot * mouseSensitivityY; //blocage de x et z 
   motor.RotateCamera(cameraRotationX);


    Vector3 jetpackVelocity = Vector3.zero;
   // Jetpack
   if(Input.GetButton("Jump"))
   {
      jetpackVelocity = Vector3.up * jetpackForce;
      SetJointSettings(0f);
      //Debug.Log("Jump cliqué" +jointSpring+"and"+jointMaxForce);
   }
   else
   {
      SetJointSettings(jointSpring);
      //Debug.Log("Jump non cliqué"+jointSpring+"and"+jointMaxForce);
   }
   //Apppliquer la force
   motor.AppliquerJetpack(jetpackVelocity);
 }
 private void SetJointSettings(float _jointSpring)
 {
   joint.yDrive = new JointDrive { positionSpring = _jointSpring, maximumForce = jointMaxForce };
 }
}
