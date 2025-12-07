using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    public static PlayerControll Instance { get; private set; }

    public float move_speed;
    public float rotation_speed;
    public GameObject camera_obj;
    public float crypto_count;

    public AudioSource walk_audio;

    private CharacterController characterController;
    private bool cursor_is_locked = true;

    public float rotY = 0;

    private float step_duration = 0.4f;
    private float step_timer;
    private float org_Step_volume;
    private float step_vol_dt;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        LockCursur(false);
        step_timer = step_duration;
        org_Step_volume = walk_audio.volume;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            moveInput.y = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveInput.y = -1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveInput.x = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveInput.x = 1;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveInput *= 1.5f;
        }

        if (Input.GetKey(KeyCode.Escape) && cursor_is_locked)
        {
            LockCursur(false);
        }
        if (Input.GetMouseButtonDown(0) && !cursor_is_locked)
        {
            LockCursur(true);
        }

        step_timer -= Time.deltaTime;
        if (moveInput.magnitude > 0.9f)
        {
            if (step_timer <= 0)
            {
                step_vol_dt = 1;
                walk_audio.volume = org_Step_volume;
                step_timer = step_duration;
                walk_audio.time = 0.07f;
                walk_audio.Play();
            }
        }
        else
        {
            //walk_audio.Stop();
            step_vol_dt -= 0.5f * Time.deltaTime;
            walk_audio.volume = Mathf.Lerp(org_Step_volume, 0, step_vol_dt);
        }

            float look_x = Input.GetAxis("Mouse X"); // horizontal
        float look_y = -Input.GetAxis("Mouse Y"); // vertical

        Vector3 move_dir = (moveInput.y * transform.forward + moveInput.x * transform.right) * move_speed * Time.deltaTime;
        characterController.Move(move_dir);

        if (cursor_is_locked)
        {
            rotY += look_y * rotation_speed * Time.deltaTime;

            transform.Rotate(0, look_x * rotation_speed * Time.deltaTime, 0);

            rotY = Mathf.Clamp(rotY, -80f, 80f);
            camera_obj.transform.localRotation = Quaternion.Euler(rotY, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Crypto")
        {
            crypto_count++;
            Destroy(other.gameObject);
        }
    }

    void LockCursur(bool locked)
    {
        if (locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        cursor_is_locked = locked;
    }
}
