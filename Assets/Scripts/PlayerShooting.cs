using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject prefab;
    public GameObject shootPoint;
    public GameObject blood_prefab;
    public ParticleSystem muzzel_ps;
    public int ammoCount;
    public int add_ammo = 5;
    public float shoot_cooldown = 1.0f;
    public float max_spread = 1.0f;

    public float muzzel_flash_duration = 0.05f;

    private float shoot_timer;
    private Light gun_flash;
    public AudioSource gun_shot_sound;

    // Start is called before the first frame update
    private void Start()
    {
        shoot_timer = shoot_cooldown;
        gun_flash = shootPoint.GetComponentInChildren<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        shoot_timer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Mouse0) && shoot_timer < 0)
        {
            Shoot();
            shoot_timer = shoot_cooldown;
        }
    }

    void Shoot()
    {
        if (ammoCount > 0)
        {
            gun_shot_sound.time = 0.11f;
            gun_shot_sound.Play();

            for (int i = 0; i < 3; i++)
            {
                Vector3 rand_dir = get_random_vector();

                Ray ray = new Ray(shootPoint.transform.position, rand_dir);

                RaycastHit hit;
                Debug.DrawRay(ray.origin, ray.direction * 10, Color.blue, 10000);
                if (Physics.Raycast(ray, out hit, 25))
                {
                    if (hit.collider.tag == "Vermin")
                    {
                        GameObject obj = Instantiate(blood_prefab, hit.point + ray.direction * 0.1f, Quaternion.identity);
                        obj.transform.forward = -ray.direction;
                        hit.collider.gameObject.SendMessageUpwards("Shot", SendMessageOptions.DontRequireReceiver);
                    }
                }
            }

            gun_flash.enabled = true;
            Invoke("disable_muzzel_flash", muzzel_flash_duration);
            muzzel_ps.Play();
            
            //Instantiate(prefab, shootPoint.transform.position, shootPoint.transform.rotation);
            ammoCount--;
        }
    }

    Vector3 get_random_vector()
    {
        float angle = Random.Range(0, 2 * Mathf.PI);
        float dist = Mathf.Sin(Mathf.Deg2Rad * Random.Range(0, max_spread));

        float x = dist * Mathf.Cos(angle);
        float y = dist * Mathf.Sin(angle);

        Vector3 rand_dir_local = new Vector3(x, y, 1);
        Vector3 rand_dir = shootPoint.transform.TransformDirection(rand_dir_local);

        return rand_dir;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ammo")
        {
            ammoCount += add_ammo;
            Destroy(other.gameObject);
        }
    }

    void disable_muzzel_flash()
    {
        gun_flash.enabled = false;
    }

}
