using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using WaterLogger.Models;

namespace WaterLogger.Pages
{
    public class CreateModel : PageModel
    {

        private readonly IConfiguration _configuration;

        public CreateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [BindProperty]
        public DrinkingWaterModel DrinkingWater { get; set; }
        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"insert into drinking_water(date, quantity) values('{DrinkingWater.Date}', {DrinkingWater.Quantity} )";

                tableCmd.ExecuteNonQuery();
            }

            return RedirectToPage("./Index");

        }
    }
}
