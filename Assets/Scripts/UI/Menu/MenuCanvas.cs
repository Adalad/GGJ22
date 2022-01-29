using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuCanvas : MonoBehaviour
{
    public GameObject SelectionPanel;
    public GameObject USMInfoPanel;
    public GameObject UrsiaInfoPanel;
    public Image USMPanel;
    public Image UrsiaPanel;
    public Image USMFlag;
    public Image UrsiaFlag;
    public Sprite[] PanelSprites;
    public Sprite[] FlagSprites;

    private AudioSource AudioComponent;

    private void Start()
    {
        AudioComponent = GetComponent<AudioSource>();
        int country = PlayerPrefs.GetInt("Country", -1);
        if (country >= 0)
        {
            SelectCountry(country);
        }
    }

    public void ShowSelection()
    {
        AudioComponent.Play();
        SelectionPanel.SetActive(true);
        USMInfoPanel.SetActive(false);
        UrsiaInfoPanel.SetActive(false);
    }

    public void ShowCountry(int country)
    {
        AudioComponent.Play();
        SelectionPanel.SetActive(false);
        if (country == 0)
        {
            USMInfoPanel.SetActive(true);
        }
        else if (country == 1)
        {
            UrsiaInfoPanel.SetActive(true);
        }
    }

    public void SelectCountry(int country)
    {
        AudioComponent.Play();
        PlayerPrefs.SetInt("Country", country);
        if (country == 0)
        {
            USMPanel.sprite = PanelSprites[1];
            USMFlag.sprite = FlagSprites[1];
            UrsiaPanel.sprite = PanelSprites[0];
            UrsiaFlag.sprite = FlagSprites[0];
        }
        else if (country == 1)
        {
            USMPanel.sprite = PanelSprites[0];
            USMFlag.sprite = FlagSprites[0];
            UrsiaPanel.sprite = PanelSprites[1];
            UrsiaFlag.sprite = FlagSprites[1];
        }
    }

    public void PlayGame()
    {
        AudioComponent.Play();
        if (PlayerPrefs.HasKey("Country"))
        {
            SceneManager.LoadScene("Gameplay");
        }
    }

    public void ExitGame()
    {
        AudioComponent.Play();
        Application.Quit();
    }
}
