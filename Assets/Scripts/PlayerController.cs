using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform player_camera;
    [SerializeField] float mouse_sensitivity = 3.5f;
    [SerializeField] bool lock_cursor = true;
    [SerializeField] float walk_speed = 6.0f;
    [SerializeField] float gravity = -13.0f;
    [SerializeField] [Range(0.0f, 0.5f)] float mv_smooth_time = 0.3f;
    [SerializeField] [Range(0.0f, 0.5f)] float mouse_smooth_time = 0.03f;


    float camera_pitch = 0.0f;
    CharacterController controller;
    float gravity_vel = 0.0f;

    Vector2 current_dir = Vector2.zero;
    Vector2 current_dir_vel = Vector2.zero;

    Vector2 current_mouse_delta = Vector2.zero;
    Vector2 current_mouse_delta_vel = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (lock_cursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
     
        
    }

    // Update is called once per frame
    void Update()
    {
        update_mouselook();
        update_mvmt();
    }

    void update_mouselook()
    {
        Vector2 target_mouse_delta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        current_mouse_delta = Vector2.SmoothDamp(current_mouse_delta, target_mouse_delta, ref current_mouse_delta_vel, mouse_smooth_time);
        camera_pitch -= current_mouse_delta.y * mouse_sensitivity;
        camera_pitch = Mathf.Clamp(camera_pitch, -90.0f, 90.0f);
        player_camera.localEulerAngles = Vector3.right * camera_pitch;
        transform.Rotate(Vector3.up * current_mouse_delta.x * mouse_sensitivity);
    }

    void update_mvmt()
    {
        Vector2 target_dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        target_dir.Normalize();

        current_dir = Vector2.SmoothDamp(current_dir, target_dir, ref current_dir_vel, mv_smooth_time);
        if (controller.isGrounded)
        {
            gravity_vel = 0.0f;
        }

        gravity_vel += gravity * Time.deltaTime;
        Vector3 velocity = (transform.forward * current_dir.y + transform.right * current_dir.x) * walk_speed + Vector3.up * gravity_vel;
        controller.Move(velocity * Time.deltaTime);

    }
}
