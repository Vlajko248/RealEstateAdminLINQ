using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RealEstateAdmin.DTO;

namespace RealEstateAdmin.Data
{
    public class CenaRepository
    {
        public List<CenaDTO> GetAll()
        {
            var cene = new List<CenaDTO>();

            using (var connection = DbHelper.GetConnection())
            using (var command = new SqlCommand("SELECT c.CenaID, c.NekretninaID, c.Iznos, c.DatumOd, c.DatumDo, c.Aktivna, n.Naziv AS NekretninaNaziv FROM Cena c LEFT JOIN Nekretnina n ON c.NekretninaID = n.NekretninaID ORDER BY c.DatumOd DESC", connection))
            {
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var cena = new CenaDTO
                        {
                            CenaID = reader.GetInt32(0),
                            NekretninaID = reader.GetInt32(1),
                            Iznos = reader.GetDecimal(2),
                            DatumOd = reader.GetDateTime(3),
                            DatumDo = reader.GetDateTime(4),
                            Aktivna = reader.GetBoolean(5),
                            NekretninaNaziv = reader.IsDBNull(6) ? string.Empty : reader.GetString(6)
                        };

                        cene.Add(cena);
                    }
                }
            }

            return cene;
        }

        public void Insert(CenaDTO cena)
        {
            if (cena == null)
            {
                throw new ArgumentNullException(nameof(cena));
            }

            using (var connection = DbHelper.GetConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                using (var command = new SqlCommand("sp_Cena_Insert", connection, transaction))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@NekretninaID", cena.NekretninaID);
                        command.Parameters.AddWithValue("@Iznos", cena.Iznos);
                        command.Parameters.AddWithValue("@DatumOd", cena.DatumOd);
                        command.Parameters.AddWithValue("@DatumDo", cena.DatumDo);
                        command.Parameters.AddWithValue("@Aktivna", cena.Aktivna);

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

        public void Update(CenaDTO cena)
        {
            if (cena == null)
            {
                throw new ArgumentNullException(nameof(cena));
            }

            using (var connection = DbHelper.GetConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                using (var command = new SqlCommand("sp_Cena_Update", connection, transaction))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CenaID", cena.CenaID);
                        command.Parameters.AddWithValue("@NekretninaID", cena.NekretninaID);
                        command.Parameters.AddWithValue("@Iznos", cena.Iznos);
                        command.Parameters.AddWithValue("@DatumOd", cena.DatumOd);
                        command.Parameters.AddWithValue("@DatumDo", cena.DatumDo);
                        command.Parameters.AddWithValue("@Aktivna", cena.Aktivna);

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

        public void Delete(int cenaId)
        {
            using (var connection = DbHelper.GetConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                using (var command = new SqlCommand("sp_Cena_Delete", connection, transaction))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CenaID", cenaId);

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
