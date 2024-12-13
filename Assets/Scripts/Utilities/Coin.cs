using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private float timeToLive = 1f, timeAlive = 0f, rotationDirection;
    public void setForce(float xMult)
    {
        xMult += Random.Range(-0.2f, 0.2f);
        float forceX = xMult * 250f * Random.Range(1f, 1.25f);
        float forceY = (1f - Mathf.Abs(xMult)) * 250f * Random.Range(1f, 1.25f);
        rb.AddForce(new Vector3(forceX, forceY, 0f));
        rotationDirection = Random.Range(400f, 700f);
        if (Random.value < 0.5f)
        {
            rotationDirection *= -1f;
        }
        transform.rotation.SetEulerAngles(0f, Random.Range(-45f, 45f), 0f);
    }

    private void Update()
    {
        if (timeAlive > timeToLive)
        {
            Destroy(gameObject);
        }
        timeAlive += Time.deltaTime;
        float lateGame = timeToLive * 0.75f;
        if (timeAlive > lateGame)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, Mathf.Pow(1f - ((timeAlive - lateGame) * 4f) / timeToLive, 3f));
        }
        if (timeAlive < 0.25f * timeToLive)
        {
            transform.localScale = Vector2.one * (4f * timeAlive / timeToLive);
        }
        rb.SetRotation(rotationDirection * timeAlive / timeToLive);

    }
}
