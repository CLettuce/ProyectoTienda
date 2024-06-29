using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



using CapaEntidad;

using System.Data.SqlClient;
using System.Data;


namespace CapaDatos
{
    public class cUbicacion
    {

        public List<ceDepartamento> ObtenerDepartamento()
        {
            List<ceDepartamento> lista = new List<ceDepartamento>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select * from departamento";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new ceDepartamento()
                                {
                                    IdDepartamento = dr["IdDepartamento"].ToString(),
                                    Descripcion = dr["Descripcion"].ToString(),
                                });
                        }
                    }
                }

            }
            catch
            {
                lista = new List<ceDepartamento>();

            }

            return lista;

        }


        public List<ceMunicipio> ObtenerMunicipio(string iddepartamento)
        {
            List<ceMunicipio> lista = new List<ceMunicipio>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select * from municipio where IdDepartamento = @iddepartamento";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@iddepartamento", iddepartamento);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new ceMunicipio()
                                {
                                    IdProvincia = dr["IdProvincia"].ToString(),
                                    Descripcion = dr["Descripcion"].ToString(),
                                });
                        }
                    }
                }

            }
            catch
            {
                lista = new List<ceMunicipio>();

            }

            return lista;

        }

    }
}
