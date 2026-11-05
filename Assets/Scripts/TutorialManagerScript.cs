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
    public bool waiting;

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
        ShowKing("You now have enough energy to PROMOTE\nYou can promote to different pieces for different abilities!\nPress 1 to promote to a Bishop for the cost of 3 energy!");
        second = false;
        gameManager.infoText.SetActive(false);
    }

    public void FirstPromotion() {
        StartCoroutine(FirstPromotionDelay());
    }

    private IEnumerator FirstPromotionDelay() {
        yield return new WaitForSeconds(0.5f);
        ShowKing("Nice! Every promotion lasts 10 seconds which is shown by the green bar in the top left and their buttons are shown on the right.\nFinish off these enemies and continue up to the next room!");
        waiting = false;
    }

    public void EnterSecondRoom() {
        StartCoroutine(SecondRoomDelay());
    }

    private IEnumerator SecondRoomDelay() {
        yield return new WaitForSeconds(2);
        ShowKing("Watch out for the bishop! Once it's in range of you it will shoot out bullets so try and dodge them and take it out!");
        gameManager.firstEnemyKilled = false;
    }

    public void FirstBishopKilled() {
        StartCoroutine(FirstBishopKilledDelay());
    }

    private IEnumerator FirstBishopKilledDelay() {
        yield return new WaitForSeconds(1);
        ShowKing("The Knight promotion gives you a melee attack that can destroy bullets\nPress 2 to promote to a knight for 5 energy!");
        second = false;
        gameManager.infoText.SetActive(false);
    }

    public void SecondRoomComplete() {
        StartCoroutine(SecondRoomCompleteDelay());
    }

    private IEnumerator SecondRoomCompleteDelay() {
        yield return new WaitForSeconds(0.5f);
        gameManager.infoText.SetActive(false);
        ShowKing("Nicely done! Clear a couple more rooms to the side and you'll be able to continue up\nYou can now promote to a Rook and Queen for 10 and 20 energy respecively.");
    }

    public void EnterThirdRoom() {
        StartCoroutine(EnterThirdRoomDelay());
    }

    private IEnumerator EnterThirdRoomDelay() {
        yield return new WaitForSeconds(2);
        ShowKing("The Knight enemy is faster and stronger than the pawn.\nBe careful not to get swarmed by them!");
    }

    public void EnterFourthRoom() {
        StartCoroutine(EnterFourthRoomDelay());
    }

    private IEnumerator EnterFourthRoomDelay() {
        yield return new WaitForSeconds(3);
        ShowKing("The Rook enemy is stronger and healthier than the others.\nYou might want to use your promotions to deal more damage!");
    }

    public void Click() {
        StartCoroutine(ClickDelay());
    }

    private IEnumerator ClickDelay() {
        yield return new WaitForSeconds(1.5f);
        ShowKing("Looks like you've proved you're worthy enough to continue up!\nKeep working your way up to defeat the Queen and claim victory!");
    }

}
