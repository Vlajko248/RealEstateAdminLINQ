using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RealEstateAdmin.DTO;

namespace RealEstateAdmin.Data
{
    public class NekretninaRepository
    {
        public List<NekretninaDTO> GetAll()
        {
            var nekretnine = new List<NekretninaDTO>();

            using (var connection = DbHelper.GetConnection())
            using (var command = new SqlCommand("SELECT n.NekretninaID, n.ProjekatID, n.KategorijaID, n.StrukturaID, n.Sifra, n.Naziv, n.Sprat, n.Kvadratura, n.Opis, n.Aktivna, p.Naziv AS ProjekatNaziv, k.Naziv AS KategorijaNaziv, s.Naziv AS StrukturaNaziv FROM Nekretnina n LEFT JOIN Projekat p ON n.ProjekatID = p.ProjekatID LEFT JOIN Kategorija k ON n.KategorijaID = k.KategorijaID LEFT JOIN Struktura s ON n.StrukturaID = s.StrukturaID ORDER BY n.Naziv", connection))
            {
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var nekretnina = new NekretninaDTO
                        {
                            NekretninaID = reader.GetInt32(0),
                            ProjekatID = reader.GetInt32(1),
                            KategorijaID = reader.GetInt32(2),
                            StrukturaID = reader.GetInt32(3),
                            Sifra = reader.GetString(4),
                            Naziv = reader.GetString(5),
                            Sprat = reader.GetInt32(6),
                            Kvadratura = reader.GetDecimal(7),
                            Opis = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                            Aktivna = reader.GetBoolean(9),
                            ProjekatNaziv = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                            KategorijaNaziv = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                            StrukturaNaziv = reader.IsDBNull(12) ? string.Empty : reader.GetString(12)
                        };

                        nekretnine.Add(nekretnina);
                    }
                }
            }

            return nekretnine;
        }

        public void Insert(NekretninaDTO nekretnina)
        {
            if (nekretnina == null)
            {
                throw new ArgumentNullException(nameof(nekretnina));
            }

            using (var connection = DbHelper.GetConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                using (var command = new SqlCommand("sp_Nekretnina_Insert", connection, transaction))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ProjekatID", nekretnina.ProjekatID);
                        command.Parameters.AddWithValue("@KategorijaID", nekretnina.KategorijaID);
                        command.Parameters.AddWithValue("@StrukturaID", nekretnina.StrukturaID);
                        command.Parameters.AddWithValue("@Sifra", nekretnina.Sifra ?? string.Empty);
                        command.Parameters.AddWithValue("@Naziv", nekretnina.Naziv ?? string.Empty);
                        command.Parameters.AddWithValue("@Sprat", nekretnina.Sprat);
                        command.Parameters.AddWithValue("@Kvadratura", nekretnina.Kvadratura);
                        command.Parameters.AddWithValue("@Opis", nekretnina.Opis ?? string.Empty);
                        command.Parameters.AddWithValue("@Aktivna", nekretnina.Aktivna);

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

        public void Update(NekretninaDTO nekretnina)
        {
            if (nekretnina == null)
            {
                throw new ArgumentNullException(nameof(nekretnina));
            }

            using (var connection = DbHelper.GetConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                using (var command = new SqlCommand("sp_Nekretnina_Update", connection, transaction))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@NekretninaID", nekretnina.NekretninaID);
                        command.Parameters.AddWithValue("@ProjekatID", nekretnina.ProjekatID);
                        command.Parameters.AddWithValue("@KategorijaID", nekretnina.KategorijaID);
                        command.Parameters.AddWithValue("@StrukturaID", nekretnina.StrukturaID);
                        command.Parameters.AddWithValue("@Sifra", nekretnina.Sifra ?? string.Empty);
                        command.Parameters.AddWithValue("@Naziv", nekretnina.Naziv ?? string.Empty);
                        command.Parameters.AddWithValue("@Sprat", nekretnina.Sprat);
                        command.Parameters.AddWithValue("@Kvadratura", nekretnina.Kvadratura);
                        command.Parameters.AddWithValue("@Opis", nekretnina.Opis ?? string.Empty);
                        command.Parameters.AddWithValue("@Aktivna", nekretnina.Aktivna);

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

        public void Delete(int nekretninaId)
        {
            using (var connection = DbHelper.GetConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                using (var command = new SqlCommand("sp_Nekretnina_Delete", connection, transaction))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@NekretninaID", nekretninaId);

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
