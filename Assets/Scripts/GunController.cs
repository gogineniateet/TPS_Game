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
        Ray ray = new Ray(firePosition.position, firePosition.forward);
        Debug.DrawRay(ray.origin, ray.direction * 30f, Color.red, 2f);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 100f))
        {
            GameObject hitZombie = hitInfo.collider.gameObject;
            if (hitZombie.tag == "Enemy")
            {

                    GameObject tempRd = hitZombie.GetComponent<EnemyController>().ragdollPrefab;
                    GameObject newTempRd = Instantiate(tempRd, hitZombie.transform.position, hitZombie.transform.rotation);
                    newTempRd.transform.Find("Hips").GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 10000);
                hitZombie.SetActive(false);

                
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
