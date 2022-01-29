using UnityEngine;

public class Cell : MonoBehaviour
{
    public int XCoordinate = 0;
    public int YCoordinate = 0;
    public Countries Country = Countries.NEUTRAL;
    public CountryAffiliation Affiliation = CountryAffiliation.CORE;
    public int Troops
    {
        get;
        private set;
    }
    public int MaterialIndex = -1;
    public Material NeutralMAterial;
    public Material[] NOTANKMaterials;
    public Material[] LanoviaMaterials;
    public GameObject TroopsPrefab;

    private MeshRenderer RendererComponent;
    private GameObject TroopsObject;

    private void Start()
    {
        RendererComponent = GetComponent<MeshRenderer>();
        Material[] materials = RendererComponent.materials;
        switch (Country)
        {
            case Countries.NOTANK:
                if (MaterialIndex >= 0)
                {
                    materials[MaterialIndex] = NOTANKMaterials[(int)Affiliation];
                }

                break;
            case Countries.LANOVIA:
                if (MaterialIndex >= 0)
                {
                    materials[MaterialIndex] = LanoviaMaterials[(int)Affiliation];
                }

                break;
            default:
                if (MaterialIndex >= 0)
                {
                    materials[MaterialIndex] = NeutralMAterial;
                }
                break;
        }

        RendererComponent.materials = materials;
    }

    public void SetTroops(int troops)
    {
        Troops = troops;
        if ((Troops == 0) && (TroopsObject != null))
        {
            Destroy(TroopsObject);
            return;
        }
        
        if (TroopsObject == null)
        {
            TroopsObject = GameObject.Instantiate(TroopsPrefab,transform);
            TroopsObject.GetComponent<Troops>().SetCountry(Country);
            TroopsObject.transform.localPosition = Vector3.up * 0.2f;
        }

        TroopsObject.GetComponent<Troops>().SetTroops(Troops);
    }
}
