using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;
    public TMP_Text notificationText;
    public GameObject notificationPanel;
    public ScrollRect scrollRect;
    public TMP_InputField inputField;
    public int maxMessages = 100;
    public GameObject notificationLinePrefab;
    public Transform contentParent;

    [SerializeField] private CanvasGroup panelCanvasGroup;
    [SerializeField, Range(0f, 1f)] private float fadedAlpha = 0.8f; // 80% synlig
    [SerializeField] private float fadeDelay = 2f;
    [SerializeField] private GameObject viewportObject;

    private List<string> messages = new List<string>();
    private bool chatOpen = false;
    private float lastActiveTime = 0f;
    private bool isFaded = false;
    private bool userScrolled = false;
    private bool mouseOverPanel = false;

    private void Awake()
    {
        Instance = this;
        if (notificationPanel != null)
            notificationPanel.SetActive(true); // Panelen är alltid aktiv
        if (panelCanvasGroup != null)
            panelCanvasGroup.alpha = 1f;
        lastActiveTime = Time.time;
        if (scrollRect != null)
            scrollRect.onValueChanged.AddListener(OnScrollValueChanged);
        if (inputField != null)
            inputField.onEndEdit.AddListener(OnInputEndEdit);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            Debug.Log("Enter pressed!");
        // Öppna chatten med Enter om den är stängd
        if (!chatOpen && Input.GetKeyDown(KeyCode.Return))
        {
            OpenChat();
            return;
        }

        // Stäng chatten med Enter om den är öppen och inputfältet INTE är aktivt
        if (chatOpen && !inputField.isFocused && Input.GetKeyDown(KeyCode.Return))
        {
            CloseChat();
            return;
        }

        // Stäng chatten om man klickar utanför notificationPanel
        if (chatOpen && Input.GetMouseButtonDown(0))
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(
                notificationPanel.GetComponent<RectTransform>(),
                Input.mousePosition,
                null))
            {
                CloseChat();
                return;
            }
        }

        // Kolla om musen är över Viewport istället för NotificationPanel
        if (viewportObject != null)
        {
            bool over = RectTransformUtility.RectangleContainsScreenPoint(
                viewportObject.GetComponent<RectTransform>(),
                Input.mousePosition,
                null);
            mouseOverPanel = over;
        }
        else
        {
            mouseOverPanel = false;
        }

        // Om chatten är öppen, inputfältet är aktivt, eller musen är över panelen, håll panelen helt synlig
        if (chatOpen || (inputField != null && inputField.isFocused) || mouseOverPanel)
        {
            if (panelCanvasGroup != null)
                panelCanvasGroup.alpha = 1f;
            lastActiveTime = Time.time;
            isFaded = false;
            return;
        }

        // Om det gått fadeDelay sekunder utan interaktion, gör panelen halvgenomskinlig
        if (!isFaded && Time.time - lastActiveTime > fadeDelay)
        {
            if (panelCanvasGroup != null)
                panelCanvasGroup.alpha = fadedAlpha;
            isFaded = true;
        }
    }

    public void ShowNotification(string message)
    {
        if (notificationLinePrefab == null || contentParent == null) return;
        notificationPanel.SetActive(true);
        GameObject line = Instantiate(notificationLinePrefab, contentParent);
        var tmp = line.GetComponent<TMPro.TMP_Text>();
        if (tmp != null) tmp.text = message;
        if (contentParent.childCount > maxMessages)
            Destroy(contentParent.GetChild(0).gameObject);
        if (scrollRect != null && !userScrolled)
        {
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0f;
        }
        if (panelCanvasGroup != null)
            panelCanvasGroup.alpha = 1f;
        lastActiveTime = Time.time;
        isFaded = false;
    }

    private void OnInputEndEdit(string input)
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                ShowNotification($"<b>Du:</b> {input}");
            }
            inputField.text = "";
            inputField.ActivateInputField(); // Håll fokus kvar
        }
    }

    public void OpenChat()
    {
        notificationPanel.SetActive(true);
        chatOpen = true;
        inputField.gameObject.SetActive(true);
        inputField.text = "";
        inputField.ActivateInputField();
        if (panelCanvasGroup != null)
            panelCanvasGroup.alpha = 1f;
        lastActiveTime = Time.time;
        isFaded = false;
    }

    public void CloseChat()
    {
        // notificationPanel.SetActive(false); // Dölj inte panelen
        chatOpen = false;
        inputField.gameObject.SetActive(false);
        if (panelCanvasGroup != null)
            panelCanvasGroup.alpha = fadedAlpha; // Gör panelen direkt halvgenomskinlig
        lastActiveTime = Time.time;
        isFaded = true;
    }

    private void OnScrollValueChanged(Vector2 value)
    {
        // Om användaren inte är längst ner, sätt flaggan
        userScrolled = scrollRect.verticalNormalizedPosition > 0.01f;
    }

    public void ScrollToBottom()
    {
        if (scrollRect != null)
        {
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0f;
            userScrolled = false;
        }
    }
} 