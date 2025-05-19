using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    private Color damagecolor = new Color(1f, 0.42f, 0.42f, 1f);
    private Color normalcolor = new Color(1f, 1f, 1f, 1f);
    public bool isCoroutineOn = false;
    SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator DamageEffect()
    {
        isCoroutineOn = true;
        sprite.color = damagecolor;
        yield return new WaitForSeconds(0.3f);
        sprite.color = normalcolor;
        isCoroutineOn = false;
    }
}
