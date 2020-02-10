using UnityEngine;

[System.Serializable]
public class Sound
{

    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 0.7f;
    [Range(0.5f, 1.5f)]
    public float pitch = 1f;

    [Range(0f, 0.5f)]
    public float randomVolume = 0.1f;
    [Range(0f, 0.5f)]
    public float randomPitch = 0.1f;

    public bool loop = false;

    public bool playOnAwake;

    private AudioSource source;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
        source.playOnAwake = playOnAwake;
    }

    public void Play()
    {
        if (source != null)
        {
            source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
            source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
            source.Play();
        }
    }
    public void Stop()
    {
        if(source != null)
            source.Stop();
    }
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    Sound[] sounds = null;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one AudioManager in the scene.");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].name);
            _go.transform.SetParent(this.transform);
            sounds[i].SetSource(_go.AddComponent<AudioSource>());
        }
        PlaySound("Music");
    }

    public void PlaySound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Play();
                return;
            }
        }

        // no sound with _name
        Debug.LogWarning("AudioManager: Sound not found in list, " + _name);
    }
    public void PlayRandomSound(string[] _name)
    {
        int randomIndex = Random.Range(0, _name.Length);

        string name = _name[randomIndex];
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == name)
            {
                sounds[i].Play();
                return;
            }
        }
    }

    public void StopSound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Stop();
                return;
            }
        }

        // no sound with _name
        Debug.LogWarning("AudioManager: Sound not found in list, " + _name);
    }

}
