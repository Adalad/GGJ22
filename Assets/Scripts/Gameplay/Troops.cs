using UnityEngine;

public class Troops : MonoBehaviour
{
    public Material[] CountryMaterials;

    public MeshRenderer[] Renderers;

    public void SetCountry(Countries country)
    {
        for (int i = 0; i < Renderers.Length; ++i)
        {
            Renderers[i].material = CountryMaterials[(int)country];
        }
    }

    public void SetTroops(int troops)
    {
        for (int i = 0; i < Renderers.Length; i++)
        {
            Renderers[i].enabled = false;
        }

        if (troops > 0)
        {
            Renderers[troops - 1].enabled = true;
        }
    }
}
