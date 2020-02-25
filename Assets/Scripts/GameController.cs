using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //Static Variable
    static GameController gameController;
    static int nb_controllers = 0;

    //Non-static attributes
    [SerializeField] Wallet wallet;
    [SerializeField] PlayerHealthController health;
    [SerializeField] MapV2 map;
    [SerializeField] WaveController waveController;

    public static void StartGame(int seed)
    {
        //Reset wallet and health
        gameController.wallet.ResetWallet();


        //FOR TESTING PURPOSED
        //gameController.wallet.AddMoney(10000);


        gameController.health.ResetHealth();

        //Remove all enemies, enemy spanwers, and reset wavecontroller
        gameController.waveController.ResetAll();

        //Remove all towers
        TowerController[] towers = FindObjectsOfType<TowerController>();
        for (int i = 0; i < towers.Length; i++)
        {
            Destroy(towers[i].gameObject);
        }

        gameController.map.m_mapSeed = seed;
        gameController.map.GenerateMapFromSeed(seed);
    }

    // called 
    public static void EndGame(bool showScreen = true)
    {
        //Don't Reset wallet and health 
        //Remove all enemies, enemy spanwers, and reset wavecontroller
        gameController.waveController.ResetAll();

        //Remove all towers
        TowerController[] towers = FindObjectsOfType<TowerController>();
        for (int i = 0; i < towers.Length; i++)
        {
            Destroy(towers[i].gameObject);
        }

        //TODO: Game Over UI
        if (showScreen) { 
            GameOverController goc = FindObjectOfType<GameOverController>();
            goc.GameOver();
        }
    }

    // called by enemy when they reach the end;
    public static void EndReached(int dmg)
    {
        bool gameover = gameController.health.SubtractHealth(dmg);
        if (gameover)
            EndGame();
    }

    //
    public static void EnemyKilled(int reward)
    {
        gameController.wallet.EnemyKillReward(reward);
    }

    public static bool Purchase(int cost)
    {
        bool purchasable = gameController.wallet.CheckMoney(cost);
        if (!purchasable) return false;
        gameController.wallet.SpendMoney(cost);
        return true;
    }

    public static void WaveCompleted(int reward)
    {
        gameController.wallet.WaveCompletionReward(reward);
    }

    //Add money to wallet for selling towers
    public static void SellTowerGetMoney(int amount)
    {
        gameController.wallet.AddMoney(amount);
    }


    // Start is called before the first frame update
    void Start()
    {
        if (nb_controllers > 0 && gameController != null)
        {
            Destroy(gameObject);
            return;
        }
        //DontDestroyOnLoad(gameObject);
        gameController = GetComponent<GameController>();
        nb_controllers += 1;

        // if no objects attached, find the objects
        if (wallet == null)
            wallet = FindObjectOfType<Wallet>();
        if (health == null)
            health = FindObjectOfType<PlayerHealthController>();
        if (map == null)
            map = FindObjectOfType<MapV2>();

        //GET SEED FROM MENU MANAGER
        MainMenuManagement mmm = FindObjectOfType<MainMenuManagement>();
        if (mmm != null)
        {
            StartGame(mmm.seed);
        }
        else
        {
            StartGame(15243);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartApplication()
    {
        EndGame(false);
        SceneManager.LoadScene("MainMenuV2");
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
