using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private DroppedCoin _prefab;
    [SerializeField] private int _count;

    private Transform _target;

    public void TriggerScript(Transform target)
    {
        _target = target;

        for (int i = 0; i < _count; i++)
        {
            SpawnCoin();
        }
    }

    private void SpawnCoin()
    {
        const float YDropForce = 3f;
        const float MaxXDropForce = 0.4f;

        float xDropForce = Random.Range(-MaxXDropForce, MaxXDropForce);
        var coin = Instantiate(_prefab, _target.position, Quaternion.identity);
        Vector2 vector = new Vector2(xDropForce, YDropForce);

        coin.Drop(vector);
    }
}
