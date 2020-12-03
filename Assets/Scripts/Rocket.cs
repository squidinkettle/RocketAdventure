using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidbody;
    BoxCollider[] boxCollider;
    AudioSource audioSource;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField]List<AudioClip> listClips = new List<AudioClip>();
    [SerializeField] List<ParticleSystem> particles = new List<ParticleSystem>();
    [SerializeField] float loadLevelTime = 2f;
    [SerializeField] GameObject thrusterLight;

    int NUMBER_OF_LEVELS = SceneManager.sceneCountInBuildSettings;

    enum State {Alive, Dying, Transcending }
    State state = State.Alive;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        thrusterLight.SetActive(false);
 
        boxCollider = GetComponentsInChildren<BoxCollider>();


    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }





    private void ProcessInput()
    {
        if (state == State.Alive)
        {
            ThrusterControl();
            RotationControl();
        }
        if(Debug.isDebugBuild)
            DebugKeys();

    }


    private void ThrusterControl()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidbody.AddRelativeForce(Vector3.up * mainThrust);
            thrusterLight.SetActive(true);
            AudioControl(0);
            particles[0].Play();
        }
        else
        {
            thrusterLight.SetActive(false);
            particles[0].Stop();
            audioSource.Stop();
        }

    }

  

    private void RotationControl()
    {
       
        rigidbody.freezeRotation = true;
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {

            transform.Rotate(Vector3.forward * rotationThisFrame);

        }
        else if (Input.GetKey(KeyCode.D))
        {

            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidbody.freezeRotation = false;
        

    }

    private void DebugKeys()
    {
        GoToNextLevel();
        TriggerBoxCollider();

    }

    private void TriggerBoxCollider()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {
            for (int x = 0; x < boxCollider.Length; x++)
            {
                boxCollider[x].isTrigger = !boxCollider[x].isTrigger;

            }
        }
    }

    private void GoToNextLevel()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
  
            LoadNextScene();

        }
     

    }

    private void OnCollisionEnter(Collision collision)
    {

        if (state != State.Alive) {return;}


        switch (collision.gameObject.tag)
        {
            case "Friendly":
                state = State.Alive;
                break;
            case "Ammo":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;


        }
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        AudioControl(1);
        particles[2].Play();
        Invoke("LoadNextScene", loadLevelTime);
    }

    private void StartDeathSequence()
    {
        particles[0].Stop();
        state = State.Dying;
        audioSource.Stop();
        AudioControl(2);
        particles[1].Play();
        Invoke("Death", loadLevelTime);
    }

    private void Death()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentSceneIndex);
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < 0 || nextSceneIndex > NUMBER_OF_LEVELS) { nextSceneIndex = 0; }
        SceneManager.LoadScene(nextSceneIndex);
    }


    private void AudioControl(int clip)
    {
        if (!audioSource.isPlaying)
        {

            audioSource.PlayOneShot(listClips[clip]);
        }


    }

}
