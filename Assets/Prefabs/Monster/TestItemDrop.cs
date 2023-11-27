using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Managers;
using UnityEngine;
using Assets.Scripts.Item;

public class TestItemDrop : Poolable
{
    public ItemData data = null;
    [SerializeField] private ItemHatchery hatchery;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ItemPickUp();
            UpdateUI();
        }
    }

    private void ItemPickUp()
    {
        hatchery.PickItem(this);
    }
    private void UpdateUI()
    {

    }



}
