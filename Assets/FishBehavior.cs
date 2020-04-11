using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class FishBehavior : MonoBehaviour
{

    private Rigidbody rb;

    //Movement variables
    private bool startMovement = false;
    private Vector3 movementVector3 = Vector3.zero;

    //Rotation variables
    private bool startRotation = false;
    private float rotationDeg;
    private float rotationThusFar;
    
    //Timing
    private float timeElapsed = 0.0f;
    private float timeThreshold = 1.0f;

    public Transform facingMarker;

    private enum Movetype {
        ROTATION,
        MOTION,
        NONE
    }

    private List<Movetype> moves = new List<Movetype>();

    // Start is called before the first frame update
    void Start()
    {
 

        rb = GetComponent<Rigidbody>();
        
        //A initial force pulse
    
    }

    private void AddMoreMoves()
    {
        Random r = new Random();
        
        for (int i = 0; i < 10; i++)
        {
            int moveIdx = r.Next(0, 2);
            Movetype move = Movetype.NONE;
            
            switch (moveIdx)
            {
                case 0:
                    move = Movetype.MOTION;
                    break;
                case 1:
                    move = Movetype.ROTATION;
                    break;
                default:
                    move = Movetype.NONE;
                    break;
            }
        
            moves.Add(move);
            
        }
        
    }
    
    private Movetype getNextMove()
    {
        if (moves.Count == 0)
        {
            AddMoreMoves();
            return Movetype.NONE;
        }

        Movetype move = moves[0];
        moves.RemoveAt(0);
        
        return move;
    }

    private void processMove(Movetype move)
    {
        switch (move)
        {
            case Movetype.MOTION:
                startMove(25);
                break;
            case Movetype.ROTATION:
                Random r = new Random();
                startRot(r.Next(0,90));
                break;
        }
    }
    
    private Vector3 GetFacing()
    {

        Vector3 facing = facingMarker.position - transform.position;
        
        return facing.normalized;
    }

    private void startMove(float mag)
    {
        Vector3 facingVector = GetFacing();

        movementVector3 = facingVector * mag;
        startMovement = true;
    }
    
    private void startRot(float deg)
    {
        startRotation = true;
        rotationThusFar = 0.0f;
        rotationDeg = deg;
    }

    private void applyRotBehavior()
    {
        if (startRotation)
        {
            float rotDelta = 1.0f;
            transform.Rotate(Vector3.forward, rotDelta);

            rotationThusFar += rotDelta;

            if (rotationThusFar >= rotationDeg)
            {
                startRotation = false;
                rotationThusFar = 0.0f;
            }
            
        }
    }

    private void applyMove()
    {
        if (startMovement == true)
        {

            Vector3 forcePulse = movementVector3;
            rb.AddForce(forcePulse, ForceMode.Impulse);
            startMovement = false;
        }
    }

    private bool checkTimer()
    {
        return timeElapsed >= timeThreshold;
    }

    private void updateTimer()
    {
        timeElapsed += Time.deltaTime;
    }

    private void resetTimer()
    {
        timeElapsed = 0.0f;
    }
    
    // Update is called once per frame
    void Update()
    {
        updateTimer();

        if (checkTimer())
        {
            processMove(getNextMove());
            resetTimer();
        }
        
        //Movements
        applyMove();
        applyRotBehavior();
    }
}
