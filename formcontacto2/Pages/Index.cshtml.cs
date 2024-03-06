using formcontacto2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace formcontacto2.Pages
{
    public class IndexModel : PageModel
    {
        private readonly string _connectionString;

        public IndexModel(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<FormContacto> Contactos { get; set; }

        public void OnGet()
        {
            Contactos = new List<FormContacto>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT Id, Nombre, Correo, Mensaje FROM FormContacto";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FormContacto contacto = new FormContacto
                            {
                                Id = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Correo = reader.GetString(2),
                                Mensaje = reader.GetString(3)
                            };
                            Contactos.Add(contacto);
                        }
                    }
                }
            }
        }

        public IActionResult OnPostEliminar(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM FormContacto WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToPage("./Index");
        }
    }
}

