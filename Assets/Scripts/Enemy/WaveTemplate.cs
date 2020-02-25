using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTemplate : MonoBehaviour
{
    //key = level, string = wave format
    static Dictionary<int, string> m_waves = new Dictionary<int, string>();

    //wave format
    // "enemyType:numberOfSpawns(int);enemyType2:numberOfSpawns2(int)"
    void Start()
    {
        DefineWaveTemplate();
    }

    static public Dictionary<string, int> FetchWave(int level)
    {
        string wave = m_waves[level];
        string[] waveComponents = wave.Split(';');
        Dictionary<string, int> waveDict = new Dictionary<string, int>();
        for (int i = 0; i < waveComponents.Length; i++)
        {
            string[] type = waveComponents[i].Split(':');
            waveDict.Add(type[0], int.Parse(type[1]));
        }
        return waveDict;
    }

    static public void DefineWaveTemplate()
    {
        // can be adapted to read from a file
        m_waves.Add(1, "normalSmall:1");
        m_waves.Add(2, "normalSmall:3");
        m_waves.Add(3, "normalSmall:8");
        m_waves.Add(4, "normalSmall:10");
        m_waves.Add(5, "normal:1");

        m_waves.Add(6, "normalSmall:10");
        m_waves.Add(7, "normalSmall:5;fireSmall:5");
        m_waves.Add(8, "fireSmall:7");
        m_waves.Add(9, "fireSmall:10;normalSmall:5");
        m_waves.Add(10, "fire:1");

        m_waves.Add(11, "normalSmall:10");
        m_waves.Add(12, "fireSmall:2;iceSmall:2");
        m_waves.Add(13, "fireSmall:5;iceSmall:5");
        m_waves.Add(14, "fireSmall:5;normalSmall:5");
        m_waves.Add(15, "ice:1");

        m_waves.Add(16, "normalSmall:15");
        m_waves.Add(17, "normal:1");
        m_waves.Add(18, "normal:3");
        m_waves.Add(19, "normal:3");
        m_waves.Add(20, "normal:5");

        m_waves.Add(21, "natureSmall:5;fireSmall:5;iceSmall:5");
        m_waves.Add(22, "natureSmall:10");
        m_waves.Add(23, "natureSmall:10;fireSmall:5");
        m_waves.Add(24, "natureSmall:10;iceSmall:5");
        m_waves.Add(25, "nature:1");

        m_waves.Add(26, "normalSmall:15");
        m_waves.Add(27, "fireSmall:15");
        m_waves.Add(28, "iceSmall:15");
        m_waves.Add(29, "natureSmall:15");
        m_waves.Add(30, "normal:15");

        m_waves.Add(31, "arcaneSmall:10");
        m_waves.Add(32, "normal:15");
        m_waves.Add(33, "natureSmall:5;iceSmall:5;normal:5");
        m_waves.Add(34, "ice:5;normal:10");
        m_waves.Add(35, "ice:10");

        m_waves.Add(36, "arcaneSmall:15");
        m_waves.Add(37, "arcaneSmall:15");
        m_waves.Add(38, "arcaneSmall:15");
        m_waves.Add(39, "arcaneSmall:15");
        m_waves.Add(40, "ice:15");

        m_waves.Add(41, "normal:20");
        m_waves.Add(42, "normal:20");
        m_waves.Add(43, "fire:20");
        m_waves.Add(44, "ice:15");
        m_waves.Add(45, "nature:15");

        m_waves.Add(46, "normal:20");
        m_waves.Add(47, "fire:10;normal:10");
        m_waves.Add(48, "ice:10;fire:10");
        m_waves.Add(49, "nature:8;ice:10");
        m_waves.Add(50, "nature:10;ice:10");

        m_waves.Add(51, "nature:10;ice:10;fire:3");
    }
}
