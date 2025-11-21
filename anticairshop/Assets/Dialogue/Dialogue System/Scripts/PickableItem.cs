
using HeneGames.DialogueSystem;
using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(Renderer))]
public class PickableItem : MonoBehaviour
{
    [Header("Item settings")]
    public string itemName = "Objet";
    public int requiredItemCount = 0;

    [Header("UI Messages")]
    public string availableMessage = "examiner l'objet";
    public string firstMessage = "tu examine l'objet";
    public string missingRequirementMessage = "Un autre objet est nécessaire";
    public string alreadyPickedMessage = "Cet objet a déjà été examiné";

    [Header("Visuals")]
    public Color targetColor = Color.yellow;
    public float pickupRange = 3f;

    [Header("UI")]
    public GameObject uiPanel;
    public TMP_Text uiText;

    private Renderer rend;
    private Color originColor;
    private bool over = false;
    private bool hasBeenPicked = false;
    private bool canInteract = true; // ✅ Nouveau flag
    private Interact interactUI;
    private GameObject mainCam;
    private Coroutine hideCoroutine;

    void Start()
    {
        mainCam = GameObject.FindWithTag("MainCamera");
        if (mainCam == null)
            mainCam = FindObjectOfType<Camera>().gameObject;

        interactUI = mainCam.GetComponent<Interact>();
        rend = GetComponent<Renderer>();
        originColor = rend.material.color;
        

        if (uiPanel != null)
            uiPanel.SetActive(false);
    }

    public void Hovering(Vector3 hitPoint)
    {
        float dist = Vector3.Distance(mainCam.transform.position, transform.position);
        if (dist <= pickupRange)
        {
            over = true;
            if (!hasBeenPicked)
            {
                interactUI.message = (DialogueManager.collectedItems >= requiredItemCount)
                    ? availableMessage
                    : missingRequirementMessage;
            }
            else
            {
                interactUI.message = alreadyPickedMessage;
            }
        }
        else
        {
            over = false;
            HideUI();
        }
    }

    public void UnHover()
    {
        over = false;
        HideUI();
    }

    void FixedUpdate()
    {
        rend.material.color = Color.Lerp(rend.material.color, over ? targetColor : originColor, Time.deltaTime * 4);

        float dist = Vector3.Distance(mainCam.transform.position, transform.position);
        if (dist > pickupRange)
        {
            over = false;
        }
        else
        {
            over = true;
        }

        if (over && Input.GetButton("Interact") && canInteract) // ✅ Vérifie canInteract
        {
            Debug.Log("Interact");
            if (!hasBeenPicked && !uiPanel.activeInHierarchy)
            {
                if (DialogueManager.collectedItems >= requiredItemCount)
                {
                    DialogueManager.collectedItems++;
                    hasBeenPicked = true;
                    uiPanel.SetActive(true);
                    ShowUI(firstMessage, 2f);
                }
                else
                {
                    uiPanel.SetActive(true);
                    ShowUI(missingRequirementMessage, 2f);
                }
            }
            else
            {
                uiPanel.SetActive(true);
                ShowUI(alreadyPickedMessage, 2f);
            }

            // ✅ Bloque l'input pendant 2 secondes
            StartCoroutine(BlockInput(2f));
        }
    }

    void ShowUI(string message, float delay = 0f)
    {
        Debug.Log("ui where");
        if (uiText != null)
            uiText.text = message;
        if (uiPanel != null)
            uiPanel.SetActive(true);

        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        if (delay > 0f)
            hideCoroutine = StartCoroutine(HideAfterDelay(delay));
    }

    void HideUI()
    {
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        if (uiText != null)
            uiText.text = "";
        if (uiPanel != null)
            uiPanel.SetActive(false);
    }

    IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideUI();
    }

    IEnumerator BlockInput(float delay)
    {
        canInteract = false;
        yield return new WaitForSeconds(delay);
        canInteract = true;
    }
}
