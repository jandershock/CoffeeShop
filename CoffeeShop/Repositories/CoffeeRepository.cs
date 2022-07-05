using CoffeeShop.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace CoffeeShop.Repositories
{
    public class CoffeeRepository : ICoffeeRepository
    {
        private readonly IBeanVarietyRepository _beanVarietyRepository;
        private readonly string _connectionString;
        public CoffeeRepository(IConfiguration configuration, IBeanVarietyRepository beanVarietyRepository)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _beanVarietyRepository = beanVarietyRepository;
        }

        private SqlConnection Connection
        {
            get { return new SqlConnection(_connectionString); }
        }
        public void AddCoffee(Coffee coffee)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Coffee (Title, BeanVarietyId)
                                        VALUES (@title, @beanVarietyId)";
                    cmd.Parameters.AddWithValue("@title", coffee.Title);
                    cmd.Parameters.AddWithValue("@beanVarietyId", coffee.BeanVarietyId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteCoffee(int id)
        {
            throw new System.NotImplementedException();
        }

        public List<Coffee> GetAllCoffee()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Coffee.Id, Coffee.Title, Coffee.BeanVarietyId, BeanVariety.[Name], BeanVariety.Region,
		                                        BeanVariety.Notes
                                        FROM Coffee
                                        JOIN BeanVariety ON BeanVariety.Id = BeanVarietyId";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Coffee> coffeeList = new List<Coffee>();
                        while (reader.Read())
                        {
                            coffeeList.Add(new Coffee()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                BeanVarietyId = reader.GetInt32(reader.GetOrdinal("BeanVarietyId")),
                                BeanVariety = new BeanVariety()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("BeanVarietyId")),
                                    Region = reader.GetString(reader.GetOrdinal("Region")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes"))
                                }
                            });
                        }
                        return coffeeList;
                    }
                }
            }
        }

        public Coffee GetCoffee(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Coffee.Id, Coffee.Title, Coffee.BeanVarietyId, BeanVariety.[Name], BeanVariety.Region,
		                                        BeanVariety.Notes
                                        FROM Coffee
                                        JOIN BeanVariety ON BeanVariety.Id = BeanVarietyId
                                        WHERE Coffee.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Coffee coffee = null;
                        if (reader.Read())
                        {
                            coffee = new Coffee()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                BeanVarietyId = reader.GetInt32(reader.GetOrdinal("BeanVarietyId")),
                                BeanVariety = new BeanVariety()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("BeanVarietyId")),
                                    Region = reader.GetString(reader.GetOrdinal("Region")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes"))
                                }
                            };
                        }
                        return coffee;
                    }
                }
            }
        }

        public void UpdateCoffee(Coffee coffee)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Coffee
                                        SET Title=@title, BeanVarietyId=@beanVarietyId
                                        WHERE Coffee.Id = @id";
                    cmd.Parameters.AddWithValue("@title", coffee.Title);
                    cmd.Parameters.AddWithValue("@beanVarietyId", coffee.BeanVarietyId);
                    cmd.Parameters.AddWithValue("@id", coffee.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
