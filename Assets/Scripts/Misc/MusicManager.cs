using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Tooltip("5 bits, eye, wheel, heli, magnet gun in order")]
    [SerializeField] private AudioClip[] audios = new AudioClip[32];

    public int currentState;
    public int nextState;

    AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void RecieveChange(int state)
    {
        nextState = state;
    }

    // Update is called once per frame
    void Update()
    {
        if (!source.isPlaying)
        {
            source.clip = audios[nextState];
            source.Play();
        }
    }
}
