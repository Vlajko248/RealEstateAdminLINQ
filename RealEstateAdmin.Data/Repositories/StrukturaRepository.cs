using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RealEstateAdmin.DTO;

namespace RealEstateAdmin.Data
{
    public class StrukturaRepository
    {
        public List<StrukturaDTO> GetAll()
        {
            var strukture = new List<StrukturaDTO>();

            using (var connection = DbHelper.GetConnection())
            using (var command = new SqlCommand("SELECT s.StrukturaID, s.KategorijaID, s.Naziv, s.Opis, k.Naziv AS KategorijaNaziv FROM Struktura s LEFT JOIN Kategorija k ON s.KategorijaID = k.KategorijaID ORDER BY s.Naziv", connection))
            {
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var struktura = new StrukturaDTO
                        {
                            StrukturaID = reader.GetInt32(0),
                            KategorijaID = reader.GetInt32(1),
                            Naziv = reader.GetString(2),
                            Opis = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            KategorijaNaziv = reader.IsDBNull(4) ? string.Empty : reader.GetString(4)
                        };

                        strukture.Add(struktura);
                    }
                }
            }

            return strukture;
        }

        public void Insert(StrukturaDTO struktura)
        {
            if (struktura == null)
            {
                throw new ArgumentNullException(nameof(struktura));
            }

            using (var connection = DbHelper.GetConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                using (var command = new SqlCommand("sp_Struktura_Insert", connection, transaction))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@KategorijaID", struktura.KategorijaID);
                        command.Parameters.AddWithValue("@Naziv", struktura.Naziv ?? string.Empty);
                        command.Parameters.AddWithValue("@Opis", struktura.Opis ?? string.Empty);

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

        public void Update(StrukturaDTO struktura)
        {
            if (struktura == null)
            {
                throw new ArgumentNullException(nameof(struktura));
            }

            using (var connection = DbHelper.GetConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                using (var command = new SqlCommand("sp_Struktura_Update", connection, transaction))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@StrukturaID", struktura.StrukturaID);
                        command.Parameters.AddWithValue("@KategorijaID", struktura.KategorijaID);
                        command.Parameters.AddWithValue("@Naziv", struktura.Naziv ?? string.Empty);
                        command.Parameters.AddWithValue("@Opis", struktura.Opis ?? string.Empty);

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

        public void Delete(int strukturaId)
        {
            using (var connection = DbHelper.GetConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                using (var command = new SqlCommand("sp_Struktura_Delete", connection, transaction))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@StrukturaID", strukturaId);

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
