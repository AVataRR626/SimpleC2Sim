using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FilteredHideShow : MonoBehaviour
{
    public static List<FilteredHideShow> globalRegister;
    public string filterString;
    public bool disableOnStart;    
    public UnityEvent onShow;
    public UnityEvent onHide;
    public UnityEvent onStart;


    private void Start()
    {
        if (globalRegister == null)
            globalRegister = new List<FilteredHideShow>();

        if (!globalRegister.Contains(this))
        {
            globalRegister.Add(this);
            Debug.Log("---- FilteredHideShow().adding: " + filterString);
        }

        if (disableOnStart)
            gameObject.SetActive(false);

        onStart.Invoke();
    }

    public static void Hide(string criteria)
    {
        Show(criteria, false);
    }

    public static void Show(string criteria)
    {
        Debug.Log("Show: " + criteria);
        Show(criteria, true);
    }

    public static void Show(string criteria, bool showMode)
    {

        if (globalRegister != null)
        {

            int i = 0;
            while (i < globalRegister.Count)
            {
                if (globalRegister[i] != null)
                {

                    Debug.Log("Show: " + criteria + "| " + globalRegister[i].filterString);
                    if (globalRegister[i].filterString.Equals(criteria))
                    {

                        if (showMode)                            
                            globalRegister[i].gameObject.SetActive(true);                            
                        else
                        {
                            if (globalRegister[i].isActiveAndEnabled)
                                globalRegister[i].gameObject.SetActive(false);
                        }

                        if (showMode)
                            globalRegister[i].onShow.Invoke();
                        else
                            globalRegister[i].onHide.Invoke();
                    }
                    
                }
                else
                {
                    globalRegister.RemoveAt(i);
                    i--;
                }


                i++;
            }

        }
    }
}
