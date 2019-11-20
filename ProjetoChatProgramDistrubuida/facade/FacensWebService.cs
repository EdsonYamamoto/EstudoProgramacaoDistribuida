using Newtonsoft.Json;
using ProjetoChatProgramDistrubuida.model;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoChatProgramDistrubuida.facade
{
    public static class FacensWebService
    {
        public static HashFacens ReqBitcoinsWebService()
        {

            var client = new RestClient("https://mineracao-facens.000webhostapp.com/request.php");
            var request = new RestRequest("", Method.GET);
            IRestResponse response = client.Execute(request);

            return JsonConvert.DeserializeObject<model.HashFacens>(response.Content);
        }
        public static string ReqBitcoinsResultWebService(string timestamp, string nounce)
        {
            var client = new RestClient("https://mineracao-facens.000webhostapp.com/submit.php");
            var request = new RestRequest("?timestamp=" + timestamp + "&nonce=" + nounce + "&poolname=Hall%20of%20Fame", Method.GET);
            IRestResponse response = client.Execute(request);
            return response.Content;
        }
    }
}
