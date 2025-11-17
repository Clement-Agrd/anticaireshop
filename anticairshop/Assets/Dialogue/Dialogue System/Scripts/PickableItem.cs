using HeneGames.DialogueSystem;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class PickableItem : MonoBehaviour
{
    [Header("Item settings")]
    public string itemName = "Objet";
    public int requiredItemCount = 0;

    [Header("UI Messages")]
    public string availableMessage = "Ramasser l'objet";
    public string missingRequirementMessage = "Un autre objet est nécessaire";

    [Header("Visuals")]
    public Color targetColor = Color.yellow;
    public float pickupRange = 3f; // distance max pour interagir

    private Renderer rend;
    private Color originColor;
    private bool over = false;

    private Interact interactUI;
    private GameObject mainCam;

    void Start()
    {
        mainCam = GameObject.FindWithTag("MainCamera");
        if (mainCam == null)
            mainCam = FindObjectOfType<Camera>().gameObject;

        interactUI = mainCam.GetComponent<Interact>();
        rend = GetComponent<Renderer>();
        originColor = rend.material.color;
    }

    // Appelé par Interact.cs
    public void Hovering(Vector3 hitPoint)
    {
        // Vérifie la distance pour être sûr
        float dist = Vector3.Distance(mainCam.transform.position, transform.position);
        if (dist <= pickupRange)
        {
            over = true;
            interactUI.message = (DialogueManager.collectedItems >= requiredItemCount) 
                ? availableMessage 
                : missingRequirementMessage;
        }
        else
        {
            over = false;
        }
    }

    public void UnHover()
    {
        over = false;
    }

    void FixedUpdate()
    {
        // Changement de couleur
        rend.material.color = Color.Lerp(rend.material.color, over ? targetColor : originColor, Time.deltaTime * 4);

        // Vérifie si on est toujours à portée
        float dist = Vector3.Distance(mainCam.transform.position, transform.position);
        if (dist > pickupRange) over = false;

        // Ramassage si hover + bouton + condition
        if (over && Input.GetButton("Interact"))
        {
            if (DialogueManager.collectedItems >= requiredItemCount)
            {
                DialogueManager.collectedItems++;
                Debug.Log("Objet ramassé : " + itemName + " | Total = " + DialogueManager.collectedItems);
                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Pas assez d’objets pour ramasser " + itemName);
            }
        }
    }
}
