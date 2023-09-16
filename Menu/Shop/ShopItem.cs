using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface ShopItem
{

    Task<bool> isUnlocked();

    bool isSelected();

    Task<string> Buy();

    void Select();

    int getPrice();

}
