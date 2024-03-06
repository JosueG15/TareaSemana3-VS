using formcontacto2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace formcontacto2.Pages
{
    public class ContactoModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public ContactoModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [BindProperty]
        public FormContacto FormContacto { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string query = "INSERT INTO FormContacto (Nombre, Correo, Mensaje) VALUES (@Nombre, @Correo, @Mensaje)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", FormContacto.Nombre);
                    command.Parameters.AddWithValue("@Correo", FormContacto.Correo);
                    command.Parameters.AddWithValue("@Mensaje", FormContacto.Mensaje);
                    await command.ExecuteNonQueryAsync();
                }
            }

            return RedirectToPage("/Index");
        }
    }
    }
