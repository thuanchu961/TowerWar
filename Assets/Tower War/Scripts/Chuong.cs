using UnityEngine;

public class Chuong : MonoBehaviour
{
    [SerializeField] GameObject effect;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("mat_dat"))
        {
            effect.SetActive(true);
        }
    }
}
