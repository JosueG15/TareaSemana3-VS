using formcontacto2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
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

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id.HasValue)
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    string query = "SELECT * FROM FormContacto WHERE Id = @Id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                FormContacto = new FormContacto()
                                {
                                    Id = reader.GetInt32("Id"),
                                    Nombre = reader.GetString("Nombre"),
                                    Correo = reader.GetString("Correo"),
                                    Mensaje = reader.GetString("Mensaje")
                                };
                                return Page();
                            }
                            else
                            {
                                return NotFound();
                            }
                        }
                    }
                }
            } else
            {
                FormContacto = new FormContacto();
            }
            return Page();
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
                string query;
                if (FormContacto.Id == 0)
                {
                    // Lógica para insertar un nuevo contacto
                    query = "INSERT INTO FormContacto (Nombre, Correo, Mensaje) VALUES (@Nombre, @Correo, @Mensaje)";
                }
                else
                {
                    // Lógica para actualizar un contacto existente
                    query = "UPDATE FormContacto SET Nombre = @Nombre, Correo = @Correo, Mensaje = @Mensaje WHERE Id = @Id";
                }

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", FormContacto.Id);
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
