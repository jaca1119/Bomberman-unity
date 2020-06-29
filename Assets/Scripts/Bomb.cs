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
        spriteRender.enabled = false;
        Destroy(gameObject, .3f);
    }

    private IEnumerator CreateExplosions(Vector3 direction)
    {
        for(int i = 1; i < flameLength; i++)
        {
            Vector3 startPoint = transform.position + direction;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(startPoint, direction, i, levelMask);


            if (!raycastHit2D.collider)
            {
                Instantiate(flamePrefab, transform.position + (i * direction), transform.rotation);
            }
            else
            {
                Debug.DrawLine(startPoint, startPoint + new Vector3(1, 0), Color.red, 5F);
                Debug.DrawLine(startPoint, raycastHit2D.transform.position, Color.white, 5F);
                Debug.Log("Raycast hit");
                break;
            }

            yield return new WaitForSeconds(.05f);
        }
    }
}
