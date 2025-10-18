using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(Animator))]
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

 [SerializeField]
 private float jetpackFuelBurnSpeed = 1f;

 [SerializeField]
 private float jetpackFuelRegenSpeed = 0.3f;
 private float jetpackFuelAmount = 1f;
 
public float GetJetpackFuelAmount()
{
   return jetpackFuelAmount;
}

 

[Header("Joint Options")]
[SerializeField]
private float jointSpring = 20f;
[SerializeField]
private float jointMaxForce = 60f;

 private PlayerMotor motor;
 private ConfigurableJoint joint;
 private Animator animator;
 
 void Start()
 {
    motor = GetComponent<PlayerMotor>();
    joint = GetComponent<ConfigurableJoint>();
    animator  = GetComponent<Animator>();
    SetJointSettings(jointSpring);
 }
 void Update()
 {
   if(PauseMenu.isOn)
   {
      if(Cursor.lockState != CursorLockMode.None)
   {
      Cursor.lockState = CursorLockMode.None;
   }
            motor.Move(Vector3.zero);
            motor.Rotate(Vector3.zero);
            motor.RotateCamera(0f);
            motor.AppliquerJetpack(Vector3.zero);

      return;
   }
   if(Cursor.lockState != CursorLockMode.Locked)
   {
      Cursor.lockState = CursorLockMode.Locked;
   }
   RaycastHit _hit;
   if(Physics.Raycast(transform.position, Vector3.down, out _hit, 100f))
   {
      joint.targetPosition = new Vector3(0f, -_hit.point.y , 0f);
   }
   else
   {
      joint.targetPosition = new Vector3(0f, 0f ,0f);//le sol
   }


    //Vélocité vitesse mouvement du joueur
    float xMov = Input.GetAxis("Horizontal");//1 devan -1 arriere
    float zMov = Input.GetAxis("Vertical");

    Vector3 moveHorizontal = transform.right * xMov;
    Vector3 moveVertical = transform.forward * zMov; 
 
    Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;//quelmle direction quelle vitesse
   motor.Move(velocity);

   //animation du jetpack
   animator.SetFloat("ForwardVelocity", zMov);

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
   if(Input.GetButton("Jump") && jetpackFuelAmount > 0 )
   {
       jetpackFuelAmount -= jetpackFuelBurnSpeed * Time.deltaTime;
      if(jetpackFuelAmount >= 0.01f)
      {
         
         jetpackVelocity = Vector3.up * jetpackForce;
         SetJointSettings(0f);
      }
     
      
      //Debug.Log("Jump cliqué" +jointSpring+"and"+jointMaxForce);
   }
   else
   {
      jetpackFuelAmount += jetpackFuelRegenSpeed * Time.deltaTime;
      SetJointSettings(jointSpring);
      //Debug.Log("Jump non cliqué"+jointSpring+"and"+jointMaxForce);
   }
   //clamper le jetpackFuel entre 0 et 1
   jetpackFuelAmount = Mathf.Clamp(jetpackFuelAmount, 0f, 1f);
   //Apppliquer la force
   motor.AppliquerJetpack(jetpackVelocity);
 }
 private void SetJointSettings(float _jointSpring)
 {
   joint.yDrive = new JointDrive { positionSpring = _jointSpring, maximumForce = jointMaxForce };
 }
}
