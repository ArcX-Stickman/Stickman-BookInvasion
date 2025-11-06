using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] options;
    private RectTransform rect;
    private int currentPosition;

    [SerializeField] private AudioClip moveSound;
    [SerializeField] private AudioClip selectSound;
    private AudioSource audioSource;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangePosition(-1);
            PlayMoveSound();
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangePosition(1);
            PlayMoveSound();
        }

        // Enter
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.E))
        {
            Interact();
            PlaySelectSound();
        }
    }

    private void ChangePosition(int _change)
    {
        currentPosition += _change;

        if (currentPosition < 0)
            currentPosition = options.Length - 1;
        else if (currentPosition > options.Length - 1)
            currentPosition = 0;

        rect.position = new Vector3(rect.position.x, options[currentPosition].position.y, 0);
    }

    private void Interact()
    {
        options[currentPosition].GetComponent<Button>().onClick.Invoke();
    }

    private void PlayMoveSound()
    {
        if (audioSource != null && moveSound != null)
        {
            audioSource.PlayOneShot(moveSound);
        }
    }

    private void PlaySelectSound()
    {
        if (audioSource != null && selectSound != null)
        {
            audioSource.PlayOneShot(selectSound);
        }
    }
}
