using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed;
    public float playerRotateSpeed;

    
    CharacterController characterController;
    Animator animator;

    [SerializeField] private Text ScoreValue;
    [SerializeField] private Text healthValue;

    int health = 100;
    int maxHealth = 100;
    int MedikitHealth = 50;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        healthValue.text= health.ToString();
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "HealthKit")
        {
            int currentHealth = maxHealth - health;
            if(maxHealth >= currentHealth)
            {
                health = health + currentHealth;
            }
            else
            {
                health = health + maxHealth;
            }
            healthValue.text = health.ToString();
            Debug.Log("Health:" + health);
            Destroy(other.gameObject);
        }
    }


}
