using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSD_AI : MonoBehaviour
{
    public Transform pet;
    private Animator anim;
    public GameObject poop_GO;
    public GameObject food_GO;
    public GameObject leaf_GO;
    public GameObject detector;
    public GameObject stress_emot;
    public GameObject sleep_emot;
    public GameObject happy_emot;
    public float screen_lim_left;
    public float screen_lim_right;
    public float move_dir;
    public float energy;
    public float boredom;
    public float stress;
    public float poopy;
    public float food;
    public bool stressed;
    public bool sleeping;
    public bool happy;
    public bool walking;
    public float energy_max;
    public float stress_max;
    public float boredom_max;
    public float food_max;
    public float poop_max;
    public float playmode;
    public float spawn_height;
    public float detect_range;
    public static WSD_AI instance;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Application.targetFrameRate = 10;
        if(!anim) anim = pet.GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        //sleep condition
        if (energy <= 0)
        {
            sleeping = true;
            walking = false;
        }
        else if (energy > energy_max)
        {
            energy = energy_max;
            sleeping = false;
        }
        //sleep
        if (sleeping)
        {
            energy += 3;
            stress -= 2;
        }
        //wake
        else
        {
            //walk condition
            if (boredom >= boredom_max)
            {
                walking = true;
                if(move_dir == 0) move_dir = Random.Range(0, 2) * 2 - 1;
            }
            else if (boredom <= 0)
            {
                walking = false;
            }
            //walking
            if (walking)
            {
                energy-=2;
                boredom--;
            }
            //sitting
            else
            {
                energy--;
                boredom+=2;
                move_dir = 0;
            }
            //move
            pet.position += Vector3.right * Time.deltaTime * move_dir;
            if (pet.position.x > screen_lim_right)
            {
                move_dir = -1;
            }
            else if (pet.position.x < screen_lim_left)
            {
                move_dir = 1;
            }
            if (move_dir > 0 &&walking)
            {
                pet.localEulerAngles = Vector3.up * 180;
            }
            else if (move_dir < 0 && walking) {
                pet.localEulerAngles = Vector3.zero;
            }
        }
        //meta_cognition
        if (stress <= 0)
        {
            happy = true;
        }
        else if (stress > stress_max)
        {
            stressed = true;
        }
        else
        {
            happy = false;
            stressed = false;
        }
        ////food cycle
        if (food > 0)
        {
            energy++;
            poopy++;
            food--;
            stress--;
        }
        else
        {
            stress+=2;
        }
        if (poopy >=poop_max)
        {
            Poop();
            poopy -= poop_max;
        }
        stress += Mathf.Ceil(transform.childCount / 2f);

        stress = Mathf.Clamp(stress, 0, stress_max * 2f);
        energy = Mathf.Clamp(energy, 0, energy_max * 2f);
        food = Mathf.Clamp(food, 0, food_max * 2f);
        boredom = Mathf.Clamp(boredom, 0, boredom_max * 2f);

        happy_emot.SetActive(happy);
        sleep_emot.SetActive(sleeping);
        stress_emot.SetActive(stressed);
        anim.SetBool("sleeping", sleeping);
        anim.SetBool("walking", walking);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(detector.transform.position, detect_range);
    }
    public void Clean() {
        foreach(Transform child in transform){
            GameObject.Destroy(child.gameObject);
        }
    }
    public void Food()
    {
        Vector3 spawn_pos = Vector3.right * Random.Range(screen_lim_left, screen_lim_right) + Vector3.up * spawn_height;
        GameObject.Instantiate(food_GO, spawn_pos, Quaternion.identity, transform);
    }
    public void Poop()
    {
        GameObject.Instantiate(poop_GO, pet.position, Quaternion.identity, transform);
    }
    public void Play()
    {
        move_dir *= -1;
        sleeping = false;
        walking = true;
        boredom += 30;
        energy += 20;
        Spawn_Leaf();
    }
    public void Spawn_Leaf() {
        Vector3 spawn_pos = Vector3.right * Random.Range(screen_lim_left, screen_lim_right) + Vector3.up * spawn_height;
        GameObject.Instantiate(leaf_GO, spawn_pos, Random.rotation);
    }
}
