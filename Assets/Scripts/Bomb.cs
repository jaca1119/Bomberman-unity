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

            Debug.DrawRay(startPoint, direction, Color.green, 30F);

            if (!raycastHit2D.collider)
            {
                Debug.DrawLine(startPoint, transform.position + (i * direction), Color.red, 10F);
                Instantiate(flamePrefab, transform.position + (i * direction), transform.rotation);
            }
            else
            {
                if (raycastHit2D.collider.CompareTag("ExplodableBlock"))
                {
                    tileMap.SetTile(tileMap.WorldToCell(raycastHit2D.point), null);
                    Instantiate(flamePrefab, transform.position + (i * direction), transform.rotation);
                }

                Debug.DrawLine(startPoint, raycastHit2D.centroid, Color.white, 10F);
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
