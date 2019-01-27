using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    AudioSource fxSound; // Emitir sons
    public AudioClip backMusic;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Audio Source responsavel por emitir os sons
        fxSound = GetComponent<AudioSource>();
        fxSound.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
