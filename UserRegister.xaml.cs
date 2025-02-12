using System;
using System.Data.SqlClient;
using System.Windows;

namespace LibraryManagementSystem
{
    public partial class UserRegister : Window
    {
        public UserRegister()
        {
            InitializeComponent();
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            string fullName = tbFullName.Text.Trim();
            string email = tbEmail.Text.Trim();
            string password = tbPassword.Password.Trim();
            string userAdNo = tbUserAdNo.Text.Trim();

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(userAdNo))
            {
                alertUser.Text = "All fields are required!";
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection("Server=TEMHANLAPTOP\\TDG2022; Database=LibraryMSWF; Integrated Security=true"))
                {
                    conn.Open();

                    // Проверяем, существует ли email
                    string checkQuery = "SELECT COUNT(*) FROM tblUsers WHERE UserEmail = @Email";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@Email", email);
                        int userExists = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (userExists > 0)
                        {
                            alertUser.Text = "Email already registered!";
                            return;
                        }
                    }

                    // Регистрируем нового пользователя
                    string insertQuery = "INSERT INTO tblUsers (UserName, UserEmail, UserPass, UserAdNo) OUTPUT INSERTED.UserId VALUES (@Name, @Email, @Pass, @AdNo)";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", fullName);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Pass", password);
                        cmd.Parameters.AddWithValue("@AdNo", userAdNo);

                        // Получаем ID нового пользователя
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            int userId = Convert.ToInt32(result);

                            MessageBox.Show("Registration successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                            // Открываем главное окно (UserHome)
                            UserHome userHome = new UserHome(userId, fullName);
                            userHome.Show();
                            this.Close();
                        }
                        else
                        {
                            alertUser.Text = "Registration failed! Try again.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            UserLogin loginWindow = new UserLogin();
            loginWindow.Show();
            this.Close();
        }
    }
}
