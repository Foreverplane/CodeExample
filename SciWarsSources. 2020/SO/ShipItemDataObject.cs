using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ShipItemData", menuName = "Ship Data", order = 51)]
public class ShipItemDataObject : ScriptableObject
{
    public NameData nameData;
    public DescriptionData descriptionData;
    public CurrencyData currencyData;
    public StatsData statsData;
    public InventoryData inventoryData;
    
}