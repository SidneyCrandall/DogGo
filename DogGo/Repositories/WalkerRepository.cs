// Responsible for Sql and CRUD to the database, at first coding it is meant for display.

using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public class WalkerRepository : IWalkerRepository
    {
        // Configuration read key-value pairs using for this document, appsetting.json (look below).
        // Part of the nuget packages installed upson creation of the project. 
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. 
        // This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public WalkerRepository(IConfiguration config)
        {
            _config = config;
        }

        // Connection to the Sql queries made below. We need a connection to the database
        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        // So we need to gather the data for the Dog Wlakers
        public List<Walker> GetAllWalkers()
        {
            // Let's connect to the databas
            using (SqlConnection conn = Connection)
            {
                // We need to open the port for the data to be sent
                conn.Open();
                // We then need to create a command for how to parse through the info, and what we would like back.
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // This is a SQL query to grab the info of the wlakers: Name, Id, Hood, but grab it from the Walker table, at this time. 
                    cmd.CommandText = @"
                        SELECT Id, [Name], ImageUrl, NeighborhoodId
                        FROM Walker
                    ";
                    // Read the data and get it ready for a return trip.
                    SqlDataReader reader = cmd.ExecuteReader();
                    // We want a list of the collection
                    List<Walker> walkers = new List<Walker>();
                    // Loop through the walker table
                    while (reader.Read())
                    {
                        Walker walker = new Walker
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
                        };
                        // Now add the data you just grabbed and get ready for me to return it
                        walkers.Add(walker);
                    }
                    // But first we have to close the connection. 
                    reader.Close();
                    // Now return to the DOM a list of walkers which we will view when navigated to. 
                    return walkers;
                }
            }
        }

        // Slightly similar to the one above, but this time we just need to grab only ONE walker, instead of the list.
        public Walker GetWalkerById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, [Name], ImageUrl, NeighborhoodId
                        FROM Walker
                        WHERE Id = @id
                    ";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    // If there is a walker that matches the criteria... 
                    if (reader.Read())
                    {
                        Walker walker = new Walker
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
                        };

                        reader.Close();
                        // Return that instance of THAT Walker.
                        return walker;
                    }
                    else
                    {
                        reader.Close();
                        // However if there isn't a single Walker that matches then return NULL. 
                        return null;
                    }
                }
            }
        }
    }
}
