using UnityEngine;

public class Teleportation : MonoBehaviour
{
    public GameObject player;
    public GameObject ref1;
    public GameObject ref2;
    public bool basemap;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player.transform.position = ref1.transform.position;
        basemap = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            GetComponent<CharacterController>().enabled = false;
            Teleport();
            transform.position += Vector3.up * 0.5f;
            GetComponent<CharacterController>().enabled = true;
        }
    }

    public void Teleport()
    {
        if (basemap)
        {
            player.transform.position =  player.transform.position - (ref1.transform.position - ref2.transform.position);
            basemap  = false;
        }
        else
        {
            player.transform.position = player.transform.position + (ref1.transform.position - ref2.transform.position);
            basemap = true;
        }
    }
}
