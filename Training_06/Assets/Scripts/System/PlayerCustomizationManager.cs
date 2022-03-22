using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCustomizationManager : MonoBehaviour
{
    public static PlayerCustomizationManager Instance;

    public List<Material> coatMaterials;
    public List<GameObject> beards;

    private void Awake() 
    {
        Instance = this; 
    }

    public Material GetRandomCoatMaterial ()
    {
        Material mat = null; 
        int rand = Random.Range(0, coatMaterials.Count);
        mat = coatMaterials[rand];
        coatMaterials.RemoveAt(rand);
        return mat; 
    }

    public GameObject GetRandomBeard ()
    {
        return beards[Random.Range(0, beards.Count)];
    }
}
