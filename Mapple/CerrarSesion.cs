using System;
using System.Collections.Generic;
using System.Threading;
using System.Net.Http;

public class CerrarSesion : IRunnable
{
  private string actualNombre;
  private Thread hilo;

  //Construye un nuevo hilo.
  public CerrarSesion(string nombre)
  {
      hilo = new Thread(Run);
      actualNombre = nombre;
  }

  //Punto de entrada de hilo.
  public void Run()
  {
      Console.WriteLine("Cerrando sesión...");
      Dictionary<string, string> params = new Dictionary<string, string>();
      params.Add("user", "coppelcredito");
      params.Add("token", SIRU_1P.token);

      //server url
      string url = "https://coppelcredito.ucontactcloud.com/Integra/resources/auth/EndSession";

      // static class "HttpUtility" with static method "newRequest(url,method,callback)"
      HttpUtility.newRequest(SIRU_1P.token, url, HttpUtility.METHOD_POST, params, new HttpUtility.Callback() {
          @Override
          public void OnSuccess(String response) {
              // on success
              Console.WriteLine($"Sesión finalizada = {response}");
              //updateToMae("2");
          }

          @Override
          public void OnError(int status_code, String message) {
              // on error
              Console.WriteLine($"cerrarSesion:Server OnError status_code = {status_code} message= {message}");
          }
      });
  }
}
