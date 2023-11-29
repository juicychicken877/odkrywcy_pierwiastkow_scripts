using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalUIScript : MonoBehaviour
{
    [SerializeField] private GameObject lifeOfMariaTab;
    [SerializeField] private GameObject informationAboutLifeBox;
    [SerializeField] private ScrollRect scrollRect;

    [SerializeField] private GameObject journalUI;
    private GameObject openedTab;
    private InputSystemScript inputSystemScript;
    private UIManagerScript UIManagerScript;

    private void Start()
    {
        inputSystemScript = InputSystemScript.Instance;
        UIManagerScript = UIManagerScript.Instance;

        inputSystemScript.OnUIJournalOn += InputSystemScript_OnUIJournalOn;

    }

    private void InputSystemScript_OnUIJournalOn(object sender, System.EventArgs e)
    {
        if (UIManagerScript.GetPermissionToChangeMember(gameObject))
            ChangeMenuActive();
        else
            UIManagerScript.GetComponent<MessageUIScript>().Message("Cant open journal because something is already opened!");
    }

    public void ChangeMenuActive()
    {
        if (journalUI.activeInHierarchy)
        {
            journalUI.SetActive(false);
        }
        else
        {
            journalUI.SetActive(true);
        }
    }

    public void AddPieceOfLife(string header, string description)
    {
        GameObject obj = Instantiate(informationAboutLifeBox, lifeOfMariaTab.transform.position, Quaternion.Euler(0, 0, 0), lifeOfMariaTab.transform);

        MariaLifeInformationBoxScript script = obj.GetComponent<MariaLifeInformationBoxScript>();
        
        // set text
        script.SetHeaderText(header);
        script.SetDescriptionText(description);

    }

    // button onclick
    public void OpenTab(GameObject obj)
    {
        ClosePreviousTab();

        obj.SetActive(true);

        scrollRect.content = obj.GetComponent<RectTransform>();
        scrollRect.verticalNormalizedPosition = 1.0f;

        openedTab = obj;
    }

    private void ClosePreviousTab()
    {
        if (openedTab != null)
        {
            openedTab.SetActive(false);
            openedTab = null;
        }
    }
}
