using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private Transform parentLevel;
    [SerializeField] private GameObject[] levels;

    [SerializeField] private GameObject defeat;
    [SerializeField] private GameObject win;
    [SerializeField] private GameObject[] effectWin;
    [SerializeField] private Text coinText;
    [SerializeField] private Text diamonText;

    public int totalEnemy;
    public int coin = 200;
    public int currentCoin;
    public int diamon = 100;
    public int currentDiamon;
    private void Awake()
    {
        instance = this;
        levels = Resources.LoadAll<GameObject>("LevelPrefabs");
    }

    private void Start()
    {
        GameObject oldLevel = Instantiate(levels[PlayerPrefs.GetInt("LevelOfMinigame")], Vector3.zero, Quaternion.identity);
        oldLevel.transform.parent = parentLevel;
        defeat.SetActive(false);
        win.SetActive(false);
        Debug.Log(PlayerPrefs.GetInt("LevelOfMinigame"));
        if (!PlayerPrefs.HasKey("LevelOfMinigame"))
        {
            coin = diamon = 0;
            coinText.text = coin.ToString();
            diamonText.text = diamon.ToString();
        }
        StartCoroutine(Delay100ms());
      //  SoundManager.instance.PlaySound(TypeSound.Background, 1, true);
    }

    IEnumerator Delay100ms()
    {
        yield return new WaitForSeconds(0.1f);
        GameObject[] floors = GameObject.FindGameObjectsWithTag("Floor");
        totalEnemy = 0;
        for (int i = 0; i < floors.Length; i++)
        {
            totalEnemy += floors[i].GetComponent<Floor>().enemies.Count;
        }
    }

    public void Win()
    {
        PlayerPrefs.SetInt("LevelOfMinigame", PlayerPrefs.GetInt("LevelOfMinigame") + 1);
        GameObject chuong = GameObject.FindGameObjectWithTag("chuong");
        if (chuong != null)
        {
            chuong.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            chuong.GetComponentInParent<Animator>().enabled = false;
        }

        for (int i = 0; i < effectWin.Length; i++)
        {
            effectWin[i].SetActive(true);
        }

        StartCoroutine(DelayWin());
    }

    IEnumerator DelayWin()
    {
        yield return new WaitForSeconds(2);
        for (int i = 0; i < effectWin.Length; i++)
        {
            effectWin[i].SetActive(false);
        }
        SoundManager.instance.PlaySound(TypeSound.Goldcoin, 1, false);
        win.SetActive(true);
        Reward();
    }

    public void Defeat()
    {
        defeat.SetActive(true);
    }

    public void Reward()
    {
        currentCoin += 200;
        currentDiamon += 100;

        PlayerPrefs.SetInt("coin", currentCoin);
        PlayerPrefs.SetInt("diamon", currentDiamon);
    }
    public void LoadReward()
    {
        coinText.text = PlayerPrefs.GetInt("coin").ToString();
        diamonText.text = PlayerPrefs.GetInt("diamon").ToString();
    }
    public void Replay()
    {
        LoadLevel();
        StartCoroutine(Delay100ms());
        win.SetActive(false);
        defeat.SetActive(false);
        PlayerPrefs.DeleteAll();
    }

    public void Next()
    {
        LoadLevel();
        StartCoroutine(Delay100ms());
        win.SetActive(false);
        LoadReward();
    }
    public void LoadLevel()
    {
        Destroy(parentLevel.GetChild(0).gameObject);
        GameObject oldLevel = Instantiate(levels[PlayerPrefs.GetInt("LevelOfMinigame")], Vector3.zero, Quaternion.identity);
        oldLevel.transform.parent = parentLevel;

    }

    public void Quit()
    {
        Application.Quit();
        PlayerPrefs.DeleteAll();
        Debug.Log("Quit game");
    }
}
