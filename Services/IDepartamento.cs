using Api_EmpleadosADO.Models;

namespace Api_EmpleadosADO.Services
{
    public interface IDepartamento
    {
        Task<List<Departamento>> GetDepartamentos();

        Task<Departamento> GetById(int idDepartamento);

        Task<Departamento> Create(Departamento departamento);

        Task<Departamento> Update(int idDepartamento, Departamento departmento);

        Task<bool> Delete(int idDepartamento);
    }
}
