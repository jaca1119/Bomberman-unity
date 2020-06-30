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
                Instantiate(flamePrefab, transform.position + (i * direction), transform.rotation);
            }
            else
            {
                if (raycastHit2D.collider.CompareTag("ExplodableBlock"))
                {
                    Vector3 hitPosition = Vector3.zero;
                    hitPosition.x = raycastHit2D.point.x - 0.01f * raycastHit2D.normal.x;
                    hitPosition.y = raycastHit2D.point.y - 0.01f * raycastHit2D.normal.y;

                    tileMap.SetTile(tileMap.WorldToCell(hitPosition), null);
                    Instantiate(flamePrefab, transform.position + (i * direction), transform.rotation);
                }

                Debug.DrawLine(startPoint, raycastHit2D.point, Color.white, 10F);
                Debug.Log("Raycast hit");
                break;
            }

            yield return new WaitForSeconds(.05f);
        }
    }
}
