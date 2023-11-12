using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSelector : MonoBehaviour
{
   public Selection[] selections;
   public GameObject Ship;
   public GameObject instanceShip;
   protected Animator animator;
   protected AnimatorOverrideController acontrol;
   protected int skinIdx=0;
   public bool wantNext=false;public bool wantPrev = false;public bool wantPlay = false;
   private Vector2 DispScale = new Vector2(250f,250f);
   [SerializeField] public AnimationClip shipAnim;
   void Start(){
    GameManager.instance.SpawnPlayer();
    instanceShip = GameManager.instance.PlayerInstance;
    instanceShip.transform.position = new Vector2(0f,0f);

    animator = Ship.GetComponentInChildren<Animator>();
    Debug.Log("Found animator " + animator.tag);
    acontrol = new AnimatorOverrideController(Resources.Load("Assets/Art/PlayerSprites/Ship_Redo_blue_0") as RuntimeAnimatorController);//animator.runtimeAnimatorController);//new AnimatorOverrideController(animator.runtimeAnimatorController);
    Debug.Log("Instance ID : " + acontrol.GetInstanceID());
    //animator.runtimeAnimatorController = Ship.GetComponentInChildren<RuntimeAnimatorController>();
    //animator.runtimeAnimatorController = Resources.Load("Assets/Art/PlayerSprites/Ship_Redo_blue_0") as RuntimeAnimatorController;//control; 
    //RuntimeAnimatorControllerinstanceShip.AddComponent(typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        //instanceShip.AddComponent<RuntimeAnimatorController>(animator.runtimeAnimatorController);
    Debug.Log("Animator Runtime Controller found! ID : " + animator.runtimeAnimatorController.GetInstanceID());
    animator.runtimeAnimatorController = acontrol;
    //skinIdx = 0;
    /*wantNext = false;
    wantPrev = false;
    wantPlay = false;*/
    Debug.Log("Player Object : " + instanceShip + "\nIndex : " + skinIdx + "\nBool Field : <" + wantNext +"," + wantPrev + "," + wantPlay + ">");
   }
  public void Update(){
    if(wantNext){ 
        skinIdx++;//%selections.Length;
        if(skinIdx>=selections.Length){
            Debug.Log("Error: exceeded array index!");
            Next();
        }
        else{
            acontrol["shot"] = selections[skinIdx].shipClip;
            animator.Play(selections[skinIdx].Name);
            shipAnim = acontrol["shot"];
            Next();
        }
        Debug.Log("Index : " + skinIdx);
        Debug.Log("Next skin is now active -> " + selections[skinIdx].Name);
    }
    if(wantPrev){
        skinIdx--; 
        if(skinIdx < 0){
            Debug.Log("Error: array index cannot be less than zero");
            Prev();
        }
        else{
            //skinIdx = (skinIdx--);//%selections.Length;
            acontrol["shot"] = selections[skinIdx].shipClip;
            //animator.Play(selections);
            Prev();
        }
    }
    if(wantPlay){
        shipAnim = acontrol["shot"];
        Debug.Log("Call next process.");
        Play();
        GameManager.instance.MainMenu();
    }
    shipAnim = acontrol["shot"];
    animator.Play(selections[skinIdx].Name);
    //Debug.Log("Player Object : " + instanceShip + "\nIndex : " + skinIdx + "\nBool Field : <" + wantNext +"," + wantPrev + "," + wantPlay + ">");
  }
  public void Next(){
    wantNext = !wantNext;
    Debug.Log("Next ship pressed");
  }
  public void Prev(){
    wantPrev = !wantPrev;
  }
  public void Play(){
    wantPlay = !wantPlay;
  }
}