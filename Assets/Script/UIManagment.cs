using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UIManagment : MonoBehaviour
{
    [SerializeField] RawImage controls;
    [SerializeField] RawImage logo;
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject player;
    [SerializeField] GameObject playerCamera;
    [SerializeField] GameObject menuCamera;
    [SerializeField] GameObject playButton;
    [SerializeField] GameObject exitButton;
    [SerializeField] GameObject controlsButton;
    [SerializeField] GameObject returnButton;
    [SerializeField] Text noiseText;
    [SerializeField] RawImage backgroundMeter; 
    [SerializeField] RawImage meter;
    [SerializeField] Text packageText;
    [SerializeField] Text gameOvertext;
    [SerializeField] VideoPlayer video;

    [SerializeField] GameObject giftA;
    [SerializeField] GameObject giftB;
    [SerializeField] GameObject giftC;
    [SerializeField] GameObject giftD;
    [SerializeField] int version;

    public void Play()
    {
        version = Random.Range(1,2);
        switch(version){
            case 1:
                giftA.transform.position = new Vector3(-1.71936798f,0,-4.74700022f);
                giftB.transform.position = new Vector3(1.87f,0.200000003f,21.6299992f);
                giftD.transform.position = new Vector3(15.2299995f,1.53999996f,12.6400003f);
                giftC.transform.position = new Vector3(10.3126087f,0.172999993f,-2.39256787f);
                break;
            
            case 2:
                giftA.transform.position = new Vector3(-8.32999992f,0,-3.74000001f);
                giftB.transform.position = new Vector3(15.1300001f,3.27999997f,18.9899998f);
                giftD.transform.position = new Vector3(-4.44000006f,1.65999997f,16.9699993f);
                giftC.transform.position = new Vector3(2.27999997f,0.172999993f,15.3999996f);
                break;
        }
        logo.enabled= false;
        playButton.SetActive(false);
        exitButton.SetActive(false);
        controlsButton.SetActive(false);
        video.Play();
        player.transform.position = new Vector3(4.5f,1.25f,-2);
        enemy.transform.position = new Vector3(0f, 5.5f, 0f);
        player.GetComponent<Player>().movementSpeed = 300f;
    }
    public void Update()
    {
        if (video.time >= 138)
        {
            video.Stop();
            playerCamera.SetActive(true);
            menuCamera.SetActive(false);

            backgroundMeter.enabled = true;
            meter.enabled = true;
            noiseText.enabled = true;
            packageText.enabled = true;
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Controls() 
    {
        controls.enabled= true;
        playButton.SetActive(false);
        exitButton.SetActive(false);
        controlsButton.SetActive(false);
        returnButton.SetActive(true);
        //metti la schermata di comandi
    }

    public void Return() 
    {
        controls.enabled = false;
        logo.enabled = true;
        gameOvertext.enabled = false;
        returnButton.SetActive(false);

        playerCamera.SetActive(false);
        menuCamera.SetActive(true);

        backgroundMeter.enabled = false;
        meter.enabled = false;
        noiseText.enabled = false;
        packageText.enabled = false;

        playButton.SetActive(true);
        exitButton.SetActive(true);
        controlsButton.SetActive(true);

    }
}
