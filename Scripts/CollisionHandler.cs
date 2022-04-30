using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField]
    private float levelLoadDelay = 2.5f;

    [SerializeField]
    private AudioClip explosion;
    [SerializeField]
    private AudioClip success;

    [SerializeField]
    private ParticleSystem explosionParticle;
    [SerializeField]
    private ParticleSystem successParticle;

    private bool soundPlayed = false;
    public bool shipCrashed = false;
    private bool collisionsDisabled = false;

    GameManager gameManager;
    Movement movement;
    Rigidbody rb;
    AudioSource audioSource;

    private void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        movement = GetComponent<Movement>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        DebugCollisions();
    }

    private void OnCollisionEnter(Collision other)
    {
        if(shipCrashed || collisionsDisabled)
        {
            return;
        }

        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Press the 'Space' key to Fly!");
                break;
            case "Finish":
                SuccessSequence();
                break;
            default:
                shipCrashed = true;
                CrashSequence();
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Fuel")
        {
            Debug.Log("Fuel Increased");
            Destroy(other.gameObject);
        }
    }

    private void CrashSequence()
    {
        if(shipCrashed == true)
        {
            if (!soundPlayed)
            {
                audioSource.Stop();
                audioSource.PlayOneShot(explosion);
                soundPlayed = true;
            }
        }
        explosionParticle.Play();
        movement.enabled = false;
        rb.constraints.Equals(false);
        gameManager.Invoke("ReloadLevel", levelLoadDelay);
    }

    private void SuccessSequence()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        successParticle.Play();
        movement.enabled = false;
        rb.isKinematic = true;
        gameManager.Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void DebugCollisions() //DEBUG
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsDisabled = !collisionsDisabled;
        }
    }
}
