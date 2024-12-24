using UnityEngine;
using UnityEngine.UI;

public class CollectorPackage : MonoBehaviour
{

    [SerializeField] int packageStealed = 0;
    [SerializeField] Text text;
    [SerializeField] int score = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Package") 
        {
            packageStealed++;
            text.text = "Pacchi Rubati: "+packageStealed;
            score += 100;
            Destroy(other.gameObject);
        }
    }
}
