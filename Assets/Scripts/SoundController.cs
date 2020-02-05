using UnityEngine;

public class SoundController : MonoBehaviour
{
    public Sound[] sounds;
    private AudioSource source;

    private void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        foreach (Sound sound in sounds)
        {
            sound.LoadClips();
        }
    }

    public void PlaySound(string name)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.name == name)
            {
                PlayRandomSound(sound);
            }
        }
    }

    private void PlayRandomSound(Sound sound)
    {
        source.PlayOneShot(sound.clips[Random.Range(0, sound.clips.Count)], sound.volume);
    }
}
