using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    //statistiche del player
    float x, z;
    [SerializeField]public float movementSpeed;
    bool crouching=false;
    Rigidbody rb;
    [SerializeField] CapsuleCollider playerCollider;
    Transform playerPosition;

    //variabili per la funzione del rumore e nascondiglio
    [SerializeField] GameObject meter;
    public int noiseMeter;
    [SerializeField] Text hideText;
    [SerializeField] Text hideCounter;
    public float timer = 6.0f;
    bool hiding=false;
    [SerializeField] Transform exitHideout;
    [SerializeField] Transform residentPosition;

    //variabili per la raccolta di oggetti
    [SerializeField] Transform handsPosition;
    [SerializeField] GameObject hands;
    [SerializeField] SphereCollider triggerItem;
    [SerializeField] Collider collisionItem;
    [SerializeField] Rigidbody bodyItem;
    
    //variabili per la UI generale
    [SerializeField] RawImage pickupImage;
    [SerializeField] Text pickupText;
    [SerializeField] Text gameOverText;
    [SerializeField] GameObject returnButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerPosition = GetComponent<Transform>();
    }

    void Update()
    {
        Crounch();
        ShowItem();
        DropItem();
        AnimationMeter();
        TimetoHide();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
        ExitHide();
        FoundYou();
    }

    private void Movement()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

        Vector3 movimento = new Vector3(x, -0.001f, z);
        movimento = transform.TransformDirection(movimento);

        rb.linearVelocity = movimento.normalized * Time.deltaTime * movementSpeed;
    }

    void Crounch() 
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            crouching = true;
            playerCollider.height = 1;
            movementSpeed *= 0.5f;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            crouching = false;
            playerCollider.height = 2;
            movementSpeed *= 2f;
        }
    }

    void ShowItem()
    {
        if (hands != null) 
        {
            hands.transform.position = handsPosition.position;
            hands.transform.rotation = handsPosition.rotation;
            triggerItem.enabled = false;
            collisionItem.enabled = false;
            bodyItem.useGravity = false;

        }
    }

    void DropItem()
    {
        if (Input.GetKey(KeyCode.Q) && hands != null) 
        {
            triggerItem.enabled = true;
            collisionItem.enabled = true;
            bodyItem.useGravity = true;
            hands = null;
        }
    }

    void AnimationMeter()
    {
        switch (noiseMeter)
        {
            case 3:
                if (meter.transform.localScale.x < 0.3f)
                {
                    meter.transform.localScale += new Vector3(0.002f, 0, 0);
                }
                break;

            case 6:
                if (meter.transform.localScale.x < 0.6f)
                {
                    meter.transform.localScale += new Vector3(0.002f, 0, 0);
                }
                break;

            case 9:
                if (meter.transform.localScale.x < 1f)
                {
                    meter.transform.localScale += new Vector3(0.002f, 0, 0);
                }  
                break;

            case 0:
                if (meter.transform.localScale.x > 0)
                {
                    meter.transform.localScale -= new Vector3(0.002f, 0, 0);
                }
                break;

            default:
                break;
            
        }
    }

    void TimetoHide()
    {
        if(meter.transform.localScale.x >= 1f)
        {
            hideText.enabled = true;
            hideCounter.enabled = true;
            if(timer > 0)
            {
                timer -= Time.deltaTime;
                hideCounter.text = ""+(int)timer;
            }
        }
    }

    void FoundYou() 
    {
        if (timer <= 0 && hiding == false)
        {
            playerPosition.position = new Vector3(10f, 1f, 6f);
            playerPosition.rotation = Quaternion.Euler(0f, -90f, 0f);
            residentPosition.position = new Vector3(4, 0, 6);
            movementSpeed = 0;

            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;

            hideText.enabled = false;
            hideCounter.enabled = false;

            gameOverText.enabled = true;
            gameOverText.text = "GameOver";
            returnButton.SetActive(true);

            noiseMeter = 0;
            timer = 6.0f;
            //trova soluzione

        }
        else if (timer <= 0 && hiding == true)
        { 
            hideText.enabled = false;
            hideCounter.enabled = false;
            noiseMeter = 0;
            timer = 6.0f;
        }
    }

    void ExitHide()
    {
        if(hiding == true)
        {
            if(Input.GetKey(KeyCode.Escape))
            {
                playerPosition.position = exitHideout.position;

                playerCollider.enabled=true;
                rb.useGravity = true;
                movementSpeed = 300;

                hiding = false;
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Package" && hands == null)
        {
            pickupImage.enabled = true;
            pickupText.enabled = true;
            pickupText.text = "Prendi " + other.name;

            if (Input.GetKey(KeyCode.E))
            {
                hands = other.gameObject;
                triggerItem = hands.GetComponent<SphereCollider>();
                collisionItem = hands.GetComponent<Collider>();
                bodyItem = hands.GetComponent<Rigidbody>();

                pickupImage.enabled = false;
                pickupText.enabled = false;
            }
        }

        if(other.tag == "Hideout")
        {
            pickupImage.enabled = true;
            pickupText.enabled = true;
            pickupText.text = "Nasconditi";

            if (Input.GetKey(KeyCode.E))
            {
                hiding = true;

                exitHideout.position = playerPosition.position;
                playerPosition.position = other.transform.position;
                playerCollider.enabled=false;
                rb.useGravity = false;
                movementSpeed = 0;

                pickupImage.enabled = false;
                pickupText.enabled = false;
            }
        }

        if(other.tag == "ExitMission")
        {
            pickupImage.enabled = true;
            pickupText.enabled = true;
            pickupText.text = "Esci dalla casa";
            if (Input.GetKey(KeyCode.E))
            {
                pickupImage.enabled = false;
                pickupText.enabled = false;

                movementSpeed = 0;
                UnityEngine.Cursor.lockState = CursorLockMode.None;
                UnityEngine.Cursor.visible = true;

                gameOverText.enabled = true;
                gameOverText.text = "Sei Fuggito col il tuo Obbiettivo";

                returnButton.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Package")
        {
            pickupImage.enabled = false;
            pickupText.enabled = false;
  
        }

        if (other.tag == "Hideout")
        {
            pickupImage.enabled = false;
            pickupText.enabled = false;
  
        }

        if(other.tag == "ExitMission")
        {
            pickupImage.enabled = false;
            pickupText.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "HazardArea" && crouching == false) 
        {
            noiseMeter += 3;
        }
    }
}
