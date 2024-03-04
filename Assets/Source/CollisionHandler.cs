using UnityEngine;
using UnityEngine.SceneManagement;
public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip crash;

    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;


    AudioSource audioSource;

    bool isTransitioning = false;
    bool collisionDisabled = false;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled;  // toggle collision
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || collisionDisabled) { return; }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Friendly");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;

        }
    }
    void StartSuccessSequence()
    {
        // todo add SFX upon crash
        // todo add particle effect upon crash
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        successParticles.Play();
        GetComponent<Move>().enabled = false;
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    void StartCrashSequence()
    {
        // todo add SFX upon crash
        // todo add particle effect upon crash
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(crash);
        crashParticles.Play();
        GetComponent<Move>().enabled = false;
        Invoke("ReloadLevel", levelLoadDelay);
    }

    void LoadNextLevel()
    {
        int level = SceneManager.GetActiveScene().buildIndex;
        int nextLevel = level+1;
        if (nextLevel == SceneManager.sceneCountInBuildSettings) 
        {
            nextLevel = 0;
        }
        SceneManager.LoadScene(nextLevel);
    }
    void ReloadLevel()
    {
        int level = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(level);
    }
}
