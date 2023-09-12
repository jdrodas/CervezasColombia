﻿using CervezasColombia_CS_API_SQLite_Dapper.Models;

namespace CervezasColombia_CS_API_SQLite_Dapper.Interfaces
{
    public interface ICervezaRepository
    {
        public Task<IEnumerable<Cerveza>> GetAllAsync();

        public Task<Cerveza> GetByIdAsync(int id);

        public Task<int> GetTotalAssociatedIngredientsAsync(int id);
        public Task<IEnumerable<Ingrediente>> GetAssociatedIngredientsAsync(int id);
    }
}