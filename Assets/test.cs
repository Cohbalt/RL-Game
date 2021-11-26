using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public Animation anim;
    public GameObject enemyPrefab1;
    void OnMouseDown() {
            anim = enemyPrefab1.GetComponent<Animation>();
            anim.Play("Hurt");
        }

    // Update is called once per frame
}
