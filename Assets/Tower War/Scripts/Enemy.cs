using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int heathCurrent;
    [SerializeField] private SkeletonAnimation skeletonAnimation;
    [SerializeField] private Text textHeath;
    [SerializeField] private Transform checkpoint;
    [SerializeField] private GameObject roundHitRed;
    [SerializeField] private GameObject soulExplosionOrange;

    public Vector3 GetCheckpoint()
    {
        return checkpoint.transform.position;
    }

    private void Start()
    {
        textHeath.text = heathCurrent.ToString();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Player.instance.canAttack)
        {
            Player.instance.canAttack = false;
            collision.GetComponent<IBusy>()?.SetBusy(true);
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(PlayAttack());
        skeletonAnimation.AnimationName = "attack";
        Player.instance.skeletonAnimation.AnimationName = "attack";
        Player.instance.roundHitBlue.gameObject.SetActive(true);
        roundHitRed.SetActive(true);
        yield return new WaitForSeconds(2);
        Player.instance.skeletonAnimation.AnimationName = "idle";
        Player.instance.roundHitBlue.gameObject.SetActive(false);
        SoundManager.instance.StopSound(TypeSound.Attack);
        roundHitRed.SetActive(false);
        if (Player.instance.GetHeathCurrent() > heathCurrent)
        {
            Player.instance.AddHeathCurrent(heathCurrent);
            transform.GetComponentInParent<Floor>().DestroyEnemy(this);
            Player.instance.khoi.gameObject.SetActive(true);
            soulExplosionOrange.SetActive(true);
            //SoundManager.instance.PlaySound(TypeSound.DieBoss, 1, false);
            yield return new WaitForSeconds(0.5f);
            Player.instance.SetBusy(false);
            Player.instance.khoi.gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            skeletonAnimation.AnimationName = "idle";
            Player.instance.soulExplosionOrange.gameObject.SetActive(true);
            heathCurrent += Player.instance.GetHeathCurrent();
            textHeath.text = heathCurrent.ToString();
            Player.instance.AddHeathCurrent(-Player.instance.GetHeathCurrent());
            SoundManager.instance.PlaySound(TypeSound.DieChar, 1, false);
            yield return new WaitForSeconds(1);
            Player.instance.soulExplosionOrange.gameObject.SetActive(false);
            Destroy(Player.instance.gameObject);
            GameManager.instance.Defeat();
        }
    }

    IEnumerator PlayAttack()
    {
        SoundManager.instance.PlaySound(TypeSound.Attack, 1, false);
        yield return new WaitForSeconds(0.833f);
        SoundManager.instance.PlaySound(TypeSound.Attack, 1, false);
        yield return new WaitForSeconds(0.833f);
        SoundManager.instance.PlaySound(TypeSound.Attack, 1, false);
    }

    [ContextMenu("UpdateHeath")]
    public void Tesst()
    {
        textHeath.text = heathCurrent.ToString();
    }
}
