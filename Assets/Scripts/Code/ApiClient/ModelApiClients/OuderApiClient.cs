using System;
using Newtonsoft.Json;
using UnityEngine;
using MySecureBackend.WebApi.Models;

public class OuderApiClient : MonoBehaviour
{
    public WebClient webClient;

    public async Awaitable<IWebRequestReponse> GetAll()
    {
        string route = "/ouder";
        return await webClient.SendGetRequest(route);
    }

    public async Awaitable<IWebRequestReponse> GetById(Guid ouderID)
    {
        string route = "/ouder/" + ouderID;
        return await webClient.SendGetRequest(route);
    }

    public async Awaitable<IWebRequestReponse> Create(Ouder ouder)
    {
        string route = "/ouder";
        string data = JsonConvert.SerializeObject(ouder, JsonHelper.CamelCaseSettings);
        return await webClient.SendPostRequest(route, data);
    }

    public async Awaitable<IWebRequestReponse> UpdateItem(Guid ouderID, Ouder ouder)
    {
        string route = "/ouder/" + ouderID;
        string data = JsonConvert.SerializeObject(ouder, JsonHelper.CamelCaseSettings);
        return await webClient.SendPutRequest(route, data);
    }

    public async Awaitable<IWebRequestReponse> Delete(Guid ouderID)
    {
        string route = "/ouder/" + ouderID;
        return await webClient.SendDeleteRequest(route);
    }
}
