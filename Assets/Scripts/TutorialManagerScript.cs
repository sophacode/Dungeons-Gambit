using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManagerScript : MonoBehaviour
{
    private GameManagerScript gameManager;
    private GameObject player;
    private PlayerScript playerScript;

    private int promptsSeen;

    public GameObject kingSprite;
    public GameObject speech;

    private bool first;
    private bool second;
    private bool waiting;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerScript>();

        promptsSeen = 0;
        first = false;
        second = true;
        waiting = false;
    }

    private void ShowKing(string text) {
        gameManager.OnPause();
        kingSprite.SetActive(true);
        speech.GetComponent<TextMeshProUGUI>().text = text;
        speech.SetActive(true);
        player.GetComponent<PlayerInput>().SwitchCurrentActionMap("King");
    }

    public void HideKing() {
        if (!waiting) {
            gameManager.OnPlay();
            kingSprite.SetActive(false);
            speech.SetActive(false);
            player.GetComponent<PlayerInput>().SwitchCurrentActionMap("Movement");
            if (!first) {
                first = true;
                StartCoroutine(FirstEnemy());
            }
            if (!second) {
                second = true;
                waiting = true;
                gameManager.OnPause();
                player.GetComponent<PlayerInput>().SwitchCurrentActionMap("King");
                playerScript.canPromote = true;
            }
        }
    }

    private void Start()
    {
        ShowKing("Welcome to Dungeons Gambit!\n Start by moving around with WASD");
    }

    private IEnumerator FirstEnemy() {
        yield return new WaitForSeconds(3);
        playerScript.canDash = true;
        ShowKing("Watch out, here comes an enemy pawn!\nTry using SHIFT to dash away from the enemy!");
    }

    public void FirstDash() {
        StartCoroutine(FirstDashDelay());
    }

    private IEnumerator FirstDashDelay() {
        yield return new WaitForSeconds(0.5f);
        ShowKing("Good Job!\nNow try using SPACE to shoot at the enemy while using your mouse to aim!");
        playerScript.canShoot = true;
    }


    public void FirstEnemyKilled() {
        ShowKing("Nice work!\nHere come some more enemies!");
    }

    public void SecondWaveFinished() {
        StartCoroutine(SecondWaveFinishedDelay());
    }

    private IEnumerator SecondWaveFinishedDelay() {
        yield return new WaitForSeconds(0.5f);
        ShowKing("You now have enough energy to PROMOTE\nPress 1 to promote to a Bishop for the cost of 3 energy!");
        second = false;
        gameManager.infoText.SetActive(false);
    }

    public void FirstPromotion() {
        StartCoroutine(FirstPromotionDelay());
    }

    private IEnumerator FirstPromotionDelay() {
        yield return new WaitForSeconds(0.5f);
        ShowKing("Nice! Every promotion lasts 10 seconds which is shown by the green bar in the top left\nFinish off these enemies and continue to the next room!");
        waiting = false;
    }

}
