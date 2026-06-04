using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RealEstateAdmin.DTO;

namespace RealEstateAdmin.Data
{
    public class ProjekatRepository
    {
        public List<ProjekatDTO> GetAll()
        {
            var projekti = new List<ProjekatDTO>();

            using (var connection = DbHelper.GetConnection())
            using (var command = new SqlCommand("SELECT p.ProjekatID, p.Naziv, p.Adresa, p.GradID, p.Opis, g.Naziv AS GradNaziv FROM Projekat p LEFT JOIN Grad g ON p.GradID = g.GradID ORDER BY p.Naziv", connection))
            {
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var projekat = new ProjekatDTO
                        {
                            ProjekatID = reader.GetInt32(0),
                            Naziv = reader.GetString(1),
                            Adresa = reader.GetString(2),
                            GradID = reader.GetInt32(3),
                            Opis = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                            GradNaziv = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
                        };

                        projekti.Add(projekat);
                    }
                }
            }

            return projekti;
        }

        public void Insert(ProjekatDTO projekat)
        {
            if (projekat == null)
            {
                throw new ArgumentNullException(nameof(projekat));
            }

            using (var connection = DbHelper.GetConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                using (var command = new SqlCommand("sp_Projekat_Insert", connection, transaction))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Naziv", projekat.Naziv ?? string.Empty);
                        command.Parameters.AddWithValue("@Adresa", projekat.Adresa ?? string.Empty);
                        command.Parameters.AddWithValue("@GradID", projekat.GradID);
                        command.Parameters.AddWithValue("@Opis", projekat.Opis ?? string.Empty);

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

        public void Update(ProjekatDTO projekat)
        {
            if (projekat == null)
            {
                throw new ArgumentNullException(nameof(projekat));
            }

            using (var connection = DbHelper.GetConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                using (var command = new SqlCommand("sp_Projekat_Update", connection, transaction))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ProjekatID", projekat.ProjekatID);
                        command.Parameters.AddWithValue("@Naziv", projekat.Naziv ?? string.Empty);
                        command.Parameters.AddWithValue("@Adresa", projekat.Adresa ?? string.Empty);
                        command.Parameters.AddWithValue("@GradID", projekat.GradID);
                        command.Parameters.AddWithValue("@Opis", projekat.Opis ?? string.Empty);

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

        public void Delete(int projekatId)
        {
            using (var connection = DbHelper.GetConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                using (var command = new SqlCommand("sp_Projekat_Delete", connection, transaction))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ProjekatID", projekatId);

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
