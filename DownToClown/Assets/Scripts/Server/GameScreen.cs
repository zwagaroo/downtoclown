using UnityEditor.Search;
using UnityEngine;

public class GameScreen : MonoBehaviour
{
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide() 
    {
        gameObject.SetActive(false);
    }


    public virtual void Update()
    {

    }

}