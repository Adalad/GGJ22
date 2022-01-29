using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingCanvas : MonoBehaviour
{
    public Sprite[] ResultSprites;
    public TMP_Text ResultText;
    public Image ResultImage;
    public AudioClip[] ResultClips;
    public AudioSource MusicSource;

    private AudioSource AudioComponent;

    void Start()
    {
        AudioComponent = GetComponent<AudioSource>();
        int result = PlayerPrefs.GetInt("Result", -1);
        if (result >= 0)
        {
            ResultText.text = result == 0 ? "WAR" : "PEACE";
            ResultImage.sprite = ResultSprites[result];
            MusicSource.clip = ResultClips[result];
            MusicSource.Play();
        }
    }

    public void ReplayGame()
    {
        AudioComponent.Play();
        SceneManager.LoadScene("Menu");
    }

    public void ExitGame()
    {
        AudioComponent.Play();
        Application.Quit();
    }
}
