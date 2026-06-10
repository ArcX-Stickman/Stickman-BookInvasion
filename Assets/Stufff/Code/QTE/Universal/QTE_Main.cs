using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QTE_Main : MonoBehaviour
{
    public bool Started;
    public KeyCode[] KeyCodes;
    public GameObject[] KeysUI;
    public ClickerManagerQTE ClickerManagerQTE;
    public bool ForceTimed;
    public float Duration;
    public float CurrentDuration;
    public bool WinQTE;
    public bool finished;
    public AudioSource AudioP;
    public AudioClip QTEButtonPOP;
    public AudioClip Fail;
    public Slider Slider;
    /// remembner arjun you gotta remeber that you can only use this script by starting the
    /// QTE by reference
    public IEnumerator QTE()
    {
        CurrentDuration = 0;
        finished = false;
        WinQTE = false;

        int index = 0;
        List<KeyCode> playerInput = new List<KeyCode>();

        // Use the inspector sequence instead of hardcoded sequence
        KeyCode[] qteSequence = KeyCodes;

        // Setup UI ONCE
        for (int i = 0; i < KeysUI.Length && i < qteSequence.Length; i++)
        {
            yield return new WaitForSecondsRealtime(0.01f);

            AudioP.PlayOneShot(QTEButtonPOP);

            KeysUI[i].SetActive(true);

            Animator anim = KeysUI[i].GetComponent<Animator>();
            anim.CrossFade("Keynorm", 0, 0);

            TextMeshProUGUI text = KeysUI[i].GetComponentInChildren<TextMeshProUGUI>();
            text.gameObject.SetActive(true);
            text.text = qteSequence[i].ToString();
        }

        while (CurrentDuration <= Duration)
        {
            CurrentDuration += Time.unscaledDeltaTime;

            // SUCCESS
            if (index >= qteSequence.Length)
            {
                WinQTE = true;
                finished = true;
                yield break;
            }

            // CORRECT INPUT
            if (Input.GetKeyDown(qteSequence[index]))
            {
                playerInput.Add(qteSequence[index]);

                Animator anim = KeysUI[index].GetComponent<Animator>();
                TextMeshProUGUI text =
                    KeysUI[index].GetComponentInChildren<TextMeshProUGUI>();

                text.gameObject.SetActive(false);
                anim.CrossFade("KeyCrackFX", 0, 0);

                AudioP.PlayOneShot(ClickerManagerQTE.ClickSFX());

                index++;
            }
            // WRONG INPUT
            else if (Input.anyKeyDown)
            {
                bool correctKeyPressed = Input.GetKeyDown(qteSequence[index]);

                if (!correctKeyPressed)
                {
                    AudioP.PlayOneShot(Fail);

                    WinQTE = false;
                    finished = true;

                    yield break;
                }
            }

            // TIMER BAR
            if (Slider != null)
            {
                Slider.value = Mathf.Lerp(
                    100f,
                    0f,
                    CurrentDuration / Duration
                );
            }

            yield return null;
        }

        // TIME OUT
        AudioP.PlayOneShot(Fail);

        WinQTE = false;
        finished = true;
    }
}
