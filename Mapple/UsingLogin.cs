using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;

public class UserLogin : IRunnable
{
   private string actualNombre;
   private Thread hilo;

   //Construye un nuevo hilo.
   public UserLogin(string nombre)
   {
       hilo = new Thread(Run);
       actualNombre = nombre;
   }

   //Punto de entrada de hilo.
   public void Run()
   {
       Dictionary<string, string> params = new Dictionary<string, string>();
       params.Add("user", "coppelcredito");
       params.Add("password", "coppelcredito");

       //server url
       string url = "https://coppelcredito.ucontactcloud.com/Integra/resources/auth/UserLogin";

       // conectarse con httputility "newRequest(url,method,callback)"
       HttpUtility.NewRequest("", url, HttpUtility.MethodPost, params, new HttpUtility.Callback()
       {
           public void OnSuccess(string response)
           {
               // on success
               Console.WriteLine($"Acceso exitoso: = {response}");
               string[] arrayresponse = response.Split("},");
               SIRU_1P.token = arrayresponse[2];
               SIRU_1P.token = SIRU_1P.token.Substring(1, SIRU_1P.token.Length - 8);
               SIRU_1P.token += "=";
           }

           public void OnError(int status_code, string message)
           {
               // on error
               MessageBox.Show($"Error en Servidor\nstatus_code= {status_code}\nmessage={message}");
               Console.WriteLine($"OnSuccess:Server Error\nstatus_code={status_code}\nmessage={message}");
           }
       });
   }
}
