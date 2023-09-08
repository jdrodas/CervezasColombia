﻿using CervezasColombia_CS_API_SQLite_Dapper.Data.Entities;

namespace CervezasColombia_CS_API_SQLite_Dapper.Interfaces
{
    public interface IEstiloRepository
    {
        public Task<IEnumerable<Estilo>> GetAllAsync();
        public Task<Estilo> GetByIdAsync(int id);
        public Task<Estilo> GetByNameAsync(string nombre);
        public Task<int> GetTotalBeersByStyleAsync(int id);
        public Task<IEnumerable<Cerveza>> GetBeersByStyleAsync(int id);
        public Task CreateAsync(Estilo unEstilo);
        public Task UpdateAsync(Estilo unEstilo);
        public Task DeleteAsync(int id);
    }
}
