using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bomb : MonoBehaviour
{
    public GameObject flamePrefab;
    public LayerMask levelMask;
    public int flameLength;

    private Tilemap tileMap;
    private SpriteRenderer spriteRender;

    // Start is called before the first frame update
    void Start()
    {
        tileMap = GameObject.Find("ExplodableBlocks").GetComponent<Tilemap>();
        spriteRender = GetComponent<SpriteRenderer>();
        Invoke("Explode", 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Explode()
    {
        Instantiate(flamePrefab, transform.position, transform.rotation);
        StartCoroutine(CreateExplosions(Vector3.left));
        StartCoroutine(CreateExplosions(Vector3.right));
        StartCoroutine(CreateExplosions(Vector3.down));
        StartCoroutine(CreateExplosions(Vector3.up));

        spriteRender.enabled = false;
        Destroy(gameObject, .3f);
    }

    private IEnumerator CreateExplosions(Vector3 direction)
    {
        for(int i = 1; i < flameLength; i++)
        {
            Vector3 startPoint = transform.position;

            RaycastHit2D raycastHit2D = Physics2D.Raycast(startPoint, direction, i, levelMask);

            if (!raycastHit2D.collider)
            {
                //Debug.DrawLine(startPoint, transform.position + (i * direction), Color.red, 10F);
                Instantiate(flamePrefab, transform.position + (i * direction), transform.rotation);
            }
            else
            {

                Vector3 hitPosition = Vector3.zero;
                hitPosition.x = raycastHit2D.point.x - 0.01f * raycastHit2D.normal.x;
                hitPosition.y = raycastHit2D.point.y - 0.01f * raycastHit2D.normal.y;

                Vector3 hitPoint = raycastHit2D.point;
                if (raycastHit2D.collider.CompareTag("ExplodableBlock"))
                {
                    //This is same like world position but WorldToCell gives different value
                    Debug.Log("HitPosition" + hitPosition);
                    Debug.Log("HitPosition worldTocell" + tileMap.WorldToCell(hitPosition));

                    Vector3Int localPos = tileMap.WorldToCell(hitPoint);
                    //This is world position but WorldToCellGives different value from previous
                    Debug.Log("Globalpos" + hitPoint);
                    Debug.Log("Globalpos worldtocell" + tileMap.WorldToCell(hitPoint));
                    Debug.Log("localPos" + localPos);
                    Debug.Log("Center localPos" + tileMap.GetCellCenterLocal(tileMap.WorldToCell(hitPoint)));
                    tileMap.SetTile(tileMap.WorldToCell(hitPosition), null);
                    Instantiate(flamePrefab, transform.position + (i * direction), transform.rotation);
                }

                Debug.DrawLine(startPoint, hitPoint, Color.white, 10F);
                Debug.Log("Raycast hit");
                break;
            }

            yield return new WaitForSeconds(.05f);
        }
    }

    private Vector3Int ToInt3(Vector3 v)
    {
        return new Vector3Int((int)v.x, (int)v.y, (int)v.z);
    }
}
