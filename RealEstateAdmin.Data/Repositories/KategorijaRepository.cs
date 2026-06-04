using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RealEstateAdmin.DTO;

namespace RealEstateAdmin.Data
{
    public class KategorijaRepository
    {
        public List<KategorijaDTO> GetAll()
        {
            var kategorije = new List<KategorijaDTO>();

            using (var connection = DbHelper.GetConnection())
            using (var command = new SqlCommand("SELECT KategorijaID, Naziv, Opis FROM Kategorija ORDER BY Naziv", connection))
            {
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var kategorija = new KategorijaDTO
                        {
                            KategorijaID = reader.GetInt32(0),
                            Naziv = reader.GetString(1),
                            Opis = reader.IsDBNull(2) ? string.Empty : reader.GetString(2)
                        };

                        kategorije.Add(kategorija);
                    }
                }
            }

            return kategorije;
        }

        public void Insert(KategorijaDTO kategorija)
        {
            if (kategorija == null)
            {
                throw new ArgumentNullException(nameof(kategorija));
            }

            using (var connection = DbHelper.GetConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                using (var command = new SqlCommand("sp_Kategorija_Insert", connection, transaction))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Naziv", kategorija.Naziv ?? string.Empty);
                        command.Parameters.AddWithValue("@Opis", kategorija.Opis ?? string.Empty);

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

        public void Update(KategorijaDTO kategorija)
        {
            if (kategorija == null)
            {
                throw new ArgumentNullException(nameof(kategorija));
            }

            using (var connection = DbHelper.GetConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                using (var command = new SqlCommand("sp_Kategorija_Update", connection, transaction))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@KategorijaID", kategorija.KategorijaID);
                        command.Parameters.AddWithValue("@Naziv", kategorija.Naziv ?? string.Empty);
                        command.Parameters.AddWithValue("@Opis", kategorija.Opis ?? string.Empty);

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

        public void Delete(int kategorijaId)
        {
            using (var connection = DbHelper.GetConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                using (var command = new SqlCommand("sp_Kategorija_Delete", connection, transaction))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@KategorijaID", kategorijaId);

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
