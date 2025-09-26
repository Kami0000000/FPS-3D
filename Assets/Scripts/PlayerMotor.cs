using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField] 
    private Camera cam;

   private Vector3 velocity;
   private Vector3 rotation;
   private float cameraRotationX = 0f;
   private float currentCameraRotationX = 0f;
   private Vector3 jetpackVelocity;

  [SerializeField]
  private float cameraRotationLimit = 85f;

   private Rigidbody rb;

   private void Start()
   {
    rb = GetComponent<Rigidbody>();
   }
   public void Move(Vector3 _velocity)
   {
    velocity = _velocity;
   }
     public void Rotate(Vector3 _rotation)
   {
    rotation = _rotation;
   }
    public void RotateCamera(float _cameraRotationX)
   {
    cameraRotationX = _cameraRotationX;
   }
   public void AppliquerJetpack(Vector3 _jetpackVelocity)
   {
      jetpackVelocity = _jetpackVelocity;
   }
   private void FixedUpdate()
   {
    PerformMovement();
    PerformRotation();
   }
   private void PerformMovement()
   {
    if(velocity!= Vector3.zero)
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }
    if(jetpackVelocity != Vector3.zero)
    {
      rb.AddForce(jetpackVelocity * Time.fixedDeltaTime, ForceMode.Acceleration);
    }
   }
   private void PerformRotation()
   {//rotation de la camera
     rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
     currentCameraRotationX -=cameraRotationX;
     currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

    cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);    
   }
 

}

