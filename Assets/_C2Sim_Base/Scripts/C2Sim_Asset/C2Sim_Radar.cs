using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class C2Sim_Radar : MonoBehaviour
{
    public static int RadarLayer = 11;
    public List<C2Sim_Asset> contactList;
    public UnityEvent onEnter;
    public UnityEvent onExit;
    public bool selfManage = true;

    private void Start()
    {
        if(contactList == null)
            contactList = new List<C2Sim_Asset>();
    }

    private void Update()
    {
        if (selfManage)
        {
            for (int i = 0; i < contactList.Count; i++)
            {
                C2Sim_Asset a = contactList[i];
                if (a == null)
                {
                    contactList.Remove(a);
                    i--;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (selfManage)
        {
            C2Sim_Asset a = other.GetComponent<C2Sim_Asset>();

            if (a != null)
                AddContact(a);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (selfManage)
        {       
            C2Sim_Asset a = other.GetComponent<C2Sim_Asset>();

            if (a != null)
                RemoveContact(a);
        }
    }

    public void AddContact(C2Sim_Asset newContact)
    {

        if (!contactList.Contains(newContact))
        {
            contactList.Add(newContact);
            onEnter.Invoke();
        }
    }

    public void RemoveContact(C2Sim_Asset oldContact)
    {
        if (contactList.Contains(oldContact))
        {
            contactList.Remove(oldContact);
            onExit.Invoke();
        }
    }
}
