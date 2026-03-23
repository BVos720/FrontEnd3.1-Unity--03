using System;
using Newtonsoft.Json;
using UnityEngine;
using MySecureBackend.WebApi.Models;

public class GameProgressApiClient : MonoBehaviour
{
    public WebClient webClient;

    public async Awaitable<IWebRequestReponse> GetAll()
    {
        string route = "/gameprogress";
        return await webClient.SendGetRequest(route);
    }

    public async Awaitable<IWebRequestReponse> GetById(Guid gameProgressID)
    {
        string route = "/gameprogress/" + gameProgressID;
        return await webClient.SendGetRequest(route);
    }

    public async Awaitable<IWebRequestReponse> Create(GameProgress gameProgress)
    {
        string route = "/gameprogress";
        string data = JsonConvert.SerializeObject(gameProgress, JsonHelper.CamelCaseSettings);
        return await webClient.SendPostRequest(route, data);
    }

    public async Awaitable<IWebRequestReponse> UpdateItem(Guid gameProgressID, GameProgress gameProgress)
    {
        string route = "/gameprogress/" + gameProgressID;
        string data = JsonConvert.SerializeObject(gameProgress, JsonHelper.CamelCaseSettings);
        return await webClient.SendPutRequest(route, data);
    }

    public async Awaitable<IWebRequestReponse> Delete(Guid gameProgressID)
    {
        string route = "/gameprogress/" + gameProgressID;
        return await webClient.SendDeleteRequest(route);
    }
}
