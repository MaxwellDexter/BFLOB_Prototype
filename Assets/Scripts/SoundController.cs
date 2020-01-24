using UnityEngine;

public class SoundController : MonoBehaviour
{
    public Sound[] sounds;

    private void Start()
    {
        foreach (Sound sound in sounds)
        {
            sound.LoadClips();
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            SoundUtils.MakeSource(sound, source);
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
        sound.source.clip = sound.clips[Random.Range(0, sound.clips.Count)];
        sound.source.Play();
    }
}
