using UnityEngine;

public class PlanetController : MonoBehaviour
{
    public GameObject active;
    public GameObject destroyed;
    public Collider colliderSelf;
    public float speed = 0.0f;

    public bool isDestroy = false;
    // Start is called before the first frame update
    void Awake()
    {
        colliderSelf = GetComponentInChildren<Collider>();
        active.SetActive(true);
        destroyed.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDestroy)
        {
            colliderSelf.enabled = false;
            active.SetActive(false);
            destroyed.SetActive(true);
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1.2f, 1.2f, 1.2f), 0.2f * Time.deltaTime);


            Invoke("DestroyPlanet", 0.5f);
        }
    }

    private void DestroyPlanet()
    {
        Destroy(gameObject);
    }

}
