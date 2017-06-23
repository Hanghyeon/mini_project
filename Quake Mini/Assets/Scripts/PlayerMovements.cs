using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum PlayerState
{
    Idle = 0,
    Move,
    Jump,
}

public class PlayerMovements : MonoBehaviour {

    [SerializeField]
    protected PlayerState PS;
    public Camera Came;
    public GameObject head;
    public float JumpPower = 15f;
    public float MaxSprintSpeed = 6f;
    public float accelSpeed = 0.5f;
    public float moveSpeed=3f;
    public float Max_LimitVelocity = 5f;
    public float Min_LimitVelocity = -5f;
    float spr = 1f;
    bool isGround = false;
    //---------------------------TEST------------------------------
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 5F;
    public float sensitivityY = 5F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    Rigidbody rb;

    float rotationY = 0F;
    //-------------------------------------------------------------

    private void Awake()
    {

        rb = this.gameObject.GetComponent<Rigidbody>();
    }



    // Update is called once per frame
    void Update () {

        Move();
        RotateMouse();
    }

    private void OnCollisionStay(Collision collision)
    {

        if (collision.gameObject.tag == "Ground" && !isGround && rb.velocity.y == 0f)
        {
            isGround = true;
        }
    }

    /*  mouse Rotaion
     *  (X,0)|(1,1)
     *  (0,0)|(0,Y)
     */
    void RotateMouse()      
    {
        // 카메라가 돌게 아니라 최상위 객체(Player)가 돌아야됨
        //camera.transform.rotation = Quaternion.Euler(Input.mousePosition.y * mouseSensiX * -1f, Input.mousePosition.x * mouseSensiY, 0f);

        if (axes == RotationAxes.MouseXAndY)
        {
            float rotationX = this.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            head.transform.localEulerAngles = new Vector3(0f, 0f, -rotationY);
            this.transform.localEulerAngles = new Vector3(0f, rotationX, 0f);
        }
        else if (axes == RotationAxes.MouseX)
        {
            this.transform.Rotate(0f, Input.GetAxis("Mouse X") * sensitivityX, 0f);
            //head.transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
        }
        else  //(axes == RotationAxes.MouseY)
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            head.transform.localEulerAngles = new Vector3(0f, head.transform.localEulerAngles.y, -rotationY);
        }   
    }

    void Move()
    {
        Jump();
        this.gameObject.transform.Translate(Vector3.forward * Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed, Space.Self);
        this.gameObject.transform.Translate(-1f*Vector3.right * Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed * Sprint(), Space.Self);
    }

    float Sprint()
    {
        if (1 == Input.GetAxis("Left Shift") && Input.GetKey(KeyCode.W))  //시프트가 눌렸을 때
        {
            if (spr >= MaxSprintSpeed)
                spr = MaxSprintSpeed;
            else
                spr += accelSpeed;

            return spr;
        }
        else        //시프트가 "안" 눌렸을 때
        {
            spr = 1f;
            return spr;
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space)&&isGround)
        {
            //rb.AddForce(Vector3.up * Time.deltaTime * JumpPower, ForceMode.VelocityChange);
            rb.velocity = new Vector3(0f, LimitVelocity(rb.velocity = Vector3.up * Time.deltaTime * JumpPower).y, 0f);
            isGround = false;
        }
    }


    public Vector3 LimitVelocity(Vector3 temp)
    {
        Vector3 res = new Vector3(
            Mathf.Clamp(temp.x, Min_LimitVelocity, Max_LimitVelocity),      // X
            Mathf.Clamp(temp.y, Min_LimitVelocity, Max_LimitVelocity),      // Y
            Mathf.Clamp(temp.z, Min_LimitVelocity, Max_LimitVelocity));     // Z

        return res;
    }


}
