using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace barbershop
{
    internal class Antrian
    {
        public void main()
        {
            Antrian pr = new Antrian();
            while (true)
            {
                try
                {
                    SqlConnection conn = null;
                    string strKoneksi = "Data source = LAPTOP-9K6G3SGG\\KHALIFRASYIDI;" +
                        "Initial catalog = Barbershop;User ID = SA; Password = bumyagaraf4";
                    conn = new SqlConnection(strKoneksi);
                    conn.Open();
                    Console.Clear();
                    while (true)
                    {
                        try
                        {
                            Console.WriteLine("\nMenu Kelola");
                            Console.WriteLine("1. Melihat Seluruh Data");
                            Console.WriteLine("2. Tambah Data");
                            Console.WriteLine("3. Hapus Data");
                            Console.WriteLine("4. Edit Data");
                            Console.WriteLine("5. Cari Data");
                            Console.WriteLine("6. Keluar");
                            Console.WriteLine("\n Enter your choice (1-6): ");
                            char ch = Convert.ToChar(Console.ReadLine());
                            switch (ch)
                            {
                                case '1':
                                    Console.Clear();
                                    Console.WriteLine("Data Antrian");
                                    Console.WriteLine();
                                    pr.baca(conn);
                                    break;
                                case '2':
                                    Console.Clear();
                                    Console.WriteLine("Input Antrian");
                                    Console.WriteLine("Masukkan Nama Pelanggan :");
                                    string namaPelanggan = Console.ReadLine();
                                    Console.WriteLine("Masukkan Nomor Antrian : ");
                                    string noAntrian = Console.ReadLine();
                                    Console.WriteLine("Masukkan Waktu Tunggu: ");
                                    string waktuTunggu = Console.ReadLine();
                                    Console.WriteLine("Masukkan ID Antrian (3 digit): ");
                                    string idAntrian = Console.ReadLine();
                                    try
                                    {
                                        pr.insert(namaPelanggan, noAntrian, waktuTunggu, idAntrian, conn);
                                    }
                                    catch
                                    {
                                        Console.WriteLine("\nAnda tidak memiliki " +
                                            "akses untuk menambah data");
                                    }
                                    break;
                                case '3':
                                    Console.Clear();
                                    Console.WriteLine("Masukkan Nama Antrian yang ingin dihapus:\n");
                                    string namaAntrianHapus = Console.ReadLine();
                                    try
                                    {
                                        pr.delete(namaAntrianHapus, conn);
                                    }
                                    catch
                                    {
                                        Console.WriteLine("\nAnda tidak memiliki " +
                                            "akses untuk menghapus data atau data yang anda masukkan salah");
                                    }
                                    break;
                                case '4':
                                    Console.Clear();
                                    Console.WriteLine("Update Data Antrian\n");
                                    Console.WriteLine("Masukkan ID Antrian yang akan diupdate: ");
                                    string idAntrianToUpdate = Console.ReadLine();
                                    Console.WriteLine("Masukkan Nama Pelanggan baru: ");
                                    string newNamaPelanggan = Console.ReadLine();
                                    Console.WriteLine("Masukkan Nomor Antrian baru (2 digit): ");
                                    string newNoAntrian = Console.ReadLine();
                                    Console.WriteLine("Masukkan Waktu Tunggu baru: ");
                                    string newWaktuTunggu = Console.ReadLine();
                                    Console.WriteLine("Masukkan ID Antrian baru (3 digit): ");
                                    string newIdAntrian = Console.ReadLine();
                                    try
                                    {
                                        pr.update(idAntrianToUpdate, newNamaPelanggan, newNoAntrian, newWaktuTunggu, newIdAntrian, conn);
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("\nAnda tidak memiliki akses untuk mengubah data atau data yang anda masukkan salah");
                                    }
                                    break;
                                case '5':
                                    Console.Clear();
                                    Console.WriteLine("Cari Data Antrian Berdasarkan Nama Antrian\n");
                                    Console.WriteLine("Masukkan Nama Antrian yang ingin dicari: ");
                                    string searchNamaAntrian = Console.ReadLine();
                                    try
                                    {
                                        pr.searchByNamaAntrian(searchNamaAntrian, conn);
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("\nTerjadi kesalahan dalam pencarian data: " + e.Message);
                                    }
                                    break;
                                case '6':
                                    conn.Close();
                                    Console.Clear();
                                    return;
                                default:
                                    Console.Clear();
                                    Console.WriteLine("\n Invalid option");
                                    break;
                            }
                        }
                        catch
                        {
                            Console.Clear();
                            Console.WriteLine("\nCheck for the value entered");
                        }
                    }
                }
                catch
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Tidak Dapat Mengakses Database Tersebut\n");
                    Console.ResetColor();
                }
            }
        }

        public void baca(SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Antrian", con);
            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                Console.WriteLine($"ID Antrian: {r["Id_antrian"]}, Nama Pelanggan: {r["Nama_pelanggan"]}, No Antrian: {r["No_antrian"]}, Waktu Tunggu: {r["Waktu_tunggu"]}");
                Console.WriteLine();
            }
            r.Close();
        }

        public void insert(string namaPelanggan, string noAntrian, string waktuTunggu, string idAntrian, SqlConnection conn)
        {
            // Validasi ID Antrian
            if (!IsIdAntrianValid(idAntrian))
            {
                Console.WriteLine("ID Antrian tidak valid. Pastikan ID terdiri dari 3 digit.");
                return;
            }

            // Jika semua validasi terpenuhi, lakukan penyisipan ke database
            string str = "INSERT INTO Antrian (Id_antrian, Nama_pelanggan, No_antrian, Waktu_tunggu) VALUES (@idAntrian, @namaPelanggan, @noAntrian, @waktuTunggu)";
            SqlCommand cmd = new SqlCommand(str, conn);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@idAntrian", idAntrian));
            cmd.Parameters.Add(new SqlParameter("@namaPelanggan", namaPelanggan));
            cmd.Parameters.Add(new SqlParameter("@noAntrian", noAntrian));
            cmd.Parameters.Add(new SqlParameter("@waktuTunggu", waktuTunggu));

            try
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine("Data Berhasil Ditambahkan");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gagal menambahkan data: " + ex.Message);
            }
        }


        public void delete(string namaPelanggan, SqlConnection con)
        {
            // Periksa apakah data dengan ID Antrian yang dimasukkan pengguna ada
            string checkQuery = "SELECT COUNT(*) FROM Antrian WHERE Nama_pelanggan = @namaPelanggan";
            SqlCommand checkCmd = new SqlCommand(checkQuery, con);
            checkCmd.Parameters.AddWithValue("@namaPelanggan", namaPelanggan);

            int count = (int)checkCmd.ExecuteScalar();

            if (count == 0)
            {
                Console.WriteLine("Data dengan Nama Antrian yang dimasukkan tidak ditemukan.");
                return;
            }

            // Jika data ditemukan, lanjutkan proses penghapusan
            string str = "DELETE FROM Antrian WHERE Nama_pelanggan = @namaPelanggan";
            SqlCommand cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@namaPelanggan", namaPelanggan));
            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Berhasil Dihapus");
        }

        public void update(string idAntrian, string newNamaPelanggan, string newNoAntrian, string newWaktuTunggu, string newIdAntrian, SqlConnection con)
        {
            // Validasi ID Antrian baru
            if (!IsIdAntrianValid(newIdAntrian))
            {
                Console.WriteLine("ID Antrian baru tidak valid. Pastikan ID terdiri dari 3 digit.");
                return;
            }

            // Cek apakah ID Antrian baru sama dengan ID Antrian yang sudah ada
            if (idAntrian != newIdAntrian)
            {
                string checkQuery = "SELECT COUNT(*) FROM Antrian WHERE Id_antrian = @newIdAntrian";
                SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                checkCmd.Parameters.AddWithValue("@newIdAntrian", newIdAntrian);

                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    Console.WriteLine("ID Antrian baru sudah digunakan.");
                    return;
                }
            }

            // Jika data tidak sama, lakukan proses update
            string str = "UPDATE Antrian SET Nama_pelanggan = @newNamaPelanggan, No_antrian = @newNoAntrian, Waktu_tunggu = @newWaktuTunggu, Id_antrian = @newIdAntrian WHERE Id_antrian = @idAntrian";
            SqlCommand cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@newNamaPelanggan", newNamaPelanggan);
            cmd.Parameters.AddWithValue("@newNoAntrian", newNoAntrian);
            cmd.Parameters.AddWithValue("@newWaktuTunggu", newWaktuTunggu);
            cmd.Parameters.AddWithValue("@newIdAntrian", newIdAntrian);
            cmd.Parameters.AddWithValue("@idAntrian", idAntrian);

            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Console.WriteLine("Data berhasil diupdate.");
            }
            else
            {
                Console.WriteLine("Data tidak ditemukan atau gagal diupdate.");
            }
        }

        private bool IsIdAntrianValid(string idAntrian)
        {
            return !string.IsNullOrWhiteSpace(idAntrian) && idAntrian.Length == 3 && idAntrian.All(char.IsDigit);
        }


        public void searchByNamaAntrian(string namaPelanggan, SqlConnection con)
        {
            string query = "SELECT * FROM Antrian WHERE Nama_pelanggan = @namaPelanggan";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@namaPelanggan", namaPelanggan);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                Console.WriteLine("Hasil Pencarian:\n");
                while (reader.Read())
                {
                    Console.WriteLine($"ID Antrian: {reader["Id_antrian"]}, Nama Pelanggan: {reader["Nama_pelanggan"]}, No Antrian: {reader["No_antrian"]}, Waktu Tunggu: {reader["Waktu_tunggu"]}");
                }
            }
            else
            {
                Console.WriteLine("Data tidak ditemukan.");
            }

            reader.Close();
        }
    }
}
