using System;
using Newtonsoft.Json;
using UnityEngine;
using MySecureBackend.WebApi.Models;

public class KindApiClient : MonoBehaviour
{
    public WebClient webClient;

    public async Awaitable<IWebRequestReponse> GetAll()
    {
        string route = "/kind";
        return await webClient.SendGetRequest(route);
    }

    public async Awaitable<IWebRequestReponse> GetById(Guid kindID)
    {
        string route = "/kind/" + kindID;
        return await webClient.SendGetRequest(route);
    }

    public async Awaitable<IWebRequestReponse> Create(Kind kind)
    {
        string route = "/kind";
        string data = JsonConvert.SerializeObject(kind, JsonHelper.CamelCaseSettings);
        return await webClient.SendPostRequest(route, data);
    }

    public async Awaitable<IWebRequestReponse> UpdateItem(Guid kindID, Kind kind)
    {
        string route = "/kind/" + kindID;
        string data = JsonConvert.SerializeObject(kind, JsonHelper.CamelCaseSettings);
        return await webClient.SendPutRequest(route, data);
    }

    public async Awaitable<IWebRequestReponse> Delete(Guid kindID)
    {
        string route = "/kind/" + kindID;
        return await webClient.SendDeleteRequest(route);
    }
}
