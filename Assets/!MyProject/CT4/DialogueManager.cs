using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private MessageDatabase messageDatabase;

    [Header("Character 1 UI")]
    [SerializeField] private TMP_Text character1Text;
    [SerializeField] private TMP_Text character1Name;
    [SerializeField] private GameObject character1TypingIndicator;

    [Header("Character 2 UI")]
    [SerializeField] private TMP_Text character2Text;
    [SerializeField] private TMP_Text character2Name;
    [SerializeField] private GameObject character2TypingIndicator;

    [Header("Settings")]
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private float autoAdvanceDelay = 3f;

    private int currentMessageIndex = 0;
    private bool isTyping = false;
    private bool isPaused = false;
    private Coroutine typingCoroutine;
    private Coroutine autoAdvanceCoroutine;
    private CancellationTokenSource cts;

    private void Start()
    {
        cts = new CancellationTokenSource();
        if (messageDatabase == null) return;
        ClearAllText();
        ShowMessage(currentMessageIndex);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) OnLeftClick();
        if (Input.GetMouseButtonDown(1)) OnRightClick();
    }

    private void OnLeftClick()
    {
        if (isPaused) return;
        if (isTyping) SkipTyping();
        else ShowNextMessage();
    }

    private void OnRightClick()
    {
        if (isPaused) ResumeDialogue();
        else PauseDialogue();
    }

    private void ShowMessage(int index)
    {
        if (index >= messageDatabase.MessageCount) return;
        var messageData = messageDatabase.GetMessage(index);
        if (messageData == null) return;

        StopTyping();
        StopAutoAdvance();
        ClearAllText();
        HideAllTypingIndicators();

        if (messageData.characterId == 0) ShowCharacter1Message(messageData);
        else ShowCharacter2Message(messageData);
    }

    private void ShowCharacter1Message(MessageDatabase.MessageData messageData)
    {
        character1Name.text = messageData.characterName;
        character1TypingIndicator.SetActive(true);
        typingCoroutine = StartCoroutine(TypeText(character1Text, messageData.text, character1TypingIndicator));
    }

    private void ShowCharacter2Message(MessageDatabase.MessageData messageData)
    {
        character2Name.text = messageData.characterName;
        character2TypingIndicator.SetActive(true);
        typingCoroutine = StartCoroutine(TypeText(character2Text, messageData.text, character2TypingIndicator));
    }

    private IEnumerator TypeText(TMP_Text textComponent, string fullText, GameObject typingIndicator)
    {
        isTyping = true;

        for (int i = 0; i < fullText.Length; i++)
        {
            if (cts.Token.IsCancellationRequested || isPaused) yield break;
            textComponent.text += fullText[i];
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        typingIndicator.SetActive(false);
        StartAutoAdvance();
    }

    private void SkipTyping()
    {
        if (typingCoroutine == null) return;
        StopCoroutine(typingCoroutine);
        isTyping = false;

        var messageData = messageDatabase.GetMessage(currentMessageIndex);
        if (messageData == null) return;

        if (messageData.characterId == 0)
        {
            character1Text.text = messageData.text;
            character1TypingIndicator.SetActive(false);
        }
        else
        {
            character2Text.text = messageData.text;
            character2TypingIndicator.SetActive(false);
        }
    }

    private void StopTyping()
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
    }

    private void ClearAllText()
    {
        character1Text.text = "";
        character2Text.text = "";
        character1Name.text = "";
        character2Name.text = "";
    }

    private void HideAllTypingIndicators()
    {
        character1TypingIndicator.SetActive(false);
        character2TypingIndicator.SetActive(false);
    }

    private void ShowNextMessage()
    {
        if (isPaused) return;
        StopAutoAdvance();
        currentMessageIndex++;
        if (currentMessageIndex < messageDatabase.MessageCount) ShowMessage(currentMessageIndex);
    }

    private void StartAutoAdvance()
    {
        if (autoAdvanceCoroutine != null) StopCoroutine(autoAdvanceCoroutine);
        autoAdvanceCoroutine = StartCoroutine(AutoAdvanceCoroutine());
    }

    private void StopAutoAdvance()
    {
        if (autoAdvanceCoroutine != null)
        {
            StopCoroutine(autoAdvanceCoroutine);
            autoAdvanceCoroutine = null;
        }
    }

    private IEnumerator AutoAdvanceCoroutine()
    {
        yield return new WaitForSeconds(autoAdvanceDelay);
        ShowNextMessage();
    }

    private void PauseDialogue()
    {
        isPaused = true;
        cts.Cancel();
        cts = new CancellationTokenSource();
    }

    private void ResumeDialogue()
    {
        isPaused = false;
        if (!isTyping)
        {
            StartAutoAdvance();
            return;
        }

        var messageData = messageDatabase.GetMessage(currentMessageIndex);
        if (messageData == null) return;

        string fullText = messageData.text;
        string currentText = messageData.characterId == 0 ? character1Text.text : character2Text.text;
        string remainingText = fullText.Substring(currentText.Length);

        if (messageData.characterId == 0)
        {
            character1TypingIndicator.SetActive(true);
            typingCoroutine = StartCoroutine(TypeText(character1Text, remainingText, character1TypingIndicator));
        }
        else
        {
            character2TypingIndicator.SetActive(true);
            typingCoroutine = StartCoroutine(TypeText(character2Text, remainingText, character2TypingIndicator));
        }
    }

    private void OnDestroy()
    {
        cts?.Cancel();
        cts?.Dispose();
        StopAutoAdvance();
        StopTyping();
    }
}