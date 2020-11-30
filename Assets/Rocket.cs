using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidbody;
    AudioSource audioSource;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;


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
        ThrusterControl();
        RotationControl();
    }


    private void ThrusterControl()
    {
        if (Input.GetKey(KeyCode.Space))
        {

            rigidbody.AddRelativeForce(Vector3.up * mainThrust);
            if (!audioSource.isPlaying)
                audioSource.Play();

        }
        else
        {
            audioSource.Stop();
        }
    }

    private void RotationControl()
    {
        rigidbody.freezeRotation = true;
        float rotationThisFrame=rcsThrust*Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {

            transform.Rotate(Vector3.forward *rotationThisFrame);

        }
        else if (Input.GetKey(KeyCode.D))
        {

            transform.Rotate(-Vector3.forward* rotationThisFrame);
        }

        rigidbody.freezeRotation = false;

    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag) {
            case "Friendly":
                print("OK");
                break;
            case "Fuel":
                break;
            case "Ammo":
                break;
            default:
                print("Ded");
                break;
         
          
            }
    }


}
