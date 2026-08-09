using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace NavKeypad
{
    public class Keypad : MonoBehaviour, ICloseableUI
    {
        [Header("Events")]
        [SerializeField] private UnityEvent onAccessGranted;
        [SerializeField] private UnityEvent onAccessDenied;

        [Header("Combination Code (9 Numbers Max)")]
        [SerializeField] private int keypadCombo = 12345;

        public UnityEvent OnAccessGranted => onAccessGranted;
        public UnityEvent OnAccessDenied => onAccessDenied;

        [Header("Settings")]
        [SerializeField] private string accessGrantedText = "Granted";
        [SerializeField] private string accessDeniedText = "Denied";

        [Header("Visuals")]
        [SerializeField] private float displayResultTime = 1f;
        [Range(0, 5)]
        [SerializeField] private float screenIntensity = 2.5f;

        [Header("Colors")]
        [SerializeField] private Color screenNormalColor = new Color(0.98f, 0.50f, 0.032f, 1f);
        [SerializeField] private Color screenDeniedColor = new Color(1f, 0f, 0f, 1f);
        [SerializeField] private Color screenGrantedColor = new Color(0f, 0.62f, 0.07f);

        [Header("SoundFx")]
        [SerializeField] private AudioClip buttonClickedSfx;
        [SerializeField] private AudioClip accessDeniedSfx;
        [SerializeField] private AudioClip accessGrantedSfx;

        [Header("Component References")]
        [SerializeField] private Renderer panelMesh;
        [SerializeField] private TMP_Text keypadDisplayText;
        [SerializeField] private AudioSource audioSource;

        [Header("Keycard Access")]
        [SerializeField] private bool requiresKeycard = true;
        [SerializeField] private int requiredGateID = 0;

        [Header("Open/Close Scaling")]
        [SerializeField] private Vector3 smallScale = new Vector3(0.2f, 0.2f, 0.2f);
        [SerializeField] private Vector3 bigScale = new Vector3(1f, 1f, 1f);
        [SerializeField] private float scaleSpeed = 10f;

        [Header("Slow Motion")]
        [SerializeField] private float slowMotionScale = 0.1f;
        [SerializeField] private bool useSlowMotion = true;

        private string currentInput;
        private bool displayingResult = false;
        private bool accessWasGranted = false;
        private bool playerNearby = false;

        private bool isOpen = false;
        private Vector3 targetScale;
        private Coroutine scaleRoutine;

        private GameObject animationMarker;

        private void Awake()
        {
            ClearInput();
            panelMesh.material.SetVector("_EmissionColor", screenNormalColor * screenIntensity);
            transform.localScale = smallScale;
            targetScale = smallScale;
            animationMarker = transform.Find("Animator").gameObject;
        }

        private void Update()
        {
            if (playerNearby)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    TryOpenKeypad();
                }
            }
            else
            {
                if (isOpen)
                {
                    CloseUI();
                }

            }

        }

        private void TryOpenKeypad()
        {
            Debug.Log("TryOpenKeypad called. requiresKeycard: " + requiresKeycard);

            if (requiresKeycard && !HasMatchingKeycardEquipped())
            {
                string equippedName = "nothing";
                if (InventoryManager.Instance != null && InventoryManager.Instance.GetEquippedItem() != null)
                    equippedName = InventoryManager.Instance.GetEquippedItem().itemType.ToString();

                Debug.Log("Blocked. Need keycard gateID " + requiredGateID + ". Equipped: " + equippedName);
                return;
            }

            Debug.Log("Opening keypad!");
            OpenUI();
        }

        private bool HasMatchingKeycardEquipped()
        {
            if (InventoryManager.Instance == null)
            {
                Debug.Log("InventoryManager.Instance is null!");
                return false;
            }

            ItemData equipped = InventoryManager.Instance.GetEquippedItem();
            if (equipped == null) return false;

            return equipped.itemType == ItemData.ItemType.KeyCard
                && equipped.gateID == requiredGateID;
        }

        private void OpenUI()
        {
            if (!MenuManager.Instance.RegisterOpenUI(this))
                return;

            isOpen = true;
            targetScale = bigScale;
            if (scaleRoutine != null) StopCoroutine(scaleRoutine);
            scaleRoutine = StartCoroutine(ScaleRoutine());
            animationMarker.SetActive(false);
            Player.Instance.EnableMovementAndJump(false);
            MenuManager.Instance.RegisterOpenUI(this);
            SetSlowMotion(true);
        }

        public void CloseUI()
        {
            isOpen = false;
            targetScale = smallScale;
            if (scaleRoutine != null) StopCoroutine(scaleRoutine);
            scaleRoutine = StartCoroutine(ScaleRoutine());
            animationMarker.SetActive(true);
            Player.Instance.EnableMovementAndJump(true);
            MenuManager.Instance.UnregisterOpenUI(this);
            SetSlowMotion(false);
        }

        private IEnumerator ScaleRoutine()
        {
            while (Vector3.Distance(transform.localScale, targetScale) > 0.001f)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleSpeed * Time.unscaledDeltaTime);
                yield return null;
            }
            transform.localScale = targetScale;
        }

        public void AddInput(string input)
        {
            audioSource.PlayOneShot(buttonClickedSfx);
            if (displayingResult || accessWasGranted) return;
            switch (input)
            {
                case "enter":
                    CheckCombo();
                    break;
                default:
                    if (currentInput != null && currentInput.Length == 9)
                    {
                        return;
                    }
                    currentInput += input;
                    keypadDisplayText.text = currentInput;
                    break;
            }
        }

        public void CheckCombo()
        {
            if (int.TryParse(currentInput, out var currentKombo))
            {
                bool codeCorrect = currentKombo == keypadCombo;
                bool granted = codeCorrect;

                if (!displayingResult)
                {
                    StartCoroutine(DisplayResultRoutine(granted));
                }
            }
            else
            {
                Debug.LogWarning("Couldn't process input for some reason..");
            }
        }

        private IEnumerator DisplayResultRoutine(bool granted)
        {
            displayingResult = true;

            if (granted) AccessGranted();
            else AccessDenied();

            yield return new WaitForSeconds(displayResultTime);
            displayingResult = false;
            if (granted) yield break;
            ClearInput();
            panelMesh.material.SetVector("_EmissionColor", screenNormalColor * screenIntensity);
        }

        private void AccessDenied()
        {
            keypadDisplayText.text = accessDeniedText;
            onAccessDenied?.Invoke();
            panelMesh.material.SetVector("_EmissionColor", screenDeniedColor * screenIntensity);
            audioSource.PlayOneShot(accessDeniedSfx);
        }

        private void ClearInput()
        {
            currentInput = "";
            keypadDisplayText.text = currentInput;
        }

        private void AccessGranted()
        {
            accessWasGranted = true;
            keypadDisplayText.text = accessGrantedText;
            onAccessGranted?.Invoke();
            panelMesh.material.SetVector("_EmissionColor", screenGrantedColor * screenIntensity);
            audioSource.PlayOneShot(accessGrantedSfx);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                playerNearby = true;
                Debug.Log("Player entered keypad range");
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                playerNearby = false;
                if (isOpen) CloseUI();
            }
        }

        private void SetSlowMotion(bool slow)
        {
            if (!useSlowMotion) return;

            if (slow)
            {
                Time.timeScale = slowMotionScale;
                Time.fixedDeltaTime = 0.02f * slowMotionScale;
            }
            else
            {
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f;
            }
        }
    }
}