﻿using CervezasColombia_CS_API_SQLite_Dapper.Cervezas;

namespace CervezasColombia_CS_API_SQLite_Dapper.Estilos
{
    public interface IEstiloRepository
    {
        public Task<IEnumerable<Estilo>> GetAllAsync();
        public Task<Estilo> GetByAttributeAsync<T>(T atributo_valor, string atributo_nombre);
        public Task<int> GetTotalAssociatedBeersAsync(int estilo_id);
        public Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(int estilo_id);
        public Task<bool> CreateAsync(Estilo unEstilo);
        public Task<bool> UpdateAsync(Estilo unEstilo);
        public Task<bool> DeleteAsync(Estilo unEstilo);
    }
}
