//using System;
//using System.Collections.Generic;
//using UnityEngine;

//public class ExampleApp : MonoBehaviour
//{
//    [Header("Test data")]
//    public User user;
//    public Ouder ouder;
//    public Kind kind;
//    public Behandeling behandeling;
//    public GameProgress gameProgress;

//    [Header("Test IDs (voor GetById / Update / Delete)")]
//    public string ouderID;
//    public string kindID;
//    public string behandelingID;
//    public string gameProgressID;

//    [Header("Dependencies")]
//    public UserApiClient userApiClient;
//    public OuderApiClient ouderApiClient;
//    public KindApiClient kindApiClient;
//    public BehandelingApiClient behandelingApiClient;
//    public GameProgressApiClient gameProgressApiClient;

//    #region Login

//    [ContextMenu("User/Register")]
//    public async void Register()
//    {
//        IWebRequestReponse webRequestResponse = await userApiClient.Register(user);

//        switch (webRequestResponse)
//        {
//            case WebRequestData<string> dataResponse:
//                Debug.Log("Register succes!");
//                // TODO: Handle succes scenario;
//                break;
//            case WebRequestError errorResponse:
//                string errorMessage = errorResponse.ErrorMessage;
//                Debug.Log("Register error: " + errorMessage);
//                // TODO: Handle error scenario. Show the errormessage to the user.
//                break;
//            default:
//                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
//        }
//    }

//    [ContextMenu("User/Login")]
//    public async void Login()
//    {
//        IWebRequestReponse webRequestResponse = await userApiClient.Login(user);

//        switch (webRequestResponse)
//        {
//            case WebRequestData<string> dataResponse:
//                Debug.Log("Login succes!");
//                // TODO: Todo handle succes scenario.
//                break;
//            case WebRequestError errorResponse:
//                string errorMessage = errorResponse.ErrorMessage;
//                Debug.Log("Login error: " + errorMessage);
//                // TODO: Handle error scenario. Show the errormessage to the user.
//                break;
//            default:
//                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
//        }
//    }

//    #endregion

//    #region Ouder

//    [ContextMenu("Ouder/Get all")]
//    public async void GetAllOuders()
//    {
//        IWebRequestReponse webRequestResponse = await ouderApiClient.GetAll();

//        switch (webRequestResponse)
//        {
//            case WebRequestData<List<Ouder>> dataResponse:
//                List<Ouder> ouders = dataResponse.Data;
//                Debug.Log("List of ouders: ");
//                ouders.ForEach(o => Debug.Log(o.Naam));
//                // TODO: Handle succes scenario.
//                break;
//            case WebRequestError errorResponse:
//                string errorMessage = errorResponse.ErrorMessage;
//                Debug.Log("Get all ouders error: " + errorMessage);
//                // TODO: Handle error scenario. Show the errormessage to the user.
//                break;
//            default:
//                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
//        }
//    }

//    [ContextMenu("Ouder/Get by id")]
//    public async void GetOuderById()
//    {
//        IWebRequestReponse webRequestResponse = await ouderApiClient.GetById(Guid.Parse(ouderID));

//        switch (webRequestResponse)
//        {
//            case WebRequestData<Ouder> dataResponse:
//                Ouder fetchedOuder = dataResponse.Data;
//                Debug.Log("Get ouder success: " + fetchedOuder.Naam);
//                // TODO: Handle succes scenario.
//                break;
//            case WebRequestError errorResponse:
//                string errorMessage = errorResponse.ErrorMessage;
//                Debug.Log("Get ouder by id error: " + errorMessage);
//                // TODO: Handle error scenario. Show the errormessage to the user.
//                break;
//            default:
//                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
//        }
//    }

//    [ContextMenu("Ouder/Create")]
//    public async void CreateOuder()
//    {
//        IWebRequestReponse webRequestResponse = await ouderApiClient.Create(ouder);

//        switch (webRequestResponse)
//        {
//            case WebRequestData<string> dataResponse:
//                Debug.Log("Create ouder success");
//                // TODO: Handle succes scenario.
//                break;
//            case WebRequestError errorResponse:
//                string errorMessage = errorResponse.ErrorMessage;
//                Debug.Log("Create ouder error: " + errorMessage);
//                // TODO: Handle error scenario. Show the errormessage to the user.
//                break;
//            default:
//                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
//        }
//    }

//    [ContextMenu("Ouder/Update")]
//    public async void UpdateOuder()
//    {
//        IWebRequestReponse webRequestResponse = await ouderApiClient.UpdateItem(Guid.Parse(ouderID), ouder);

//        switch (webRequestResponse)
//        {
//            case WebRequestData<string> dataResponse:
//                Debug.Log("Update ouder success");
//                // TODO: Handle succes scenario.
//                break;
//            case WebRequestError errorResponse:
//                string errorMessage = errorResponse.ErrorMessage;
//                Debug.Log("Update ouder error: " + errorMessage);
//                // TODO: Handle error scenario. Show the errormessage to the user.
//                break;
//            default:
//                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
//        }
//    }

//    [ContextMenu("Ouder/Delete")]
//    public async void DeleteOuder()
//    {
//        IWebRequestReponse webRequestResponse = await ouderApiClient.Delete(Guid.Parse(ouderID));

//        switch (webRequestResponse)
//        {
//            case WebRequestData<string> dataResponse:
//                Debug.Log("Delete ouder success");
//                // TODO: Handle succes scenario.
//                break;
//            case WebRequestError errorResponse:
//                string errorMessage = errorResponse.ErrorMessage;
//                Debug.Log("Delete ouder error: " + errorMessage);
//                // TODO: Handle error scenario. Show the errormessage to the user.
//                break;
//            default:
//                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
//        }
//    }

//    #endregion

//    #region Kind

//    [ContextMenu("Kind/Get all")]
//    public async void GetAllKinderen()
//    {
//        IWebRequestReponse webRequestResponse = await kindApiClient.GetAll();

//        switch (webRequestResponse)
//        {
//            case WebRequestData<List<Kind>> dataResponse:
//                List<Kind> kinderen = dataResponse.Data;
//                Debug.Log("List of kinderen: ");
//                kinderen.ForEach(k => Debug.Log(k.Naam));
//                // TODO: Handle succes scenario.
//                break;
//            case WebRequestError errorResponse:
//                string errorMessage = errorResponse.ErrorMessage;
//                Debug.Log("Get all kinderen error: " + errorMessage);
//                // TODO: Handle error scenario. Show the errormessage to the user.
//                break;
//            default:
//                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
//        }
//    }

//    [ContextMenu("Kind/Get by id")]
//    public async void GetKindById()
//    {
//        IWebRequestReponse webRequestResponse = await kindApiClient.GetById(Guid.Parse(kindID));

//        switch (webRequestResponse)
//        {
//            case WebRequestData<Kind> dataResponse:
//                Kind fetchedKind = dataResponse.Data;
//                Debug.Log("Get kind success: " + fetchedKind.Naam);
//                // TODO: Handle succes scenario.
//                break;
//            case WebRequestError errorResponse:
//                string errorMessage = errorResponse.ErrorMessage;
//                Debug.Log("Get kind by id error: " + errorMessage);
//                // TODO: Handle error scenario. Show the errormessage to the user.
//                break;
//            default:
//                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
//        }
//    }

//    [ContextMenu("Kind/Create")]
//    public async void CreateKind()
//    {
//        IWebRequestReponse webRequestResponse = await kindApiClient.Create(kind);

//        switch (webRequestResponse)
//        {
//            case WebRequestData<string> dataResponse:
//                Debug.Log("Create kind success");
//                // TODO: Handle succes scenario.
//                break;
//            case WebRequestError errorResponse:
//                string errorMessage = errorResponse.ErrorMessage;
//                Debug.Log("Create kind error: " + errorMessage);
//                // TODO: Handle error scenario. Show the errormessage to the user.
//                break;
//            default:
//                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
//        }
//    }

//    [ContextMenu("Kind/Update")]
//    public async void UpdateKind()
//    {
//        IWebRequestReponse webRequestResponse = await kindApiClient.UpdateItem(Guid.Parse(kindID), kind);

//        switch (webRequestResponse)
//        {
//            case WebRequestData<string> dataResponse:
//                Debug.Log("Update kind success");
//                // TODO: Handle succes scenario.
//                break;
//            case WebRequestError errorResponse:
//                string errorMessage = errorResponse.ErrorMessage;
//                Debug.Log("Update kind error: " + errorMessage);
//                // TODO: Handle error scenario. Show the errormessage to the user.
//                break;
//            default:
//                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
//        }
//    }

//    [ContextMenu("Kind/Delete")]
//    public async void DeleteKind()
//    {
//        IWebRequestReponse webRequestResponse = await kindApiClient.Delete(Guid.Parse(kindID));

//        switch (webRequestResponse)
//        {
//            case WebRequestData<string> dataResponse:
//                Debug.Log("Delete kind success");
//                // TODO: Handle succes scenario.
//                break;
//            case WebRequestError errorResponse:
//                string errorMessage = errorResponse.ErrorMessage;
//                Debug.Log("Delete kind error: " + errorMessage);
//                // TODO: Handle error scenario. Show the errormessage to the user.
//                break;
//            default:
//                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
//        }
//    }

//    #endregion

//    #region Behandeling

//    [ContextMenu("Behandeling/Get all")]
//    public async void GetAllBehandelingen()
//    {
//        IWebRequestReponse webRequestResponse = await behandelingApiClient.GetAll();

//        switch (webRequestResponse)
//        {
//            case WebRequestData<List<Behandeling>> dataResponse:
//                List<Behandeling> behandelingen = dataResponse.Data;
//                Debug.Log("List of behandelingen: ");
//                behandelingen.ForEach(b => Debug.Log(b.Type));
//                // TODO: Handle succes scenario.
//                break;
//            case WebRequestError errorResponse:
//                string errorMessage = errorResponse.ErrorMessage;
//                Debug.Log("Get all behandelingen error: " + errorMessage);
//                // TODO: Handle error scenario. Show the errormessage to the user.
//                break;
//            default:
//                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
//        }
//    }

//    [ContextMenu("Behandeling/Get by id")]
//    public async void GetBehandelingById()
//    {
//        IWebRequestReponse webRequestResponse = await behandelingApiClient.GetById(Guid.Parse(behandelingID));

//        switch (webRequestResponse)
//        {
//            case WebRequestData<Behandeling> dataResponse:
//                Behandeling fetchedBehandeling = dataResponse.Data;
//                Debug.Log("Get behandeling success: " + fetchedBehandeling.Type);
//                // TODO: Handle succes scenario.
//                break;
//            case WebRequestError errorResponse:
//                string errorMessage = errorResponse.ErrorMessage;
//                Debug.Log("Get behandeling by id error: " + errorMessage);
//                // TODO: Handle error scenario. Show the errormessage to the user.
//                break;
//            default:
//                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
//        }
//    }

//    [ContextMenu("Behandeling/Create")]
//    public async void CreateBehandeling()
//    {
//        IWebRequestReponse webRequestResponse = await behandelingApiClient.Create(behandeling);

//        switch (webRequestResponse)
//        {
//            case WebRequestData<string> dataResponse:
//                Debug.Log("Create behandeling success");
//                // TODO: Handle succes scenario.
//                break;
//            case WebRequestError errorResponse:
//                string errorMessage = errorResponse.ErrorMessage;
//                Debug.Log("Create behandeling error: " + errorMessage);
//                // TODO: Handle error scenario. Show the errormessage to the user.
//                break;
//            default:
//                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
//        }
//    }

//    [ContextMenu("Behandeling/Update")]
//    public async void UpdateBehandeling()
//    {
//        IWebRequestReponse webRequestResponse = await behandelingApiClient.UpdateItem(Guid.Parse(behandelingID), behandeling);

//        switch (webRequestResponse)
//        {
//            case WebRequestData<string> dataResponse:
//                Debug.Log("Update behandeling success");
//                // TODO: Handle succes scenario.
//                break;
//            case WebRequestError errorResponse:
//                string errorMessage = errorResponse.ErrorMessage;
//                Debug.Log("Update behandeling error: " + errorMessage);
//                // TODO: Handle error scenario. Show the errormessage to the user.
//                break;
//            default:
//                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
//        }
//    }

//    [ContextMenu("Behandeling/Delete")]
//    public async void DeleteBehandeling()
//    {
//        IWebRequestReponse webRequestResponse = await behandelingApiClient.Delete(Guid.Parse(behandelingID));

//        switch (webRequestResponse)
//        {
//            case WebRequestData<string> dataResponse:
//                Debug.Log("Delete behandeling success");
//                // TODO: Handle succes scenario.
//                break;
//            case WebRequestError errorResponse:
//                string errorMessage = errorResponse.ErrorMessage;
//                Debug.Log("Delete behandeling error: " + errorMessage);
//                // TODO: Handle error scenario. Show the errormessage to the user.
//                break;
//            default:
//                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
//        }
//    }

//    #endregion

//    #region GameProgress

//    [ContextMenu("GameProgress/Get all")]
//    public async void GetAllGameProgress()
//    {
//        IWebRequestReponse webRequestResponse = await gameProgressApiClient.GetAll();

//        switch (webRequestResponse)
//        {
//            case WebRequestData<List<GameProgress>> dataResponse:
//                List<GameProgress> gameProgressList = dataResponse.Data;
//                Debug.Log("List of gameProgress: ");
//                gameProgressList.ForEach(g => Debug.Log("Points: " + g.Points));
//                // TODO: Handle succes scenario.
//                break;
//            case WebRequestError errorResponse:
//                string errorMessage = errorResponse.ErrorMessage;
//                Debug.Log("Get all gameProgress error: " + errorMessage);
//                // TODO: Handle error scenario. Show the errormessage to the user.
//                break;
//            default:
//                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
//        }
//    }

//    [ContextMenu("GameProgress/Get by id")]
//    public async void GetGameProgressById()
//    {
//        IWebRequestReponse webRequestResponse = await gameProgressApiClient.GetById(Guid.Parse(gameProgressID));

//        switch (webRequestResponse)
//        {
//            case WebRequestData<GameProgress> dataResponse:
//                GameProgress fetchedGameProgress = dataResponse.Data;
//                Debug.Log("Get gameProgress success: Points=" + fetchedGameProgress.Points);
//                // TODO: Handle succes scenario.
//                break;
//            case WebRequestError errorResponse:
//                string errorMessage = errorResponse.ErrorMessage;
//                Debug.Log("Get gameProgress by id error: " + errorMessage);
//                // TODO: Handle error scenario. Show the errormessage to the user.
//                break;
//            default:
//                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
//        }
//    }

//    [ContextMenu("GameProgress/Create")]
//    public async void CreateGameProgress()
//    {
//        IWebRequestReponse webRequestResponse = await gameProgressApiClient.Create(gameProgress);

//        switch (webRequestResponse)
//        {
//            case WebRequestData<string> dataResponse:
//                Debug.Log("Create gameProgress success");
//                // TODO: Handle succes scenario.
//                break;
//            case WebRequestError errorResponse:
//                string errorMessage = errorResponse.ErrorMessage;
//                Debug.Log("Create gameProgress error: " + errorMessage);
//                // TODO: Handle error scenario. Show the errormessage to the user.
//                break;
//            default:
//                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
//        }
//    }

//    [ContextMenu("GameProgress/Update")]
//    public async void UpdateGameProgress()
//    {
//        IWebRequestReponse webRequestResponse = await gameProgressApiClient.UpdateItem(Guid.Parse(gameProgressID), gameProgress);

//        switch (webRequestResponse)
//        {
//            case WebRequestData<string> dataResponse:
//                Debug.Log("Update gameProgress success");
//                // TODO: Handle succes scenario.
//                break;
//            case WebRequestError errorResponse:
//                string errorMessage = errorResponse.ErrorMessage;
//                Debug.Log("Update gameProgress error: " + errorMessage);
//                // TODO: Handle error scenario. Show the errormessage to the user.
//                break;
//            default:
//                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
//        }
//    }

//    [ContextMenu("GameProgress/Delete")]
//    public async void DeleteGameProgress()
//    {
//        IWebRequestReponse webRequestResponse = await gameProgressApiClient.Delete(Guid.Parse(gameProgressID));

//        switch (webRequestResponse)
//        {
//            case WebRequestData<string> dataResponse:
//                Debug.Log("Delete gameProgress success");
//                // TODO: Handle succes scenario.
//                break;
//            case WebRequestError errorResponse:
//                string errorMessage = errorResponse.ErrorMessage;
//                Debug.Log("Delete gameProgress error: " + errorMessage);
//                // TODO: Handle error scenario. Show the errormessage to the user.
//                break;
//            default:
//                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
//        }
//    }

//    #endregion

//}
