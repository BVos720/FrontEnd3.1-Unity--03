using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Object2DApiClient : MonoBehaviour
{
    public WebClient webClient;

    public async Awaitable<IWebRequestReponse> ReadObject2Ds(string environmentId)
    {
        string route = "/object2d/environment/" + environmentId;

        IWebRequestReponse webRequestResponse = await webClient.SendGetRequest(route);
        return ParseObject2DListResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> CreateObject2D(Object2D object2D)
    {
        string route = "/object2d";
        string data = JsonConvert.SerializeObject(object2D, JsonHelper.CamelCaseSettings);

        IWebRequestReponse webRequestResponse = await webClient.SendPostRequest(route, data);
        return ParseObject2DResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> UpdateObject2D(Object2D object2D)
    {
        string route = "/object2d/" + object2D.GUID;
        string data = JsonConvert.SerializeObject(object2D, JsonHelper.CamelCaseSettings);

        return await webClient.SendPutRequest(route, data);
    }

    public async Awaitable<IWebRequestReponse> DeleteObject2D(string guid)
    {
        string route = "/object2d/" + guid;
        return await webClient.SendDeleteRequest(route);
    }

    private IWebRequestReponse ParseObject2DResponse(IWebRequestReponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Debug.Log("Response data raw: " + data.Data);
                Object2D object2D = JsonConvert.DeserializeObject<Object2D>(data.Data);
                WebRequestData<Object2D> parsedWebRequestData = new WebRequestData<Object2D>(object2D);
                return parsedWebRequestData;
            default:
                return webRequestResponse;
        }
    }

    private IWebRequestReponse ParseObject2DListResponse(IWebRequestReponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Debug.Log("Response data raw: " + data.Data);
                List<Object2D> environments = JsonConvert.DeserializeObject<List<Object2D>>(data.Data);
                WebRequestData<List<Object2D>> parsedData = new WebRequestData<List<Object2D>>(environments);
                return parsedData;
            default:
                return webRequestResponse;
        }
    }
}