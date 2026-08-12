using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShadowHint : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject hintPanel;
    [SerializeField] private TMP_Text hintText;

    [Header("Settings")]
    [SerializeField] private float displayTime = 3f;
    [SerializeField] private float slowMotionScale = 0.05f;
    [SerializeField] private string message = "You can hide in the shadows to avoid enemies.";

    private bool hasTriggered = false;

    private void Start()
    {
        hintPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasTriggered) return;
        if (!collision.CompareTag("Player")) return;

        hasTriggered = true;
        StartCoroutine(ShowHint());
    }

    private IEnumerator ShowHint()
    {
        hintText.text = message;
        hintPanel.SetActive(true);
        Time.timeScale = slowMotionScale;
        Time.fixedDeltaTime = 0.02f * slowMotionScale;

        yield return new WaitForSecondsRealtime(displayTime);

        hintPanel.SetActive(false);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
}