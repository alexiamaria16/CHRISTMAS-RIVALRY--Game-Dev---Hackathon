using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wrap_box : MonoBehaviour
{
    public GameObject giftPrefab, gift;

    public Transform giftPos;

    public bool hasGift = false;

    public float duration = 4f;
    public float start = 0f;
    public float now = 0f;

    public float giftValue = 0f;

    public bool countTime = false;
    public void startCraft( float reward )
    {
        countTime = true;
        start = Time.deltaTime;
        now = 0f;
        Debug.Log("start time: " + start);
        giftValue = reward;
    }
    public void stopCraft()
    {
        countTime = false;
        start = 0;
        now = 0;
        hasGift = false;
    }
    public GameObject pickGift()
    {
        hasGift = false;
        now = 0f;
        start = 0f;
        countTime = false;
        gift.GetComponent<gift>().reward = giftValue;
        return gift;
    }
    // Update is called once per frame
    void Update()
    {
        if (countTime)
        {
            now += Time.deltaTime;
            Debug.Log("Now: " + now);
            if (now - start > duration && hasGift == false)
            {
                Debug.Log("Item Crafted: ");
                countTime = false;
                gift = Instantiate(giftPrefab, transform);
                gift.transform.position = giftPos.position;
                
                hasGift = true;

            }
        }
    }
}
