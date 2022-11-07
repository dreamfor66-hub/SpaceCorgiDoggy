using System.Collections;
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
    
    public GameObject Vfx_SuperBark;
    public Vector3 Vfx_SuperBark_Offset;
    public float Vfx_SuperBark_Dur;

    public int reflections;
    public float maxLength;

    private LineRenderer lineRenderer;
    private Ray ray;
    private RaycastHit hit;
    private Vector3 rayDir;

    public List<Collider> HitTargets = new List<Collider>();
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
        transform.position += moveDir * moveSpd *Time.deltaTime;
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
        GameObject addObject = (GameObject)Instantiate(Vfx_Bark, transform.position+Vfx_Bark_Offset, transform.rotation, transform);
        Destroy(addObject, Vfx_Bark_Dur);
    }

    void DrawRay(Vector3 startPos, Vector3 direction, float dist, int index)
    {
        //ray = new Ray(transform.position, (Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z)) - transform.position));
        //lineRenderer.positionCount = 1;
        //lineRenderer.SetPosition(0, transform.position);
        //float remainingLength = maxLength;

        //for (int i = 0; i < reflections; i++)
        //{
        //    if (Physics.Raycast(ray.origin, ray.direction, out hit, remainingLength))
        //    {
        //        lineRenderer.positionCount += 1;
        //        lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
        //        remainingLength -= Vector3.Distance(ray.origin, hit.point);
        //        ray = new Ray((hit.point + hit.normal * 0.001f), Vector3.Reflect(ray.direction, hit.normal));
        //        if (hit.collider.tag != "HurtCollider")
        //        {
        //            break;
        //        }
        //        else
        //        {
        //            lineRenderer.positionCount += 1;
        //            lineRenderer.SetPosition(lineRenderer.positionCount - 1, ray.origin + ray.direction * remainingLength);
        //        }
        //    }
        //}
        //var i = 0;
        //while (i < reflections)
        //{
        //    if (Physics.Raycast(ray.origin, ray.direction, out hit, remainingLength))
        //    {
        //        lineRenderer.positionCount += 1;
        //        lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
        //        remainingLength -= Vector3.Distance(ray.origin, hit.point);
        //        ray = new Ray((hit.point + hit.normal * 0.0000000001f), Vector3.Reflect(ray.direction, hit.normal));
        //        if (hit.collider.tag != "HurtCollider")
        //        {
        //            break;
        //        }
        //        else
        //        {
        //            lineRenderer.positionCount += 1;
        //            lineRenderer.SetPosition(lineRenderer.positionCount - 1, ray.origin + ray.direction * remainingLength);
        //        }
        //    }

        //    i++;
        //}

        //RaycastHit hit1;

        //        lineRenderer.positionCount = 2;
        //        lineRenderer.SetPosition(0, transform.position);
        //;

        //        if (Physics.Raycast(transform.position, (Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z)) - transform.position), out hit1, maxLength))
        //        {
        //            lineRenderer.SetPosition(1, hit1.point);

        //            Vector3 dir2 = Vector3.Reflect((Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z)) - transform.position), hit1.normal);
        //            RaycastHit hit2;
        //            lineRenderer.positionCount = 3;
        //            if (Physics.Raycast(hit1.point, dir2, maxLength))
        //            {
        //                lineRenderer.SetPosition(2, hit1.point);
        //            }
        //            else
        //            {
        //                lineRenderer.SetPosition(2, hit1.point + dir2 * maxLength);
        //            }
        //        }
        //        else
        //        {
        //            lineRenderer.SetPosition(1,  ((Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z)) - transform.position)) * maxLength);
        //        }

        if (reflections < index) return;

        lineRenderer.positionCount = index + 2;
        RaycastHit hit1;
        if (Physics.Raycast(startPos,direction,out hit1, dist))
        {
            if (hit1.collider.tag == "HurtCollider")
            {
                if(!HitTargets.Contains(hit1.collider))
                {
                    HitTargets.Add(hit1.collider);
                }
                
                lineRenderer.SetPosition(index + 1, hit1.point);
                DrawRay(hit1.point, Vector3.Reflect(direction, hit1.normal), dist, index + 1);
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
            ChargeCount = 0;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            ChargeCount += 20f * Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && ChargeCount > 20)
        {
            LaunchBeam();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0) && ChargeCount < 20)
        {
            Bark();
        }

        if (ChargeCount > 100)
        {
            ChargeCount = 100;
        }

        if (ChargeCount == 100)
        {
            LaunchBeam();
            ChargeCount = 0;
        }

        else if (ChargeCount > 30 && ChargeCount < 100)
        {
            BeamCharge();
        }
    }

    void BeamCharge()
    {
        //업데이트 되고있을 항목
        Vector3 startPos = transform.position;
        Vector3 direction = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z)) - transform.position;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPos);
        DrawRay(startPos, direction, maxLength, 0);
    }

    void LaunchBeam()
    {
        GameObject addObject = (GameObject)Instantiate(Vfx_SuperBark, transform.position + Vfx_SuperBark_Offset, transform.rotation, transform);
        //var row = addObject.GetComponent<ParticleSystem>().textureSheetAnimation.rowIndex;
        //row = Random.Range(0, 4);
        Destroy(addObject, Vfx_SuperBark_Dur);

        lineRenderer.positionCount = 0;
        DrawRay(transform.position, Vector3.zero, 0, 0);
        // 딱 한번만 발생할 항목

        ChargeCount = 0;
        Debug.Log("레이저빔 발사!!!");

        

        foreach(Collider colls in HitTargets)
        {
            colls.gameObject.transform.root.GetComponentInChildren<PlanetController>().isDestroy = true;
        }


        HitTargets.Clear();
    }
}
