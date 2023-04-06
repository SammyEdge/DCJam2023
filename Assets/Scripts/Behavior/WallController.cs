using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerMovement;

public class WallController : MonoBehaviour, Hittable//, Shiftable
{
    private GameObject Utils;
    private GameObject Player;
    public ObjectTypes HittableObjectType = ObjectTypes.Wall;
    private float timer;
    ObjectTypes Hittable.HittableObjectType { get => this.HittableObjectType; }
    //public TimeState timeState { get => this.timeState; set => this.timeState = value; }
    public TimeState timeState;

    // Start is called before the first frame update
    void Start()
    {
        //timer = 2;
        Utils = GameObject.FindGameObjectWithTag("Utils");
        Player = GameObject.FindGameObjectWithTag("Player");
        timeState = TimeState.Original;
    }

    // Update is called once per frame
    void Update()
    {
        // test proizvoditelnosti
        //timer -= Time.deltaTime;
        //if (timer < 0)
        //{
        //    print("action");
        //    timer = 2;
        //}
        // test proizvoditelnosti
    }

    public bool Hit()
    {
        if (gameObject.transform.parent.TryGetComponent<WallStuff>(out WallStuff wallStuff))
        {
            if (wallStuff.Wall && wallStuff.breakable && Utils.GetComponent<Utils>().FacingGameObject(gameObject))
            {
                //Player.GetComponent<PlayerStats>().ChangeStamina(1);
                if (Player.GetComponent<PlayerStats>().timeState == TimeState.Original)
                {
                        Player.GetComponent<PlayerStats>().ChangeEnergy(1);
                }
                else
                {
                    wallStuff.hp -= 1;
                    Player.GetComponent<PlayerStats>().TakeDamage(-1);
                }   

                    if (wallStuff.hp <= 0)
                    {
                        wallStuff.Wall = false;
                        wallStuff.WallObject.SetActive(false);
                        wallStuff.TorchObject.SetActive(false);
                        wallStuff.Torch = false;
                        Utils.GetComponent<Utils>().UpdateCursor(gameObject);
                    }
                }
                return true;
            }
            return true;
        }
        else
        {
            return false;
        }
    }
    public void OnMouseEnter()
    {
        if (gameObject.transform.parent.TryGetComponent<WallStuff>(out WallStuff wallStuff))
        {
            if (wallStuff.Wall && wallStuff.breakable)
            {
                Utils.GetComponent<Utils>().UpdateCursor(gameObject, CursorAction.Break);
            }
        }
    }
    public void OnMouseOver()
    {
        if (gameObject.transform.parent.TryGetComponent<WallStuff>(out WallStuff wallStuff))
        {
            if (wallStuff.Wall && wallStuff.breakable && !Player.GetComponent<PlayerMovement>().moving.Current)
            {
                Utils.GetComponent<Utils>().UpdateCursor(gameObject, CursorAction.Break);
            }
        }
    }
    public void OnMouseExit()
    {
        //print("wallexit");
        if (gameObject.transform.parent.TryGetComponent<WallStuff>(out WallStuff wallStuff))
        {
            if (wallStuff.Wall && wallStuff.breakable)
            {
                Utils.GetComponent<Utils>().UpdateCursor(gameObject);
            }
        }
    }

    public void Shift()
    {
        if (this.timeState == TimeState.Original)
        {
            this.timeState = TimeState.Shifted;
        }
        else
        {
            this.timeState = TimeState.Original;
        }
    }
}
