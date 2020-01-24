using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    [HideInInspector]
    public List<AudioClip> clips;
    [HideInInspector]
    public AudioSource source;

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    [Range(0f, 1f)]
    public float spatialBlend;
    [HideInInspector]
    public float maxDistance;
    [HideInInspector]
    public AudioRolloffMode rollOffMode;

    public int numberOfVersions;
    public string prefix;
    public string location;

    public void LoadClips()
    {
        clips = new List<AudioClip>();
        if (numberOfVersions > 1)
        {
            for (int i = 0; i < numberOfVersions; i++)
            {
                string number = "_" + (i + 1);
                AudioClip clip = Resources.Load<AudioClip>(GetPath(number));
                clips.Add(clip);
            }
        }
        else
        {
            clips.Add(Resources.Load<AudioClip>(GetPath("")));
        }
    }

    private string GetPath(string variationNumber)
    {
        return location + "/" + prefix + variationNumber;
    }
}
