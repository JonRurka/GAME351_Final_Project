using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    public float move_speed;
    public float rotation_speed;
    public GameObject camera_obj;

    private CharacterController characterController;
    private bool cursor_is_locked = true;

    public float rotY = 0;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        LockCursur(false);

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
