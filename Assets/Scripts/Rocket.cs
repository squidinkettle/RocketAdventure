using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidbody;
    AudioSource audioSource;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField]List<AudioClip> listClips = new List<AudioClip>();
    [SerializeField] List<ParticleSystem> particles = new List<ParticleSystem>();

    enum State {Alive, Dying, Transcending }
    State state = State.Alive;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
     
        
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
    }


    private void ThrusterControl()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidbody.AddRelativeForce(Vector3.up * mainThrust);

            AudioControl(0);
            particles[0].Play();
        }
        else
        {
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
        Invoke("LoadNextScene", 1f);
    }

    private void StartDeathSequence()
    {
        particles[0].Stop();
        state = State.Dying;
        audioSource.Stop();
        AudioControl(2);
        particles[1].Play();
        Invoke("Death", 1f);
    }

    private void Death()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }


    private void AudioControl(int clip)
    {
        if (!audioSource.isPlaying)
        {

            audioSource.PlayOneShot(listClips[clip]);
        }


    }

}
