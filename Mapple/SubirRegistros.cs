using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

public class SubirRegistros : IRunnable
{
   private string actualNombre;
   private Thread hilo;
   static string fechaActual = DateTime.Now.ToString("yyyyMMdd");
   static string horaActual = DateTime.Now.ToString("HHmmss");
   static string fechaHoraActual = fechaActual ThoraActual;

   //Construye un nuevo hilo.
   public SubirRegistros(string nombre)
   {
       hilo = new Thread(Run);
       actualNombre = nombre;
   }

   public void insertaLog(string cte, string tel, string fecAlta, string msj)
   {
       string sqlLog = " INSERT INTO log_api VALUES (cte , tel , 'fecAlta ', LOCALTIMESTAMP, 'msj ')";
       try
       {
           using (SqlConnection conn2 = Conexiones.connect224())
           {
               SqlCommand stmt2 = new SqlCommand(sqlLog, conn2);

               //Se inserta el log
               stmt2.ExecuteNonQuery();
           }
       }
       catch (Exception ex)
       {
           Console.WriteLine("-----insertaLog(): ex.Message , -----cte: cte , tel:tel , fecAlta: fecAlta , msj:msj);
       }
   }

   //Punto de entrada de hilo.
   public void Run()
   {
       string SQL = "select num_telefono,idu_num_cliente,nom_cliente,opc_tipo_telefono,fec_nacimiento,opc_sexo,opc_estado_civil,nom_ciudad,idu_fec_alta_telefono,fec_actualizacion_telefono,opc_origen,opc_suborigen,nom_tienda from mae_confirmacion_on where opc_estatus = '0' AND idu_num_cliente not in (select num_cliente from tmp_tablabloqueo where fecha_desbloqueo > current_Date AND num_cliente is not null) AND num_telefono not in (select num_telefonos from tmp_tablabloqueo where fecha_desbloqueo > current_Date AND num_telefonos is not null);";
       try (SqlConnection conn = Conexiones.connect224();
               SqlCommand stmt = new SqlCommand(SQL, conn);
               SqlDataReader rs = stmt.ExecuteReader())
       {
           while (rs.Read())
           {
               string telefono = rs.GetString(0);
               string numCte = rs.GetString(1);
               string nomCte = rs.GetString(2);
               string tipoTel = rs.GetString(3);
               string fecNac = rs.GetString(4);
               string sexo = rs.GetString(5);
               string estadoCivil = rs.GetString(6);
               string ciudad = rs.GetString(7);
               string fecAlta = rs.GetString(8);
               string fecActualiza = rs.GetString(9);
               string origen = rs.GetString(10);
               string subOrigen = rs.GetString(11);
               string tiendaActualiza = rs.GetString(12);

               ciudad = ciudad.Replace(",", ".");

               Console.WriteLine("cambiara estatus a 1");
               string SQL2 = $"update mae_confirmacion_on set opc_estatus = '1' where opc_estatus = '0' and idu_num_cliente= {numCte} and num_telefono= {telefono} and idu_fec_alta_telefono = '{fecAlta}'";

               insertaLog(numCte, telefono, fecAlta, "Se actualiza la mae_confirmacion_on a estatus 1");
               try
               {
                  using (SqlConnection conn2 = Conexiones.connect224())
                  {
                      SqlCommand stmt2 = new SqlCommand(SQL2, conn2);
                      stmt2.ExecuteNonQuery();
                  }
               }
               catch (Exception ex)
               {
                  Console.WriteLine("updateToMae(String estatus):ex.Message);
               }

               Dictionary<string, string> params = new Dictionary<string, string>();
               string call = $"{{calldate : null,campaign : \"ConfirmacionOnline<-\",destination: \"{telefono} \",alternatives: \"\",agent : \"\",data: \"numCliente={numCte} :nomCliente={nomCte} :numTelefono={telefono} :tipoTelefono={tipoTel} :fecNacimiento={fecNac} :sexo={sexo} :estadoCivil={estadoCivil} :ciudad={ciudad} :fecAltaTelefono={fecAlta} :fecActualizacion={fecActualiza} :origen={origen} :subOrigen={subOrigen} :tiendaActualizo={tiendaActualiza} \",source: \"{fechaHoraActual} \",bulk : false,\"automatic\" : true }}";
               Console.WriteLine(call); 
               params.Add("call", call);

               subeRegistro(numCte, telefono, fecAlta, params);
           }
       }
       catch (Exception ex)
       {
           Console.WriteLine($"obtenerClientes(): {ex.Message}");
       }
   }

    public void actualizaMaeTo2(string numCte, string telefono, string fecAlta)
    {
        insertaLog(numCte, telefono, fecAlta, "Se actualiza la mae_confirmacion_on a estatus 2");
        string SQL3 = $"update mae_confirmacion_on set opc_estatus = '2' where opc_estatus = '1' and idu_num_cliente= {numCte} and num_telefono = {telefono}";
        try (SqlConnection conn3 = Conexiones.connect224();
                SqlCommand stmt3 = new SqlCommand(SQL3, conn3);
                SqlDataReader rs3 = stmt3.ExecuteReader())
        {
            while(rs3.Read())   
            {
                Console.WriteLine("cambio estatus a 2");
            }
        }
        catch (Exception ex)
        {
            //System.out.println("updateToMae(String estatus):"+ex.getMessage());
        }
    }
}