using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IPreviousPosition, IBusy
{
    public static Player instance;

    private Vector3 prevPos;
    private bool busy;

    [SerializeField] private int heathCurrent;
    [SerializeField] private Text heathText;
    public SkeletonAnimation skeletonAnimation;
    public ParticleSystem roundHitBlue;
    public ParticleSystem khoi;
    public ParticleSystem soulExplosionOrange;

    private bool canDestroyFloor;
    private GameObject floorToDestroy;
    [SerializeField] private int indexFloor;
    [SerializeField] private Animator animatorFloor;

    public bool canAttack;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        busy = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        heathText.text = heathCurrent.ToString();
        animatorFloor.SetFloat("floor", indexFloor);
    }

    private void OnMouseDrag()
    {
        if (!busy)
        {
            canAttack = false;
            Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPos.z = -1;
            transform.position = newPos;
        }
    }

    private void OnMouseUp()
    {
        canAttack = true;
        prevPos.z = -1;
        if (canDestroyFloor && floorToDestroy != null)
        {
            if (floorToDestroy.transform != transform.parent)
            {
                StartCoroutine(WaitingForX00ms());
                indexFloor++;
                canDestroyFloor = false;
                animatorFloor.SetFloat("floor", indexFloor);
            }
        }
        if (transform.parent.GetComponent<Floor>() != null && transform.parent.GetComponent<Floor>().enemies.Count > 0)
        {
            Vector3 newPos = transform.parent.GetComponent<Floor>().enemies[0].GetCheckpoint();
            newPos.z = -1;
            transform.position = newPos;
            transform.parent.GetComponent<Floor>().select.SetActive(false);
        }
        else
        {
            transform.position = prevPos;
        }
    }

    IEnumerator WaitingForX00ms()
    {
        if (floorToDestroy.GetComponent<Floor>().floorWhere == FloorWhere.Middle)
        {
            floorToDestroy.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
        floorToDestroy.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.3f);
        Destroy(floorToDestroy);
    }



    public void SetPreviousPosition(Vector3 pos)
    {
        prevPos = pos;
    }

    public void SetBusy(bool busy)
    {
        this.busy = busy;
    }

    public void AddHeathCurrent(int heath)
    {
        heathCurrent += heath;
        heathText.text = heathCurrent.ToString();
    }

    public int GetHeathCurrent()
    {
        return heathCurrent;
    }

    public void SetCanDestroyFloor(GameObject floorToDestroy)
    {
        canDestroyFloor = true;
        this.floorToDestroy = floorToDestroy;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("mong_house"))
        {
            busy = false;
            SoundManager.instance.PlaySound(TypeSound.Attack, 1, false);
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            GetComponent<BoxCollider2D>().isTrigger = true;
            prevPos = transform.position;
            StartCoroutine(DeactiveKhoi());
        }
    }

    IEnumerator DeactiveKhoi()
    {
        khoi.gameObject.SetActive(true);
 
        yield return new WaitForSeconds(1f);
        khoi.gameObject.SetActive(false);
 
    }

    [ContextMenu("UpdateHeath")]
    public void Tesst()
    {
        heathText.text = heathCurrent.ToString();
    }
}
