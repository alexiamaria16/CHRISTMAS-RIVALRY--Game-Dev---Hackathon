using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class assembly_box : MonoBehaviour
{
    public GameObject[] parts_needed;
    public GameObject finished_toyPrefab, toy;

    public Transform toyPos;

    public TMP_Text mat1, mat2;

    public int[] parts_number;
    public int[] parts_received;

    public bool canCraft = false;
    public bool hasToy = false;

    public float duration = 4f;
    public float start = 0f;
    public float now = 0f;

    public bool countTime = false;
    public void startCraft()
    {
        countTime = true;
        start = Time.deltaTime;
        Debug.Log("start time: " + start);
    }
    public void stopCraft()
    {
        countTime = false;
        start = 0;
        now = 0;
        Debug.Log("stop craft: ");
    }
    public bool ReceivePart(GameObject part)
    {
        for ( int i = 0; i < parts_needed.Length; i++ )
        {
            if ( part.tag == parts_needed[i].tag )
            {
                if ( parts_number[i] > parts_received[i] )
                {
                    parts_received[i]++;
                    bool isReady = checkPartsCompleted();
                    if (isReady == true)
                        canCraft = true;
                    return true;
                }
            }
        }
        return false;
    }
    public bool checkPartsCompleted()
    {
        mat1.text = parts_received[0].ToString() + "/" + parts_number[0].ToString();
        if (parts_number.Length > 1)
        {
            mat1.text = parts_received[0].ToString() + "/" + parts_number[0].ToString();
            mat2.text = parts_received[1].ToString() + "/" + parts_number[1].ToString();
        }
        bool complete = true;
        for (int i = 0; i < parts_number.Length; i++)
            if (parts_number[i] != parts_received[i])
            {
                
                    
                //Debug.Log("Present Not Complete");
                complete = false;
            }
        return complete;
    }

    public GameObject pickToy( GameObject parent )
    {
        hasToy = false;
        return toy;
    }
    // Update is called once per frame
    void Update()
    {
        if ( countTime )
        {
            now += Time.deltaTime;
            Debug.Log("Now: " + now);
            if ( now - start > duration)
            {
                Debug.Log("Item Crafted: ");
                canCraft = false;
                countTime = false;
                toy = Instantiate(finished_toyPrefab, transform);
                toy.transform.position = toyPos.position;
                for (int i = 0; i < parts_received.Length; i++)
                    parts_received[i] = 0;
               
                hasToy = true;
            }
        }
    }
}
