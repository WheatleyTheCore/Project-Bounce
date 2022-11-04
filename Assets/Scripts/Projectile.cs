using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 direction;
    public float speed;

    public System.Action destoryed;

    private void Update()
    {
        this.transform.position += this.direction * this.speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.destoryed != null)
        {
            this.destoryed.Invoke();
        }
        Destroy(this.gameObject);

    }
}
