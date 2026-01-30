using System.Collections.Generic;
using UnityEditor;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Templates.AR;
using Assets.MobileARTemplateAssets.Scripts;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

namespace UnityEngine.XR.Templates.AR
{
    /// <summary>
    /// Handles dismissing the object menu when clicking out the UI bounds, and showing the
    /// menu again when the create menu button is clicked after dismissal. Manages object deletion in the AR demo scene,
    /// and also handles the toggling between the object creation menu button and the delete button.
    /// </summary>
    public class ARTemplateMenuManager : MonoBehaviour
    {
        
        [SerializeField]
        Button m_ButtonZone;

        


        //private Script m_BaguaLogicScript;
        /// <summary>
        /// Button that opens the create menu.
        /// </summary>
        
      
        public Button ButtonZone
        {
            get => m_ButtonZone;
            set => m_ButtonZone = value;
        }

        [SerializeField]
        Button m_ButtonObject;

        public Button ButtonObject
        {
            get => m_ButtonObject;
            set => m_ButtonObject = value;
        }


        [SerializeField]
        [Tooltip("Button that deletes a selected object.")]
        Button m_DeleteButton;

        /// <summary>
        /// Button that deletes a selected object.
        /// </summary>
        public Button DeleteButton
        {
            get => m_DeleteButton;
            set => m_DeleteButton = value;
        }

        [SerializeField]
        [Tooltip("The menu with all the creatable objects.")]
        GameObject m_ObjectMenu;
        

        /// <summary>
        /// The menu with all the creatable objects.
        /// </summary>
        public GameObject ObjectMenu
        {
            get => m_ObjectMenu;
            set => m_ObjectMenu = value;
        }

        [SerializeField]
        GameObject m_ObjectMenuZone;

        public GameObject ObjectMenuZone
        {
            get => m_ObjectMenuZone;
            set => m_ObjectMenuZone = value;
        }

        [SerializeField]
        [Tooltip("The modal with debug options.")]
        GameObject m_ModalMenu;

        [SerializeField]
        GameObject m_ModalMenuBagua;


        [SerializeField]
        GameObject m_ModalMenuObject;
        /// <summary>
        /// The modal with debug options.
        /// </summary>
        public GameObject ModalMenu
        {
            get => m_ModalMenu;
            set => m_ModalMenu = value;
        }

        public GameObject ModalMenuBagua
        {
            get => m_ModalMenuBagua;
            set => m_ModalMenuBagua = value;
        }

        public GameObject ModalMenuObject
        {
            get => m_ModalMenuObject;
            set => m_ModalMenuObject = value;
        }


        [SerializeField]
        [Tooltip("The animator for the object creation menu.")]
        Animator m_ObjectMenuAnimator;
        [SerializeField]
        Animator m_ObjectMenuAnimatorZone;
        /// <summary>
        /// The animator for the object creation menu.
        /// </summary>
        public Animator ObjectMenuAnimator
        {
            get => m_ObjectMenuAnimator;
            set => m_ObjectMenuAnimator = value;
        }
        public Animator ObjectMenuAnimatorZone
        {
            get => m_ObjectMenuAnimatorZone;
            set => m_ObjectMenuAnimatorZone = value;
        }


        [SerializeField]
        [Tooltip("The object spawner component in charge of spawning new objects.")]
        ObjectSpawner m_ObjectSpawner;

        /// <summary>
        /// The object spawner component in charge of spawning new objects.
        /// </summary>
        public ObjectSpawner ObjectSpawner
        {
            get => m_ObjectSpawner;
            set => m_ObjectSpawner = value;
        }

        [SerializeField]
        [Tooltip("Button that closes the object creation menu.")]
        Button m_CancelButton;
        [SerializeField]
        Button m_CancelButtonZone;

        /// <summary>
        /// Button that closes the object creation menu.
        /// </summary>
        public Button CancelButton
        {
            get => m_CancelButton;
            set => m_CancelButton = value;
        }
        public Button CancelButtonZone
        {
            get => m_CancelButtonZone;
            set => m_CancelButtonZone = value;
        }

        [SerializeField]
        Button m_OptionsButtonBagua;

        public Button OptionsButtonBagua
        {
            get => m_OptionsButtonBagua;
            set => m_OptionsButtonBagua = value;
        }

        [SerializeField]
        Button m_OptionsButtonObject;

        public Button OptionsButtonObject
        {
            get => m_OptionsButtonObject;
            set => m_OptionsButtonObject = value;
        }

        [SerializeField]
        [Tooltip("The interaction group for the AR demo scene.")]
        XRInteractionGroup m_InteractionGroup;

        /// <summary>
        /// The interaction group for the AR demo scene.
        /// </summary>
        public XRInteractionGroup interactionGroup
        {
            get => m_InteractionGroup;
            set => m_InteractionGroup = value;
        }

        [SerializeField]
        [Tooltip("The slider for activating plane debug visuals.")]
        DebugSlider m_DebugPlaneSlider;

        [SerializeField]

        DebugSlider m_BaguaSlider;

        /// <summary>
        /// The slider for activating plane debug visuals.
        /// </summary>
        public DebugSlider debugPlaneSlider
        {
            get => m_DebugPlaneSlider;
            set => m_DebugPlaneSlider = value;
        }

        public DebugSlider BaguaSlider
        {
            get => m_BaguaSlider;
            set => m_BaguaSlider = value;
        }

        [SerializeField]
        [Tooltip("The plane manager in the AR demo scene.")]
        ARPlaneManager m_PlaneManager;

        /// <summary>
        /// The plane manager in the AR demo scene.
        /// </summary>
        public ARPlaneManager planeManager
        {
            get => m_PlaneManager;
            set => m_PlaneManager = value;
        }

        [SerializeField]
        [Tooltip("Determines whether or not to fade the AR Planes when visualization is toggled.")]
        bool m_UseARPlaneFading = true;

        /// <summary>
        /// Determines whether or not to fade the AR Planes when visualization is toggled.
        /// </summary>
        public bool useARPlaneFading
        {
            get => m_UseARPlaneFading;
            set => m_UseARPlaneFading = value;
        }

        [SerializeField]
        [Tooltip("The AR debug menu.")]
        ARDebugMenu m_ARDebugMenu;

        /// <summary>
        /// The AR debug menu.
        /// </summary>
        public ARDebugMenu arDebugMenu
        {
            get => m_ARDebugMenu;
            set => m_ARDebugMenu = value;
        }

        [SerializeField]
        [Tooltip("The slider for activating the debug menu.")]
        DebugSlider m_DebugMenuSlider;

        /// <summary>
        /// The slider for activating the debug menu.
        /// </summary>
        public DebugSlider debugMenuSlider
        {
            get => m_DebugMenuSlider;
            set => m_DebugMenuSlider = value;
        }

        [SerializeField]
        private BaguaPlacer script;

        public BaguaPlacer Script
        {
            get => script;
            set => script = value;
        }
        [SerializeField]
        XRInputValueReader<Vector2> m_TapStartPositionInput = new XRInputValueReader<Vector2>("Tap Start Position");

        /// <summary>
        /// Input to use for the screen tap start position.
        /// </summary>
        /// <seealso cref="TouchscreenGestureInputController.tapStartPosition"/>
        public XRInputValueReader<Vector2> tapStartPositionInput
        {
            get => m_TapStartPositionInput;
            set => XRInputReaderUtility.SetInputProperty(ref m_TapStartPositionInput, value, this);
        }

        [SerializeField]
        XRInputValueReader<Vector2> m_DragCurrentPositionInput = new XRInputValueReader<Vector2>("Drag Current Position");

        /// <summary>
        /// Input to use for the screen tap start position.
        /// </summary>
        /// <seealso cref="TouchscreenGestureInputController.dragCurrentPosition"/>
        public XRInputValueReader<Vector2> dragCurrentPositionInput
        {
            get => m_DragCurrentPositionInput;
            set => XRInputReaderUtility.SetInputProperty(ref m_DragCurrentPositionInput, value, this);
        }

        bool m_IsPointerOverUI;
        bool m_ShowObjectMenu;
        bool m_ShowObjectMenuZone;
        bool m_ShowOptionsModal;
        bool m_ShowOptionsModalBagua;
        bool m_ShowOptionsModalObject;

        bool m_VisualizePlanes = true;
        bool m_VisualizeBagua = false;
        bool m_ShowDebugMenu;
        bool m_InitializingDebugMenu;
        float m_DebugMenuPlanesButtonValue = 0f;
        Vector2 m_ObjectButtonOffset = Vector2.zero;
        Vector2 m_ObjectMenuOffset = Vector2.zero;
        readonly List<ARPlane> m_ARPlanes = new List<ARPlane>();
        readonly Dictionary<ARPlane, ARPlaneMeshVisualizer> m_ARPlaneMeshVisualizers = new Dictionary<ARPlane, ARPlaneMeshVisualizer>();
        readonly Dictionary<ARPlane, ARPlaneMeshVisualizerFader> m_ARPlaneMeshVisualizerFaders = new Dictionary<ARPlane, ARPlaneMeshVisualizerFader>();

        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        void OnEnable()
        {
            m_ButtonObject.onClick.AddListener(ShowMenuZone);
            m_ButtonZone.onClick.AddListener(ShowMenu); 
            m_CancelButton.onClick.AddListener(HideMenu);
            m_CancelButtonZone.onClick.AddListener(HideMenuZone);
            m_DeleteButton.onClick.AddListener(DeleteFocusedObject);
            m_PlaneManager.trackablesChanged.AddListener(OnPlaneChanged);
        }

        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        void OnDisable()
        {
            m_ShowObjectMenu = false;
            m_ShowObjectMenuZone = false;
            m_ButtonObject.onClick.RemoveListener(ShowMenu);
            m_CancelButton.onClick.RemoveListener(HideMenu);
            m_ButtonZone.onClick.RemoveListener(ShowMenu);
            m_CancelButtonZone.onClick.RemoveListener(HideMenu);
            m_DeleteButton.onClick.RemoveListener(DeleteFocusedObject);
            m_PlaneManager.trackablesChanged.RemoveListener(OnPlaneChanged);
        }

        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        void Start()
        {
            // Auto turn on/off debug menu. We want it initially active so it calls into 'Start', which will
            // allow us to move the menu properties later if the debug menu is turned on.
            if (m_ARDebugMenu != null)
            {
                m_ARDebugMenu.gameObject.SetActive(true);
                m_InitializingDebugMenu = true;

                InitializeDebugMenuOffsets();
            }

            HideMenu();

            m_DebugMenuSlider.value = m_ShowDebugMenu ? 1 : 0;
            m_DebugPlaneSlider.value = m_VisualizePlanes ? 1 : 0;
            m_BaguaSlider.value = m_VisualizeBagua ? 1 : 0; 
        }

        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        void Update()
        {
            m_IsPointerOverUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(-1);
            if (m_InitializingDebugMenu)
            {
                m_ARDebugMenu.gameObject.SetActive(false);
                m_InitializingDebugMenu = false;
            }

            if (m_ShowObjectMenu || m_ShowOptionsModal|| m_ShowOptionsModalBagua || m_ShowOptionsModalObject || m_ShowObjectMenuZone)
            {
                if (!m_IsPointerOverUI && (m_TapStartPositionInput.TryReadValue(out _) || m_DragCurrentPositionInput.TryReadValue(out _)))
                {
                    if (m_ShowObjectMenu)
                        HideMenu();

                    if (m_ShowObjectMenuZone)
                        HideMenu();

                    if (m_ShowOptionsModal)
                        m_ModalMenu.SetActive(false);

                    if (m_ShowOptionsModalBagua)
                        m_ModalMenuBagua.SetActive(false);

                    if (m_ShowOptionsModalObject)
                        m_ModalMenuObject.SetActive(false);
                }

                if (m_ShowObjectMenu)
                {
                    m_DeleteButton.gameObject.SetActive(false);
                }
                else
                {
                    m_DeleteButton.gameObject.SetActive(m_InteractionGroup?.focusInteractable != null);
                }

                m_IsPointerOverUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(-1);
            }
            else
            {
                //se non vengono visualizzati i men� resettiamo lo stato di isPointerOverUI e mostriamo i bottoni
                
                m_ButtonObject.gameObject.SetActive(true);
                m_ButtonZone.gameObject.SetActive(true);
                m_OptionsButtonBagua.gameObject.SetActive(true);
                m_OptionsButtonObject.gameObject.SetActive(true);
                m_DeleteButton.gameObject.SetActive(m_InteractionGroup?.focusInteractable != null);
            }

            
        }

        /// <summary>
        /// Set the index of the object in the list on the ObjectSpawner to a specific value.
        /// This is effectively an override of the default behavior or randomly spawning an object.
        /// </summary>
        /// <param name="objectIndex">The index in the array of the object to spawn with the ObjectSpawner</param>
        public void SetObjectToSpawn(int objectIndex)
        {
            if (m_ObjectSpawner == null)
            {
                Debug.LogWarning("Object Spawner not configured correctly: no ObjectSpawner set.");
            }
            else
            {
                if (m_ObjectSpawner.objectPrefabs.Count > objectIndex)
                {
                    m_ObjectSpawner.spawnOptionIndex = objectIndex;
                }
                else
                {
                    Debug.LogWarning("Object Spawner not configured correctly: object index larger than number of Object Prefabs.");
                }
            }

            HideMenu();
        }

        void ShowMenu()
        {
            m_ShowObjectMenu = true;
            m_ObjectMenu.SetActive(true);
          
            if (!m_ObjectMenuAnimator.GetBool("Show"))
            {
                m_ObjectMenuAnimator.SetBool("Show", true);
            }
           
            AdjustARDebugMenuPosition();
        }

        void ShowMenuZone()
        {
            m_ShowObjectMenuZone = true;
            m_ObjectMenuZone.SetActive(true);

            if (!m_ObjectMenuAnimatorZone.GetBool("Show"))
            {
                m_ObjectMenuAnimatorZone.SetBool("Show", true);
            }
            AdjustARDebugMenuPosition();

        }

       
        
        /// <summary>
        /// Shows or hides the menu modal when the options button is clicked.
        /// </summary>
        public void ShowHideModal()
        {
            if (m_ModalMenu.activeSelf)
            {
                m_ShowOptionsModal = false;
                m_ModalMenu.SetActive(false);
            }
            else
            {
                m_ShowOptionsModal = true;
                m_ModalMenu.SetActive(true);
            }
        }

        //metodo che gestisce la visualizzazione del menu bagua
        public void ShowHideModalBagua()
        {
            if (m_ModalMenuBagua.activeSelf)
            {
                m_ShowOptionsModalBagua = false;
                m_ModalMenuBagua.SetActive(false);
            }
            else
            {
                m_ShowOptionsModalBagua = true;
                m_ModalMenuBagua.SetActive(true);
            }
        }

        public void ShowHideModalObject()
        {
            if (m_ModalMenuObject.activeSelf)
            {
                m_ShowOptionsModalObject = false;
                m_ModalMenuObject.SetActive(false);
            }
            else
            {
                m_ShowOptionsModalObject = true;
                m_ModalMenuObject.SetActive(true);
            }
        }

        /// <summary>
        /// Shows or hides the plane debug visuals.
        /// </summary>
        public void ShowHideDebugPlane()
        {
            m_VisualizePlanes = !m_VisualizePlanes;
            m_DebugPlaneSlider.value = m_VisualizePlanes ? 1 : 0;
            ChangePlaneVisibility(m_VisualizePlanes);
        }

        //metodo per collegare lo slider del bottone alla visualizzazione del bagua
        public void ShowHideBagua()
        {
            m_VisualizeBagua = !m_VisualizeBagua;
            if (script == null)
            {
                Debug.LogError("riferimento allo script nullo");
                return;
            }
            m_BaguaSlider.value = m_VisualizeBagua ? 1 : 0;
            
            if (m_VisualizeBagua)
            {
                //attiva bagua
                script.applyBagua();
            }
            else
            {
                //disattiva bagua e resetta i parametri
                m_VisualizeBagua = false;
                
                script.ResetBagua();
            }
        }
        /// <summary>
        /// Shows or hides the AR debug menu.
        /// </summary>
        public void ShowHideDebugMenu()
        {
            m_ShowDebugMenu = !m_ShowDebugMenu;
            m_DebugMenuSlider.value = m_ShowDebugMenu ? 1 : 0;

            // There is a bug in the ARDebugMenu that when the debug menu is enabled, it will always
            // turn off the line visualizers regardless of previous state. This means that the toggle
            // UI can appear "on" while the vizualizers are "off" and the toggle will behave opposite
            // of the value shown in the UI.
            // In the code below, we capture the previous value and only set the value back to 1 if it
            // is different than what the ARDebugMenu is tracking for that UI element. Otherwise it will
            // cause the same toggle behavior described above.
            if (m_ShowDebugMenu)
            {
                m_ARDebugMenu.gameObject.SetActive(true);
                AdjustARDebugMenuPosition();
                if (m_ARDebugMenu.showPlanesButton.value != m_DebugMenuPlanesButtonValue)
                    m_ARDebugMenu.showPlanesButton.value = m_DebugMenuPlanesButtonValue;
            }
            else
            {
                m_DebugMenuPlanesButtonValue = m_ARDebugMenu.showPlanesButton.value;
                if (m_DebugMenuPlanesButtonValue == 1f)
                    m_ARDebugMenu.showPlanesButton.value = 0f;

                m_ARDebugMenu.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Clear all created objects in the scene.
        /// </summary>
        public void ClearAllObjects()
        {
            foreach (Transform child in m_ObjectSpawner.transform)
            {
                Destroy(child.gameObject);
            }
        }

        /// <summary>
        /// Triggers hide animation for menu.
        /// </summary>
        public void HideMenu()
        {
            m_ObjectMenuAnimator.SetBool("Show", false);
            m_ShowObjectMenu = false;
         
            AdjustARDebugMenuPosition();
        }

        
       
        public void HideMenuZone()
        {
            m_ObjectMenuAnimatorZone.SetBool("Show", false);
            m_ShowObjectMenuZone = false;
            AdjustARDebugMenuPosition();
        }
       
        void ChangePlaneVisibility(bool setVisible)
        {
            foreach (var plane in m_ARPlanes)
            {
                if (m_ARPlaneMeshVisualizers.TryGetValue(plane, out var visualizer))
                {
                    visualizer.enabled = m_UseARPlaneFading ? true : setVisible;
                }

                if (m_ARPlaneMeshVisualizerFaders.TryGetValue(plane, out var fader))
                {
                    if (m_UseARPlaneFading)
                        fader.visualizeSurfaces = setVisible;
                    else
                        fader.SetVisualsImmediate(1f);
                }
            }
        }

        void DeleteFocusedObject()
        {
            var currentFocusedObject = m_InteractionGroup.focusInteractable;
            if (currentFocusedObject != null)
            {
                Destroy(currentFocusedObject.transform.gameObject);
            }
        }

        void InitializeDebugMenuOffsets()
        {
            if (m_ButtonZone.TryGetComponent<RectTransform>(out var buttonRect))
                m_ObjectButtonOffset = new Vector2(0f, buttonRect.anchoredPosition.y + buttonRect.rect.height + 10f);
            else
                m_ObjectButtonOffset = new Vector2(0f, 200f);

            if (m_ObjectMenu.TryGetComponent<RectTransform>(out var menuRect))
                m_ObjectMenuOffset = new Vector2(0f, menuRect.anchoredPosition.y + menuRect.rect.height + 10f);
            else
                m_ObjectMenuOffset = new Vector2(0f, 345f);
        }

        void AdjustARDebugMenuPosition()
        {
            if (m_ARDebugMenu == null)
                return;

            float screenWidthInInches = Screen.width / Screen.dpi;

            if (screenWidthInInches < 5)
            {
                Vector2 menuOffset = m_ShowObjectMenu ? m_ObjectMenuOffset : m_ObjectButtonOffset;

                if (m_ARDebugMenu.toolbar.TryGetComponent<RectTransform>(out var rect))
                {
                    rect.anchorMin = new Vector2(0.5f, 0);
                    rect.anchorMax = new Vector2(0.5f, 0);
                    rect.eulerAngles = new Vector3(rect.eulerAngles.x, rect.eulerAngles.y, 90);
                    rect.anchoredPosition = new Vector2(0, 20) + menuOffset;
                }

                if (m_ARDebugMenu.displayInfoMenuButton.TryGetComponent<RectTransform>(out var infoMenuButtonRect))
                    infoMenuButtonRect.localEulerAngles = new Vector3(infoMenuButtonRect.localEulerAngles.x, infoMenuButtonRect.localEulerAngles.y, -90);

                if (m_ARDebugMenu.displayConfigurationsMenuButton.TryGetComponent<RectTransform>(out var configurationsMenuButtonRect))
                    configurationsMenuButtonRect.localEulerAngles = new Vector3(configurationsMenuButtonRect.localEulerAngles.x, configurationsMenuButtonRect.localEulerAngles.y, -90);

                if (m_ARDebugMenu.displayCameraConfigurationsMenuButton.TryGetComponent<RectTransform>(out var cameraConfigurationsMenuButtonRect))
                    cameraConfigurationsMenuButtonRect.localEulerAngles = new Vector3(cameraConfigurationsMenuButtonRect.localEulerAngles.x, cameraConfigurationsMenuButtonRect.localEulerAngles.y, -90);

                if (m_ARDebugMenu.displayDebugOptionsMenuButton.TryGetComponent<RectTransform>(out var debugOptionsMenuButtonRect))
                    debugOptionsMenuButtonRect.localEulerAngles = new Vector3(debugOptionsMenuButtonRect.localEulerAngles.x, debugOptionsMenuButtonRect.localEulerAngles.y, -90);

                if (m_ARDebugMenu.infoMenu.TryGetComponent<RectTransform>(out var infoMenuRect))
                {
                    infoMenuRect.anchorMin = new Vector2(0.5f, 0);
                    infoMenuRect.anchorMax = new Vector2(0.5f, 0);
                    infoMenuRect.pivot = new Vector2(0.5f, 0);
                    infoMenuRect.anchoredPosition = new Vector2(0, 150) + menuOffset;
                }

                if (m_ARDebugMenu.configurationMenu.TryGetComponent<RectTransform>(out var configurationsMenuRect))
                {
                    configurationsMenuRect.anchorMin = new Vector2(0.5f, 0);
                    configurationsMenuRect.anchorMax = new Vector2(0.5f, 0);
                    configurationsMenuRect.pivot = new Vector2(0.5f, 0);
                    configurationsMenuRect.anchoredPosition = new Vector2(0, 150) + menuOffset;
                }

                if (m_ARDebugMenu.cameraConfigurationMenu.TryGetComponent<RectTransform>(out var cameraConfigurationsMenuRect))
                {
                    cameraConfigurationsMenuRect.anchorMin = new Vector2(0.5f, 0);
                    cameraConfigurationsMenuRect.anchorMax = new Vector2(0.5f, 0);
                    cameraConfigurationsMenuRect.pivot = new Vector2(0.5f, 0);
                    cameraConfigurationsMenuRect.anchoredPosition = new Vector2(0, 150) + menuOffset;
                }

                if (m_ARDebugMenu.debugOptionsMenu.TryGetComponent<RectTransform>(out var debugOptionsMenuRect))
                {
                    debugOptionsMenuRect.anchorMin = new Vector2(0.5f, 0);
                    debugOptionsMenuRect.anchorMax = new Vector2(0.5f, 0);
                    debugOptionsMenuRect.pivot = new Vector2(0.5f, 0);
                    debugOptionsMenuRect.anchoredPosition = new Vector2(0, 150) + menuOffset;
                }
            }
        }

        void OnPlaneChanged(ARTrackablesChangedEventArgs<ARPlane> eventArgs)
        {
            if (eventArgs.added.Count > 0)
            {
                foreach (var plane in eventArgs.added)
                {
                    m_ARPlanes.Add(plane);
                    if (plane.TryGetComponent<ARPlaneMeshVisualizer>(out var vizualizer))
                    {
                        m_ARPlaneMeshVisualizers.Add(plane, vizualizer);
                        if (!m_UseARPlaneFading)
                        {
                            vizualizer.enabled = m_VisualizePlanes;
                        }
                    }

                    if (!plane.TryGetComponent<ARPlaneMeshVisualizerFader>(out var visualizer))
                    {
                        visualizer = plane.gameObject.AddComponent<ARPlaneMeshVisualizerFader>();
                    }
                    m_ARPlaneMeshVisualizerFaders.Add(plane, visualizer);
                    visualizer.visualizeSurfaces = m_VisualizePlanes;
                }
            }

            if (eventArgs.removed.Count > 0)
            {
                foreach (var plane in eventArgs.removed)
                {
                    var planeGameObject = plane.Value;
                    if (planeGameObject == null)
                        continue;

                    if (m_ARPlanes.Contains(planeGameObject))
                        m_ARPlanes.Remove(planeGameObject);

                    if (m_ARPlaneMeshVisualizers.ContainsKey(planeGameObject))
                        m_ARPlaneMeshVisualizers.Remove(planeGameObject);

                    if (m_ARPlaneMeshVisualizerFaders.ContainsKey(planeGameObject))
                        m_ARPlaneMeshVisualizerFaders.Remove(planeGameObject);
                }
            }

            // Fallback if the counts do not match after an update
            if (m_PlaneManager.trackables.count != m_ARPlanes.Count)
            {
                m_ARPlanes.Clear();
                m_ARPlaneMeshVisualizers.Clear();
                m_ARPlaneMeshVisualizerFaders.Clear();

                foreach (var plane in m_PlaneManager.trackables)
                {
                    m_ARPlanes.Add(plane);
                    if (plane.TryGetComponent<ARPlaneMeshVisualizer>(out var vizualizer))
                    {
                        m_ARPlaneMeshVisualizers.Add(plane, vizualizer);
                        if (!m_UseARPlaneFading)
                        {
                            vizualizer.enabled = m_VisualizePlanes;
                        }
                    }

                    if (!plane.TryGetComponent<ARPlaneMeshVisualizerFader>(out var fader))
                    {
                        fader = plane.gameObject.AddComponent<ARPlaneMeshVisualizerFader>();
                    }
                    m_ARPlaneMeshVisualizerFaders.Add(plane, fader);
                    fader.visualizeSurfaces = m_VisualizePlanes;
                }
            }
        }
    }
}
