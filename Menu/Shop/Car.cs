using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Car : MonoBehaviour, ShopItem
{

    [SerializeField] public int id;
    [SerializeField] public int price;

    private PostRacerAPI postRacerAPI;

    public void Deselect()
    {
        
    }

    public async Task<string> Buy()
    {
        return await postRacerAPI.BuyCar(PlayerPrefs.GetString("username"), id, price);
    }

    public bool isSelected()
    {
        if (PlayerPrefs.GetInt("selectedcolor", 0) == id)
        {
            return true;
        }
        return false;
    }

    public async Task<bool> isUnlocked()
    {
        if (id == 0)
            return true;
        postRacerAPI = new PostRacerAPI();
        price = (int)await postRacerAPI.GetCarPrice(id);
        List<int> ids = await postRacerAPI.getCars(PlayerPrefs.GetString("username"));
        return ids.Contains(id);
    }

    public void Select()
    {
        PlayerPrefs.SetInt("selectedcolor", id);
    }

    public int getPrice()
    {
        return price;
    }

}
