using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.Globalization;
using WaterLogger.Models;

namespace WaterLogger.Pages
{
    public class UpdateModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public DrinkingWaterModel DrinkingWater { get; set; }

        public UpdateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult OnGet(int id)
        {
            DrinkingWater = GetById(id);

            return Page();
        }

        private DrinkingWaterModel GetById(int id)
        {
            var drinkingWaterRecord = new DrinkingWaterModel();
            using(var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"Select * from drinking_water where id = {id}";

                SqliteDataReader reader = tableCmd.ExecuteReader();

                while (reader.Read())
                {
                    drinkingWaterRecord.Id = reader.GetInt32(0);
                    drinkingWaterRecord.Date = DateTime.Parse(reader.GetString(1), CultureInfo.CurrentUICulture.DateTimeFormat);
                    drinkingWaterRecord.Quantity = reader.GetFloat(2);
                    drinkingWaterRecord.Container = reader.GetString(3) is null ? Container.Glass : reader.GetString(3) switch
                    {
                        "Glass" => Container.Glass,
                        "Bottle" => Container.Bottle,
                        "BigBottle" => Container.BigBottle,
                        _ => Container.Glass
                    };

                }

                return drinkingWaterRecord;
            }

        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            using(var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"update drinking_water set date = '{DrinkingWater.Date}', quantity= {DrinkingWater.Quantity}, container = '{DrinkingWater.Container}' where id = {DrinkingWater.Id}";

                tableCmd.ExecuteNonQuery();
            }

            return RedirectToPage("./Index");
        }
    }
}
