using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    public float meleeRange = 1f;
    public int meleeDamage = 20;

    // Adrenaline system reference
    private PlayerAmmo adrenalineSystem;
    public Image ammoBarFill; // Assign in Inspector

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        adrenalineSystem = GetComponent<PlayerAmmo>();
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

        // Update ammo/adrenaline bar UI
        if (ammoBarFill != null && adrenalineSystem != null)
        {
            ammoBarFill.fillAmount = adrenalineSystem.AdrenalineLevel / adrenalineSystem.MaxAdrenaline;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))    MeleeAttack(Vector2.up);
        if (Input.GetKeyDown(KeyCode.DownArrow))  MeleeAttack(Vector2.down);
        if (Input.GetKeyDown(KeyCode.LeftArrow))  MeleeAttack(Vector2.left);
        if (Input.GetKeyDown(KeyCode.RightArrow)) MeleeAttack(Vector2.right);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void MeleeAttack(Vector2 direction)
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