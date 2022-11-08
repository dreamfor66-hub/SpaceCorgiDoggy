using UnityEngine;

public class PlanetController : MonoBehaviour
{
    public GameObject active;
    public GameObject destroyed;
    public float speed = 0.0f;
    public float COMPLETE_DESTROY_DELAY = 20f;
    public Collider batteryCollider;

    public bool isDestroy = false;
    // Start is called before the first frame update
    void Awake()
    {
        active.SetActive(true);
        destroyed.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDestroy)
        {
            active.SetActive(false);
            destroyed.SetActive(true);
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1.2f, 1.2f, 1.2f), 0.2f * Time.deltaTime);


            Invoke("DestroyPlanet", COMPLETE_DESTROY_DELAY);
        }
    }

    private void DestroyPlanet()
    {
        Destroy(gameObject);
    }

}
