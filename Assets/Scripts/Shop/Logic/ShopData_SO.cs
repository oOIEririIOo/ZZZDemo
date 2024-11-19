using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shop", menuName = "Shop/Shop Data")]
public class ShopData_SO : ScriptableObject
{
    public List<Good> goodLists = new List<Good>();


    [System.Serializable]
    public class Good
    {
        public ItemData_SO itemData;
        public int price;
    }


}
