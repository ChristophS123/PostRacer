using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Hat : MonoBehaviour, ShopItem
{

    [SerializeField] public int id;
    public int price;

    private PostRacerAPI postRacerAPI;

    private async void Start()
    {
        
    }

    public void Deselect()
    {
        PlayerPrefs.SetInt("selectedhat", -1);
    }

    public async Task<string> Buy()
    {
        return await postRacerAPI.BuyHat(PlayerPrefs.GetString("username"), id, price);
    }

    public bool isSelected()
    {
        if(PlayerPrefs.GetInt("selectedhat", -1) == id)
        {
            return true;
        }
        return false;
    }

    public async Task<bool> isUnlocked()
    {
        postRacerAPI = new PostRacerAPI();
        price = (int)await postRacerAPI.GetHatPrice(id);
        List<int> ids = await postRacerAPI.getHats(PlayerPrefs.GetString("username"));
        return ids.Contains(id);
    }

    public void Select()
    {
        PlayerPrefs.SetInt("selectedhat", id);
    }

    public int getPrice()
    {
        return price;
    }

}
