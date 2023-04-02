using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Breaker : MonoBehaviour, IPointerClickHandler
{

    // Это следующий вариант если с рейкастом по ходу дела что-то не срастется. Этот скрипт мы вешаем на объект по которому хотим кликать. При клике выполняется OnPointerClick
    public GameObject Parent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
    }
}
