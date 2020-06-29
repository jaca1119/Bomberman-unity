using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject flamePrefab;
    public LayerMask levelMask;
    public int flameLength;

    private SpriteRenderer spriteRender;

    // Start is called before the first frame update
    void Start()
    {
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
                Debug.DrawLine(startPoint, raycastHit2D.transform.position, Color.white, 10F);
                Debug.Log("Raycast hit");
                break;
            }

            yield return new WaitForSeconds(.05f);
        }
    }
}
