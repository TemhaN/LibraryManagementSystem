using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace LibraryManagementSystem
{
    /// <summary>
    /// Interaction logic for UserLogin.xaml
    /// </summary>
    public partial class UserLogin : Window
    {
        public static int userId;
        public static string userName; // Перенесено в поле класса

        public UserLogin()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbUserEmail.Text) && !string.IsNullOrEmpty(tbUserPass.Password))
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection("Server=TEMHANLAPTOP\\TDG2022; Database=LibraryMSWF; Integrated Security=true"))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand("UserLogin", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@UserEmail", tbUserEmail.Text);
                            cmd.Parameters.AddWithValue("@UserPass", tbUserPass.Password);


                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read()) // Если найден пользователь
                                {
                                    userId = Convert.ToInt32(reader["UserId"]);

                                    // Проверяем, не NULL ли `UserName`
                                    userName = reader["UserName"] != DBNull.Value ? reader["UserName"].ToString() : "Unknown";

                                    MessageBox.Show("Logged in successfully...", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                                    // Передаём userId и userName в UserHome
                                    UserHome userHome = new UserHome(userId, userName);
                                    userHome.Show();
                                    this.Close();
                                }
                                else
                                {
                                    alertUser.Text = "Invalid email or password...";
                                    tbUserEmail.Clear();
                                    tbUserPass.Clear();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                alertUser.Text = "Enter email and password properly...";


            }
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            UserRegister registerWindow = new UserRegister();
            registerWindow.Show();
            this.Close();
        }
    }
}
