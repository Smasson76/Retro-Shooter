using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChooseYourShip : MonoBehaviour
{
    public static ChooseYourShip instance;
    public Selection[] Ships;
    public GameObject PlayerObject;
    public GameObject PlayerInstance;
    public Animator animator;
    public Animator source_animator;
    public RuntimeAnimatorController RAC;
    private AnimatorOverrideController ORAC;
    public AnimationClip clip;
    public bool click_Next = false;
    //public bool click_Prev = false;
    public bool click_Play = false;
    private string current_state;
    private string prev_state;
    public int death_index;

    protected int index = 0;
    /**************************************************************************
    UI ELEMENTS
    **************************************************************************/
    private UnityEvent myEvent1 = new UnityEvent();
    private UnityEvent myEvent2 = new UnityEvent();
    public UnityEngine.UI.Button _playButton;
    public UnityEngine.UI.Button _nextButton;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start(){
        if(GameManager.instance.PlayerInstance == null){
            GameManager.instance.SpawnPlayer();
            PlayerInstance = GameObject.FindWithTag("Player");
        }
        else{
            PlayerInstance = GameManager.instance.PlayerInstance;
        }
        animator = PlayerInstance.GetComponentInChildren<Animator>();
        source_animator = GameManager.instance.PlayerObject.GetComponentInChildren<Animator>();
        PlayerInstance.transform.position = new Vector2(0f,0f);
        animator.ResetTrigger(Ships[index].Name);
        current_state = "stateBlue";
        /*************************************************************
        Button stuff
        *************************************************************/
        _playButton.onClick.AddListener(Play);
        _nextButton.onClick.AddListener(Next);
    }

    void Update(){
        //ClearStates();
        if(!GameManager.instance.selection_has_been_made){
        prev_state = Ships[index].Name;
        if(click_Next){
            Debug.Log("Next ship has been called");
            if(index < Ships.Length-1 && click_Next){
                index++;
                click_Next = false;
            }
            else{
                index = 0;
                click_Next = false;
            }
        }
         Animator master_animator = GameManager.instance.PlayerObject.GetComponentInChildren<Animator>();
         master_animator = this.source_animator;
         animator.SetTrigger(Ships[index].Name);
         if(click_Play){
            Debug.Log("Play called!");
            GameManager.instance.setAnimation_string(Ships[index].Name);
            //GameManager.instance.PlayerObject.GetComponent<SimpleMovement>().set_state(Ships[index].Name);
            GameManager.instance.SelectionMade();
            musicManager.Instance.playSound("selected");
            click_Play = false;
         }
         clip = Ships[index].shipClip;
        Debug.Log("Index : " + index + "\nName : " + Ships[index].Name + "\nClip : " + Ships[index].shipClip);
        Debug.Log("Animation Clip : " + clip);
        Debug.Log("Ship Color : " + PlayerInstance.GetComponentInChildren<SpriteRenderer>().sharedMaterial.GetColor("_Color"));
        }
        Debug.Log("attempting to set " + GameManager.instance.getAnimation_string());
        animator.SetTrigger(GameManager.instance.PlayerObject.GetComponent<SimpleMovement>().get_state());
    }
    public void Next(){
        Debug.Log("called Next()");
        musicManager.Instance.playSound("nextSkin");
        click_Next = true;
        Debug.Log("Bool field : click_Next = " + click_Next);

    }
    public void Play(){
        click_Play = true;
    }
    public void ResetSkin(string skinname)
    {
        Animator respawn_anim = new Animator();
        respawn_anim = GameManager.instance.PlayerInstance.GetComponentInChildren<Animator>();
        respawn_anim.SetTrigger(skinname);
    }
    public void ClearStates(){
        for(int i=0;i<Ships.Length -1;i++){
            animator.ResetTrigger(Ships[i].Name);
        }
    }
}