using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Creates a UI Canvas with controls for the deer in the GraphicsTest scene
/// </summary>
public class UISetupScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DeerAnimationAndSoundController _deerController;
    
    // UI References
    private Canvas _canvas;
    private CanvasScaler _canvasScaler;
    private DeerUIController _deerUIController;
    
    // Button references
    private Button _idleButton;
    private Button _runButton;
    private Button _jumpButton;
    private Button _dieButton;
    private Button _antidoteButton;
    private Slider _runSpeedSlider;
    private Text _runSpeedText;

    private void Awake()
    {
        // Find the deer controller if not assigned
        if (_deerController == null)
        {
            _deerController = FindObjectOfType<DeerAnimationAndSoundController>();
            
            if (_deerController == null)
            {
                GameObject deerObject = GameObject.Find("Pudu");
                if (deerObject != null)
                {
                    // Add the controller component
                    _deerController = deerObject.AddComponent<DeerAnimationAndSoundController>();
                    Debug.Log("Added DeerAnimationAndSoundController to Pudu");
                }
                else
                {
                    Debug.LogError("Could not find Pudu GameObject in the scene!");
                    return;
                }
            }
        }
        
        // Create the Canvas and UI elements
        SetupCanvas();
        SetupUIElements();
        
        // Connect the UI to the controller
        SetupDeerUIController();
    }
    
    private void SetupCanvas()
    {
        // Create Canvas GameObject
        GameObject canvasObject = new GameObject("DeerControlCanvas");
        _canvas = canvasObject.AddComponent<Canvas>();
        _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        
        // Add Canvas Scaler
        _canvasScaler = canvasObject.AddComponent<CanvasScaler>();
        _canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        _canvasScaler.referenceResolution = new Vector2(1920, 1080);
        _canvasScaler.matchWidthOrHeight = 0.5f;
        
        // Add Graphic Raycaster
        canvasObject.AddComponent<GraphicRaycaster>();
        
        // Add EventSystem if not already in scene
        if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }
    }
    
    private void SetupUIElements()
    {
        // Create panel background
        GameObject panelObject = CreateUIElement("ControlPanel", _canvas.transform);
        RectTransform panelRect = panelObject.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0, 0);
        panelRect.anchorMax = new Vector2(1, 0.3f);
        panelRect.offsetMin = new Vector2(20, 20);
        panelRect.offsetMax = new Vector2(-20, -20);
        
        Image panelImage = panelObject.AddComponent<Image>();
        panelImage.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
        
        // Create button container
        GameObject buttonContainer = CreateUIElement("ButtonContainer", panelRect);
        HorizontalLayoutGroup layout = buttonContainer.AddComponent<HorizontalLayoutGroup>();
        layout.spacing = 20;
        layout.padding = new RectOffset(20, 20, 20, 20);
        layout.childAlignment = TextAnchor.MiddleCenter;
        
        RectTransform buttonRect = buttonContainer.GetComponent<RectTransform>();
        buttonRect.anchorMin = new Vector2(0, 0);
        buttonRect.anchorMax = new Vector2(1, 1);
        buttonRect.offsetMin = Vector2.zero;
        buttonRect.offsetMax = Vector2.zero;
        
        // Create buttons
        _idleButton = CreateButton("IdleButton", "Idle", buttonRect);
        _runButton = CreateButton("RunButton", "Run", buttonRect);
        _jumpButton = CreateButton("JumpButton", "Jump", buttonRect);
        _dieButton = CreateButton("DieButton", "Die", buttonRect);
        _antidoteButton = CreateButton("AntidoteButton", "Antidote", buttonRect);
        
        // Create slider for run speed
        GameObject sliderContainer = CreateUIElement("SliderContainer", panelRect);
        RectTransform sliderRect = sliderContainer.GetComponent<RectTransform>();
        sliderRect.anchorMin = new Vector2(0, 0);
        sliderRect.anchorMax = new Vector2(1, 0.3f);
        sliderRect.offsetMin = new Vector2(20, 10);
        sliderRect.offsetMax = new Vector2(-20, 10);
        
        // Add label for slider
        GameObject labelObject = CreateUIElement("SpeedLabel", sliderRect);
        _runSpeedText = labelObject.AddComponent<Text>();
        _runSpeedText.font = Font.CreateDynamicFontFromOSFont("Arial", 16);
        _runSpeedText.fontSize = 16;
        _runSpeedText.alignment = TextAnchor.MiddleCenter;
        _runSpeedText.text = "Speed: 1.0";
        _runSpeedText.color = Color.white;
        
        RectTransform labelRect = labelObject.GetComponent<RectTransform>();
        labelRect.anchorMin = new Vector2(0, 0);
        labelRect.anchorMax = new Vector2(0.2f, 1);
        labelRect.offsetMin = Vector2.zero;
        labelRect.offsetMax = Vector2.zero;
        
        // Create the slider
        GameObject sliderObject = CreateUIElement("RunSpeedSlider", sliderRect);
        _runSpeedSlider = sliderObject.AddComponent<Slider>();
        _runSpeedSlider.minValue = 0.5f;
        _runSpeedSlider.maxValue = 2.0f;
        _runSpeedSlider.value = 1.0f;
        
        RectTransform sliderObjRect = sliderObject.GetComponent<RectTransform>();
        sliderObjRect.anchorMin = new Vector2(0.25f, 0);
        sliderObjRect.anchorMax = new Vector2(0.95f, 1);
        sliderObjRect.offsetMin = Vector2.zero;
        sliderObjRect.offsetMax = Vector2.zero;
        
        // Create slider components
        GameObject backgroundObj = CreateUIElement("Background", sliderObjRect);
        Image bgImage = backgroundObj.AddComponent<Image>();
        bgImage.color = new Color(0.1f, 0.1f, 0.1f, 1f);
        RectTransform bgRect = backgroundObj.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;
        
        // Create fill area
        GameObject fillAreaObj = CreateUIElement("FillArea", sliderObjRect);
        RectTransform fillAreaRect = fillAreaObj.GetComponent<RectTransform>();
        fillAreaRect.anchorMin = new Vector2(0, 0.25f);
        fillAreaRect.anchorMax = new Vector2(1, 0.75f);
        fillAreaRect.offsetMin = new Vector2(5, 0);
        fillAreaRect.offsetMax = new Vector2(-5, 0);
        
        // Create fill
        GameObject fillObj = CreateUIElement("Fill", fillAreaRect);
        Image fillImage = fillObj.AddComponent<Image>();
        fillImage.color = new Color(0.3f, 0.8f, 0.3f, 1f);
        RectTransform fillRect = fillObj.GetComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = new Vector2(0.5f, 1);
        fillRect.offsetMin = Vector2.zero;
        fillRect.offsetMax = Vector2.zero;
        
        // Create handle
        GameObject handleAreaObj = CreateUIElement("HandleArea", sliderObjRect);
        RectTransform handleAreaRect = handleAreaObj.GetComponent<RectTransform>();
        handleAreaRect.anchorMin = Vector2.zero;
        handleAreaRect.anchorMax = Vector2.one;
        handleAreaRect.offsetMin = Vector2.zero;
        handleAreaRect.offsetMax = Vector2.zero;
        
        GameObject handleObj = CreateUIElement("Handle", handleAreaRect);
        Image handleImage = handleObj.AddComponent<Image>();
        handleImage.color = new Color(0.9f, 0.9f, 0.9f, 1f);
        RectTransform handleRect = handleObj.GetComponent<RectTransform>();
        handleRect.anchorMin = new Vector2(0.5f, 0);
        handleRect.anchorMax = new Vector2(0.5f, 1);
        handleRect.offsetMin = new Vector2(-10, -5);
        handleRect.offsetMax = new Vector2(10, 5);
        
        // Setup slider references
        _runSpeedSlider.fillRect = fillRect;
        _runSpeedSlider.handleRect = handleRect;
        _runSpeedSlider.targetGraphic = handleImage;
        _runSpeedSlider.direction = Slider.Direction.LeftToRight;
    }
    
    private void SetupDeerUIController()
    {
        // Add controller to the canvas
        _deerUIController = _canvas.gameObject.AddComponent<DeerUIController>();
        
        // Connect references
        _deerUIController._deerController = _deerController;
        _deerUIController._idleButton = _idleButton;
        _deerUIController._runButton = _runButton;
        _deerUIController._jumpButton = _jumpButton;
        _deerUIController._dieButton = _dieButton;
        _deerUIController._antidoteButton = _antidoteButton;
        _deerUIController._runSpeedSlider = _runSpeedSlider;
        _deerUIController._runSpeedText = _runSpeedText;
        _deerUIController._minRunSpeed = 0.5f;
        _deerUIController._maxRunSpeed = 2.0f;
    }
    
    private GameObject CreateUIElement(string name, Transform parent)
    {
        GameObject element = new GameObject(name);
        element.AddComponent<RectTransform>();
        element.transform.SetParent(parent, false);
        return element;
    }
    
    private Button CreateButton(string name, string text, Transform parent)
    {
        // Create button object
        GameObject buttonObject = CreateUIElement(name, parent);
        Button button = buttonObject.AddComponent<Button>();
        
        // Add button image
        Image image = buttonObject.AddComponent<Image>();
        image.color = new Color(0.3f, 0.3f, 0.3f, 1f);
        
        // Set up button display
        button.targetGraphic = image;
        ColorBlock colors = button.colors;
        colors.normalColor = new Color(0.3f, 0.3f, 0.3f, 1f);
        colors.highlightedColor = new Color(0.4f, 0.4f, 0.4f, 1f);
        colors.pressedColor = new Color(0.2f, 0.2f, 0.2f, 1f);
        colors.selectedColor = new Color(0.4f, 0.4f, 0.4f, 1f);
        button.colors = colors;
        
        // Create text for button
        GameObject textObject = CreateUIElement("Text", buttonObject.transform);
        Text buttonText = textObject.AddComponent<Text>();
        buttonText.text = text;
        buttonText.font = Font.CreateDynamicFontFromOSFont("Arial", 16);
        buttonText.color = Color.white;
        buttonText.alignment = TextAnchor.MiddleCenter;
        buttonText.fontSize = 16;
        
        // Set text to fill button
        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = new Vector2(10, 10);
        textRect.offsetMax = new Vector2(-10, -10);
        
        // Size the button
        RectTransform buttonRect = buttonObject.GetComponent<RectTransform>();
        buttonRect.sizeDelta = new Vector2(100, 50);
        
        return button;
    }
} 