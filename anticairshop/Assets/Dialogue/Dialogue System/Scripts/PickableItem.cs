using HeneGames.DialogueSystem;
using UnityEngine;

public class PickableItem : MonoBehaviour
{
    [Header("Item settings")]
    public string itemName = "Objet";
    public int requiredItemCount = 0;

    [Header("UI Messages")]
    public string availableMessage = "Ramasser l'objet";
    public string missingRequirementMessage = "Un autre objet est nécessaire";

    private Interact interactUI;
    private GameObject mainCam;

    private bool canHover = false;
    private bool insideRange = false;

    void Start()
    {
        mainCam = GameObject.FindWithTag("MainCamera");

        if (mainCam == null)
            mainCam = FindObjectOfType<Camera>().gameObject;

        interactUI = mainCam.GetComponent<Interact>();
    }

    // === IMPORTANT ===
    // Interact.cs appelle Hovering(Vector3 hitPoint)
    public void Hovering(Vector3 hitPoint)
    {
        insideRange = true;

        // Si le joueur a assez d’objets → message normal
        if (DialogueManager.collectedItems >= requiredItemCount)
            interactUI.message = availableMessage;
        else
            interactUI.message = missingRequirementMessage;
    }

    // Interact.cs appelle Interacting() quand le joueur appuie sur le bouton
    public void Interacting()
    {
        // Vérifie si le joueur est bien en train de regarder l’objet
        if (!insideRange) return;

        // Check condition
        if (DialogueManager.collectedItems < requiredItemCount)
        {
            Debug.Log("Pas assez d’objets pour ramasser " + itemName);
            return;
        }

        // Ajoute un item
        DialogueManager.collectedItems++;
        Debug.Log("Objet ramassé : " + itemName + " | Total = " + DialogueManager.collectedItems);

        // Désactive l’objet
        gameObject.SetActive(false);
    }

    // Reset (appelé en fin de frame par Interact.cs quand on ne regarde plus l’objet)
    public void UnHover()
    {
        insideRange = false;
    }
}