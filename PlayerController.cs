using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    public bool isMelee = true;
    public Image attackIcon;
    public Sprite meleeIcon;
    public Sprite rangedIcon;

    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;

    public float meleeRange = 1f;
    public int meleeDamage = 20;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float h = 0f;
        float v = 0f;

        if (Input.GetKey(KeyCode.A)) h = -1f;
        if (Input.GetKey(KeyCode.D)) h =  1f;
        if (Input.GetKey(KeyCode.W)) v =  1f;
        if (Input.GetKey(KeyCode.S)) v = -1f;

        movement = new Vector2(h, v).normalized;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isMelee = !isMelee;
            Debug.Log("Режим атаки: " + (isMelee ? "Ближний" : "Дальний"));
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))    Attack(Vector2.up);
        if (Input.GetKeyDown(KeyCode.DownArrow))  Attack(Vector2.down);
        if (Input.GetKeyDown(KeyCode.LeftArrow))  Attack(Vector2.left);
        if (Input.GetKeyDown(KeyCode.RightArrow)) Attack(Vector2.right);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void Attack(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.position, direction, meleeRange, LayerMask.GetMask("Enemy"));
            
        Debug.DrawRay(rb.position, direction * meleeRange, Color.red, 0.5f);

        if (hit.collider != null)
        {
            EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(meleeDamage);
                Debug.Log("Нанесён урон врагу ближней атакой: " + meleeDamage);
            }
        }
    }
}