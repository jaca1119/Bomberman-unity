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

    public GameObject bombPowerup;
    public GameObject flamePowerup;

    private Tilemap tileMap;
    private SpriteRenderer spriteRender;


    void Start()
    {
        tileMap = GameObject.Find("ExplodableBlocks").GetComponent<Tilemap>();
        spriteRender = GetComponent<SpriteRenderer>();
        Invoke("Explode", 3f);
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

                    int randPowerup = Mathf.RoundToInt(UnityEngine.Random.Range(0, 3));

                    if (randPowerup == 0)
                    {
                        Vector2 wrapPoint = raycastHit2D.point;
                        wrapPoint.x = Mathf.RoundToInt(wrapPoint.x);
                        wrapPoint.y = Mathf.RoundToInt(wrapPoint.y);

                        Instantiate(bombPowerup, wrapPoint, transform.rotation);
                    }
                    //else if (true)
                    //{
                    //    Vector2 wrapPoint = raycastHit2D.point;
                    //    wrapPoint.x = Mathf.RoundToInt(wrapPoint.x);
                    //    wrapPoint.y = Mathf.RoundToInt(wrapPoint.y);

                    //    Instantiate(flamePowerup, wrapPoint, transform.rotation);
                    //}
                }
                break;
            }

            yield return new WaitForSeconds(.05f);
        }
    }
}
