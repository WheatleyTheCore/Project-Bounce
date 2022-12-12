using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public Projectile laserPrefab;
    public System.Action killed;
    public System.Action levelBeat;
    public bool laserActive { get; private set; }

    private Camera cam;
    private float angle;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {

        /*
         1. get the mouse position x and y in either pixels or world units or whatever, with the player object being (0, 0)
         2. Make those values into a vector and normalize it to get direction vector for projectile, and use arctan to get angle to rotate (actually double check how you do rotations)
         3. Apply rotation to player object so it turns to point towards mouse
         4. When player clicks, create projectile with current rotation and pass it the direction unit vector that it will use to move
         */

        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 processedMousePos = mousePos - transform.position;

        int sign = (processedMousePos.x < 0) ? 1 : -1;

        angle = Vector2.Angle(new Vector2(processedMousePos.x, processedMousePos.y), Vector2.up) * sign;
        
        //Debug.Log(angle);

        transform.eulerAngles = new Vector3(0, 0, angle);

        Vector3 position = transform.position;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            position.x -= speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            position.x += speed * Time.deltaTime;
        }

        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        // Clamp the position of the character so they do not go out of bounds
        position.x = Mathf.Clamp(position.x, leftEdge.x, rightEdge.x);
        transform.position = position;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        // Only one laser can be active at a given time so first check that
        // there is not already an active laser
        if (!laserActive)
        {
            laserActive = true;

            Projectile laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            laser.direction = Quaternion.Euler(0, 0, angle) * Vector3.up;
            laser.angle = angle;
            laser.destroyed += OnLaserDestroyed;
            laser.hitGoal += OnHitGoal;
        }
    }

    private void OnHitGoal(Projectile laser)
    {
        laserActive = false;
        levelBeat.Invoke();

    }
    private void OnLaserDestroyed(Projectile laser)
    {
        laserActive = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Missile") ||
            other.gameObject.layer == LayerMask.NameToLayer("Invader"))
        {
            if (killed != null)
            {
                killed.Invoke();
            }
        }
    }

}
