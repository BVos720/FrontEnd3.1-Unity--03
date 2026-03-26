using System.Threading.Tasks;
using UnityEngine;

public class LoginController : MonoBehaviour
{
    public UserApiClient userApiClient;

    public async Task<string> Login(string email, string password)
    {
        User user = new User(email, password);
        IWebRequestReponse response = await userApiClient.Login(user);

        switch (response)
        {
            case WebRequestData<string> dataResponse:
                Debug.Log("Login succes! Token: " + dataResponse.Data);
                return dataResponse.Data;
            case WebRequestError errorResponse:
                Debug.Log("Login error: " + errorResponse.ErrorMessage);
                return null;
            default:
                throw new System.NotImplementedException("No implementation for webRequestResponse of class: " + response.GetType());
        }
    }

    public async Task<bool> Register(string email, string password)
    {
        User user = new User(email, password);
        IWebRequestReponse response = await userApiClient.Register(user);

        switch (response)
        {
            case WebRequestData<string> dataResponse:
                Debug.Log("Register succes!");
                return true;
            case WebRequestError errorResponse:
                Debug.Log("Register error: " + errorResponse.ErrorMessage);
                return false;
            default:
                throw new System.NotImplementedException("No implementation for webRequestResponse of class: " + response.GetType());
        }
    }
}
