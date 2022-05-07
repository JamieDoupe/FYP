using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform FirePoint;
    public GameObject Pulse;
    public bool CanFire;
    [SerializeField] private AudioSource PulseSound;

    private void Start()
    {
        CanFire = true;
    }

    // Update is called once per frame


    /* Checks if the player can shoot and are pressing the fire 1 button, 
     * if so a pulse is launched and a countdown begins before they can fire again */
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && CanFire)
        {
            Shoot();
            CanFire = false;
            StartCoroutine(firetimer());
        }
        else
        {
          
        }
    }

    // Creates a pulse projectile and plays the sound clip
    void Shoot()
    {
        PulseSound.Play();
        Instantiate(Pulse, FirePoint.position, FirePoint.rotation);
    }

    // Counts time before a pulse can be fired again
    IEnumerator firetimer()
    {
        yield return new WaitForSeconds(2);

        CanFire = true;
    }
}
