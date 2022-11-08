using UnityEngine;

public class PlanetsHolder : MonoBehaviour
{
    public GameObject prefabPlanetBattery;

    void Start()
    {
        Debug.Log("PLANETS HOLDER");
        InstantiatePlanets();
    }

    void InstantiatePlanets()
    {
        Instantiate(
            prefabPlanetBattery,
            new Vector3(10, 0, 0),
            Quaternion.identity,
            this.gameObject.transform
        );
    }
}
