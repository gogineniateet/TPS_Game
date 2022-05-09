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
    public GameObject GameOverPanel;
   // public GameObject GameWonPanel;

    
    [SerializeField] private Text healthValue;

    int health = 100;
    int maxHealth = 100;
    //int MedikitHealth = 50;

    // Start is called before the first frame update
    void Start()
    {
        GameOverPanel.SetActive(false);
        //GameWonPanel.SetActive(false);
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

    // decreasing the shooter health when enemy attacks.
    public void TakeHit(float value)
    {
        health = (int)Mathf.Clamp(health - value, 0, maxHealth); //  medical is health
        //Debug.Log("heath :" + health);
        healthValue.text = health.ToString();
        if (health <= 0)
        {
            Vector3 position = new Vector3(transform.position.x, Terrain.activeTerrain.SampleHeight(this.transform.position), transform.position.z);
            Destroy(this.gameObject);
            GameOverPanel.SetActive(true);
            //Debug.Log("Shooter dead");
        }
    }

    // collecting health kit
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "HealthKit")
        {
            int currentHealth = maxHealth - health;
            if (maxHealth >= currentHealth)
            {
                health = health + currentHealth;
            }
            else
            {
                health = health + maxHealth;
            }
            healthValue.text = health.ToString();
            Destroy(other.gameObject);
        }
        //if(other.gameObject.tag == "Safe")
        //{
        //    GameWonPanel.SetActive(true);
        //}

        if(other.gameObject.tag == "Water")
        {
            Destroy(this.gameObject);
            GameOverPanel.SetActive(true);
        }
        if(other.gameObject.tag == "Spawn")
        {
            ObjectPool.Instance.AddToPool(5);
        }

    }
}
