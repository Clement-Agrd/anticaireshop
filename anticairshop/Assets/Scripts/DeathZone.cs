using System;
using System.Collections;
using DefaultNamespace;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private CheckPoint checkPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(TeleportPlayer(other.gameObject));
        }
    }

    IEnumerator TeleportPlayer(GameObject player)
    {
        player.GetComponent<PlayerMovement>().enabled = false;
        player.transform.position = checkPoint.gameObject.transform.position;
        player.GetComponent<Teleportation>().basemap = checkPoint.baseMap;
        yield return new WaitForSeconds(1f);
        player.GetComponent<PlayerMovement>().enabled = true;
    }

    public void SetCheckPoint(CheckPoint checkPoint)
    {
        this.checkPoint = checkPoint;
    }
}
