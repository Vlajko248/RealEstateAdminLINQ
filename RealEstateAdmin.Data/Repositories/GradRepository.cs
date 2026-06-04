using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RealEstateAdmin.DTO;

namespace RealEstateAdmin.Data
{
    public class GradRepository
    {
        public List<GradDTO> GetAll()
        {
            var gradovi = new List<GradDTO>();

            using (var connection = DbHelper.GetConnection())
            using (var command = new SqlCommand("SELECT GradID, Naziv, PostanskiBroj FROM Grad ORDER BY Naziv", connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var grad = new GradDTO
                        {
                            GradID = reader.GetInt32(0),
                            Naziv = reader.GetString(1),
                            PostanskiBroj = reader.GetString(2)
                        };
                        gradovi.Add(grad);
                    }
                }
            }

            return gradovi;
        }

        public void Insert(GradDTO grad)
        {
            if (grad == null)
            {
                throw new ArgumentNullException(nameof(grad));
            }

            using (var connection = DbHelper.GetConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                using (var command = new SqlCommand("sp_Grad_Insert", connection, transaction))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Naziv", grad.Naziv ?? string.Empty);
                        command.Parameters.AddWithValue("@PostanskiBroj", grad.PostanskiBroj ?? string.Empty);

                        command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Update(GradDTO grad)
        {
            if (grad == null)
            {
                throw new ArgumentNullException(nameof(grad));
            }

            using (var connection = DbHelper.GetConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                using (var command = new SqlCommand("sp_Grad_Update", connection, transaction))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@GradID", grad.GradID);
                        command.Parameters.AddWithValue("@Naziv", grad.Naziv ?? string.Empty);
                        command.Parameters.AddWithValue("@PostanskiBroj", grad.PostanskiBroj ?? string.Empty);

                        command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Delete(int gradId)
        {
            using (var connection = DbHelper.GetConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                using (var command = new SqlCommand("sp_Grad_Delete", connection, transaction))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@GradID", gradId);

                        command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
