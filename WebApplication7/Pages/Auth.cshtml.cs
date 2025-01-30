using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using WebApplication7.Pages.Models;

namespace WebApplication7.Pages
{
    [IgnoreAntiforgeryToken]
    public class Auth : PageModel
    {
        [BindProperty]
        public string email { get; set; }
        public string UserID { get; set; }
        public string AvatarUrl { get; set; }
        [BindProperty]
        public string password { get; set; }
        public string Message { get; set; }

        public void OnGet()
        {
            // �������� ������ ��� �������������� ��������� �����������
            if (HttpContext.Session.GetString("UserEmail") != null)
            {
                TempData["Notification"] = "�� ��� ������������";
                Response.Redirect("/index");
            }
        }

        public async Task<IActionResult> OnPostAsync(string Email, string Password)
        {
            Email = email;
            Password = password;
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                Message = "Email � ������ �� ����� ���� �������.";
                return BadRequest(new { Message });
            }

            string connectionString = "Data Source=C:/Users/nubik/Desktop/WebApplication7/WebApplication7/database/table4.db";
            string sqlExpression = "SELECT id, email, password, AvatarUrl FROM users WHERE email = @Email AND password = @Password";

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqliteCommand(sqlExpression, connection))
                    {
                        command.Parameters.AddWithValue("@Email", Email);
                        command.Parameters.AddWithValue("@Password", Password);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows)
                            {
                                Message = "�������� ����� ��� ������.";
                                return Unauthorized();
                            }

                            await reader.ReadAsync();
                            UserContext.UserID = reader.GetInt32(0).ToString();
                            UserContext.Email = Email;
                            UserContext.Password = Password;
                            UserContext.AvatarUrl = reader["AvatarUrl"].ToString();

                            HttpContext.Session.SetString("UserEmail", Email);
                            HttpContext.Session.SetString("Id", UserContext.UserID);

                            // ���������� ������������ �� �������� ����������� � �������������� �� ������� ��������
                            Message = "�������� �����������.";
                            return RedirectToPage("/index");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // ����������� ������
                Console.WriteLine($"������: {ex.Message}");
                Message = "��������� ������ �� �������.";
                return StatusCode(500, new { Message });
            }
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            // Очистка данных сессии
            HttpContext.Session.Clear();

            // Сброс значений UserContext
            UserContext.UserID = null;
            UserContext.Email = null;
            UserContext.Password = null;
            UserContext.AvatarUrl = null;
            UserContext.Sum = 0;

            // Перенаправление на главную страницу или страницу входа
            return RedirectToPage("/Auth");
        }
    }
}
