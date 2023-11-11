using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseYourShip : MonoBehaviour
{
    public Selection[] Ships;
    public GameObject PlayerObject;
    public GameObject PlayerInstance;
    public Animator animator;
    public Animator instancedAnimator;
    public RuntimeAnimatorController RAC;
    private AnimatorOverrideController ORAC;
    public AnimationClip clip;
    public bool click_Next = false;
    public bool click_Prev = false;
    public bool click_Play = false;

    protected int index = 0;

    void Start(){
        GameManager.instance.SpawnPlayer();
        PlayerInstance = GameObject.FindWithTag("Player");
        animator = PlayerInstance.GetComponentInChildren<Animator>();
        //animator.runtimeAnimatorController = Resources.Load("Assets/Art/PlayerSprites/Ship_Redo_blue_0") as RuntimeAnimatorController;
        //RuntimeAnimatorController iRAC = Resources.Load("Assets/Art/PlayerSprites/Ship_Redo_blue_0") as RuntimeAnimatorController;
        PlayerInstance.transform.position = new Vector2(0f,0f);
        
        /*RAC = new AnimatorOverrideController(animator.runtimeAnimatorController);//PlayerInstance.GetComponentInChildren<RuntimeAnimatorController>();
        animator.runtimeAnimatorController = new AnimatorOverrideController(RAC);
        animator.runtimeAnimatorController = ORAC;*/
        animator.ResetTrigger(Ships[index].Name);
    }
    void Update(){
        animator = PlayerInstance.GetComponentInChildren<Animator>();
        //animator.runtimeAnimatorController = Resources.Load("Assets/Art/PlayerSprites/Ship_Redo_blue_0") as RuntimeAnimatorController;
        //instancedAnimator = animator;
        //instancedAnimator = PlayerInstance.GetComponent<Animator>();
        animator.ResetTrigger(Ships[index].Name);
        if(click_Next){
            //toggleBool(click_Next);
            //animator.ResetTrigger("clicked_next");
            Debug.Log("Next ship has been called");
            if(index < Ships.Length-1 && click_Next){
                index++;
                click_Next = false;
            }
            else{
                index = 0;
                click_Next = false;
            }
            Debug.Log("Index : " + index + "\nName : " + Ships[index].Name + "\nClip : " + Ships[index].shipClip);
            //animator.Play(Ships[index].Name);
            //Destroy(PlayerInstance);
            //GameManager.instance.SpawnPlayer();
            //animator.ResetTrigger("clicked_next");
        }
        if(click_Prev){
            //toggleBool(click_Prev);
            //animator.ResetTrigger("clicked_prev");
            //animator.ResetTrigger("clicked_next");
            Debug.Log("Previous ship has been called");
            //int counter = 0;
            while(index != (index-1)){
                Next();
            }
            click_Prev = false;
            Debug.Log("Index : " + index + "\nName : " + Ships[index].Name + "\nClip : " + Ships[index].shipClip);
            //animator.clip = clip;
            //PlayerInstance.GetComponentInChildren<Animator>().Play(Ships[index].Name);
        }
         animator.SetTrigger(Ships[index].Name);
    }
    void toggleBool(bool arg){
        arg = !arg;
    }
    public void Next(){
        toggleBool(click_Next);
        //animator.ResetTrigger(Ships[index].Name);
    }
    public void Prev(){
        toggleBool(click_Prev);
        //animator.ResetTrigger(Ships[index].Name);
    }
    public void Play(){
        Debug.Log("Play has been called");
        GameManager.instance.MainMenu();
    }
}