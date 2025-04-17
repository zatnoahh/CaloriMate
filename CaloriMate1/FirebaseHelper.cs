using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase;
using Firebase.Database;
using Firebase.Database.Query;

namespace CaloriMate1
{
    public class FirebaseHelper
    {
        FirebaseClient firebase = new FirebaseClient("https://caloriemate-d5225-default-rtdb.asia-southeast1.firebasedatabase.app/"); // replace with your actual Firebase URL

        public async Task AddFoodLog(string date, string food, int calories, string meal)
        {
            await firebase
                .Child("FoodLogs")
                .PostAsync(new FoodLogEntry
                {
                    Date = date,
                    Food = food,
                    Calories = calories,
                    MealTime = meal
                });
        }

        public async Task<List<FoodLogEntry>> GetAllFoodLogs()
        {
            return (await firebase
                .Child("FoodLogs")
                .OnceAsync<FoodLogEntry>())
                .Select(item => new FoodLogEntry
                {
                    Date = item.Object.Date,
                    Food = item.Object.Food,
                    Calories = item.Object.Calories,
                    MealTime = item.Object.MealTime
                }).ToList();
        }
    }
}
