using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    protected Vector3 direction;
    protected int damage;
    private float speed, r, g, b;

    public void setUp(Movement direction, int damage)
    {
        r = spriteRenderer.color.r;
        g = spriteRenderer.color.g;
        b = spriteRenderer.color.b;
        speed = getSpeed();
        this.damage = damage;
        if (direction == Movement.Right)
        {
            this.direction = Vector3.right;
        }
        else if (direction == Movement.Left)
        {
            this.direction = Vector3.left;
        }
        else if (direction == Movement.Up)
        {
            this.direction = Vector3.up;
        }
        else
        {
            this.direction = Vector3.down;
        }
        // flip the asset accordingly
        transform.rotation = Quaternion.Euler(0, 0, -90 * (int)direction);


        Metronome.instance.onBeat += onBeat;
        projectileSpecificSetUp();
        StartCoroutine(fadeIn());
    }

    protected abstract void projectileSpecificSetUp();

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private IEnumerator fadeIn()
    {
        float timeElapsed = 0f, duration = 0.5f;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            spriteRenderer.color = new Color(r, g, b, Mathf.Pow(timeElapsed / duration, 4f));
            yield return null;
        }
        spriteRenderer.color = new Color(r, g, b, 1f);
    }

    private void onBeat(object sender, System.EventArgs e)
    {
        projectileSpecificOnBeat();
    }
    protected abstract void projectileSpecificOnBeat();

    protected void destroy()
    {
        Metronome.instance.onBeat -= onBeat;
        StopAllCoroutines();
        Destroy(gameObject);
    }
    protected abstract float getSpeed();
}
