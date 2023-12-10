using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parts_box : MonoBehaviour
{
    public GameObject part;

    public GameObject getObject( GameObject target)
    {
        GameObject new_part = Instantiate(part, transform, true);
        new_part.transform.parent = target.transform;
        new_part.transform.localPosition = new Vector3(0, 0, 0);
        return new_part;
    }
}

