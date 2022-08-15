using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int digit;
    public char shape;
    public bool move = false;
    public Vector3 moveTo;
    public float speed = 20f;

    // Start is called before the first frame update
    void Update()
    {
       if(move){
           transform.position = Vector3.MoveTowards(transform.position, moveTo, speed*Time.deltaTime);
       }
    }

    public int getDigit(){return this.digit;}

    public void setOnPlace(Vector3 place, float speed){
        this.move = true;
        this.moveTo = place;
        this.speed = speed;
    }
}
