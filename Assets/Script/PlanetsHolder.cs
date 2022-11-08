using System.Collections.Generic;
using UnityEngine;

public class PlanetsHolder : MonoBehaviour
{
    public GameObject prefabPlanetBattery;

    public int COUNT_MIN = 70;
    public int COUNT_MAX = 90;
    private int count = 0;

    public float POS_X_INIT = 10;
    public float POS_X_GAP = 5;
    public float POS_X_JITTER = 2.5f;

    public float POS_Y_MIN = -6;
    public float POS_Y_MAX = 6;

    public float SCALE_MIN = 0.2f;
    public float SCALE_MAX = 0.7f;

    public float SPEED_MIN = 0.0f;
    public float SPEED_MAX = 0.007f;

    private List<PlanetController> planetControllers = new();

    void Start()
    {
        count = Random.Range(COUNT_MIN, COUNT_MAX + 1);
        InstantiatePlanets(count);
    }

    void InstantiatePlanets(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var posXJitter = Random.Range(-POS_X_JITTER, POS_X_JITTER);
            var posX = POS_X_INIT + (i * POS_X_GAP) + posXJitter;
            var posY = Random.Range(POS_Y_MIN, POS_Y_MAX);
            var pos = new Vector3(posX, posY, 0);
            var planet = Instantiate(
                prefabPlanetBattery,
                pos,
                Quaternion.identity,
                this.gameObject.transform
            );
            var scale = Random.Range(SCALE_MIN, SCALE_MAX);
            planet.transform.localScale = new Vector3(scale, scale, 1);
            var pc = planet.gameObject.GetComponentInChildren<PlanetController>();
            pc.speed = Random.Range(SPEED_MIN, SPEED_MAX);
            planet.name = $"planet_{i}";
            planetControllers.Add(pc);
        }
    }

    void Update()
    {
        for (int i = 0; i < count; i++)
        {
            var pc = planetControllers[i];
            if (pc == null) continue;
            var planet = pc.gameObject;
            var pos = planet.transform.localPosition;
            pos.x -= pc.speed;
            planet.transform.localPosition = pos;
        }
    }
}
