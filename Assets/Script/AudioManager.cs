using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;
    public static float seVolume= 1;

    private AudioSource bgmSource;

    /// <summary>
    /// ‹N“®‚âƒV[ƒ““Ç‚İ‚İ‚ÉBGM/SE‚Ì‰¹—Ê‚ğ”½‰f‚·‚é
    /// </summary>
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        bgmSource = GetComponent<AudioSource>();

        if (bgmSource != null)
        {
            bgmSource.volume = PlayerPrefs.GetFloat("bgmVolume", 1);
        }
        seVolume = PlayerPrefs.GetFloat("seVolume", 1);
        playBGM(bgmSource.clip);
    }

    /// <summary>
    /// BGM‚Ì‰¹—Ê‚ğİ’è
    /// </summary>
    /// <param name="volume"></param>
    public void setBGMVolume(float volume)
    {
        bgmSource.volume = volume;
        PlayerPrefs.SetFloat("bgmVolume", volume);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// SE‚Ì‰¹—Ê‚ğİ’è
    /// </summary>
    /// <param name="volume"></param>
    public void setSEVolume(float volume)
    {
        seVolume = volume;
        PlayerPrefs.SetFloat("seVolume", volume);
        PlayerPrefs.Save();
    }

    public float getSEVolume()
    {
        return seVolume;
    }

    public void playBGM(AudioClip clip)
    {
        if(bgmSource.clip == clip)
        {
            return;
        }
        bgmSource.clip = clip;
        bgmSource.Play();
    }
}
