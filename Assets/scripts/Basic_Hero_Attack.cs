using UnityEngine;
using System.Collections;

public class Basic_Hero_Attack : MonoBehaviour
{
    private GameObject parent;
    public GameObject Parent { set { parent = value; } get { return parent; } }

    private float speed = 10.0F; 
    private Vector3 direction;
    public Vector3 Direction { set { direction = value; } }

    private SpriteRenderer sprite;

    public float Termination_Time;

    public int Damage;

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        Destroy(gameObject, Termination_Time); 
    }


    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Unit unit = collider.GetComponent<Unit>();

        if (unit && unit.gameObject != parent)
        {
            unit.Take_Damage(Damage);
            Destroy(gameObject);
        }
        else if (!unit)
        {
            Destroy(gameObject);
        }
    }
    
        private void FixedUpdate()
    {
        sprite.flipX = direction.x < 0.0F;
    }
}
