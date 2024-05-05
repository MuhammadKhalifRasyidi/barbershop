using System;
using System.Data;
using System.Data.SqlClient;

namespace barbershop
{
    internal class Program
    {
        public static void Main(string[] args) //metode utama yg dieksekusi saat aplikasi dijalankan
        {
            SqlConnection conn = null; //untuk menghubungkan aplikasi dengan databse
            string strKoneksi = "Data source = LAPTOP-9K6G3SGG\\KHALIFRASYIDI;" +
                "Initial catalog = Barbershop;User ID = SA; Password = bumyagaraf4"; //menunjukkan alamat database yg dihubungkan aplikasi
            conn = new SqlConnection(strKoneksi); //membuat objek koneksi baru
            conn.Open(); //Membuka koneksi ke database
            Console.Clear();
            while (true) // loop yang akan menampilkan menu pengelola data dan menunggu input dari pengguna
            {
                try //blok try-catch yang menangkap kesalahan yang mungkin terjadi selama eksekusi kode di dalamnya. Jika terjadi kesalahan, pesan kesalahan akan ditampilkan kepada pengguna dan loop akan melanjutkan ke iterasi berikutnya
                {
                    Console.WriteLine("\nMenu Pengelola Data BARBERSHOP"); //console WriteLine menampilkan teks di konsol
                    Console.WriteLine("1. Data Pelanggan");
                    Console.WriteLine("2. Data Tukang Cukur");
                    Console.WriteLine("3. Data Pembookingan");
                    Console.WriteLine("4. Data Antrian");
                    Console.WriteLine("5. Exit");
                    Console.WriteLine("\n Masukkan Pilihan(1 - 5): ");
                    char ch = Convert.ToChar(Console.ReadLine()); //Membaca input karakter yang akan digunakan pengguna untuk memilih opsi menu
                    switch (ch) //mengevaluasi nilai dari karakter input dan mengeksekusi blok kode yang sesuai dengan nilai tersebut
                    {
                        case '1': //pemanggilan metode main
                            {
                                Pelanggan pelanggan = new Pelanggan();
                                pelanggan.main();
                            }
                            break;
                        case '2':
                            {
                                TukangCukur tukangCukur = new TukangCukur();
                                tukangCukur.main();
                            }
                            break;
                        case '3':
                            {
                                Pembookingan pembookingan = new Pembookingan();
                                pembookingan.main();
                            }
                            break;
                        case '4':
                            {
                                Antrian antrian = new Antrian();
                                antrian.main();
                            }
                            break;
                        case '5':
                            //conn.Close();
                            Console.Clear();
                            return;
                        default:
                            {
                                Console.Clear();
                                Console.WriteLine("\n Invalid option"); 
                            }
                            break;
                    }
                }
                catch
                {
                    Console.Clear(); //membersihkan konsol dari semua teks yang ada
                    Console.WriteLine("\nCheck for the value entered");
                }
            }
        }
    }
}
