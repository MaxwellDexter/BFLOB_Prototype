using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PrototypeManager : MonoBehaviour
{
    public TextMeshProUGUI showHideText;
    public AudioSource musicAudioSource;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            if (musicAudioSource.isPlaying)
            {
                musicAudioSource.Stop();
            }
            else
            {
                musicAudioSource.Play();
            }
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            showHideText.enabled = !showHideText.enabled;
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            SceneManager.LoadScene(0);
        }
    }
}
