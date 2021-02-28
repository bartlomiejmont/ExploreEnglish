using UnityEngine;
using System;
using System.Diagnostics;
using Random = UnityEngine.Random;

public class WordSpawnMulti : MonoBehaviour
{
    public GameObject word;

    public float maxRange = 6f;
    public float minRange = 3f;
    public float spawnInterval = 1f;
    public float taskTimer;
    

    private float timer = 0.0f;
    private float minimumY;
    private float maximumY;
    private float minimumX;
    private float maximumX;

    void Start()
    {
        minimumY = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -Camera.main.transform.position.z)).y;
        maximumY = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, -Camera.main.transform.position.z)).y;
        minimumX = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -Camera.main.transform.position.z)).x;
        maximumX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, -Camera.main.transform.position.z)).x;
    }

    void Update()
    {
        taskTimer += Time.deltaTime;
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer -= spawnInterval;
            InstantiateRandomWordObject();
        }
    }

    void InstantiateRandomWordObject()
    {
        var targetPending = true;
        float spawnX = 0;
        float spawnY = 0;

        while (targetPending)
        {

            if (Random.value > 0.5f)
            {
                Range[] rangesX =
                {
                    new Range(minimumX - maxRange, minimumX - minRange),
                    new Range(maximumX + minRange, maximumX + maxRange)
                };
                spawnX = RandomValueFromRanges(rangesX);
                spawnY = Random.Range(minimumY - maxRange, maximumY + maxRange);
            }
            else
            {
                Range[] rangesY =
                {
                    new Range(minimumY - maxRange, minimumY - minRange),
                    new Range(maximumY + minRange, maximumY + maxRange)
                };
                spawnX = Random.Range(minimumX - maxRange, maximumX + maxRange);
                spawnY = RandomValueFromRanges(rangesY);
            }

            // Avoiding spawning 2 word on top of each other
            Collider[] colliders = Physics.OverlapBox(new Vector3(spawnX, spawnY, 0), new Vector3(1.5f, 0.5f, 0));
            targetPending = colliders.Length > 0;
        }

        GameObject asteroidObject = Instantiate(word, new Vector3(spawnX, spawnY, 0), Quaternion.identity);
    }

    public static float RandomValueFromRanges(Range[] ranges)
    {
        if (ranges.Length == 0)
            return 0;
        float count = 0;
        foreach (Range r in ranges)
            count += r.range;
        
        float sel = Random.Range(0, count);
        foreach (Range r in ranges)
        {
            if (sel < r.range)
            {
                return r.min + sel;
            }
            sel -= r.range;
        }
        throw new Exception("This should never happen");
    }

    public struct Range
    {
        public float min;
        public float max;
        public float range { get { return max - min + 1; } }
        public Range(float minRange, float maxRange)
        {
            min = minRange; 
            max = maxRange;
        }
    }
}