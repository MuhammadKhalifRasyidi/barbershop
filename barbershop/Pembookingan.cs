using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace barbershop
{
    internal class Pembookingan
    {
        public void main()
        {
            Pembookingan pr = new Pembookingan();
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
                                    Console.WriteLine("Data Pembookingan");
                                    Console.WriteLine();
                                    pr.baca(conn);
                                    break;
                                case '2':
                                    Console.Clear();
                                    Console.WriteLine("Input Pembookingan");
                                    Console.WriteLine("Masukkan ID Booking (contoh : 123) : ");
                                    string idBooking = Console.ReadLine();
                                    Console.WriteLine("Masukkan Nama Pelanggan : ");
                                    string namaPelanggan = Console.ReadLine();
                                    Console.WriteLine("Masukkan Nomor Telepon (contoh : 08********) : ");
                                    string noTelp = Console.ReadLine();
                                    Console.WriteLine("Masukkan Waktu Booking (contoh : 1) : ");
                                    string waktuBooking = Console.ReadLine();
                                    Console.WriteLine("Masukkan Nomor Antrian (contoh : 12): ");
                                    string noAntrian = Console.ReadLine();
                                    try
                                    {
                                        pr.insert(idBooking, namaPelanggan, noTelp, waktuBooking, noAntrian, conn);
                                    }
                                    catch
                                    {
                                        Console.WriteLine("\nAnda tidak memiliki " +
                                            "akses untuk menambah data");
                                    }
                                    break;
                                case '3':
                                    Console.Clear();
                                    Console.WriteLine("Masukkan Nama Booking yang ingin dihapus:\n");
                                    string namaBookingHapus = Console.ReadLine();
                                    try
                                    {
                                        pr.delete(namaBookingHapus, conn);
                                    }
                                    catch
                                    {
                                        Console.WriteLine("\nAnda tidak memiliki " +
                                            "akses untuk menghapus data atau data yang anda masukkan salah");
                                    }
                                    break;
                                case '4':
                                    Console.Clear();
                                    Console.WriteLine("Update Data Pembookingan\n");
                                    Console.WriteLine("Masukkan ID Booking yang akan diupdate (contoh : 123) : ");
                                    string idBookingToUpdate = Console.ReadLine();
                                    Console.WriteLine("Masukkan Nama Pelanggan baru: ");
                                    string newNamaPelanggan = Console.ReadLine();
                                    Console.WriteLine("Masukkan Nomor Telepon baru (contoh : 08********) : ");
                                    string newNoTelp = Console.ReadLine();
                                    Console.WriteLine("Masukkan Waktu Booking baru (contoh : 1) : ");
                                    string newWaktuBooking = Console.ReadLine();
                                    Console.WriteLine("Masukkan Nomor Antrian baru (contoh : 12) : ");
                                    string newNoAntrian = Console.ReadLine();
                                    try
                                    {
                                        pr.update(idBookingToUpdate, newNamaPelanggan, newNoTelp, newWaktuBooking, newNoAntrian, conn);
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("\nAnda tidak memiliki akses untuk mengubah data atau data yang anda masukkan salah");
                                    }
                                    break;
                                case '5':
                                    Console.Clear();
                                    Console.WriteLine("Cari Data Pembookingan Berdasarkan Nama Booking\n");
                                    Console.WriteLine("Masukkan Nama Booking yang ingin dicari: ");
                                    string searchNamaBooking = Console.ReadLine();
                                    try
                                    {
                                        pr.searchByNamaBooking(searchNamaBooking, conn);
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
            SqlCommand cmd = new SqlCommand("SELECT * FROM Pembookingan", con);
            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                Console.WriteLine($"ID Booking: {r["Id_booking"]}, Nama Pelanggan: {r["Nama_pelanggan"]}, No Telepon: {r["No_telp"]}, Waktu Booking: {r["Waktu_booking"]}, No Antrian: {r["No_antrian"]}");
                Console.WriteLine();
            }
            r.Close();
        }

        public void insert(string idBooking, string namaPelanggan, string noTelp, string waktuBooking, string noAntrian, SqlConnection conn)
        {
            // Validasi ID Booking
            if (!IsidbookingValid(idBooking))
            {
                Console.WriteLine("ID Booking tidak boleh kosong harus berisi maksimal 3 angka.");
                return;
            }

            if (!IsNameValid(namaPelanggan))
            {
                Console.WriteLine("Nama pelanggan tidak valid. Pastikan nama hanya berisi huruf dan tidak kosong.");
                return;
            }

            // Validasi Nomor Telepon
            if (string.IsNullOrWhiteSpace(noTelp))
            {
                Console.WriteLine("Nomor Telepon tidak boleh kosong.");
                return;
            }
            else
            {
                // Validasi Nomor Telepon hanya terdiri dari angka
                if (!IsPhoneNumberValid(noTelp))
                {
                    Console.WriteLine("Nomor Telepon hanya boleh terdiri dari angka.");
                    return;
                }
            }

            // Validasi Waktu Booking
            if (string.IsNullOrWhiteSpace(waktuBooking))
            {
                Console.WriteLine("Waktu Booking tidak boleh kosong.");
                return;
            }

            // Validasi Nomor Antrian
            if (string.IsNullOrWhiteSpace(noAntrian))
            {
                Console.WriteLine("Nomor Antrian tidak boleh kosong.");
                return;
            }
            else
            {
                // Validasi Nomor Antrian hanya terdiri dari angka
                if (!IsNumeric(noAntrian))
                {
                    Console.WriteLine("Nomor Antrian hanya boleh terdiri dari angka.");
                    return;
                }
            }

            // Lakukan penyisipan ke database jika semua data valid
            string str = "INSERT INTO Pembookingan (Id_booking, Nama_pelanggan, No_telp, Waktu_booking, No_antrian) VALUES (@idBooking, @namaPelanggan, @noTelp, @waktuBooking, @noAntrian)";
            SqlCommand cmd = new SqlCommand(str, conn);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@idBooking", idBooking));
            cmd.Parameters.Add(new SqlParameter("@namaPelanggan", namaPelanggan));
            cmd.Parameters.Add(new SqlParameter("@noTelp", noTelp));
            cmd.Parameters.Add(new SqlParameter("@waktuBooking", waktuBooking));
            cmd.Parameters.Add(new SqlParameter("@noAntrian", noAntrian));

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

        private void AddToQueue(string idBooking, string namaPelanggan, string noAntrian, SqlConnection conn)
        {
            // Lakukan penyisipan ke dalam tabel antrian
            string queueStr = "INSERT INTO Antrian (Id_booking, Nama_pelanggan, No_antrian) VALUES (@idBooking, @namaPelanggan, @noAntrian)";
            SqlCommand queueCmd = new SqlCommand(queueStr, conn);
            queueCmd.CommandType = CommandType.Text;

            queueCmd.Parameters.Add(new SqlParameter("@idBooking", idBooking));
            queueCmd.Parameters.Add(new SqlParameter("@namaPelanggan", namaPelanggan));
            queueCmd.Parameters.Add(new SqlParameter("@noAntrian", noAntrian));

            try
            {
                queueCmd.ExecuteNonQuery();
                Console.WriteLine("Data Antrian Berhasil Ditambahkan");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gagal menambahkan data antrian: " + ex.Message);
            }
        }

        // Method untuk memeriksa apakah string hanya terdiri dari angka
        private bool IsNumeric(string str)
        {
            foreach (char c in str)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        // Method untuk memeriksa apakah nomor telepon valid (hanya angka)
        private bool IsPhoneNumberValid(string phoneNumber)
        {
            return IsNumeric(phoneNumber);
        }

        private bool IsidbookingValid(string idbooking)
        {
            return idbooking.Length == 3 && idbooking.All(char.IsDigit);
        }

        private bool IsNameValid(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && name.All(char.IsLetter);
        }


        public void delete(string namaBooking, SqlConnection con)
        {
            // Periksa apakah data dengan ID booking yang dimasukkan pengguna ada
            string checkQuery = "SELECT COUNT(*) FROM Pembookingan WHERE Nama_pelanggan = @namaBooking";
            SqlCommand checkCmd = new SqlCommand(checkQuery, con);
            checkCmd.Parameters.AddWithValue("@namaBooking", namaBooking);

            int count = (int)checkCmd.ExecuteScalar();

            if (count == 0)
            {
                Console.WriteLine("Data dengan Nama Booking yang dimasukkan tidak ditemukan.");
                return;
            }

            // Jika data ditemukan, lanjutkan proses penghapusan
            string str = "DELETE FROM Pembookingan WHERE Nama_pelanggan = @namaBooking";
            SqlCommand cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@namaBooking", namaBooking));
            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Berhasil Dihapus");
        }


        public void update(string idBooking, string newNamaPelanggan, string newNoTelp, string newWaktuBooking, string newNoAntrian, SqlConnection con)
        {
            // Validasi ID Booking
            if (string.IsNullOrWhiteSpace(idBooking))
            {
                Console.WriteLine("ID Booking tidak boleh kosong.");
                return;
            }

            // Lakukan pencarian data untuk memeriksa apakah data sudah ada
            string query = "SELECT COUNT(*) FROM Pembookingan WHERE Id_booking = @idBooking AND Nama_pelanggan = @newNamaPelanggan AND No_telp = @newNoTelp AND Waktu_booking = @newWaktuBooking AND No_antrian = @newNoAntrian";
            SqlCommand checkCmd = new SqlCommand(query, con);
            checkCmd.Parameters.AddWithValue("@idBooking", idBooking);
            checkCmd.Parameters.AddWithValue("@newNamaPelanggan", newNamaPelanggan);
            checkCmd.Parameters.AddWithValue("@newNoTelp", newNoTelp);
            checkCmd.Parameters.AddWithValue("@newWaktuBooking", newWaktuBooking);
            checkCmd.Parameters.AddWithValue("@newNoAntrian", newNoAntrian);

            int count = (int)checkCmd.ExecuteScalar();

            if (count > 0)
            {
                Console.WriteLine("Data yang akan diupdate sama dengan data yang sudah ada.");
                return;
            }

            // Lakukan update ke database jika semua data valid
            string updateQuery = "UPDATE Pembookingan SET Nama_pelanggan = @newNamaPelanggan, No_telp = @newNoTelp, Waktu_booking = @newWaktuBooking, No_antrian = @newNoAntrian WHERE Id_booking = @idBooking";
            SqlCommand cmd = new SqlCommand(updateQuery, con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@newNamaPelanggan", newNamaPelanggan);
            cmd.Parameters.AddWithValue("@newNoTelp", newNoTelp);
            cmd.Parameters.AddWithValue("@newWaktuBooking", newWaktuBooking);
            cmd.Parameters.AddWithValue("@newNoAntrian", newNoAntrian);
            cmd.Parameters.AddWithValue("@idBooking", idBooking);

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

        public void searchByNamaBooking(string namaBooking, SqlConnection con)
        {
            string query = "SELECT * FROM Pembookingan WHERE Nama_pelanggan = @namaBooking";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@namaBooking", namaBooking);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                Console.WriteLine("Hasil Pencarian:\n");
                while (reader.Read())
                {
                    Console.WriteLine($"ID Booking: {reader["Id_booking"]}, Nama Pelanggan: {reader["Nama_pelanggan"]}, No Telepon: {reader["No_telp"]}, Waktu Booking: {reader["Waktu_booking"]}, No Antrian: {reader["No_antrian"]}");
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
