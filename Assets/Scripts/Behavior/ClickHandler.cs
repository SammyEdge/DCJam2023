using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour
{
    private KeyBinds keyBinds = new KeyBinds();
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    print("Click");
        //    KickWall();
        //}
        if (Input.GetKeyDown(keyBinds.LeftMouseClick))
        {

            // tut proveryaem 4to za obekt kliknuli
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 8))
            {
                // no dobavit suda proverku, smotrim li mi licom v obekt
                // .TryGetComponent rewrite
                if (hit.transform.gameObject.TryGetComponent<Hittable>(out Hittable hittable))
                {
                    //print(hit.transform.gameObject.GetComponent<InitialHitHandler>().ObjectType);
                    //(hit.transform.gameObject.GetComponent<Hittable>() as Hittable).Hit();
                    hittable.Hit();
                }


                /*if (hit.transform.gameObject.GetComponent<InitialHitHandler>().ObjectType == ObjectTypes.Wall)
                {
                    KickWall(hit);
                }
                if (hit.transform.gameObject.GetComponent<InitialHitHandler>().ObjectType == ObjectTypes.Monster)
                {
                    print(hit.transform.gameObject.GetComponent<InitialHitHandler>().ObjectType);
                    hit.transform.gameObject.GetComponent<MonsterController>().PlayHitAnimation();
                    hit.transform.gameObject.GetComponent<MonsterController>().MeleeStrikeMonster();
                }*/
                if (hit.transform.gameObject.GetComponent<InitialHitHandler>().ObjectType == ObjectTypes.LootSack)
                {
                    //print(ObjectTypes.LootSack);
                }

                // Buttons test
                /*
                if (hit.transform.gameObject.GetComponent<InitialHitHandler>().ObjectType == ObjectTypes.Redpill)
                {
                    Cursor.lockState = CursorLockMode.Confined;
                }
                if (hit.transform.gameObject.GetComponent<InitialHitHandler>().ObjectType == ObjectTypes.Bluepill)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                }
                if (hit.transform.gameObject.GetComponent<InitialHitHandler>().ObjectType == ObjectTypes.Thirdpill)
                {
                    Cursor.lockState = CursorLockMode.None;
                }*/
            }
        }
        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    print("Click");
        //    KickWall();
        //}
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;
        //if (Physics.Raycast(ray, out hit, 7))
        //{
        //    Debug.Log(hit.transform.gameObject.name);
        //}

    }

    public void KickWall(RaycastHit hit)
    {
        if (hit.transform.parent.TryGetComponent<WallStuff>(out WallStuff wallStuff))
        {
            if (wallStuff.Wall)
            {
                if (wallStuff.breakable)
                {
                    wallStuff.hp -= 1;
                    if (wallStuff.hp <= 0)
                    {
                        wallStuff.Wall = false;
                        wallStuff.WallObject.SetActive(false);
                        wallStuff.TorchObject.SetActive(false);
                        wallStuff.Torch = false;
                    }
                }
            }
        }
    }
}

