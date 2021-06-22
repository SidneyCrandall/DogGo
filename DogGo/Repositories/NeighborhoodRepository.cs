﻿// These are data request to grab info of the neighborhood

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DogGo.Repositories
{
    public class NeighborhoodRepository : INeighborhoodRepository
    {
        private readonly IConfiguration _config;

        public NeighborhoodRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultCOnnection"));
            }
        }

        // We want to grab all the Neighborhoods and list them so we can attach them to a walker
        public List<Neighborhood> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, Name FROM Neighborhood";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Neighborhood> neighborhoods = new List<Neighborhood>();

                    while (reader.Read())
                    {
                        Neighborhood neighborhood = new Neighborhood()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };
                        neighborhoods.Add(neighborhood);
                    }

                    reader.Close();

                    return neighborhoods;
                }
            }
        }
    }
}