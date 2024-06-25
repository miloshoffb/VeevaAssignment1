using Assignment1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;

namespace Assignment1.Pages
{
    public class IndexModel : PageModel
    {
        public IndexModel()
        {
            PatientInput = new();
        }

        public void OnGet()
        {

        }

        [BindProperty]
        public PatientInput PatientInput { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                TempData["Success"] = false;
                TempData["Message"] = "Form is invalid, please fix any errors.";

                return Page();
            }

            try
            {
                PatientInput.CalculatedBsa = CalculateBsa(PatientInput.BsaFormula, PatientInput.WeightKg, PatientInput.HeightCm);

                int rowsAffected = SaveToDatabase(PatientInput.CalculatedBsa);

                // Saving only 1 record so it should only affect 1 row.
                if (rowsAffected == 1)
                {
                    TempData["Success"] = true;
                    TempData["Message"] = "Patient information submitted successfully.";
                }
                else
                {
                    TempData["Success"] = false;
                    TempData["Message"] = "There was an error submitting patient information.";
                }   
            }
            catch (Exception e)
            {
                TempData["Success"] = false;

                // Depending on who will be using this application, you may not want to display exception messages.
                TempData["Message"] = $"There was an error submitting patient information. {e.Message}";
            }

            return RedirectToPage("./Index");
        }

        private static double CalculateBsa(BsaFormula bsaFormula, double weightKg, double heightCm) => bsaFormula switch
        {
            BsaFormula.DuBois => 0.007184 * Math.Pow(weightKg, 0.425) * Math.Pow(heightCm, 0.725),
            BsaFormula.Mosteller => 0.016667 * Math.Pow(weightKg, 0.5) * Math.Pow(heightCm, 0.5),
            _ => throw new ArgumentOutOfRangeException("BSA Formula was not specified.")
        };

        // Assuming we only want the BSA value saved to the database and no patient information (as the question suggests).
        private static int SaveToDatabase(double bsa)
        {
            // I would normally put the connection string in appsettings.json but im leaving it here for simplicity.
            string connectionString = "Server=server;Database=database;User Id=username;Password=password;";
            int rowsAffected = 0;

            using (SqlConnection connection = new(connectionString))
            {
                string sql = @"INSERT INTO [dbo].[Bsa]
                               (
                                   [Bsa]
                               )
                               VALUES
                               (
                                   @Bsa
                               );";

                SqlCommand cmd = new(sql, connection)
                {
                    CommandType = CommandType.Text
                };

                cmd.Parameters.AddWithValue("@Bsa", bsa);

                connection.Open();

                rowsAffected = cmd.ExecuteNonQuery();

                connection.Close();
            }

            return rowsAffected;
        }
    }
}
