using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rowButton : MonoBehaviour
{
    public int col;
    public AudioSource audio;

    public int getCol(){
        return col;
    }


    public void SetCardOnPlace(){
        audio.Play();
        GameManager.currDrawn.setOnPlace(this.transform.position, 30f);
        GameManager.currColumn = this.col;
        GameManager.cardOnRightPlace = true;
    }
}
