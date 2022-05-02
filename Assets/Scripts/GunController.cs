using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Transform firePosition;
    public ParticleSystem particalSystem;
    public GameObject Blood;

    int ammo = 50;
    int maxAmmo = 50;
    int ammoKit = 30;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // player shooting
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
            particalSystem.Play();
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
                Instantiate(Blood, hitInfo.point, Quaternion.identity);
                Debug.Log("Killed Enemy");
                //score.ScoreUpdate(1); // Updating score - enemy kill
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AmmoBox")
        {
            int currentAmmo = maxAmmo - ammo;
            if (ammoKit >= currentAmmo)
                ammo = ammo + currentAmmo;
            else
                ammo = ammo + ammoKit;

            Debug.Log("Ammo :" + currentAmmo);
            Destroy(other.gameObject);
        }
    }
}
