using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private float penaltyPerMiss = 0.2f;
    [SerializeField] private float gagThreshold = 0.5f;
    [SerializeField] private float chokeThreshold = 0.2f;
    [SerializeField] private GameObject notePrefab;
    [SerializeField] private Transform noteSpawn;
    [SerializeField] private Transform bar;
    [SerializeField] private AudioClip chewSound;
    [SerializeField] private AudioClip gagSound;
    [SerializeField] private AudioClip chokeSound;
    [SerializeField] private AudioClip shriekSound;
    [SerializeField] private AudioClip deliciousSound;
    [SerializeField] private TextMeshProUGUI accuracyText;
    [SerializeField] private Sprite closedMouthSprite;
    [SerializeField] private Sprite openMouthSprite;
    [SerializeField] private Image dudeImage;
    [SerializeField] private float perlinSpeed = 0.01f;

    public GameObject CurrentNote { get; set; }
    public bool IsNoteActive { get; set; }

    private float perlinX = 0;
    private float accuracy = 1;
    private bool alive = true;
    private AudioSource audioSource;
    private float soundDelay;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;

        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        soundDelay = Mathf.Abs(bar.position.y - noteSpawn.position.y) / Note.Speed;
        Debug.Log(soundDelay);
        StartCoroutine(BiteLoop());
        StartCoroutine(GameTime());
    }

    void Update()
    {
        perlinX += perlinSpeed;

        if (Input.GetKeyDown(KeyCode.Space) && IsNoteActive)
        {
            // give accuracy based on how close the note was to the middle of the bar
            Debug.Log("Yo");
            CurrentNote.SetActive(false);
            Destroy(CurrentNote);
        }
    }

    public void ApplyMiss()
    {
        accuracy -= penaltyPerMiss;
        accuracyText.text = (accuracy * 100).ToString() + "%";
        if (accuracy <= 0)
        {
            SceneManager.LoadScene("Lose");
        }
    }

    private IEnumerator BiteLoop()
    {
        yield return new WaitForSeconds(2f);
        while (true)
        {
            float totalDelay = Mathf.PerlinNoise(perlinX, 1f) * 0.5f + 0.5f;
            Instantiate(notePrefab, noteSpawn.position, Quaternion.identity);
            StartCoroutine(DelayMouth(chewSound, totalDelay));
            yield return new WaitForSeconds(totalDelay);
        }
    }

    private IEnumerator DelayMouth(AudioClip sound, float biteDelay)
    {
        yield return new WaitForSeconds(soundDelay - biteDelay / 2f);
        dudeImage.sprite = closedMouthSprite;
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(chewSound);

        yield return new WaitForSeconds(biteDelay / 2f);
        dudeImage.sprite = openMouthSprite;
    }

    private IEnumerator GameTime()
    {
        yield return new WaitForSeconds(60f);
        SceneManager.LoadScene("Win");
    }
}
