using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed;
    public float playerRotateSpeed;

    public Transform firePosition;
    CharacterController characterController;
    Animator animator;
    //ScoreManager score;


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(inputX, 0f, inputZ);
        animator.SetFloat("Speed", movement.magnitude);
        
   
         characterController.SimpleMove(movement * Time.deltaTime * playerSpeed);
     

        // player rotation
        if (movement.magnitude > 0f)
        {
            Quaternion tempDirection = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, tempDirection, Time.deltaTime * playerRotateSpeed);
        }

        // player shooting
        if(Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    public void Fire()
    {
        Debug.DrawRay(firePosition.position, transform.forward * 100, Color.red, 1f);
        Ray ray = new Ray(firePosition.position, firePosition.forward);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 100f))
        {
            if (hitInfo.collider.tag == "Enemy")
            {
                //Debug.Log("Killed Enemy");
                //score.ScoreUpdate(1); // Updating score - enemy kill
            }
        }
    }
}
