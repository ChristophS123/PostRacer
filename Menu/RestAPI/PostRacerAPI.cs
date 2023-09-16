using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using UnityEngine;
using Newtonsoft.Json;
using System.Net.Http;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

class PostRacerAPI
{

    private string URL = "https://sv-studios.de/PostRacer/index.php/api/";

    public class SignUpData
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class HighScoreGetData
    {
        public string username { get; set; }
    }

    public class HighScoreSetData
    {
        public string username { get; set; }
        public float value { get; set;  }
    }

    public class ShopItemData
    {
        public string username { get; set; }
        public int id { get; set; }
    }

    [System.Serializable]
    public class UserData
    {
        public string username;
        public string password;
        public int lastscore;
        public int highscore;
        public int highscorepakets;
        public int coins;
    }

    public class AutomaticSignInData
    {
        public string username { get; set; }
        public string password { get; set; }
        public string token { get; set; }
    }

    public class ToplistGetResponse
    {
        public List<UserData> userData { get; set; }
    }

    public class PriceIDObject
    {
        public int id { get; set; }
    }

    public async Task<string> SignUp(string username, string password)
    {
        SignUpData data = new SignUpData
        {
            username = username,
            password = password
        };
        string jsonData = JsonConvert.SerializeObject(data);
        using (var client = new HttpClient())
        {
            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(URL + "auth/sign-up", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                Dictionary<string, object> responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse);
                Int64 result = (Int64)responseData["success"];
                if(result == 1)
                {
                    string token = (string)responseData["token"];
                    PlayerPrefs.SetString("token", token);
                    PlayerPrefs.Save();
                    return "";
                } else
                {
                    return (string)responseData["message"];
                }
            } catch(Exception e)
            {
                Debug.LogError(e);
                return "Internal Server error";
            } 
        }
    }

    /*public  bool SignIn(string username, string password)
    {
        SignUpData data = new SignUpData
        {
            username = username,
            password = password
        };
        string jsonData = JsonConvert.SerializeObject(data);
        using (var client = new HttpClient())
        {
            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = client.PostAsync(URL + "auth/sign-in", content).Result;
                string apiResponse = response.Content.ReadAsStringAsync().Result;
                Dictionary<string, object> responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse);
                Debug.Log(responseData["message"]);
                Int64 success = (Int64)responseData["success"];
                return success == 1;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }
    } */

    public async Task<string> SignIn(string username, string password)
    {
        SignUpData data = new SignUpData
        {
            username = username,
            password = password
        };
        string jsonData = JsonConvert.SerializeObject(data);
        using (var client = new HttpClient())
        {
            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(URL + "auth/sign-in", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                Dictionary<string, object> responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse);
                Int64 success = (Int64)responseData["success"];
                if(success == 1)
                {
                    string token = (string)responseData["token"];
                    PlayerPrefs.SetString("token", token);
                    return "";
                }
                else
                {
                    return (string)responseData["message"];
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return "Internal Server error";
            }
        }
    }

    public async Task<bool> AutomaticSignIn(string token)
    {
        AutomaticSignInData data = new AutomaticSignInData
        {
            username = "null",
            password = "null",
            token = token
        };
        string jsonData = JsonConvert.SerializeObject(data);
        using (var client = new HttpClient())
        {
            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(URL + "auth/automatic-sign-in", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                Dictionary<string, object> responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse);
                Debug.Log(responseData);
                Int64 success = (Int64)responseData["success"];
                if (success == 1)
                {
                    string username = (string)responseData["username"];
                    if(username == PlayerPrefs.GetString("username"))
                    {
                        return true;
                    } else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }
    }

    public async Task<float> GetHighscore(string username)
    {
        HighScoreGetData data = new HighScoreGetData
        {
            username = username
        };
        string jsonData = JsonConvert.SerializeObject(data);
        using (var client = new HttpClient())
        {
            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(URL + "highscore/get", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                Dictionary<string, object> responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse);
                Int64 highscore = (Int64)responseData["highscore"];
                return highscore;
            } catch (Exception e)
            {
                Debug.LogError(e);
            }
         }
        return 0;
    } 

    public async Task SetHighscore(string username, float amount)
    {
        HighScoreSetData data = new HighScoreSetData
        {
            username = username,
            value = amount
        };
        string jsonData = JsonConvert.SerializeObject(data);
        using (var client = new HttpClient())
        {
            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(URL + "highscore/set", content);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }

    public async Task setHighscorePakets(string username, float amount)
    {
        HighScoreSetData data = new HighScoreSetData
        {
            username = username,
            value = amount
        };
        string jsonData = JsonConvert.SerializeObject(data);
        using (var client = new HttpClient())
        {
            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(URL + "highscorepakets/set", content);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }


    public async Task<float> GetHighscorePakets(string username)
    {
        HighScoreGetData data = new HighScoreGetData
        {
            username = username
        };
        string jsonData = JsonConvert.SerializeObject(data);
        using (var client = new HttpClient())
        {
            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(URL + "highscorepakets/get", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                Dictionary<string, object> responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse);
                Int64 highscore = (Int64)responseData["highscorepakets"];
                return highscore;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        return 0;
    }

    public async Task<float> GetCoins(string username)
    {
        HighScoreGetData data = new HighScoreGetData
        {
            username = username
        };
        string jsonData = JsonConvert.SerializeObject(data);
        using (var client = new HttpClient())
        {
            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(URL + "coins/get", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                Dictionary<string, object> responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse);
                Int64 coins = (Int64)responseData["coins"];
                return coins;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        return 0;
    }

    public async Task<string> LoadLatestNews()
    {
        HighScoreGetData data = new HighScoreGetData
        {
            username = ""
        };
        string jsonData = JsonConvert.SerializeObject(data);
        using (var client = new HttpClient())
        {
            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(URL + "news/load-latest", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                Dictionary<string, object> responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse);
                Int64 success = (Int64)responseData["success"];
                Debug.Log(success);
                if(success == 1)
                {
                    return (string)responseData["message"];
                }
                return "";
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        return "";
    }

    public async Task<float> GetVersion()
    {
        HighScoreGetData data = new HighScoreGetData
        {
            username = ""
        };
        string jsonData = JsonConvert.SerializeObject(data);
        using (var client = new HttpClient())
        {
            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(URL + "version/get", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                Dictionary<string, object> responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse);
                Int64 version = (Int64)responseData["current_version"];
                return version;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        return 0;
    }

    public async Task<float> GetPlayerTopListScore(string username)
    {
        HighScoreGetData data = new HighScoreGetData
        {
            username = username
        };
        string jsonData = JsonConvert.SerializeObject(data);
        using (var client = new HttpClient())
        {
            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(URL + "specifictoplist/get", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                Dictionary<string, object> responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse);
                Int64 value = (Int64)responseData["value"];
                return value;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        return 0;
    }

    public async Task<float> GetPlayerPaketsTopListScore(string username)
    {
        HighScoreGetData data = new HighScoreGetData
        {
            username = username
        };
        string jsonData = JsonConvert.SerializeObject(data);
        using (var client = new HttpClient())
        {
            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(URL + "specificpaketstoplist/get", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                Dictionary<string, object> responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse);
                Int64 value = (Int64)responseData["value"];
                return value;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        return 0;
    }

    public async Task SetCoins(string username, float amount)
    {
        HighScoreSetData data = new HighScoreSetData
        {
            username = username,
            value = amount
        };
        string jsonData = JsonConvert.SerializeObject(data);
        using (var client = new HttpClient())
        {
            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(URL + "coins/set", content);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }


    public async Task<float> GetLastscore(string username)
    {
        HighScoreGetData data = new HighScoreGetData
        {
            username = username
        };
        string jsonData = JsonConvert.SerializeObject(data);
        using (var client = new HttpClient())
        {
            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(URL + "lastscore/get", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                Dictionary<string, object> responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse);
                Int64 highscore = (Int64)responseData["lastscore"];
                return highscore;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        return 0;
    }

    public async Task SetLastScore(string username, float amount)
    {
        HighScoreSetData data = new HighScoreSetData
        {
            username = username,
            value = amount
        };
        string jsonData = JsonConvert.SerializeObject(data);
        using (var client = new HttpClient())
        {
            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(URL + "lastscore/set", content);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }

    public async Task LoadTopList(List<Text> texts)
    {
        HighScoreSetData data = new HighScoreSetData
        {
            username = "",
            value = 0F
        };
        string jsonData = JsonConvert.SerializeObject(data);
        using (var client = new HttpClient())
        {
            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                var response = await client.PostAsync(URL + "toplist/get", null);
                string apiResponse = await response.Content.ReadAsStringAsync();
                List<UserData> users = JsonConvert.DeserializeObject<List<UserData>>(apiResponse);
                for (int i = 0; i < 10; i++)
                {
                    if (i < users.Count)
                    {
                        string username = "";
                        if(users[i].username.Length > 10)
                        {
                            for(int n = 0; n < 10; n++)
                            {
                                username += users[i].username[n];
                            }
                            username += "...";
                        } else
                        {
                            username = users[i].username;
                        }
                        texts[i].text = (i + 1) + ". " + username + " -> " + users[i].highscore;
                    }
                    else
                    {
                        texts[i].text = (i + 1) + ". Free -> 0000";
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }

    public async Task LoadPaketTopList(List<Text> texts)
    {
        HighScoreSetData data = new HighScoreSetData
        {
            username = "",
            value = 0F
        };
        string jsonData = JsonConvert.SerializeObject(data);
        using (var client = new HttpClient())
        {
            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(URL + "toplistpakets/get", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                List<UserData> users = JsonConvert.DeserializeObject<List<UserData>>(apiResponse);
                for (int i = 0; i < 10; i++)
                {
                    if (i < users.Count)
                    {
                        string username = "";
                        if (users[i].username.Length > 10)
                        {
                            for (int n = 0; n < 10; n++)
                            {
                                username += users[i].username[n];
                            }
                            username += "...";
                        }
                        else
                        {
                            username = users[i].username;
                        }
                        texts[i].text = (i + 1) + ". " + username + " -> " + users[i].highscorepakets;
                    }
                    else
                    {
                        texts[i].text = (i + 1) + ". Free -> 0000";
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }

    public async Task<string> BuyHat(String username, int hatID, int price)
    {
        List<int> ids = await getHats(username);
        if(ids != null)
        {
            if(ids.Contains(hatID))
            {
                return "Already unlocked";
            }
        }
        float oldCoins = await GetCoins(username);
        float newCoins = oldCoins - price;
        if(newCoins < 0)
        {
            return "Not enougth coins.";
        }
        await SetCoins(username, newCoins);
        ShopItemData data = new ShopItemData
        {
            username = username,
            id = hatID
        };
        string jsonData = JsonConvert.SerializeObject(data);
        using (var client = new HttpClient())
        {
            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(URL + "hats/set", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                Dictionary<string, object> responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse);
                Int64 success = (Int64)responseData["success"];
                if(success != 1)
                {
                    return "";
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return "error!";
        }
    }

    public async Task<List<int>> getHats(string username)
    {
        HighScoreGetData data = new HighScoreGetData
        {
            username = username
        };
        string jsonData = JsonConvert.SerializeObject(data);
        using (var client = new HttpClient())
        {
            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(URL + "hats/get", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                List<ShopItemData> shopItems = JsonConvert.DeserializeObject<List<ShopItemData>>(apiResponse);
                List<int> ids = new List<int>();
                foreach(ShopItemData i in shopItems)
                {
                    ids.Add(i.id);
                }
                return ids;
            } catch(Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }
        
    }


    public async Task<string> BuyCar(String username, int hatID, int price)
    {
        List<int> ids = await getCars(username);
        if (ids != null)
        {
            if (ids.Contains(hatID))
            {
                return "Already unlocked";
            }
        }
        float oldCoins = await GetCoins(username);
        float newCoins = oldCoins - price;
        if (newCoins < 0)
        {
            return "Not enougth coins.";
        }
        await SetCoins(username, newCoins);
        ShopItemData data = new ShopItemData
        {
            username = username,
            id = hatID
        };
        string jsonData = JsonConvert.SerializeObject(data);
        using (var client = new HttpClient())
        {
            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(URL + "cars/set", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                Dictionary<string, object> responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse);
                Int64 success = (Int64)responseData["success"];
                if (success != 1)
                {
                    return "";
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return "error!";
        }
    }

    public async Task<List<int>> getCars(string username)
    {
        HighScoreGetData data = new HighScoreGetData
        {
            username = username
        };
        string jsonData = JsonConvert.SerializeObject(data);
        using (var client = new HttpClient())
        {
            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(URL + "cars/get", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                List<ShopItemData> shopItems = JsonConvert.DeserializeObject<List<ShopItemData>>(apiResponse);
                List<int> ids = new List<int>();
                foreach (ShopItemData i in shopItems)
                {
                    ids.Add(i.id);
                }
                return ids;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

    }

    public async Task<float> GetHatPrice(int id)
    {
        PriceIDObject data = new PriceIDObject
        {
            id = id
        };
        string jsonData = JsonConvert.SerializeObject(data);
        using (var client = new HttpClient())
        {
            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(URL + "hatprice/get", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                Dictionary<string, object> responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse);
                Int64 price = (Int64)responseData["price"];
                return price;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        return 0;
    }

    public async Task<float> GetCarPrice(int id)
    {
        PriceIDObject data = new PriceIDObject
        {
            id = id
        };
        string jsonData = JsonConvert.SerializeObject(data);
        using (var client = new HttpClient())
        {
            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(URL + "carprice/get", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                Dictionary<string, object> responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse);
                Int64 price = (Int64)responseData["price"];
                return price;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        return 0;
    }

    // Clan API

    public class ClanUsernameData
    {
        public string username { get; set; }
    }

    public class ClanNameAndUsernameData
    {
        public string clanname { get; set; }
        public string username { get; set; }
    }

    public class ClanNameData
    {
        public string clanname { get; set; }
    }

    public class ClanNameAndOwnerData
    {
        public string clanname { get; set; }
        public string owner { get; set; }
    }

    public async Task<string> GetClanFromUser(string username)
    {
        ClanUsernameData data = new ClanUsernameData
        {
            username = username
        };
        string jsonData = JsonConvert.SerializeObject(data);
        using (var client = new HttpClient())
        {
            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(URL + "clan/get", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                Dictionary<string, object> responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse);
                string clan = (string)responseData["clan"];
                return clan;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        return "";
    }

    public async Task<string> GetOwnerFromClan(string clan)
    {
        ClanNameData data = new ClanNameData
        {
            clanname = clan
        };
        string jsonData = JsonConvert.SerializeObject(data);
        using (var client = new HttpClient())
        {
            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(URL + "clan/owner", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                Dictionary<string, object> responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse);
                string owner = (string)responseData["owner"];
                return owner;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        return "";
    }

    public async Task<List<string>> GetMembersFromClan(string clan)
    {
        ClanNameData data = new ClanNameData
        {
            clanname = clan
        };
        string jsonData = JsonConvert.SerializeObject(data);
        using (var client = new HttpClient())
        {
            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(URL + "clan/members", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                List<string> users = JsonConvert.DeserializeObject<List<string>>(apiResponse);
                return users;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        return null;
    }

    public async Task RemovePlayerFromClan(string username)
    {
        ClanUsernameData data = new ClanUsernameData
        {
            username = username
        };
        string jsonData = JsonConvert.SerializeObject(data);
        using (var client = new HttpClient())
        {
            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(URL + "clan/remove-member", content);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }

    public async Task<string> InvitePlayerToClan(string clanName, string username)
    {
        ClanNameAndUsernameData data = new ClanNameAndUsernameData
        {
            clanname = clanName,
            username = username
        };
        string jsonData = JsonConvert.SerializeObject(data);
        using (var client = new HttpClient())
        {
            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(URL + "clan/invite", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                Dictionary<string, object> responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse);
                return (string)responseData["message"];
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        return "Internal Server error!";
    }

    public async Task<string> KickPlayerFromClan(string clanName, string username)
    {
        ClanNameAndUsernameData data = new ClanNameAndUsernameData
        {
            clanname = clanName,
            username = username
        };
        string jsonData = JsonConvert.SerializeObject(data);
        using (var client = new HttpClient())
        {
            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(URL + "clan/kick", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                Dictionary<string, object> responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse);
                return (string)responseData["message"];
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        return "Internal Server error!";
    }

    public async Task<string> DeleteClan(string clanName)
    {
        ClanNameData data = new ClanNameData
        {
            clanname = clanName
        };
        string jsonData = JsonConvert.SerializeObject(data);
        using (var client = new HttpClient())
        {
            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(URL + "clan/delete", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                Dictionary<string, object> responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse);
                return (string)responseData["message"];
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        return "Internal Server error!";
    }

    public async Task<string> GetInvitation(string username)
    {
        ClanUsernameData data = new ClanUsernameData
        {
            username = username
        };
        string jsonData = JsonConvert.SerializeObject(data);
        using (var client = new HttpClient())
        {
            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(URL + "clan/load-invitation", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                Dictionary<string, object> responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse);
                Int64 success = (Int64)responseData["success"];
                if(success == 1)
                {
                    return (string)responseData["invitation"];
                }
                return "";
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        return "";
    }

    public async Task AcceptInvitation(string username)
    {
        ClanUsernameData data = new ClanUsernameData
        {
            username = username
        };
        string jsonData = JsonConvert.SerializeObject(data);
        using (var client = new HttpClient())
        {
            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(URL + "clan/accept-invitation", content);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }

    public async Task DenyInvitation(string username)
    {
        ClanUsernameData data = new ClanUsernameData
        {
            username = username
        };
        string jsonData = JsonConvert.SerializeObject(data);
        using (var client = new HttpClient())
        {
            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(URL + "clan/deny-invitation", content);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }

    public async Task<string> CreateClan(string clanName, string owner)
    {
        ClanNameAndOwnerData data = new ClanNameAndOwnerData
        {
            clanname = clanName,
            owner = owner
        };
        string jsonData = JsonConvert.SerializeObject(data);
        using (var client = new HttpClient())
        {
            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(URL + "clan/create", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                Dictionary<string, object> responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse);
                Int64 success = (Int64)responseData["success"];
                if(success != 1)
                {
                    return (string)responseData["message"];
                } else
                {
                    return "";
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        return "Internal Server error!";
    }

}