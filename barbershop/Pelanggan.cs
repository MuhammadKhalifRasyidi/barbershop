using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace barbershop
{
    internal class Pelanggan
    {
        public void main()
        {
            Pelanggan pr = new Pelanggan();
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
                                    Console.WriteLine("Data Pelanggan");
                                    Console.WriteLine();
                                    pr.baca(conn);
                                    break;
                                case '2':
                                    Console.Clear();
                                    Console.WriteLine("Input Pelanggan");
                                    Console.WriteLine("Masukkan Nama Pelanggan :");
                                    string namaPelanggan = Console.ReadLine();
                                    Console.WriteLine("Masukkan Nomor Telepon (contoh : 08**********) : ");
                                    string noTelp = Console.ReadLine();
                                    Console.WriteLine("Masukkan ID Pelanggan (contoh : 001) : ");
                                    string idPelanggan = Console.ReadLine();
                                    try
                                    {
                                        pr.insert(namaPelanggan, noTelp, idPelanggan, conn);
                                    }
                                    catch
                                    {
                                        Console.WriteLine("\nAnda tidak memiliki " +
                                            "akses untuk menambah data");
                                    }
                                    break;
                                case '3':
                                    Console.Clear();
                                    Console.WriteLine("Masukkan Nama Pelanggan yang ingin dihapus:\n");
                                    string namaPelangganHapus = Console.ReadLine();
                                    try
                                    {
                                        pr.delete(namaPelangganHapus, conn);
                                    }
                                    catch
                                    {
                                        Console.WriteLine("\nAnda tidak memiliki " +
                                            "akses untuk menghapus data atau data yang anda masukkan salah");
                                    }
                                    break;
                                case '4':
                                    Console.Clear();
                                    Console.WriteLine("Update Data Pelanggan\n");
                                    Console.WriteLine("Masukkan ID Pelanggan yang akan diupdate: ");
                                    string idPelangganToUpdate = Console.ReadLine();
                                    Console.WriteLine("Masukkan Nama Pelanggan baru: ");
                                    string newNamaPelanggan = Console.ReadLine();
                                    Console.WriteLine("Masukkan Nomor Telepon baru (contoh : 08**********) : ");
                                    string newNoTelp = Console.ReadLine();
                                    try
                                    {
                                        pr.update(idPelangganToUpdate, newNamaPelanggan, newNoTelp, conn);
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("\nAnda tidak memiliki akses untuk mengubah data atau data yang anda masukkan salah");
                                    }
                                    break;
                                case '5':
                                    Console.Clear();
                                    Console.WriteLine("Cari Data Pelanggan Berdasarkan Nama Pelanggan\n");
                                    Console.WriteLine("Masukkan Nama Pelanggan yang ingin dicari: ");
                                    string searchnamaPelanggan = Console.ReadLine();
                                    try
                                    {
                                        pr.searchByNamaPelanggan(searchnamaPelanggan, conn);
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
            SqlCommand cmd = new SqlCommand("SELECT * FROM Pelanggan", con);
            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                Console.WriteLine($"ID Pelanggan: {r["idPelanggan"]}, Nama Pelanggan: {r["Nama_pelanggan"]}, No Telepon: {r["No_telp"]}");
                Console.WriteLine();
            }
            r.Close();
        }

        public void insert(string namaPelanggan, string noTelp, string idPelanggan, SqlConnection conn)
        {
            // Validasi nama pelanggan
            if (!IsNameValid(namaPelanggan))
            {
                Console.WriteLine("Nama pelanggan tidak valid. Pastikan nama hanya berisi huruf dan tidak kosong.");
                return;
            }

            // Validasi nomor telepon
            if (!IsPhoneNumberValid(noTelp))
            {
                Console.WriteLine("Nomor telepon tidak valid. Pastikan nomor telepon hanya berisi angka dan memiliki panjang minimal 9 dan maksimal 12 angka.");
                return;
            }

            // Validasi ID pelanggan
            if (!IsCustomerIdValid(idPelanggan))
            {
                Console.WriteLine("ID pelanggan tidak valid. Pastikan ID hanya berisi angka dan tidak kosong.");
                return;
            }

            // Jika semua data valid, lakukan penyisipan ke database
            string str = "INSERT INTO Pelanggan (Nama_pelanggan, No_telp, idPelanggan) VALUES (@namaPelanggan, @noTelp, @idPelanggan)";
            SqlCommand cmd = new SqlCommand(str, conn);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@namaPelanggan", namaPelanggan));
            cmd.Parameters.Add(new SqlParameter("@noTelp", noTelp));
            cmd.Parameters.Add(new SqlParameter("@idPelanggan", idPelanggan));

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

        private bool IsNameValid(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && name.All(char.IsLetter);
        }

        private bool IsPhoneNumberValid(string phoneNumber)
        {
            return !string.IsNullOrWhiteSpace(phoneNumber) && phoneNumber.All(char.IsDigit) && phoneNumber.Length >= 9 && phoneNumber.Length <= 13;
        }

        private bool IsCustomerIdValid(string customerId)
        {
            return customerId.Length == 3 && customerId.All(char.IsDigit);
        }


        public void delete(string namaPelanggan, SqlConnection con)
        {
            // Periksa apakah data dengan ID pelanggan yang dimasukkan pengguna ada
            string checkQuery = "SELECT COUNT(*) FROM Pelanggan WHERE Nama_pelanggan = @namaPelanggan";
            SqlCommand checkCmd = new SqlCommand(checkQuery, con);
            checkCmd.Parameters.AddWithValue("@namaPelanggan", namaPelanggan);

            int count = (int)checkCmd.ExecuteScalar();

            if (count == 0)
            {
                Console.WriteLine("Data dengan Nama Pelanggan yang dimasukkan tidak ditemukan.");
                return;
            }

            // Jika data ditemukan, lanjutkan proses penghapusan
            string str = "DELETE FROM Pelanggan WHERE Nama_pelanggan = @namaPelanggan";
            SqlCommand cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@namaPelanggan", namaPelanggan));
            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Berhasil Dihapus");
        }


        public void update(string idPelanggan, string newNamaPelanggan, string newNoTelp, SqlConnection con)
        {
            if (!IsNameValid(newNamaPelanggan))
            {
                Console.WriteLine("Nama pelanggan tidak valid. Pastikan nama hanya berisi huruf dan tidak kosong.");
                return;
            }

            if (!IsPhoneNumberValid(newNoTelp))
            {
                Console.WriteLine("Nomor telepon tidak valid. Pastikan nomor telepon hanya berisi angka dan tidak kosong.");
                return;
            }

            string checkQuery = "SELECT COUNT(*) FROM Pelanggan WHERE idPelanggan = @idPelanggan AND Nama_pelanggan = @newNamaPelanggan AND No_telp = @newNoTelp";
            SqlCommand checkCmd = new SqlCommand(checkQuery, con);
            checkCmd.Parameters.AddWithValue("@idPelanggan", idPelanggan);
            checkCmd.Parameters.AddWithValue("@newNamaPelanggan", newNamaPelanggan);
            checkCmd.Parameters.AddWithValue("@newNoTelp", newNoTelp);

            int count = (int)checkCmd.ExecuteScalar();

            if (count > 0)
            {
                Console.WriteLine("Data yang akan diupdate sama dengan data yang sudah ada.");
                return;
            }
                
            // Jika data tidak sama, lakukan proses update
            string updateQuery = "UPDATE Pelanggan SET Nama_pelanggan = @newNamaPelanggan, No_telp = @newNoTelp WHERE idPelanggan = @idPelanggan";
            SqlCommand cmd = new SqlCommand(updateQuery, con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@newNamaPelanggan", newNamaPelanggan);
            cmd.Parameters.AddWithValue("@newNoTelp", newNoTelp);
            cmd.Parameters.AddWithValue("@idPelanggan", idPelanggan);

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


        public void searchByNamaPelanggan(string namaPelanggan, SqlConnection con)
        {
            string query = "SELECT * FROM Pelanggan WHERE Nama_Pelanggan = @namaPelanggan";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@namaPelanggan", namaPelanggan);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                Console.WriteLine("Hasil Pencarian:\n");
                while (reader.Read())
                {
                    Console.WriteLine($"ID Pelanggan: {reader["idPelanggan"]}, Nama Pelanggan: {reader["Nama_pelanggan"]}, No Telepon: {reader["No_telp"]}");
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
