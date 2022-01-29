using TMPro;
using UnityEngine;

public class GameplayCanvas : MonoBehaviour
{
    public GameObject TurnButton;
    public GameObject TroopsPanel;
    public TMP_Text TroopsText;

    public void OnTurn()
    {
        TurnButton.SetActive(false);
        GameManager.instance.EndTurn();
    }

    public void CPUFinished()
    {
        TurnButton.SetActive(true);
    }

    public void EditTroops(int troops)
    {
        TroopsText.text = troops.ToString();
        TroopsPanel.SetActive(true);
    }

    public void AddTroops()
    {
        int troops = Mathf.Clamp(int.Parse(TroopsText.text) + 1, 0, GameManager.instance.MaxAllowedTroops);
        TroopsText.text = troops.ToString();
    }

    public void RemoveTroops()
    {
        int troops = Mathf.Clamp(int.Parse(TroopsText.text) - 1, 0, GameManager.instance.MaxAllowedTroops);
        TroopsText.text = troops.ToString();
    }

    public void SetTroops()
    {
        GameManager.instance.SetTroops(int.Parse(TroopsText.text));
        TroopsPanel.SetActive(false);
    }
}
