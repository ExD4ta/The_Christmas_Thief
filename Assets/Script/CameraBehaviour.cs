using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    float xm, ym;
    [SerializeField] int maxAngle;
    [SerializeField] int sensibility;
    [SerializeField] Animator anim;
    public bool cameralock;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
        {
            anim.Play("headBobbing");
        }
    }

    private void FixedUpdate()
    {
        if (cameralock == false)
        {
            xm += Input.GetAxisRaw("Mouse X");
            ym -= Input.GetAxisRaw("Mouse Y");

            ym = Mathf.Clamp(ym, -maxAngle, maxAngle);
            transform.localRotation = Quaternion.Euler(ym * Time.deltaTime * sensibility, 0, 0);
            transform.parent.rotation = Quaternion.Euler(0, xm * Time.deltaTime * sensibility, 0);
        }
    }
    
}
