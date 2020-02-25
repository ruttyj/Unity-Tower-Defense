using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * ##################################################
 *                  ODDS TABLE
 * ##################################################
 * 
 * @Author          Jordan Rutty
 * @Description     Can be used for drop tables or random generation based on item weights.
 *
 * @Usage
 *  lootTable = new OddsTable();
 *  lootTable.Add("GoldNugget",  5,  mNuggetPrefab);
 *  lootTable.Add("GoldBar",     3,  mBarPrefab);
 *  lootTable.Add("GoldBundle",  1,  mBundlePrefab);
 *  string chosenItem = lootTable.Roll();
 *  GameObject prefab = (GameObject)lootTable.GetPayload(chosenItem);
 *
 */
class OddsTable
{
    float mOddsTotal;
    SortedDictionary<string, Vector2> mOdds;
    Hashtable mOddPayload;

    public OddsTable()
    {
        mOdds = new SortedDictionary<string, Vector2>();
        mOddPayload = new Hashtable();
        mOddsTotal = 0;
    }

    // Add item with weight and optional payload
    public void Add(string key, float weight, object payload = null)
    {
        mOdds.Add(key, new Vector2(mOddsTotal, mOddsTotal += weight));
        mOddPayload.Add(key, payload);
    }

    // Get a key based on weighted odds
    public string Roll()
    {
        return getResultKey(Random.Range(0.0f * 1000F, mOddsTotal * 1000F) / 1000F);
    }
    public string GetForNormalizedRoll(float dec)
    {
        Debug.Log(dec * mOddsTotal);
        return getResultKey(dec * mOddsTotal);
    }

    protected string getResultKey(float randomNumber)
    {
        string result = "None";
        foreach (KeyValuePair<string, Vector2> entry in mOdds)
        {
            if (entry.Value.x <= randomNumber && randomNumber <= entry.Value.y)
            {
                result = entry.Key;
                break;
            }
        }
        return result;
    }



    // Retreive the object associated to that key
    public object GetPayload(string key)
    {
        if (mOddPayload.ContainsKey(key))
            return mOddPayload[key];
        return null;
    }
}
