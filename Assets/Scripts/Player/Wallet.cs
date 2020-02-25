using UnityEngine;
using System.Collections;
using TMPro;

public class Wallet : MonoBehaviour
{
    const int startingGold = 100;

    [SerializeField]
    const float baseInterestRate = 0.05f;

    [SerializeField]
    float interestRate;
    [SerializeField]
    float interestRateFrequency = 10.0f;
    float interestRateTimer;

    [SerializeField]
    int currentGold;

    //GUI
    [SerializeField]
    TextMeshProUGUI guiGoldText;
    [SerializeField]
    TextMeshProUGUI guiKillText;
    int killCount = 0;

    // Use this for initialization
    void Start()
    {
        currentGold = startingGold;
        interestRate = baseInterestRate;
        interestRateTimer = interestRateFrequency;
        guiGoldText.text = currentGold.ToString();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Timer();
        if (interestRateTimer <= 0.0f)
        {
            AppendInterest();
            interestRateTimer = interestRateFrequency;
        }
    }

    /*
     * Apply Interest Rate
     */

    private void AppendInterest()
    {
        currentGold += (int)(currentGold * interestRate);
        guiGoldText.text = currentGold.ToString();

    }

    /*
     * Add money to wallet upon wavecompletion
     */
    public void WaveCompletionReward(int fixedSum)
    {
        currentGold += fixedSum;
        guiGoldText.text = currentGold.ToString();

    }

    /*
     * Add money to wallet upon emeny kill
     */
    public void EnemyKillReward(int fixedSum)
    {

        killCount++;
        currentGold += fixedSum;
        guiGoldText.text = currentGold.ToString();
        guiKillText.text = killCount.ToString();


    }

    /*
     * Check if money can be spent
     */
    public bool CheckMoney(int fixedSum)
    {
        if (currentGold >= fixedSum) return true;
        else return false;
    }

    /*
     * Spend money
     */
    public void SpendMoney(int fixedSum)
    {
        currentGold -= fixedSum;
        guiGoldText.text = currentGold.ToString();

    }

    /*
     * Reset wallet
     */
    public void ResetWallet()
    {
        currentGold = startingGold;
        guiGoldText.text = currentGold.ToString();

    }

    public void AddMoney(int amount)
    {
        currentGold += amount;
        guiGoldText.text = currentGold.ToString();

    }

    /*
     * 
     */
    public void Timer()
    {
        interestRateTimer -= Time.deltaTime;
    }
}
