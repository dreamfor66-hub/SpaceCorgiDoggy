using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CorgiController : MonoBehaviour
{
    public float moveSpd;
    public float scrollSpd;
    //public Rigidbody rb;
    public Vector2 moveInput;
    public Vector3 moveDir;
    public Collider colliderSelf;

    public Vector2 center;
    public Vector2 size;
    float height;

    public GameObject Vfx_Bark;
    public Vector3 Vfx_Bark_Offset;
    public float Vfx_Bark_Dur;

    public GameObject Vfx_Charge;
    public GameObject ChargeVFX;
    public bool ChargeVfxTrigger;

    public GameObject Vfx_SuperBark_1;
    public GameObject Vfx_SuperBark_2;
    public Vector3 Vfx_SuperBark_Offset;
    public float Vfx_SuperBark_Dur;

    public int reflections;
    public float maxLength;

    private LineRenderer lineRenderer;
    //private Ray ray;
    //private RaycastHit hit;
    //private Vector3 rayDir;

    public HashSet<Collider> HitTargets = new HashSet<Collider>();
    public float ChargeCount = 0;

    // Start is called before the first frame update
    void Awake()
    {
        colliderSelf = GetComponentInChildren<Collider>();
        lineRenderer = GetComponent<LineRenderer>();
        //rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveDirCheck();
        InputCheck();
        MoveScroll();

    }

    private void Update()
    {

        BarkInputCheck();
        ChargeCountCheck();
        //ray = new Ray(transform.position, transform.right);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var tag = collision.gameObject.tag;
        if (tag == "PlanetCollider")
        {
            Debug.Log("행성에 부딪힘ㅠ");
            var pc = collision.gameObject.transform.parent.GetComponentInChildren<PlanetController>();
            var bc = pc.batteryCollider;
            Destroy(collision.gameObject);
            pc.isDestroy = true;
            bc.gameObject.SetActive(true);
        }
        else if (tag == "BatteryCollider")
        {
            Debug.Log("배터리 먹음");
            Destroy(collision.gameObject);
            Destroy(collision.transform.parent.gameObject);
        }
    }
    /// 코기 <=> [행성, 배터리] 충돌체크 후 적절히 처리

    void moveDirCheck()
    {
        moveInput.x = Input.GetAxis("Vertical");
        moveDir = Vector3.up * moveInput.x;

        float ly = size.y * 0.5f - height;
        float clampY = Mathf.Clamp(transform.position.y, -ly + center.y, ly + center.y);
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, clampY, transform.position.z), 0.2f);
        //transform.position += Vector3.Lerp(transform.position, moveDir * moveSpd * Time.deltaTime, 1f);
    }
    void InputCheck()
    {
        transform.position = Vector3.Lerp(transform.position, transform.position + moveDir * moveSpd, .5f * Time.deltaTime);
        //if (Input.GetAxisRaw("Vertical")!=0)
        //{
        //    Move();
        //}
    }
    void Move()
    {
        //rb.velocity = moveDir * moveSpd * Time.deltaTime;
        //transform.position += moveDir * moveSpd * Time.deltaTime;
        transform.position += moveDir * moveSpd * Time.deltaTime;
    }
    void MoveScroll()
    {
        transform.position += Vector3.right * scrollSpd * Time.deltaTime;
    }

    void BarkInputCheck()
    {
        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    Bark();
        //}
    }

    void Bark()
    {
        GameObject addObject = (GameObject)Instantiate(Vfx_Bark, transform.position + Vfx_Bark_Offset, transform.rotation, transform);
        Destroy(addObject, Vfx_Bark_Dur);
    }

    void DrawRay(Vector3 startPos, Vector3 direction, float dist, int index, bool needHitTest = false)
    {
        if (reflections < index) return;

        lineRenderer.positionCount = index + 2;
        RaycastHit hit1;
        int layerMask = 1 << LayerMask.NameToLayer("Default");
        if (Physics.Raycast(startPos, direction, out hit1, dist, layerMask))
        {
            if (hit1.collider.tag == "PlanetCollider")
            {
                if (needHitTest)
                {
                    HitTargets.Add(hit1.collider);
                }

                lineRenderer.SetPosition(index + 1, hit1.point);
                DrawRay(hit1.point, Vector3.Reflect(direction, hit1.normal), dist, index + 1, needHitTest);
            }
            else
            {
                lineRenderer.SetPosition(index + 1, direction * dist);
            }
        }
        else
        {
            lineRenderer.SetPosition(index + 1, direction * dist);
        }

    }

    void ChargeCountCheck()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ChargeVfxTrigger = true;
            ChargeCount = 0;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            ChargeCount += 20f * Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && ChargeCount > 5)
        {
            LaunchBeam();
            ChargeCount = 0;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0) && ChargeCount < 5)
        {
            Bark();
        }

        if (ChargeCount > 100)
        {
            ChargeCount = 100;
        }

        // 아래 블럭은 디버깅용으로 잠깐 제외

        //if (ChargeCount == 100)
        //{
        //    LaunchBeam();
        //    ChargeCount = 0;
        //}

        //else if (ChargeCount > 5 && ChargeCount < 100)
        if (ChargeCount > 5)
        {
            BeamCharge();
        }
    }

    void BeamCharge()
    {
        if (ChargeVfxTrigger)
        {
            ChargeVFX = (GameObject)Instantiate(Vfx_Charge, transform.position + new Vector3(0.2f, 0, 0), transform.rotation, transform);
            ChargeVfxTrigger = false;
        }
        //업데이트 되고있을 항목
        Vector3 startPos = transform.position;
        Vector3 direction = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z)) - transform.position;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPos);
        DrawRay(startPos, direction, maxLength, 0);
    }

    void LaunchBeam()
    {
        Destroy(ChargeVFX, 0.15f);
        ChargeVfxTrigger = true;
        var randomVFX = Random.Range(0, 2);
        GameObject addObject = (GameObject)Instantiate(randomVFX == 0 ? Vfx_SuperBark_2 : Vfx_SuperBark_1, transform.position + Vfx_SuperBark_Offset, transform.rotation, transform);
        //var row = addObject.GetComponent<ParticleSystem>().textureSheetAnimation.rowIndex;
        //row = Random.Range(0, 4);
        Destroy(addObject, Vfx_SuperBark_Dur);

        lineRenderer.positionCount = 0;

        Vector3 startPos = transform.position;
        Vector3 direction = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z)) - transform.position;
        DrawRay(startPos, direction, maxLength, 0, true);
        DrawRay(startPos, Vector3.zero, 0, 0, false);

        ChargeCount = 0;
        Debug.Log("레이저빔 발사!!!");



        foreach (Collider colls in HitTargets)
        {
            var pc = colls.gameObject.transform.parent.GetComponentInChildren<PlanetController>();
            pc.isDestroy = true;
            pc.gameObject.SetActive(true);
            Destroy(colls); // 콜라이더는 즉시 삭제
        }


        HitTargets.Clear();
    }
}
