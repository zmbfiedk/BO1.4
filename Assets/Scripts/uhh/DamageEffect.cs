using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    private Color damagecolor = new Color(1f, 0.42f, 0.42f, 1f);
    private Color normalcolor = new Color(1f, 1f, 1f, 1f);
    public bool isCoroutineOn = false;
    SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        if (gameObject.CompareTag("Player"))
        {
            damagecolor = new Color(1f, 0.42f, 0.42f, 1f);
        }
        if (gameObject.CompareTag("Enemy"))
        {
            damagecolor = new Color(3f, 3f, 3f, 1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enabled == true)
        {
            Debug.Log("working");
        }
    }
    public IEnumerator Effect()
    {
        isCoroutineOn = true;
        sprite.color = damagecolor;
        Debug.Log("damaged");
        yield return new WaitForSeconds(0.3f);
        Debug.Log("back to normal");
        sprite.color = normalcolor;
        isCoroutineOn = false;
    }
}
