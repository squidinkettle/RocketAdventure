﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Ocilator : MonoBehaviour
{

    [SerializeField] Vector3 movementVector=new Vector3(10f,10f,10f);

    [Range(0,1)][SerializeField]float movementFactor;

    Vector3 startingPosition;


    [SerializeField] float period=2f;
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float cycles = Time.time / period;
        const float tau = Mathf.PI * 2f; //6.28
        float rawSinWave = Mathf.Sin(cycles * tau);
        movementFactor = rawSinWave / 2f+0.5f;
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }
}
