using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace barbershop
{
    internal class TukangCukur
    {
        public void main()
        {
            TukangCukur pr = new TukangCukur();
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
                                    Console.WriteLine("Data Tukang Cukur");
                                    Console.WriteLine();
                                    pr.baca(conn);
                                    break;
                                case '2':
                                    Console.Clear();
                                    Console.WriteLine("Input Tukang Cukur");
                                    Console.WriteLine("Masukkan Nama :");
                                    string nama = Console.ReadLine();
                                    Console.WriteLine("Masukkan Jadwal Kerja: ");
                                    string jadwalKerja = Console.ReadLine();
                                    Console.WriteLine("Masukkan Pengalaman: ");
                                    string pengalaman = Console.ReadLine();
                                    Console.WriteLine("Masukkan ID Tukang Cukur: ");
                                    string idTkgCukur = Console.ReadLine();
                                    try
                                    {
                                        pr.insert(nama, jadwalKerja, pengalaman, idTkgCukur, conn);
                                    }
                                    catch
                                    {
                                        Console.WriteLine("\nAnda tidak memiliki " +
                                            "akses untuk menambah data");
                                    }
                                    break;
                                case '3':
                                    Console.Clear();
                                    Console.WriteLine("Masukkan Nama Tukang Cukur yang ingin dihapus:\n");
                                    string namaTkgCukurHapus = Console.ReadLine();
                                    try
                                    {
                                        pr.delete(namaTkgCukurHapus, conn);
                                    }
                                    catch
                                    {
                                        Console.WriteLine("\nAnda tidak memiliki " +
                                            "akses untuk menghapus data atau data yang anda masukkan salah");
                                    }
                                    break;
                                case '4':
                                    Console.Clear();
                                    Console.WriteLine("Update Data Tukang Cukur\n");
                                    Console.WriteLine("Masukkan ID Tukang Cukur yang akan diupdate: ");
                                    string idTkgCukurToUpdate = Console.ReadLine();
                                    Console.WriteLine("Masukkan Nama baru: ");
                                    string newNama = Console.ReadLine();
                                    Console.WriteLine("Masukkan Jadwal Kerja baru (Format: HH:mm): ");
                                    string newJadwalKerja = Console.ReadLine();
                                    Console.WriteLine("Masukkan Pengalaman baru: ");
                                    string newPengalaman = Console.ReadLine();
                                    Console.WriteLine("Masukkan ID Tukang Cukur baru: ");
                                    string newIdTkgCukur = Console.ReadLine();
                                    try
                                    {
                                        pr.update(idTkgCukurToUpdate, newNama, newJadwalKerja, newPengalaman, newIdTkgCukur, conn);
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("\nAnda tidak memiliki akses untuk mengubah data atau data yang anda masukkan salah");
                                    }
                                    break;
                                case '5':
                                    Console.Clear();
                                    Console.WriteLine("Cari Data Tukang Cukur Berdasarkan Nama Tukang Cukur\n");
                                    Console.WriteLine("Masukkan Nama Tukang Cukur yang ingin dicari: ");
                                    string searchNamaTkgCukur = Console.ReadLine();
                                    try
                                    {
                                        pr.searchByNamaTkgCukur(searchNamaTkgCukur, conn);
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
            SqlCommand cmd = new SqlCommand("SELECT * FROM TukangCukur", con);
            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                Console.WriteLine($"ID Tukang Cukur: {r["Id_TkgCukur"]}, Nama: {r["Nama"]}, Jadwal Kerja: {r["Jadwal_kerja"]}, Pengalaman: {r["pengalaman"]}");
                Console.WriteLine();
            }
            r.Close();
        }

        public void insert(string nama, string jadwalKerja, string pengalaman, string idTkgCukur, SqlConnection conn)
        {
            if (!IsNameValid(nama))
            {
                Console.WriteLine("Nama pelanggan tidak valid. Pastikan nama hanya berisi huruf dan tidak kosong.");
                return;
            }

            // Validasi ID Tukang Cukur
            if (!IsIdTukangCukurValid(idTkgCukur))
            {
                Console.WriteLine("ID tukang cukur tidak valid. Pastikan ID terdiri dari 5 karakter huruf dan angka.");
                return;
            }

            // Jika semua validasi terpenuhi, lakukan penyisipan ke database
            string str = "INSERT INTO TukangCukur (Nama, Jadwal_kerja, pengalaman, Id_TkgCukur) VALUES (@nama, @jadwalKerja, @pengalaman, @idTkgCukur)";
            SqlCommand cmd = new SqlCommand(str, conn);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@nama", nama));
            cmd.Parameters.Add(new SqlParameter("@jadwalKerja", jadwalKerja));
            cmd.Parameters.Add(new SqlParameter("@pengalaman", pengalaman));
            cmd.Parameters.Add(new SqlParameter("@idTkgCukur", idTkgCukur));

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

        public void delete(string namaTkgCukur, SqlConnection con)
        {
            // Periksa apakah data dengan ID Tukang Cukur yang dimasukkan pengguna ada
            string checkQuery = "SELECT COUNT(*) FROM TukangCukur WHERE nama = @namaTkgCukur";
            SqlCommand checkCmd = new SqlCommand(checkQuery, con);
            checkCmd.Parameters.AddWithValue("@namaTkgCukur", namaTkgCukur);

            int count = (int)checkCmd.ExecuteScalar();

            if (count == 0)
            {
                Console.WriteLine("Data dengan Nama Tukang Cukur yang dimasukkan tidak ditemukan.");
                return;
            }

            // Jika data ditemukan, lanjutkan proses penghapusan
            string str = "DELETE FROM TukangCukur WHERE nama = @namaTkgCukur";
            SqlCommand cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@namaTkgCukur", namaTkgCukur));
            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Berhasil Dihapus");
        }


        public void update(string idTkgCukur, string newNama, string newJadwalKerja, string newPengalaman, string newIdTkgCukur, SqlConnection con)
        {
            // Validasi ID Tukang Cukur baru
            if (!IsIdTukangCukurValid(newIdTkgCukur))
            {
                Console.WriteLine("ID tukang cukur baru tidak valid. Pastikan ID terdiri dari 3 karakter angka.");
                return;
            }

            // Cek apakah ID Tukang Cukur baru sama dengan ID Tukang Cukur yang sudah ada
            if (idTkgCukur != newIdTkgCukur)
            {
                string checkQuery = "SELECT COUNT(*) FROM TukangCukur WHERE Id_TkgCukur = @newIdTkgCukur";
                SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                checkCmd.Parameters.AddWithValue("@newIdTkgCukur", newIdTkgCukur);

                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    Console.WriteLine("ID tukang cukur baru sudah digunakan.");
                    return;
                }
            }

            // Jika data tidak sama, lakukan proses update
            string str = "UPDATE TukangCukur SET Nama = @newNama, Jadwal_kerja = @newJadwalKerja, pengalaman = @newPengalaman, Id_TkgCukur = @newIdTkgCukur WHERE Id_TkgCukur = @idTkgCukur";
            SqlCommand cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@newNama", newNama);
            cmd.Parameters.AddWithValue("@newJadwalKerja", newJadwalKerja);
            cmd.Parameters.AddWithValue("@newPengalaman", newPengalaman);
            cmd.Parameters.AddWithValue("@newIdTkgCukur", newIdTkgCukur);
            cmd.Parameters.AddWithValue("@idTkgCukur", idTkgCukur);

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

        private bool IsNameValid(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && name.All(char.IsLetter);
        }

        private bool IsIdTukangCukurValid(string idTkgCukur)
        {
            return !string.IsNullOrWhiteSpace(idTkgCukur) && idTkgCukur.Length == 3 && idTkgCukur.All(char.IsLetterOrDigit);
        }

        public void searchByNamaTkgCukur(string namaTkgCukur, SqlConnection con)
        {
            string query = "SELECT * FROM TukangCukur WHERE nama = @namaTkgCukur";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@namaTkgCukur", namaTkgCukur);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                Console.WriteLine("Hasil Pencarian:\n");
                while (reader.Read())
                {
                    Console.WriteLine($"ID Tukang Cukur: {reader["Id_TkgCukur"]}, Nama: {reader["Nama"]}, Jadwal Kerja: {reader["Jadwal_kerja"]}, Pengalaman: {reader["pengalaman"]}");
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
