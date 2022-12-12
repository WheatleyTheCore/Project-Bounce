using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public Vector3 direction = Vector3.up;
    public float angle = 0;
    public System.Action<Projectile> destroyed;
    public System.Action<Projectile> hitGoal;
    public new BoxCollider2D collider { get; private set; }

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        transform.eulerAngles = new Vector3(0, 0, angle);
    }

    private void OnDestroy()
    {
        if (destroyed != null)
        {
            destroyed.Invoke(this);
        }
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void CheckCollision(Collider2D other)
    {
        if (other.gameObject.CompareTag("Horizontal"))
        {
            direction.Set(direction[0], direction[1] * -1, direction[2]);
        }
        else if (other.gameObject.CompareTag("Vertical"))
        {
            direction.Set(direction[0] * -1, direction[1], direction[2]);
        }
        else if (other.gameObject.CompareTag("Death"))
        {
            Destroy(gameObject);
        } else if (other.gameObject.CompareTag("Goal"))
        {
            Debug.Log("Hit Goal");
            Destroy(gameObject);
            hitGoal.Invoke(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckCollision(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        CheckCollision(other);
    }

}
