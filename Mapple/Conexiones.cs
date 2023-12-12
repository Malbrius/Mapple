using System.Data.SqlClient;

public class Conexiones{
    /**
     * Cuando estemos desarrollando, comentar las variables del servidor productivo
     *  y descomentar las variables del servidor de pruebas.
     * */

   // Servidor productivo
   private static readonly string url = "jdbc:postgresql://10.50.0.224:5432/credito";
   private static readonly string user = "syscredito";
   private static readonly string password = "2587fb5cd0e0c6112394cf4b033ce6f7";

   //Server pruebas
   /*
   private static readonly string url = "jdbc:postgresql://10.27.113.84:5432/e_commerce";
   private static readonly string user = "sysdatos";
   private static readonly string password = "f7c0853fd9048a496fa6b70eb21f4fb6";
   */

   public static SqlConnection Connect224(){
       return new SqlConnection(url, user, password);
   }
}
