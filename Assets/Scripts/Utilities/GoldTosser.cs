using UnityEngine;

public class GoldTosser : MonoBehaviour
{
    [SerializeField] private Coin coin;
    public static GoldTosser instance;

    private void Start()
    {
        instance = this;
    }

    public void tossGold(GameObject spawner, int amount)
    {
        for (float i = 1; i <= amount; i++)
        {
            var coinInstance = Instantiate(coin);
            coinInstance.transform.position = spawner.transform.position;
            coinInstance.setForce((2f / (amount+1) * i) - 1);
        }
    }
}
