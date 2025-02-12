using System;
using System.Data.SqlClient;
using System.Windows;

namespace LibraryManagementSystem
{
    public partial class AdminLogin : Window
    {
        public AdminLogin()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = tbAdminEmail.Text.Trim();
            string password = tbAdminPass.Password.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                alertAdmin.Text = "Введите email и пароль.";
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection("Server=TEMHANLAPTOP\\TDG2022; Database=LibraryMSWF; Integrated Security=true"))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("AdminLogin", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@adminEmail", System.Data.SqlDbType.NVarChar, 100)).Value = email;
                        cmd.Parameters.Add(new SqlParameter("@adminPass", System.Data.SqlDbType.NVarChar, 100)).Value = password;

                        object result = cmd.ExecuteScalar();
                        bool isAuthenticated = result != null && Convert.ToInt32(result) > 0;

                        if (isAuthenticated)
                        {
                            alertAdmin.Text = "";
                            MessageBox.Show("Успешный вход!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                            AdminHome adminHome = new AdminHome();
                            adminHome.Show();

                            tbAdminEmail.Clear();
                            tbAdminPass.Clear();
                            this.Close();
                        }
                        else
                        {
                            alertAdmin.Text = "Неверный email или пароль.";
                            tbAdminPass.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}\n\nДетали:\n{ex.StackTrace}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
