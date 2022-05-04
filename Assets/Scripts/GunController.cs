using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{
    public Transform firePosition;
    public ParticleSystem particalSystem;
    public GameObject Blood;
    public Animator animator;
    [SerializeField] private Text ammoText;
    [SerializeField] private Text ScoreValue;
    private static int score = 0;

    int ammo = 50;
    int maxAmmo = 50;
    int ammoKit = 50;



    // Update is called once per frame
    void Update()
    {   // player shooting
        if(Input.GetButtonDown("Fire1"))
        {
            if (ammo > 0)
            {
                Fire();
                particalSystem.Play();
                animator.SetTrigger("isShooting");
                ammo--;
                ammoText.text = ammo.ToString();
            }              
        }
    }

    public void Fire()
    {
        Debug.DrawRay(firePosition.position, transform.forward * 100, Color.red, 1f);
        Ray ray = new Ray(firePosition.position, firePosition.forward);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 100f))
        { 
            GameObject hitEnemy = hitInfo.collider.gameObject;
            //Debug.Log(hitEnemy);
            if (hitEnemy.tag == "Enemy")
            {
                hitEnemy.SetActive(false);
                Instantiate(Blood, hitInfo.point, Quaternion.identity);                
                Debug.Log("Killed Enemy");
            }
        }
    }

    // collecting amination
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AmmoBox")
        {
            int currentAmmo = maxAmmo - ammo;
            if (ammoKit >= currentAmmo)
            {
                ammo = ammo + currentAmmo;
            }
            else
            {
                ammo = ammo + ammoKit;
            }

            Debug.Log("Ammo :" + currentAmmo);
            Destroy(other.gameObject);
        }
    }
}
