using DogGo.Models;
using System;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Repositories
{
    public class DogRepository : IDogRepository
    {
        private readonly IConfiguration _config;

        public DogRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Dog> GetAllDogs()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, [Name], OwnerId, Breed, Notes, ImageUrl
                                        FROM Dog";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Dog> dogs = new List<Dog>();

                    while (reader.Read())
                    {
                        Dog dog = new Dog
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),

                            /*A note about CRUD on Dog data. You will find that the Notes and ImageUrl columns are allowed to be null in the Dog table. 
                             * Working with null in ADO.NET can be kinda tricky.
                             * You have to check for a null value before you try to get the value from a SqlDataReader object.
                             * When inserting or updating using a null value, you have to use a special DbNull.Value value.*/
                            // An image and notes may not actually hold value. SO we need to create an if/else like statement
                            // Show me this if there is info, if there isn't do this instead 
                            Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null :
                                reader.GetString(reader.GetOrdinal("Notes")),

                            ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null :
                                reader.GetString(reader.GetOrdinal("ImageUrl"))
                        };

                        dogs.Add(dog);
                    }

                    reader.Close();
                    return dogs;
                }
            }
        }

        public Dog GetDogById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, [Name], Breed, Notes, ImageUrl, OwnerId
                                        FROM Dog
                                        WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Dog dog = new Dog()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),

                            Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null :
                                reader.GetString(reader.GetOrdinal("Notes")),
                            ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null :
                                reader.GetString(reader.GetOrdinal("ImageUrl")),

                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId"))
                        };

                        reader.Close();
                        return dog;
                    }
                    reader.Close();
                    return null;
                }
            }
        }

        public void AddDog(Dog dog)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INMSERT INTO Dog ([Name], OwnerId, Breed, Notes, ImageUrl)
                                        OUTPUT INSERTED.ID
                                        VALUES (@name, @ownerId, @breed, @notes, @imageUrl);";

                    // The columns must contain input in order to be added to the database
                    cmd.Parameters.AddWithValue("@name", dog.Name);
                    cmd.Parameters.AddWithValue("@breed", dog.Breed);
                    cmd.Parameters.AddWithValue("@ownerId", dog.OwnerId);

                    // These columns can remain or have a value of null and still be added to the DB
                    if (dog.Notes == null)
                    {
                        cmd.Parameters.AddWithValue("@notes", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@notes", dog.Notes);
                    }

                    if (dog.ImageUrl == null)
                    {
                        cmd.Parameters.AddWithValue("@imageUrl", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@imageUrl", dog.ImageUrl);
                    }

                    int newlyCreatedId = (int)cmd.ExecuteScalar();

                    dog.Id = newlyCreatedId;
                }
            }
        }

        public void UpdateDog(Dog dog)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                      UPDATE Dog
                                      SET
                                      [Name] = @name,
                                      Breed = @breed,
                                      Notes = @notes,
                                      ImageUrl = @imageUrl,
                                      OwnerId = @ownerId
                                      WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@name", dog.Name);
                    cmd.Parameters.AddWithValue("@breed", dog.Breed);
                    cmd.Parameters.AddWithValue("@notes", dog.Notes);
                    cmd.Parameters.AddWithValue("@imageUrl", dog.ImageUrl);
                    cmd.Parameters.AddWithValue("@ownerId", dog.OwnerId);
                    cmd.Parameters.AddWithValue("@id", dog.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteDog(int dogId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                      DELETE FROM Dog
                                      WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", dogId);
                    cmd.ExecuteNonQuery();
                }
            }
        }


    }
}