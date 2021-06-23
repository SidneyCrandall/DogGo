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
                        SELECT w.Id, w.[Name], w.ImageUrl, w.NeighborhoodId, Neighborhood.Name as Neighborhood
                        FROM Walker w
                        JOIN Neighborhood on w.NeighborhoodId = Neighborhood.Id
                    ";
                    // Read the data and get it ready for a return trip.
                    SqlDataReader reader = cmd.ExecuteReader();
                    // We want a list of the collection
                    List<Walker> walkers = new List<Walker>();
                    // Loop through the walker table
                    while (reader.Read())
                    {
                        // We want to grab the data of the neighborhood so we can display a name instead of a number. 
                        Neighborhood neighborhood = new Neighborhood
                        {
                            Name = reader.GetString(reader.GetOrdinal("Neighborhood")),
                        };

                        // We want to grab the data of the walkers sso we can display info in a list
                        Walker walker = new Walker
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                            Neighborhood = neighborhood
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
                        SELECT w.Id, w.[Name], w.ImageUrl, w.NeighborhoodId, Neighborhood.Name as Neighborhood
                        FROM Walker w
                        JOIN Neighborhood on w.NeighborhoodId = Neighborhood.Id
                        WHERE w.Id = @id
                    ";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    // If there is a walker that matches the criteria... 
                    if (reader.Read())
                    {

                        Neighborhood neighborhood = new Neighborhood
                        {
                            Name = reader.GetString(reader.GetOrdinal("Neighborhood")),
                        };

                        Walker walker = new Walker
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                            Neighborhood = neighborhood
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

        // We would like to display the name of a neighborhood instead of an Id number.
        public List<Walker> GetWalkersInNeighborhood(int neighborhoodId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT w.Id, w.[Name], w.ImageUrl, w.NeighborhoodId, Neighborhood.Name as Neighborhood
                                        FROM Walker w
                                        JOIN Neighborhood on w.NeighborhoodId = Neighborhood.Id
                                        WHERE NeighborhoodId = @neighborhoodId";

                    cmd.Parameters.AddWithValue("@neighborhoodId", neighborhoodId);

                    // Send the query
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Walker> walkers = new List<Walker>();

                    while (reader.Read())
                    {
                        Neighborhood neighborhood = new Neighborhood
                        {
                            Name = reader.GetString(reader.GetOrdinal("Neighborhood"))
                        };

                        Walker walker = new Walker
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                            Neighborhood = neighborhood
                        };

                        walkers.Add(walker);

                    }

                    reader.Close();
                    return walkers;
                }
            }
        }

        public void AddWalker(Walker walker)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Walker([Name], ImageUrl, NeighborhoodId)
                                               OUTPUT INSERTED.ID
                                               VALUES (@name, @imageUrl, @neighborhoodId);";

                    cmd.Parameters.AddWithValue("@name", walker.Name);
                    cmd.Parameters.AddWithValue("@imageUrl", walker.ImageUrl);
                    cmd.Parameters.AddWithValue("@neighborhoodId", walker.NeighborhoodId);

                    int id = (int)cmd.ExecuteScalar();

                    walker.Id = id;
                }
            }
        }

        public void UpdateWalker(Walker walker)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Walker
                            SET Name = @name, 
                                ImageUrl = @imageurl,
                                NeighborhoodId = @neighborhoodId
                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@name", walker.Name);
                    cmd.Parameters.AddWithValue("@imageUrl", walker.ImageUrl);
                    cmd.Parameters.AddWithValue("@neighborhoodId", walker.NeighborhoodId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteWalker(int walkerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM Walker
                                               WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", walkerId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
