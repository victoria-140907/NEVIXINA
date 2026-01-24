using UnityEngine;

public class SimplePlatformMove : MonoBehaviour
{
    public enum MoveType { Horizontal, Vertical, PingPong }
    public MoveType moveType = MoveType.Horizontal;

    public float moveDistance = 3f; // Distancia que se mueve
    public float speed = 2f;        // Velocidad
    public float startDelay = 0f;   // Retraso inicial (para desincronizar)

    private Vector3 startPos;
    private float timer;

    void Start()
    {
        startPos = transform.position;
        timer = startDelay;
    }

    void Update()
    {
        timer += Time.deltaTime;

        float movement = Mathf.Sin(timer * speed) * moveDistance;

        Vector3 newPos = startPos;

        switch (moveType)
        {
            case MoveType.Horizontal:
                newPos.x += movement;
                break;
            case MoveType.Vertical:
                newPos.y += movement;
                break;
            case MoveType.PingPong:
                newPos.x += Mathf.PingPong(timer * speed, moveDistance);
                break;
        }

        transform.position = newPos;
    }
}
