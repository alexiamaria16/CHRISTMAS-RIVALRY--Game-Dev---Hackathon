using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deliveryBox : MonoBehaviour
{

    public bool isBonusDelivery = false;
    public GameManager manager;
    
    public void receiveGift( GameObject giftObject, bool player1 )
    {
        gift script = giftObject.GetComponent<gift>();
        float totalReward = script.reward;
        if (isBonusDelivery == true)
            totalReward *= 1.3f;
        manager.receiveOrder(giftObject.tag, totalReward, player1);
        Destroy(giftObject);
    }
}
