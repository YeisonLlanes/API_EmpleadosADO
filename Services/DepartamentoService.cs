using Api_EmpleadosADO.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Api_EmpleadosADO.Services
{
    public class DepartamentoService: IDepartamento 
    {
        private readonly string _conexion = "";

        public DepartamentoService(IConfiguration configuration)
        {
            _conexion = configuration.GetConnectionString("ConexionBD");            
        }

        public async Task<List<Departamento>> GetDepartamentos()
        {
            List<Departamento> _lista = new List<Departamento>();
            string consulta = "SELECT * FROM Departamentos";

            //Metodo 1
            
            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                SqlCommand cmd = new SqlCommand(consulta, conexion);
                //Utilizar Try Catch para el manejo de excepciones
                if (conexion.State == System.Data.ConnectionState.Closed)
                    conexion.Open();

                //conexion.OpenAsync();

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        
                        //Forma 1 
                        //var dpto = new Departamento();
                        //dpto.IdDepartamento = Convert.ToInt32(dr["idDepartamento"]);
                        //dpto.Descripcion = dr["Descripcion"].ToString();
                        //_lista.Add(dpto);

                        //Forma 2
                        _lista.Add(new Departamento
                        {
                            IdDepartamento = Convert.ToInt32(dr["idDepartamento"]),
                            Descripcion = dr["Descripcion"].ToString()
                        }
                        );
                    }
                    conexion.Close();
                }
               
            }
            return _lista;

            //Metodo 2 con Datatable serializando y deserealizando con Newtonsoft.Json

            /*var dt = new DataTable();
            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                using (var adaptador = new SqlDataAdapter(consulta, conexion))
                {
                    await conexion.OpenAsync();
                    adaptador.Fill(dt);
                    await conexion.CloseAsync();
                }
            }

            var departamento = JsonConvert.SerializeObject(dt);
            _lista = JsonConvert.DeserializeObject<List<Departamento>>(departamento); 
            return _lista;*/
            
            
        }

        public async Task<Departamento> GetById(int idDepartamento)
        {
            Departamento _departamento = new Departamento();
            string consulta = "SELECT * FROM Departamentos WHERE idDepartamento = @idDpto";

            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                SqlCommand cmd = new SqlCommand(consulta, conexion);
                cmd.Parameters.AddWithValue("@idDpto", idDepartamento);
                
                conexion.Open();

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        _departamento.IdDepartamento = Convert.ToInt32(dr["idDepartamento"]);
                        _departamento.Descripcion = dr["Descripcion"].ToString();
                    }
                    conexion.Close();
                }

            }
            return _departamento;
        }
        public async Task<Departamento> Create(Departamento departamento)
        {
            var dpto = new Departamento();
            string consulta = "INSERT INTO Departamentos (Descripcion)" +
                              " VALUES (@descripcion)";

            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                SqlCommand cmd = new SqlCommand(consulta, conexion);
                cmd.Parameters.Add("@descripcion", SqlDbType.NVarChar, 50).Value = departamento.Descripcion;

                conexion.Open();
                await cmd.ExecuteNonQueryAsync();
                conexion.Close();
            }
            return dpto;

        }
        
        public async Task<Departamento> Update(int idDepartamento, Departamento departmento)
        {
            Departamento _departamento = new Departamento();
            string consulta = "UPDATE Departamentos SET  Descripcion = @descripcion WHERE idDepartamento = @idDpto";

            //Valido que exista el registro a actualizar
            var dpto = await GetById(idDepartamento);

            if (dpto.IdDepartamento == 0 || dpto.Descripcion == null)
            {
                return dpto;
            }

            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                SqlCommand cmd = new SqlCommand(consulta, conexion);
                cmd.Parameters.Add("@descripcion", SqlDbType.NVarChar, 50).Value = departmento.Descripcion;
                cmd.Parameters.AddWithValue("@idDpto", idDepartamento);


                conexion.Open();
                await cmd.ExecuteNonQueryAsync();
                conexion.Close();
            }
            return dpto;
        }

        public async Task<bool> Delete(int idDepartamento)
        {
            string consulta = "DELETE FROM Departamentos WHERE idDepartamento = @idDpto";

            //Valido que exista el registro a actualizar
            var dpto = await GetById(idDepartamento);

            if (dpto.IdDepartamento == 0 || dpto.Descripcion == null)
            {
                return false;
            }

            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                SqlCommand cmd = new SqlCommand(consulta, conexion);
                cmd.Parameters.AddWithValue("@idDpto", idDepartamento);


                conexion.Open();
                await cmd.ExecuteNonQueryAsync();
                conexion.Close();
            }
            return true;
        }

    }
}
